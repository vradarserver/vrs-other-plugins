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
using System.Linq;
using InterfaceFactory;
using VirtualRadar.Interface;

namespace Plugin.EnumerateAircraftLists
{
    /// <summary>
    /// The plugin entry point.
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string Id                { get { return "EnumerateAircraftLists"; } }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string Name              { get { return "Enumerate Aircraft Lists"; } }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string Version           { get { return "2.4.4"; } }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string Status            { get { return "Enabled"; } }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public bool HasOptions          { get { return false; } }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public string PluginFolder      { get; set; }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public event EventHandler StatusChanged;

        /// <summary>
        /// Raises <see cref="StatusChanged"/>.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnStatusChanged(EventArgs args)
        {
            StatusChanged?.Invoke(this, args);
        }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public void GuiThreadStartup()
        {
        }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public void RegisterImplementations(IClassFactory classFactory)
        {
        }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public void ShowWinFormsOptionsUI()
        {
        }

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public void Shutdown()
        {
        }

        // Fields set up by Startup() method
        private System.Timers.Timer _Timer;
        private AircraftListEnumerator _AircraftListEnumerator; 

        /// <summary>
        /// See IPlugin interface.
        /// </summary>
        public void Startup(PluginStartupParameters parameters)
        {
            _AircraftListEnumerator = new AircraftListEnumerator();

            _Timer = new System.Timers.Timer() {
                Interval = 1000, // milliseconds
                Enabled = false,
                AutoReset = false,
            };
            _Timer.Elapsed += Timer_Elapsed;
            _Timer.Start();

            // An earlier version of this had AutoReset set to true. That works fine with this example but
            // it could cause issues with other people's code, particularly if whatever they're doing in the
            // Elapsed event handler takes more than 1000 milliseconds (or however long the timer interval
            // is set to). AutoReset could trigger overlapping concurrent calls to the event handler, which
            // might come as a surprise.
            //
            // So to be safe I've changed the sample so that the event handler has to explicitly restart the
            // timer once all of its work is complete. That way there is no possibility of overlapping calls.
        }

        /// <summary>
        /// Timer Elapsed event handler, runs on a background thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Timer_Elapsed(object sender, EventArgs args)
        {
            try {
                // Get an array of snapshots
                var snapshots = _AircraftListEnumerator.TakeSnapshotsForAllLists();

                // Do something with them
                var newStatus = $"[{DateTime.Now}] Tracking {snapshots.Sum(r => r.Snapshot.Count)} aircraft across {snapshots.Length} list(s)";
                StatusDescription = newStatus;
                OnStatusChanged(EventArgs.Empty);

                // Restart the timer
                _Timer.Start();
            } catch(Exception ex) {
                var log = Factory.ResolveSingleton<ILog>();
                log.WriteLine($"Caught exception in {GetType().FullName} timer: {ex}");
            }
        }
    }
}
