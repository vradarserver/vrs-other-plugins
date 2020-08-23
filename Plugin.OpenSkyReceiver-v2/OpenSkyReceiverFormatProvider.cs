﻿// Copyright © 2020 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using VirtualRadar.Interface.Listener;

namespace Plugin.OpenSkyReceiver
{
    /// <summary>
    /// An implementation of <see cref="IReceiverFormatProvider"/> that the plugin uses to get
    /// its custom receiver integrated with the stock receiver formats that come with VRS.
    /// </summary>
    class OpenSkyReceiverFormatProvider : IReceiverFormatProvider
    {
        /// <summary>
        /// See interface docs. DO NOT CHANGE THIS WILLY-NILLY. It is saved to user
        /// configuration files.
        /// </summary>
        public string UniqueId => "plugin-openskyreceiver";

        /// <summary>
        /// See interface docs.
        /// </summary>
        public string ShortName => "Open Sky";

        /// <summary>
        /// See interface docs.
        /// </summary>
        public bool IsRawFormat => false;

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <returns></returns>
        public IMessageBytesExtractor CreateMessageBytesExtractor()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// See interface docs.
        /// </summary>
        /// <param name="extractor"></param>
        /// <returns></returns>
        public bool IsUsableBytesExtractor(IMessageBytesExtractor extractor)
        {
            throw new System.NotImplementedException();
        }
    }
}
