// <copyright file="EqualsOverrideTheoriesSuccessiveAssertion.cs" company="natsnudasoft">
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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;

    /// <summary>
    /// Encapsulates a unit test that verifies whether an implementation of an Equals method is
    /// correctly implemented with regards to a collection of instances of
    /// <see cref="EqualsOverrideTheory"/>.
    /// </summary>
    /// <seealso cref="EqualsOverrideAssertion" />
    public sealed class EqualsOverrideTheoriesSuccessiveAssertion : EqualsOverrideAssertion
    {
        private readonly EqualsOverrideTheory[] equalsOverrideTheories;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EqualsOverrideTheoriesSuccessiveAssertion"/>
        /// class.
        /// </summary>
        /// <remarks>
        /// This overload is provided so AutoFixture does not provide specimens for instances of
        /// <see cref="EqualsOverrideTheory"/>.
        /// </remarks>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        public EqualsOverrideTheoriesSuccessiveAssertion(ISpecimenBuilder specimenBuilder)
            : base(specimenBuilder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EqualsOverrideTheoriesSuccessiveAssertion"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        /// <param name="equalsOverrideTheories">The collection of instances of
        /// <see cref="EqualsOverrideTheory"/> that Equals methods will be checked with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/>, or
        /// <paramref name="equalsOverrideTheories"/> is <see langword="null"/>.</exception>
        public EqualsOverrideTheoriesSuccessiveAssertion(
            ISpecimenBuilder specimenBuilder,
            params EqualsOverrideTheory[] equalsOverrideTheories)
            : base(specimenBuilder)
        {
            ParameterValidation.IsNotNull(equalsOverrideTheories, nameof(equalsOverrideTheories));

            this.equalsOverrideTheories = equalsOverrideTheories;
        }

        /// <summary>
        /// Verifies that the specified Equals method returns <see langword="true"/> when provided
        /// with a parameter that is the owner of the Equals method.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="EqualsOverrideException">The specified Equals method did not return
        /// <see langword="true"/>.</exception>
        protected override void VerifyEqualsMethod(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            for (int i = 0; i < this.equalsOverrideTheories.Length; ++i)
            {
                var equalsOverrideTheory = this.equalsOverrideTheories[i];
                var left = equalsOverrideTheory.Left;
                var right = equalsOverrideTheory.Right;
                if (left.GetType() != methodInfo.ReflectedType)
                {
                    const string formatString = "The EqualsOverrideTheory at index {1} has an " +
                        "incorrect Left type. Must match the type of the owner method {0}.";
                    throw new EqualsOverrideException(string.Format(
                        CultureInfo.InvariantCulture,
                        formatString,
                        methodInfo.ReflectedType.Name,
                        i));
                }
                else if (methodInfo.GetParameters().Single().ParameterType.IsInstanceOfType(right))
                {
                    var expectedResult = equalsOverrideTheory.ExpectedResult;
                    var resultsMatchExpected = Enumerable.Range(0, this.SuccessiveCount)
                        .Select(j => (bool)methodInfo.Invoke(left, new object[] { right }))
                        .All(b => b == expectedResult);
                    if (!resultsMatchExpected)
                    {
                        const string formatString = "The type {0} does not correctly implement " +
                            "the EqualsOverrideTheory at index {1}. Should return expected " +
                            "result when calling multiple times.";
                        throw new EqualsOverrideException(string.Format(
                            CultureInfo.InvariantCulture,
                            formatString,
                            methodInfo.ReflectedType.Name,
                            i));
                    }
                }
            }
        }
    }
}