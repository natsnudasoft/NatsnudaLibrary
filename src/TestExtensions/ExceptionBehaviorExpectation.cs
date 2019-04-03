// <copyright file="ExceptionBehaviorExpectation.cs" company="natsnudasoft">
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
    using System.Collections.Generic;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;
    using Natsnudasoft.NatsnudaLibrary;
    using static System.FormattableString;

    /// <summary>
    /// Provides a behaviour expectation that will apply a known value to a parameter in a
    /// constructor and check for a specified exception when calling the constructor with that
    /// value.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the expected exception.</typeparam>
    /// <seealso cref="GuardClauseExtensions" />
    /// <seealso cref="IBehaviorExpectation" />
    public class ExceptionBehaviorExpectation<T> : IBehaviorExpectation
        where T : Exception
    {
        private readonly ISpecimenBuilder specimenBuilder;
        private readonly IGuardClauseExtensions guardClauseExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBehaviorExpectation{T}"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service used to workaround
        /// a problem with guard clause assertions.</param>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <param name="values">The values to apply to the parameter to assert the guard clause.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/>,
        /// <paramref name="parameterName"/>, or <paramref name="values"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty.</exception>
        public ExceptionBehaviorExpectation(
            ISpecimenBuilder specimenBuilder,
            string parameterName,
            params object[] values)
            : this(specimenBuilder, new GuardClauseExtensions(), parameterName, values)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBehaviorExpectation{T}"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service used to workaround
        /// a problem with guard clause assertions.</param>
        /// <param name="guardClauseExtensions">An instance of helper extensions for the
        /// <see cref="GuardClauseAssertion"/> class.</param>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <param name="values">The values to apply to the parameter to assert the guard clause.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/>,
        /// <paramref name="guardClauseExtensions"/>, <paramref name="parameterName"/>, or
        /// <paramref name="values"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is empty.</exception>
        public ExceptionBehaviorExpectation(
            ISpecimenBuilder specimenBuilder,
            IGuardClauseExtensions guardClauseExtensions,
            string parameterName,
            params object[] values)
        {
            ParameterValidation.IsNotNull(specimenBuilder, nameof(specimenBuilder));
            ParameterValidation.IsNotNull(guardClauseExtensions, nameof(guardClauseExtensions));
            ParameterValidation.IsNotNull(parameterName, nameof(parameterName));
            ParameterValidation.IsNotEmpty(parameterName, nameof(parameterName));
            ParameterValidation.IsNotNull(values, nameof(values));
            if (values.Length == 0)
            {
                throw new ArgumentException("Collection must not be empty.", nameof(values));
            }

            this.specimenBuilder = specimenBuilder;
            this.ParameterName = parameterName;
            this.Values = values;
            this.guardClauseExtensions = guardClauseExtensions;
        }

        /// <summary>
        /// Gets the name of the parameter to apply a known value to.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the values to apply to the parameter to assert the guard clause.
        /// </summary>
        public IEnumerable<object> Values { get; }

        /// <summary>
        /// Verifies that the behaviour of the specified command matches the defined expectations.
        /// </summary>
        /// <param name="command">The command whose behaviour must be examined.</param>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is
        /// <see langword="null"/>.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exception is propagated by guard clause.")]
        public void Verify(IGuardClauseCommand command)
        {
            ParameterValidation.IsNotNull(command, nameof(command));

            MethodInvokeCommand methodInvokeCommand;
            if (this.guardClauseExtensions.TryGetMethodInvokeCommand(
                    command,
                    out methodInvokeCommand) &&
                methodInvokeCommand.ParameterInfo.Name == this.ParameterName)
            {
                var newCommand =
                    this.guardClauseExtensions.CreateExtendedCommand(this.specimenBuilder, command);
                foreach (var value in this.Values)
                {
                    var expectedExceptionThrown = false;
                    try
                    {
                        newCommand.Execute(value);
                    }
                    catch (Exception ex) when (ex.GetType() == typeof(T))
                    {
                        expectedExceptionThrown = true;
                    }
                    catch (Exception ex)
                    {
                        throw newCommand.CreateException(Invariant($"\"{value.ToString()}\""), ex);
                    }

                    if (!expectedExceptionThrown)
                    {
                        throw newCommand.CreateException(Invariant($"\"{value.ToString()}\""));
                    }
                }
            }
        }
    }
}