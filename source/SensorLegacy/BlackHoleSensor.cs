//-------------------------------------------------------------------------------
// <copyright file="BlackHoleSensor.cs" company="bbv Software Services AG">
//   Copyright (c) 2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace SensorLegacy
{
    using System;

    public class BlackHoleSensor : Notifier
    {
        private VhptBlackHoleSubOrbitDetectionEngine _e;

        private bool on;

        public BlackHoleSensor()
        {
            _e = new VhptBlackHoleSubOrbitDetectionEngine();

            try
            {
                GlobalEventBroker.Instance.Register(this);
            }
            catch
            {
                // WTF!!! Stupid event broker throws and I don't know why!
                // No time to analyze this shit!
            }
        }

        public void Detect()
        {
            this.@on = true;

            this._e.BlackHoleDetected += this.Detected;
        }

        private void Detected(object sender, EventArgs e)
        {
            if (this.@on)
            {
                this.Notify();
            }
        }

        public void Stop()
        {
            // Toggle, toggle
            this.@on = !this.@on;
        }
    }
}