// <copyright file="EqualsOverrideAssertion.cs" company="natsnudasoft">
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
    using System.Linq;
    using System.Reflection;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;

    /// <summary>
    /// Provides an abstract base class for encapsulating unit tests that verify whether an
    /// implementation of an Equals method is correctly implemented.
    /// </summary>
    /// <seealso cref="IdiomaticAssertion" />
    public abstract class EqualsOverrideAssertion : IdiomaticAssertion
    {
        private const int DefaultSuccessiveCount = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideAssertion"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/> is
        /// <see langword="null"/>.</exception>
        protected EqualsOverrideAssertion(ISpecimenBuilder specimenBuilder)
            : this(specimenBuilder, DefaultSuccessiveCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideAssertion"/> class.
        /// </summary>
        /// <param name="specimenBuilder">The anonymous object creation service to use to create new
        /// specimens.</param>
        /// <param name="successiveCount">The number of invocations to test on an Equals override
        /// for successive contract tests.</param>
        /// <exception cref="ArgumentNullException"><paramref name="specimenBuilder"/> is
        /// <see langword="null"/>.</exception>
        protected EqualsOverrideAssertion(ISpecimenBuilder specimenBuilder, int successiveCount)
        {
            ParameterValidation.IsNotNull(specimenBuilder, nameof(specimenBuilder));

            this.SpecimenBuilder = specimenBuilder;
            this.SuccessiveCount = successiveCount;
        }

        /// <summary>
        /// Gets the anonymous object creation service.
        /// </summary>
        public ISpecimenBuilder SpecimenBuilder { get; }

        /// <summary>
        /// Gets the number of invocations to test on an Equals override for successive contract
        /// tests.
        /// </summary>
        protected int SuccessiveCount { get; }

        /// <summary>
        /// Verifies the assertion of this <see cref="EqualsOverrideAssertion"/> on the specified
        /// method if it is an Equals method.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to perform the verification on.
        /// </param>
        public sealed override void Verify(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            if (IsEqualsMethod(methodInfo))
            {
                this.VerifyEqualsMethod(methodInfo);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> represents a valid Equals
        /// method implementation.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="MethodInfo"/> is an Equals method;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        protected static bool IsEqualsMethod(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            var ownerType = methodInfo.ReflectedType;
            var parameters = methodInfo.GetParameters();
            return ownerType != null &&
                methodInfo.Name == nameof(object.Equals) &&
                methodInfo.ReturnType == typeof(bool) &&
                parameters.Length == 1;
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> represents an Equals method
        /// implementation with a parameter type of <see cref="object"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="MethodInfo"/> is an Equals method
        /// with a parameter type of <see cref="object"/>; otherwise, <see langword="false"/>.
        /// </returns>
        protected static bool IsEqualsParameterObjectType(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            var parameterType = methodInfo.GetParameters().First().ParameterType;
            return parameterType == typeof(object);
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> represents an Equals method
        /// implementation with a parameter type that can be assigned <see langword="null"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to check.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="MethodInfo"/> is an Equals method
        /// with a parameter type that can be assigned <see langword="null"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        protected static bool IsEqualsParameterNullableType(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            var parameterType = methodInfo.GetParameters().First().ParameterType;
            return parameterType.IsClass ||
                parameterType.IsInterface ||
                Nullable.GetUnderlyingType(parameterType) != null;
        }

        /// <summary>
        /// Verifies the assertion of this <see cref="EqualsOverrideAssertion"/> on the specified
        /// Equals method.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> to perform the verification on.
        /// </param>
        protected abstract void VerifyEqualsMethod(MethodInfo methodInfo);

        /// <summary>
        /// Creates a specimen of the specified type using the owned
        /// <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the specimen to create.</param>
        /// <returns>An instance of the specified <see cref="Type"/> created by the owned
        /// <see cref="ISpecimenBuilder"/>.</returns>
        protected object CreateSpecimen(Type type)
        {
            var context = new SpecimenContext(this.SpecimenBuilder);
            return context.Resolve(type);
        }
    }
}