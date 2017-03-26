// <copyright file="EqualsOverrideTheory.cs" company="natsnudasoft">
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

    /// <summary>
    /// Provides a class to encapsulate a theory describing the expected result of an Equals method
    /// override when called on the specified object with the specified argument.
    /// </summary>
    public sealed class EqualsOverrideTheory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualsOverrideTheory"/> class.
        /// </summary>
        /// <param name="left">The object that the Equals method will be called on.</param>
        /// <param name="right">The object that will be provided as an argument to the Equals
        /// method.</param>
        /// <param name="expectedResult">The expected result of the Equals method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/>, or
        /// <paramref name="right"/> is <see langword="null"/>.</exception>
        public EqualsOverrideTheory(object left, object right, bool expectedResult)
        {
            ParameterValidation.IsNotNull(left, nameof(left));
            ParameterValidation.IsNotNull(right, nameof(right));

            this.Left = left;
            this.Right = right;
            this.ExpectedResult = expectedResult;
        }

        /// <summary>
        /// Gets the object that the Equals method will be called on.
        /// </summary>
        public object Left { get; }

        /// <summary>
        /// Gets the object that will be provided as an argument to the Equals method.
        /// </summary>
        public object Right { get; }

        /// <summary>
        /// Gets a value indicating whether the Equals method should return <see langword="true"/>
        /// or <see langword="false"/>.
        /// </summary>
        public bool ExpectedResult { get; }
    }
}
