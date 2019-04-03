// <copyright file="ParameterTypeFavoringConstructorQueryTests.cs" company="natsnudasoft">
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

namespace Natsnudasoft.NatsnudasoftTests.TestExtension
{
    using System;
    using System.Linq;
    using AutoFixture;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;
    using Xunit;
    using SutAlias =
        Natsnudasoft.NatsnudaLibrary.TestExtensions.ParameterTypeFavoringConstructorQuery;

    public sealed class ParameterTypeFavoringConstructorQueryTests
    {
        private static readonly Type SutType = typeof(SutAlias);

        [Fact]
        public void ConstructorHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetConstructors());
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutAlias(fixture.Create<Type[]>()));

            Assert.Null(ex);
        }

        [Fact]
        public void SelectMethodsHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(nameof(SutAlias.SelectMethods)));
        }

        [Theory]
        [InlineData]
        [InlineData(typeof(string))]
        public void SelectMethodsConstructorNotFoundThrows(params Type[] parameterTypes)
        {
            var sut = new SutAlias(parameterTypes);

            var ex = Record.Exception(() => sut.SelectMethods(typeof(SelectConstructorTestClass)));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void SelectMethodsConstructorFoundReturnsSingleConstructor()
        {
            var expected = typeof(SelectConstructorTestClass)
                .GetConstructor(new Type[] { typeof(int) });
            var sut = new SutAlias(typeof(int));

            var actual = sut.SelectMethods(typeof(SelectConstructorTestClass));

            Assert.Single(actual);
            Assert.IsType<ConstructorMethod>(actual.First());
            Assert.Equal(expected, ((ConstructorMethod)actual.First()).Constructor);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "Class is used via reflection.")]
        private sealed class SelectConstructorTestClass
        {
            public SelectConstructorTestClass(int intParameter)
            {
                this.IntProperty = intParameter;
            }

            private SelectConstructorTestClass()
            {
            }

            public int IntProperty { get; }
        }
    }
}