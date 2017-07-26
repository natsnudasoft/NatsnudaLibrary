// <copyright file="IDateTimeOffsetProvider.cs" company="natsnudasoft">
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
    /// Provides abstractions of the non-deterministic members of <see cref="DateTimeOffset"/>.
    /// </summary>
    public interface IDateTimeOffsetProvider
    {
        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date and time on
        /// the current computer, with the offset set to the local time's offset from Coordinated
        /// Universal Time (UTC).
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object whose date and time are set to the current
        /// Coordinated Universal Time (UTC) date and time and whose offset is
        /// <see cref="TimeSpan.Zero"/>.
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}