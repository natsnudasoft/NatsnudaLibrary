// <copyright file="EqualsOverrideSelfAssertion.cs" company="natsnudasoft">
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
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;

    /// <summary>
    /// Encapsulates a unit test that verifies whether an implementation of an Equals method is
    /// correctly implemented with regards to <c>x.Equals(x)</c> returns <see langword="true"/>.
    /// </summary>
    /// <seealso cref="EqualsOverrideAssertion" />
    public sealed class EqualsOverrideSelfAssertion : EqualsOverrideAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideSelfAssertion"/>
        /// class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/> is
        /// <see langword="null"/>.</exception>
        public EqualsOverrideSelfAssertion(ISpecimenBuilder specimenBuilder)
            : base(specimenBuilder)
        {
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

            var owner = this.CreateSpecimen(methodInfo.ReflectedType);
            if (!(bool)methodInfo.Invoke(owner, new object[] { owner }))
            {
                throw new EqualsOverrideException(string.Format(
                    CultureInfo.InvariantCulture,
                    "The type {0} not correctly implement x.Equals(x). Should return true.",
                    methodInfo.ReflectedType.Name));
            }
        }
    }
}