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
using InterfaceFactory;
using VirtualRadar.Interface;

namespace Plugin.AirportDataThumbnails
{
#pragma warning disable 0067        // Event never used
    public class Plugin : IPlugin
    {
        public string Id                { get { return "AirportDataThumbnails"; } }
        public string Name              { get { return "Airport-Data.com Thumbnails"; } }
        public string Version           { get { return "2.4.4"; } }
        public string Status            { get { return "Enabled"; } }
        public string StatusDescription { get { return ""; } }
        public bool HasOptions          { get { return false; } }
        public string PluginFolder      { get; set; }

        public event EventHandler StatusChanged;

        public void GuiThreadStartup()
        {
        }

        public void RegisterImplementations(IClassFactory classFactory)
        {
            classFactory.Register<IAirportDataDotCom, AirportDataDotCom>();
        }

        public void ShowWinFormsOptionsUI()
        {
        }

        public void Shutdown()
        {
        }

        public void Startup(PluginStartupParameters parameters)
        {
        }
    }
}
