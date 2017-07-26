// <copyright file="SystemRandom.cs" company="natsnudasoft">
// Copyright (c) Adrian John Dunstan. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace Natsnudasoft.NatsnudaLibrary
{
    using System;

    /// <summary>
    /// A wrapper around the <see cref="Random"/> class, that implements the <see cref="IRandom"/>
    /// interface.
    /// </summary>
    /// <seealso cref="Random" />
    /// <seealso cref="IRandom" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SystemRandom : Random, IRandom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemRandom"/> class, using a
        /// time-dependent default seed value.
        /// </summary>
        public SystemRandom()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemRandom"/> class, using the specified
        /// seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value for the pseudo-random
        /// number sequence. If a negative number is specified, the absolute value of the number is
        /// used.
        /// </param>
        public SystemRandom(int seed)
            : base(seed)
        {
        }
    }
}
