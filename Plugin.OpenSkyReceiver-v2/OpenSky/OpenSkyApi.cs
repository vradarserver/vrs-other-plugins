// Copyright © 2020 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Plugin.OpenSkyReceiver.OpenSky
{
    /// <summary>
    /// Handes calls to the OpenSky Network REST API.
    /// </summary>
    class OpenSkyApi
    {
        private const string BaseUrl = "https://opensky-network.org/api";

        private const int TimeoutSeconds = 60;

        public AllStateVectorsResponseModel GetAllStateVectors(AllStateVectorsRequestModel request)
        {
            var queryString = new StringBuilder();

            if(request != null) {
                AppendQueryStringVariable(queryString, "time",      request.TimeAsSecondsSinceUnixEpoch);
                AppendQueryStringVariable(queryString, "icao24",    request.Icao24s);
                AppendQueryStringVariable(queryString, "lamin",     request.LatitudeLow);
                AppendQueryStringVariable(queryString, "lomin",     request.LongitudeLow);
                AppendQueryStringVariable(queryString, "lamax",     request.LatitudeHigh);
                AppendQueryStringVariable(queryString, "lomax",     request.LongitudeHigh);
            }

            var url = $"{BaseUrl}/states/all{queryString}";
            var json = Get(url);

            return JsonConvert.DeserializeObject<AllStateVectorsResponseModel>(json);
        }

        private static void AppendQueryStringVariable(StringBuilder buffer, string key, double? value)
        {
            if(value != null) {
                AppendQueryStringVariable(
                    buffer,
                    key,
                    value.Value.ToString(CultureInfo.InvariantCulture)
                );
            }
        }

        private static void AppendQueryStringVariable(StringBuilder buffer, string key, string[] value)
        {
            if(value != null) {
                for(var idx = 0;idx < value.Length;++idx) {
                    AppendQueryStringVariable(
                        buffer,
                        key,
                        value[idx]
                    );
                }
            }
        }

        private static void AppendQueryStringVariable(StringBuilder buffer, string key, string value)
        {
            if(key != null) {
                buffer.Append(buffer.Length == 0 ? '?' : '&');
                buffer.Append(Uri.EscapeDataString(key));
                if(value != null) {
                    buffer.Append($"={Uri.EscapeDataString(value)}");
                }
            }
        }

        private static string Get(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = TimeoutSeconds * 1000;

            using(var response = request.GetResponse()) {
                using(var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8)) {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
