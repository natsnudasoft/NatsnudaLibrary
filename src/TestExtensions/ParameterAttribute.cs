// <copyright file="ParameterAttribute.cs" company="natsnudasoft">
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
    using NatsnudaLibrary;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    /// <summary>
    /// Provides a customization attribute that will apply a known value to a parameter in a
    /// constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class ParameterAttribute : CustomizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        public ParameterAttribute(string parameterName)
        {
            ParameterValidation.IsNotNull(parameterName, nameof(parameterName));
            ParameterValidation.IsNotEmpty(parameterName, nameof(parameterName));

            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <param name="specimenValue">The value to apply to the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName"/>, or
        /// <paramref name="specimenValue"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        public ParameterAttribute(string parameterName, object specimenValue)
        {
            ParameterValidation.IsNotNull(parameterName, nameof(parameterName));
            ParameterValidation.IsNotEmpty(parameterName, nameof(parameterName));
            ParameterValidation.IsNotNull(specimenValue, nameof(specimenValue));

            this.ParameterName = parameterName;
            this.SpecimenValue = specimenValue;
        }

        /// <summary>
        /// Gets the name of the parameter to apply a known value to.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the value to apply to the parameter.
        /// </summary>
        public object SpecimenValue { get; }

        /// <summary>
        /// Gets a customization that applies a known value to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter that the known value will be applied to.</param>
        /// <returns>The customization that will apply a known value to a parameter.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameter"/> is
        /// <see langword="null"/>.</exception>
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            ParameterValidation.IsNotNull(parameter, nameof(parameter));

            ICustomization customization;
            if (this.SpecimenValue == null)
            {
                customization = new ParameterCustomization(
                    parameter.ParameterType,
                    this.ParameterName);
            }
            else
            {
                customization = new ParameterCustomization(
                    parameter.ParameterType,
                    this.ParameterName,
                    this.SpecimenValue);
            }

            return customization;
        }
    }
}
