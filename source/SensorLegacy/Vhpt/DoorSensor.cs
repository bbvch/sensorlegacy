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

namespace SensorLegacy.Vhpt
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Handlers;

    using SensorLegacy.Events;

    public class DoorSensor : Base, IDisposable
    {
        private bool _modeEnabled;

        private BlackHoleSensor _bhs;

        private VhptDoor _vhptDoor;

        private bool _pm;

        private bool _closed;

        private bool _opened;

        private static readonly VhptTravelCoordinator Coordinator;

        public DoorSensor(BlackHoleSensor bhs)
        {
            this._modeEnabled = bool.Parse(ConfigurationManager.AppSettings.Get("modeEnabled"));
            this._bhs = bhs;
        }

        public void Initialize()
        {
            this._closed = this._opened = this._pm = false;
        }

        public void StartObservation()
        {
            this._bhs.Detect();

            this._vhptDoor = new VhptDoor();
            this._vhptDoor.Opened += this.Opened;
            this._vhptDoor.Closed += this.Closed;
        }

        private void Closed(object sender, EventArgs e)
        {
            this._closed = true;

            if (this._pm)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open! PANIC!!!");

                    VhptTravelCoordinator _vphtCoordinator = new VhptTravelCoordinator();
                    _vphtCoordinator.TravelTo(!this._modeEnabled ? 42 : 0);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed! PANIC!!!");
                }
            }


            if (!this._pm)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open!");

                    VhptTravelCoordinator _vphtCoordinator = new VhptTravelCoordinator();
                    _vphtCoordinator.TravelTo(42);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed!");
                }
            }
        }

        private void Opened(object sender, EventArgs e)
        {
            this._opened = true;

            if (this._pm)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open! PANIC!!!");

                    VhptTravelCoordinator _vphtCoordinator = new VhptTravelCoordinator();
                    _vphtCoordinator.TravelTo(!this._modeEnabled ? 42 : 0);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed! PANIC!!!");
                }
            }


            if (!this._pm)
            {
                if (this._opened)
                {
                    Console.WriteLine("door is open!");

                    VhptTravelCoordinator _vphtCoordinator = new VhptTravelCoordinator();
                    _vphtCoordinator.TravelTo(42);
                }

                if (this._closed)
                {
                    Console.WriteLine("door is closed!");
                }
            }
        }

        public void Close()
        {
            this._bhs.Stop();

            if (this._vhptDoor == null)
            {
                this._vhptDoor = new VhptDoor();
            }
            else
            {
                FieldInfo obF = this._vhptDoor.GetType().GetField("observer");
                if (obF != null)
                {
                    object ob;
                    if((ob = obF.GetValue(new VhptDoor())) != null)
                    {
                        try
                        {
                            ((IDisposable)(ob)).Dispose();
                        }
                        catch (ObjectDisposedException e)
                        {
                            throw new IOException();
                        }
                    }
                }
            }

            this._vhptDoor.Opened -= this.Opened;
            this._vhptDoor.Closed -= this.Closed;
        }

        [EventSubscription("topic://BlackHoleDetected", typeof(OnPublisher))]
        public void BL(object sender, EventArgs e)
        {
            this._pm = true;

            Console.WriteLine("black hole detected! PANIC!!!");
        }

        public void Dispose()
        {
            this._vhptDoor.Dispose();
        }
    }
}