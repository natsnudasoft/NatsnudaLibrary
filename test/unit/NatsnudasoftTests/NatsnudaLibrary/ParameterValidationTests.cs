// <copyright file="ParameterValidationTests.cs" company="natsnudasoft">
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

namespace Natsnudasoft.NatsnudasoftTests.NatsnudaLibrary
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using SutAlias = Natsnudasoft.NatsnudaLibrary.ParameterValidation;

    public sealed class ParameterValidationTests
    {
        [Fact]
        public void IsNotNullWithNullValueThrows()
        {
            const string NullValue = null;
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotNull(NullValue, ParamName));

            Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(ParamName, ((ArgumentNullException)ex).ParamName);
        }

        [Fact]
        public void IsNotNullWithValueDoesNotThrow()
        {
            const string NotNullString = "value";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotNull(NotNullString, ParamName));

            Assert.Null(ex);
        }

        [Fact]
        public void IsNotNullWithNullableNoValueThrows()
        {
            int? nullableNoValue = null;
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotNull(nullableNoValue, ParamName));

            Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal(ParamName, ((ArgumentNullException)ex).ParamName);
        }

        [Fact]
        public void IsNotNullWithNullableValueDoesNotThrow()
        {
            int? nullableValue = 5;
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotNull(nullableValue, ParamName));

            Assert.Null(ex);
        }

        [Fact]
        public void IsNotEmptyWithEmptyValueThrows()
        {
            const string EmptyString = "";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotEmpty(EmptyString, ParamName));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ParamName, ((ArgumentException)ex).ParamName);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(null)]
        public void IsNotEmptyWithAllowedValueDoesNotThrow(string value)
        {
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsNotEmpty(value, ParamName));

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("t", "t", false)]
        [InlineData("r", "t", false)]
        [InlineData(null, "t", false)]
        [InlineData(null, null, false)]
        [InlineData("t", "t", true)]
        [InlineData("s", "t", true)]
        [InlineData(null, "t", true)]
        [InlineData(null, null, true)]
        public void IsGreaterThanWithDisallowedValueThrows(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, compareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("u", "t", false)]
        [InlineData("u", null, false)]
        [InlineData("v", "t", true)]
        [InlineData("u", null, true)]
        public void IsGreaterThanWithAllowedValueDoesNotThrow(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, compareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("t", "t", false)]
        [InlineData("u", "t", false)]
        [InlineData("t", null, false)]
        [InlineData(null, null, false)]
        [InlineData("t", "t", true)]
        [InlineData("u", "t", true)]
        [InlineData("t", null, true)]
        [InlineData(null, null, true)]
        public void IsLessThanWithDisallowedValueThrows(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, compareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", "t", false)]
        [InlineData(null, "t", false)]
        [InlineData("s", "t", true)]
        [InlineData(null, "t", true)]
        public void IsLessThanWithAllowedValueDoesNotThrow(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, compareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("s", "t", false)]
        [InlineData(null, "t", false)]
        [InlineData("s", "t", true)]
        [InlineData(null, "t", true)]
        public void IsGreaterThanOrEqualToWithDisallowedValueThrows(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsGreaterThanOrEqualTo(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThanOrEqualTo(value, compareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("t", "t", false)]
        [InlineData("u", "t", false)]
        [InlineData("u", null, false)]
        [InlineData(null, null, false)]
        [InlineData("t", "t", true)]
        [InlineData("u", "t", true)]
        [InlineData("u", null, true)]
        [InlineData(null, null, true)]
        public void IsGreaterThanOrEqualToWithAllowedValueDoesNotThrow(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsGreaterThanOrEqualTo(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThanOrEqualTo(value, compareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("u", "t", false)]
        [InlineData("u", null, false)]
        [InlineData("u", "t", true)]
        [InlineData("u", null, true)]
        public void IsLessThanOrEqualToWithDisallowedValueThrows(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, compareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", "t", false)]
        [InlineData("t", "t", false)]
        [InlineData(null, "t", false)]
        [InlineData(null, null, false)]
        [InlineData("s", "t", true)]
        [InlineData("t", "t", true)]
        [InlineData(null, "t", true)]
        [InlineData(null, null, true)]
        public void IsLessThanOrEqualToWithAllowedValueDoesNotThrow(
            string value,
            string compareValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, compareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, compareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("s", "s", "u", false)]
        [InlineData("u", "s", "u", false)]
        [InlineData("r", "s", "u", false)]
        [InlineData("v", "s", "u", false)]
        [InlineData(null, "s", "u", false)]
        [InlineData(null, null, null, false)]
        [InlineData("s", "s", "u", true)]
        [InlineData("u", "s", "u", true)]
        [InlineData("r", "s", "u", true)]
        [InlineData("v", "s", "u", true)]
        [InlineData(null, "s", "u", true)]
        [InlineData(null, null, null, true)]
        public void IsBetweenWithDisallowedValueThrows(
            string value,
            string minValue,
            string maxValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, minValue, maxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, minValue, maxValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("t", "s", "u", false)]
        [InlineData("t", "s", "u", true)]
        public void IsBetweenWithAllowedValueDoesNotThrow(
            string value,
            string minValue,
            string maxValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, minValue, maxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, minValue, maxValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData("r", "s", "u", false)]
        [InlineData("v", "s", "u", false)]
        [InlineData(null, "s", "u", false)]
        [InlineData("t", "t", null, false)]
        [InlineData("r", "s", "u", true)]
        [InlineData("v", "s", "u", true)]
        [InlineData(null, "s", "u", true)]
        [InlineData("t", "t", null, true)]
        public void IsBetweenInclusiveWithDisallowedValueThrows(
            string value,
            string minValue,
            string maxValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsBetweenInclusive(value, minValue, maxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetweenInclusive(value, minValue, maxValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", "s", "u", false)]
        [InlineData("t", "s", "u", false)]
        [InlineData("u", "s", "u", false)]
        [InlineData("t", null, "t", false)]
        [InlineData(null, null, null, false)]
        [InlineData("s", "s", "u", true)]
        [InlineData("t", "s", "u", true)]
        [InlineData("u", "s", "u", true)]
        [InlineData("t", null, "t", true)]
        [InlineData(null, null, null, true)]
        public void IsBetweenInclusiveWithAllowedValueDoesNotThrow(
            string value,
            string minValue,
            string maxValue,
            bool useComparer)
        {
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsBetweenInclusive(value, minValue, maxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetweenInclusive(value, minValue, maxValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Fact]
        public void IsTrueWithFalseValueThrows()
        {
            const bool Value = false;
            const string Message = "This is a message.";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsTrue(Value, Message, ParamName));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ParamName, ((ArgumentException)ex).ParamName);
        }

        [Fact]
        public void IsTrueWithTrueValueDoesNowThrow()
        {
            const bool Value = true;
            const string Message = "This is a message.";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsTrue(Value, Message, ParamName));

            Assert.Null(ex);
        }

        [Fact]
        public void IsFalseWithTrueValueThrows()
        {
            const bool Value = true;
            const string Message = "This is a message.";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsFalse(Value, Message, ParamName));

            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ParamName, ((ArgumentException)ex).ParamName);
        }

        [Fact]
        public void IsFalseWithFalseValueDoesNotThrow()
        {
            const bool Value = false;
            const string Message = "This is a message.";
            const string ParamName = "testParam";
            var ex = Record.Exception(
                () => SutAlias.IsFalse(Value, Message, ParamName));

            Assert.Null(ex);
        }

        private static IComparer<string> GetStringComparer()
        {
            return StringComparer.Ordinal;
        }
    }
}
