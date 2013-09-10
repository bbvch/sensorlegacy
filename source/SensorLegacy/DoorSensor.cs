//-------------------------------------------------------------------------------
// <copyright file="DoorSensor.cs" company="bbv Software Services AG">
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
    using System.Configuration;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Handlers;

    public class DoorSensor : Base
    {
        private bool _modeEnabled;

        private BlackHoleSensor _bhs;

        private VhptDoor _vhptDoor;

        public DoorSensor(BlackHoleSensor bhs)
        {
            _modeEnabled = bool.Parse(ConfigurationManager.AppSettings.Get("modeEnabled"));
            _vhptDoor = new VhptDoor();
            _bhs = bhs;
        }

        public void Initialize()
        {
            
        }

        public void StartObservation()
        {
            _bhs.Detect();

            _vhptDoor.Opened += Opened;
            _vhptDoor.Closed += Closed;
        }

        private void Closed(object sender, EventArgs e)
        {
        }

        private void Opened(object sender, EventArgs e)
        {
        }

        public void StopObservation()
        {
            _bhs.Stop();

            _vhptDoor.Opened -= Opened;
            _vhptDoor.Closed -= Closed;
        }

        [EventSubscription("topic://BlackHoleDetected", typeof(OnPublisher))]
        public void HandleBlackHoleDetection(object sender, EventArgs e)
        {
            // this.stateMachine.Fire(Events.BlackHoleDetected);
        }
    }
}