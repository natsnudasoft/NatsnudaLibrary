// <copyright file="EqualsOverrideNullAssertion.cs" company="natsnudasoft">
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
    using System.Reflection;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;

    /// <summary>
    /// Encapsulates a unit test that verifies whether an implementation of an Equals method is
    /// correctly implemented with regards to <c>x.Equals(<see langword="null"/>)</c> returns
    /// <see langword="false"/>.
    /// </summary>
    /// <seealso cref="EqualsOverrideAssertion" />
    public sealed class EqualsOverrideNullAssertion : EqualsOverrideAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideNullAssertion"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/> is
        /// <see langword="null"/>.</exception>
        public EqualsOverrideNullAssertion(ISpecimenBuilder specimenBuilder)
            : base(specimenBuilder)
        {
        }

        /// <summary>
        /// Verifies that the specified Equals method returns <see langword="false"/> when provided
        /// with a parameter value of <see langword="null"/>.
        /// </summary>
        /// <param name="methodInfo">The method to verify.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="EqualsOverrideException">The specified Equals method did not return
        /// <see langword="false"/>.</exception>
        protected override void VerifyEqualsMethod(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            if (IsEqualsParameterNullableType(methodInfo))
            {
                var owner = this.CreateSpecimen(methodInfo.ReflectedType);
                if ((bool)methodInfo.Invoke(owner, new object[] { null }))
                {
                    throw new EqualsOverrideException(string.Format(
                        CultureInfo.InvariantCulture,
                        "The type {0} not correctly implement x.Equals(null). Should return false.",
                        methodInfo.ReflectedType.Name));
                }
            }
        }
    }
}