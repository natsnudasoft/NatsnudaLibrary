﻿// <copyright file="ParameterCustomization.cs" company="natsnudasoft">
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
    using AutoFixture;
    using Natsnudasoft.NatsnudaLibrary;

    /// <summary>
    /// Provides a customization that will apply a known value to a parameter in a constructor.
    /// </summary>
    public sealed class ParameterCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCustomization"/> class.
        /// </summary>
        /// <param name="declaringType">The <see cref="Type"/> of the object that contains the
        /// parameter to apply a known value to.</param>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="declaringType"/>, or
        /// <paramref name="parameterName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        public ParameterCustomization(
            Type declaringType,
            string parameterName)
        {
            ParameterValidation.IsNotNull(declaringType, nameof(declaringType));
            ParameterValidation.IsNotNull(parameterName, nameof(parameterName));
            ParameterValidation.IsNotEmpty(parameterName, nameof(parameterName));

            this.DeclaringType = declaringType;
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCustomization"/> class.
        /// </summary>
        /// <param name="declaringType">The <see cref="Type"/> of the object that contains the
        /// parameter to apply a known value to.</param>
        /// <param name="parameterName">The name of the parameter to apply a known value to.</param>
        /// <param name="specimenValue">The value to apply to the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="declaringType"/>,
        /// <paramref name="parameterName"/>, or <paramref name="specimenValue"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterName"/> is empty.
        /// </exception>
        public ParameterCustomization(
            Type declaringType,
            string parameterName,
            object specimenValue)
        {
            ParameterValidation.IsNotNull(declaringType, nameof(declaringType));
            ParameterValidation.IsNotNull(parameterName, nameof(parameterName));
            ParameterValidation.IsNotEmpty(parameterName, nameof(parameterName));
            ParameterValidation.IsNotNull(specimenValue, nameof(specimenValue));

            this.DeclaringType = declaringType;
            this.ParameterName = parameterName;
            this.SpecimenValue = specimenValue;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the object that contains the parameter to apply a known
        /// value to.
        /// </summary>
        public Type DeclaringType { get; }

        /// <summary>
        /// Gets the name of the parameter to apply a known value to.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the value to apply to the parameter.
        /// </summary>
        public object SpecimenValue { get; }

        /// <summary>
        /// Customises the specified fixture with this customisation.
        /// </summary>
        /// <param name="fixture">The fixture to customise.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fixture"/> is
        /// <see langword="null"/>.</exception>
        public void Customize(IFixture fixture)
        {
            ParameterValidation.IsNotNull(fixture, nameof(fixture));

            if (this.SpecimenValue == null)
            {
                fixture.Customizations.Add(new ParameterSpecimenBuilder(
                    this.DeclaringType,
                    this.ParameterName));
            }
            else
            {
                fixture.Customizations.Add(new ParameterSpecimenBuilder(
                    this.DeclaringType,
                    this.ParameterName,
                    this.SpecimenValue));
            }
        }
    }
}