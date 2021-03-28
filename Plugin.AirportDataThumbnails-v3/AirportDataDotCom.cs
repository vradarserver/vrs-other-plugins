﻿// Copyright © 2021 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using VirtualRadar.Interface;

namespace Plugin.AirportDataThumbnails
{
    /// <summary>
    /// The plugin's implementation of the <see cref="IAirportDataDotCom"/> interface.
    /// </summary>
    /// <remarks>
    /// This is a carbon-copy of the implementation that was removed in 2.4.5-preview-4 after both AVG
    /// and Malwarebytes started blocking links to airport-data.com.
    /// </remarks>
    class AirportDataDotCom : IAirportDataDotCom
    {
        /// <summary>
        /// The timeout for requests for thumbnails
        /// </summary>
        const int ThumbnailTimeout = 10000;

        /// <summary>
        /// The timeout for entries in the thumbnail cache in minutes. Entries that have not
        /// been accessed within this period of time are automatically removed.
        /// </summary>
        const int ThumbnailCacheMaxMinutes = 60;

        /// <summary>
        /// A private class that acts as a key into the thumbnail cache.
        /// </summary>
        class ThumbnailKey
        {
            public string Icao { get; private set; }
            public int MaxThumbnails { get; private set; }

            public ThumbnailKey(string icao, int maxThumbnails)
            {
                Icao = icao;
                MaxThumbnails = maxThumbnails;
            }

            public override string ToString()
            {
                return String.Format("{0}:{1}", Icao, MaxThumbnails);
            }

            public override bool Equals(object obj)
            {
                var result = Object.ReferenceEquals(this, obj);
                if(!result) {
                    var other = obj as ThumbnailKey;
                    result = other != null && other.Icao == Icao && other.MaxThumbnails == MaxThumbnails;
                }

                return result;
            }

            public override int GetHashCode()
            {
                return (Icao ?? "").GetHashCode();
            }
        }

        /// <summary>
        /// A private class that holds the cache result for a thumbnail.
        /// </summary>
        class CachedThumbnail
        {
            public WebRequestResult<AirportDataThumbnailsJson> Thumbnail { get; set; }

            public DateTime LastAccessTimeUtc { get; set; }
        }

        /// <summary>
        /// A cache of recent thumbnail requests and their responses.
        /// </summary>
        private static ExpiringDictionary<ThumbnailKey, CachedThumbnail> _ThumbnailCache = new ExpiringDictionary<ThumbnailKey,CachedThumbnail>(ThumbnailCacheMaxMinutes * 60000, 60000);

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <param name="icao"></param>
        /// <param name="registration"></param>
        /// <param name="maxThumbnails"></param>
        /// <returns></returns>
        public WebRequestResult<AirportDataThumbnailsJson> GetThumbnails(string icao, string registration, int maxThumbnails)
        {
            var thumbnailKey = new ThumbnailKey(icao, maxThumbnails);
            var cachedThumbnail = _ThumbnailCache.GetForKeyAndRefresh(thumbnailKey);
            if(cachedThumbnail != null) {
                cachedThumbnail.LastAccessTimeUtc = DateTime.UtcNow;
            } else {
                cachedThumbnail = new CachedThumbnail() {
                    LastAccessTimeUtc = DateTime.UtcNow,
                    Thumbnail = RequestThumbnails(icao, registration, maxThumbnails),
                };
                _ThumbnailCache.UpsertAndRefresh(thumbnailKey, cachedThumbnail);
            }

            return cachedThumbnail.Thumbnail;
        }

        private WebRequestResult<AirportDataThumbnailsJson> RequestThumbnails(string icao, string registration, int maxThumbnails)
        {
            var requestUrl = String.Format("http://www.airport-data.com/api/ac_thumb.json?m={0}&r={1}&n={2}", HttpUtility.UrlEncode(icao), HttpUtility.UrlEncode(registration ?? ""), maxThumbnails);
            var request = HttpWebRequest.Create(requestUrl);
            request.Timeout = ThumbnailTimeout;

            var result = new WebRequestResult<AirportDataThumbnailsJson>();
            try {
                using(var response = (HttpWebResponse)WebRequestHelper.GetResponse(request)) {
                    result.HttpStatusCode = response.StatusCode;
                    if(result.HttpStatusCode == HttpStatusCode.OK) {
                        using(var responseStream = WebRequestHelper.GetResponseStream(response)) {
                            var deserialiser = new DataContractJsonSerializer(typeof(AirportDataThumbnailsJson));
                            result.Result = (AirportDataThumbnailsJson)deserialiser.ReadObject(responseStream);
                        }
                    }
                }
            } catch(WebException ex) {
                var webResponse = ex.Response as HttpWebResponse;
                if(webResponse != null) result.HttpStatusCode = webResponse.StatusCode;
                else                    throw;
            }

            return result;
        }
    }
}
