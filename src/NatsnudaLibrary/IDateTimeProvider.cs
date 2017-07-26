// <copyright file="IDateTimeProvider.cs" company="natsnudasoft">
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
    /// Provides abstractions of the non-deterministic members of <see cref="DateTime"/>.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time on this
        /// computer, expressed as the local time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date on this
        /// computer, with the time component set to 00:00:00.
        /// </summary>
        DateTime Today { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time on this
        /// computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }
    }
}