// <copyright file="IGuardClauseExtensions.cs" company="natsnudasoft">
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

namespace Natsnudasoft.NatsnudaLibrary.TestExtensions
{
    using System;
    using System.Reflection;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;

    /// <summary>
    /// Provides methods to extend the functionality of the
    /// <see cref="GuardClauseAssertion"/> class.
    /// </summary>
    public interface IGuardClauseExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IGuardClauseCommand"/> from the specified
        /// <see cref="IGuardClauseCommand"/> that replaces the expansion with one that uses
        /// <see cref="ParameterInfo"/> instead of <see cref="Type"/> and supports async method.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service used to workaround
        /// a problem with guard clause assertions.</param>
        /// <param name="command">The <see cref="IGuardClauseCommand"/> that will be executed
        /// as part of the assertion.</param>
        /// <returns>A new instance of <see cref="IGuardClauseCommand"/> with the newly supplied
        /// features.</returns>
        IGuardClauseCommand CreateExtendedCommand(
            ISpecimenBuilder specimenBuilder,
            IGuardClauseCommand command);

        /// <summary>
        /// Attempts to retrieve an instance of <see cref="MethodInvokeCommand"/> from an instance
        /// of <see cref="IGuardClauseCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="IGuardClauseCommand"/> to attempt to extract an
        /// instance of <see cref="MethodInvokeCommand"/> from.</param>
        /// <param name="methodInvokeCommand">Will contain an instance of
        /// <see cref="MethodInvokeCommand"/> if one is found; otherwise <see langword="null"/>.
        /// </param>
        /// <returns><see langword="true"/> if an instance of <see cref="MethodInvokeCommand"/> was
        /// found on the specified <see cref="IGuardClauseCommand"/>; otherwise
        /// <see langword="false"/>.</returns>
        bool TryGetMethodInvokeCommand(
            IGuardClauseCommand command,
            out MethodInvokeCommand methodInvokeCommand);
    }
}