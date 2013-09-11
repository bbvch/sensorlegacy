//-------------------------------------------------------------------------------
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

namespace SensorLegacy
{
    using System;
    using System.Reactive.Linq;

    public sealed class VhptBlackHoleSubOrbitDetectionEngine : IDisposable
    {
        private readonly IDisposable engine;

        public VhptBlackHoleSubOrbitDetectionEngine()
        {
            this.engine =
                Observable.Timer(TimeSpan.FromSeconds(10))
                    .Subscribe(interval => this.BlackHoleDetected(this, EventArgs.Empty));
        }

        public event EventHandler BlackHoleDetected = delegate { };

        public void Dispose()
        {
            this.engine.Dispose();
        }
    }
}