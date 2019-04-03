// <copyright file="SystemDateTimeProvider.cs" company="natsnudasoft">
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
    /// Represents a class which is capable of providing values of <see cref="DateTime"/> using the
    /// default <see cref="DateTime"/> members.
    /// </summary>
    /// <seealso cref="IDateTimeProvider" />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Menees.Analyzers",
            "MEN013:UseUTCTime",
            Justification = "Matching existing implementation.")]
        public DateTime Now => DateTime.Now;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Menees.Analyzers",
            "MEN013:UseUTCTime",
            Justification = "Matching existing implementation.")]
        public DateTime Today => DateTime.Today;

        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}