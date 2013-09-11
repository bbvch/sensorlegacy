//-------------------------------------------------------------------------------
// <copyright file="VhptDoor.cs" company="bbv Software Services AG">
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

    public sealed class VhptDoor : IDisposable
    {
        private readonly IDisposable observer;

        public VhptDoor()
        {
            var doorIsOpen = from interval in Observable.Interval(TimeSpan.FromSeconds(1))
                select Convert.ToBoolean(interval % 2);

            this.observer = doorIsOpen.Subscribe(value =>
            {
                if (value)
                {
                    this.Opened(this, EventArgs.Empty);
                }
                else
                {
                    this.Closed(this, EventArgs.Empty);
                }
            });
        }

        public event EventHandler Opened = delegate { };

        public event EventHandler Closed = delegate { };

        public void Dispose()
        {
            this.observer.Dispose();
        }
    }
}