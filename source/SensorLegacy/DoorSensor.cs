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

    public class DoorSensor : Base, IDisposable
    {
        private bool _modeEnabled;

        private BlackHoleSensor _bhs;

        private VhptDoor _vhptDoor;

        private bool _panicMode;

        private bool _closed;

        private bool _opened;

        private VhptTravelCoordinator _vphtCoordinator;

        public DoorSensor(BlackHoleSensor bhs)
        {
            _modeEnabled = bool.Parse(ConfigurationManager.AppSettings.Get("modeEnabled"));
            _vhptDoor = new VhptDoor();
            _vphtCoordinator = new VhptTravelCoordinator();
            _bhs = bhs;
        }

        public void Initialize()
        {
            _closed = _opened = _panicMode = false;
        }

        public void StartObservation()
        {
            _bhs.Detect();

            _vhptDoor.Opened += Opened;
            _vhptDoor.Closed += Closed;
        }

        private void Closed(object sender, EventArgs e)
        {
            _closed = true;

            if (this._panicMode)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open! PANIC!!!");

                    this._vphtCoordinator.TravelTo(!_modeEnabled ? 42 : 0);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed! PANIC!!!");
                }
            }


            if (!this._panicMode)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open!");

                    this._vphtCoordinator.TravelTo(42);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed!");
                }
            }
        }

        private void Opened(object sender, EventArgs e)
        {
            _opened = true;

            if (this._panicMode)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open! PANIC!!!");

                    this._vphtCoordinator.TravelTo(!_modeEnabled ? 42 : 0);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed! PANIC!!!");
                }
            }


            if (!this._panicMode)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open!");

                    this._vphtCoordinator.TravelTo(42);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed!");
                }
            }
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
            _panicMode = true;

            Console.WriteLine("black hole detected! PANIC!!!");
        }

        public void Dispose()
        {
            _vhptDoor.Dispose();
        }
    }
}