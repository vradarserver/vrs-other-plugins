// Copyright © 2020 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System.Collections.Generic;
using System.Linq;
using InterfaceFactory;
using VirtualRadar.Interface;
using VirtualRadar.Interface.Listener;

namespace Plugin.EnumerateAircraftLists
{
    /// <summary>
    /// A class that can be called to enumerate aircraft lists.
    /// </summary>
    public class AircraftListEnumerator
    {
        /// <summary>
        /// Describes a snapshot taken for a single aircraft list.
        /// </summary>
        public class AircraftListSnapshot
        {
            /// <summary>
            /// The feed that the aircraft list is getting aircraft messages from.
            /// </summary>
            public IFeed Feed { get; private set; }

            /// <summary>
            /// A collection of <see cref="IAircraft"/> clones.
            /// </summary>
            public IList<IAircraft> Snapshot { get; private set; }

            /// <summary>
            /// The timestamp that the snapshot was taken. Probably not of much use.
            /// </summary>
            public long Timestamp { get; private set; }

            /// <summary>
            /// The data version value as of the time the snapshot was taken.
            /// </summary>
            /// <remarks>
            /// If you have two snapshots of the same list taken at different moments in time then you can tell
            /// what has changed between the two by comparing this value on the old list against the 'Changed'
            /// properties on the new list. Properties where the 'Changed' value is higher than the DataVersion
            /// on the old list have changed since the old snapshot was taken.
            /// </remarks>
            public long DataVersion { get; private set; }

            /// <summary>
            /// Creates a new object.
            /// </summary>
            /// <param name="feed"></param>
            /// <param name="snapshot"></param>
            /// <param name="timestamp"></param>
            /// <param name="dataVersion"></param>
            public AircraftListSnapshot(IFeed feed, IList<IAircraft> snapshot, long timestamp, long dataVersion)
            {
                Feed = feed;
                Snapshot = snapshot;
                Timestamp = timestamp;
                DataVersion = dataVersion;
            }
        }

        /// <summary>
        /// The singleton feed manager that the class will use.
        /// </summary>
        private IFeedManager _FeedManager;

        /// <summary>
        /// Creates a new object.
        /// </summary>
        public AircraftListEnumerator()
        {
            _FeedManager = Factory.ResolveSingleton<IFeedManager>();
        }

        /// <summary>
        /// Returns an array of snapshots of aircraft lists from every feed.
        /// </summary>
        /// <returns></returns>
        public AircraftListSnapshot[] TakeSnapshotsForAllLists()
        {
            return EnumerateSnapshotsForAllLists().ToArray();
        }

        /// <summary>
        /// Enumerates snapshots of the aircraft list for every feed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AircraftListSnapshot> EnumerateSnapshotsForAllLists()
        {
            foreach(var feed in _FeedManager.Feeds) {
                long timestamp;
                long dataVersion;
                var snapshot = feed.AircraftList.TakeSnapshot(out timestamp, out dataVersion);

                yield return new AircraftListSnapshot(feed, snapshot, timestamp, dataVersion);
            }
        }
    }
}
