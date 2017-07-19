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
        [InlineData(10, false)]
        [InlineData(15, false)]
        [InlineData(13, true)]
        [InlineData(15, true)]
        public void IsGreaterThanWithDisallowedValueThrows(int value, bool useComparer)
        {
            const int CompareValue = 15;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, CompareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("u", false)]
        [InlineData(null, false)]
        [InlineData("v", true)]
        public void IsGreaterThanWithAllowedValueDoesNotThrow(string value, bool useComparer)
        {
            const string CompareValue = "t";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThan(value, CompareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(40, false)]
        [InlineData(30, false)]
        [InlineData(35, true)]
        [InlineData(30, true)]
        public void IsLessThanWithDisallowedValueThrows(int value, bool useComparer)
        {
            const int CompareValue = 30;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, CompareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", false)]
        [InlineData(null, false)]
        [InlineData("r", true)]
        public void IsLessThanWithAllowedValueDoesNotThrow(string value, bool useComparer)
        {
            const string CompareValue = "t";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThan(value, CompareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsGreaterThanOrEqualToWithDisallowedValueThrows(bool useComparer)
        {
            const int Value = 10;
            const int CompareValue = 15;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(()
                    => SutAlias.IsGreaterThanOrEqualTo(Value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThanOrEqualTo(Value, CompareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("t", false)]
        [InlineData("u", false)]
        [InlineData(null, false)]
        [InlineData("t", true)]
        [InlineData("v", true)]
        public void IsGreaterThanOrEqualToWithAllowedValueDoesNotThrow(
            string value,
            bool useComparer)
        {
            const string CompareValue = "t";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsGreaterThanOrEqualTo(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsGreaterThanOrEqualTo(value, CompareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsLessThanOrEqualToWithDisallowedValueThrows(bool useComparer)
        {
            const int Value = 67;
            const int CompareValue = 25;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(Value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(Value, CompareValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", false)]
        [InlineData("t", false)]
        [InlineData(null, false)]
        [InlineData("r", true)]
        [InlineData("t", true)]
        public void IsLessThanOrEqualToWithAllowedValueDoesNotThrow(string value, bool useComparer)
        {
            const string CompareValue = "t";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, CompareValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsLessThanOrEqualTo(value, CompareValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(102, false)]
        [InlineData(5, false)]
        [InlineData(10, false)]
        [InlineData(50, false)]
        [InlineData(89, true)]
        [InlineData(7, true)]
        [InlineData(10, true)]
        [InlineData(50, true)]
        public void IsBetweenWithDisallowedValueThrows(int value, bool useComparer)
        {
            const int MinValue = 10;
            const int MaxValue = 50;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, MinValue, MaxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, MinValue, MaxValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("t", false)]
        [InlineData(null, false)]
        [InlineData("t", true)]
        public void IsBetweenWithAllowedValueDoesNotThrow(string value, bool useComparer)
        {
            const string MinValue = "s";
            const string MaxValue = "u";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, MinValue, MaxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetween(value, MinValue, MaxValue, ParamName));
            }

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(102, false)]
        [InlineData(5, false)]
        [InlineData(80, true)]
        [InlineData(6, true)]
        public void IsBetweenInclusiveWithDisallowedValueThrows(int value, bool useComparer)
        {
            const int MinValue = 10;
            const int MaxValue = 50;
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetIntComparer();
                ex = Record.Exception(()
                    => SutAlias.IsBetweenInclusive(value, MinValue, MaxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetweenInclusive(value, MinValue, MaxValue, ParamName));
            }

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.Equal(ParamName, ((ArgumentOutOfRangeException)ex).ParamName);
        }

        [Theory]
        [InlineData("s", false)]
        [InlineData("t", false)]
        [InlineData("u", false)]
        [InlineData(null, false)]
        [InlineData("s", true)]
        [InlineData("t", true)]
        [InlineData("u", true)]
        public void IsBetweenInclusiveWithAllowedValueDoesNotThrow(string value, bool useComparer)
        {
            const string MinValue = "s";
            const string MaxValue = "u";
            const string ParamName = "testParam";
            Exception ex;
            if (useComparer)
            {
                var comparer = GetStringComparer();
                ex = Record.Exception(()
                    => SutAlias.IsBetweenInclusive(value, MinValue, MaxValue, ParamName, comparer));
            }
            else
            {
                ex = Record.Exception(
                    () => SutAlias.IsBetweenInclusive(value, MinValue, MaxValue, ParamName));
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

        private static IComparer<int> GetIntComparer()
        {
            return Comparer<int>.Create((l, r) => l.CompareTo(r));
        }

        private static IComparer<string> GetStringComparer()
        {
            return StringComparer.Ordinal;
        }
    }
}
