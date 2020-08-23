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
using System.Runtime.Serialization;

namespace Plugin.OpenSkyReceiver.OpenSky
{
    [DataContract]
    class StateVector
    {
        [DataMember(Name = "icao24")]
        public string Icao24 { get; set; }

        [DataMember(Name = "callsign")]
        public string Callsign { get; set; }

        [DataMember(Name = "origin_country")]
        public string OriginCountry { get; set; }

        [DataMember(Name = "time_position")]
        public double? UnixEpochSecondsOfLastPosition { get; set; }

        public DateTime? TimeOfLastPosition => UnixEpochSecondsOfLastPosition == null ? (DateTime?)null : Moments.UnixEpoch.AddSeconds(UnixEpochSecondsOfLastPosition.Value);

        [DataMember(Name = "last_contact")]
        public double UnixEpochSecondsOfLastMessage { get; set; }

        public DateTime TimeOfLastMessage => Moments.UnixEpoch.AddSeconds(UnixEpochSecondsOfLastMessage);

        [DataMember(Name = "longitude")]
        public double? Longitude { get; set; }

        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }

        [DataMember(Name = "baro_altitude")]
        public float? BarometricAltitudeMetres { get; set; }

        public float? AltitudeFeet => ConvertUnits.MetresToFeet(BarometricAltitudeMetres);

        [DataMember(Name = "on_ground")]
        public bool OnGround { get; set; }

        [DataMember(Name = "velocity")]
        public float? GroundSpeedMetresPerSecond { get; set; }

        public float? GroundSpeedKnots => ConvertUnits.MetresPerSecondToKnots(GroundSpeedMetresPerSecond);

        [DataMember(Name = "true_track")]
        public float? Track { get; set; }

        [DataMember(Name = "vertical_rate")]
        public float? VerticalRateMetresPerSecond { get; set; }

        public float? VerticalRateFeetPerSecond => ConvertUnits.MetresPerSecondToFeetPerSecond(VerticalRateMetresPerSecond);

        [DataMember(Name = "sensors")]
        public int[] SensorIDs { get; set; }

        [DataMember(Name = "geo_altitude")]
        public float? GeometricAltitudeMetres { get; set; }

        [DataMember(Name = "squawk")]
        public string Squawk { get; set; }

        [DataMember(Name = "spi")]
        public bool SpecialPurposeIndicator { get; set; }

        [DataMember(Name = "position_source")]
        public PositionSource PositionSource { get; set; }
    }
}
