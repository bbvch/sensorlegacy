//-------------------------------------------------------------------------------
// <copyright file="FilteredEnumerable.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.Collections.Generic;

    public class FilteredEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> source;
        private readonly Func<T, bool> predicate;

        internal FilteredEnumerable(IEnumerable<T> source,
            Func<T, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in this.source)
            {
                if (this.predicate(item))
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public FilteredEnumerable<T> Where(Func<T, bool> extraPredicate)
        {
            return new FilteredEnumerable<T>(this.source,
                x => this.predicate(x) && extraPredicate(x));
        }
    }
}