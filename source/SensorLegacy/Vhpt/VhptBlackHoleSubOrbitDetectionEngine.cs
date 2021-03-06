﻿//-------------------------------------------------------------------------------
// <copyright file="VhptBlackHoleSubOrbitDetectionEngine.cs" company="bbv Software Services AG">
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
    using System.Reactive.Linq;

    public sealed class VhptBlackHoleSubOrbitDetectionEngine : IDisposable
    {
        private readonly IDisposable engine;

        private EngineThingy engineThingy;

        public VhptBlackHoleSubOrbitDetectionEngine()
        {
            this.engine =
                Observable.Timer(TimeSpan.FromSeconds(10))
                    .Subscribe(
                        interval =>
                        {
                            Console.WriteLine("Vhpt: Detection engine warming up...");
                            this.BlackHoleDetected(this, EventArgs.Empty);
                            Console.WriteLine("Vhpt: Detection engine powering down...");
                        });

            this.engineThingy = new EngineThingy { Engine = this.engine };
            this.engineThingy.BlackHoleDetected += this.BlackHoleDetected;
        }

        public event EventHandler BlackHoleDetected = delegate { };

        public void Dispose()
        {
            this.engineThingy.Engine.Dispose();
        }
    }
}