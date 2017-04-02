// <copyright file="ParameterSpecimenBuilderTests.cs" company="natsnudasoft">
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
    using System.Reflection;
    using Moq;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;
    using SutAlias = Natsnudasoft.NatsnudaLibrary.TestExtensions.ParameterSpecimenBuilder;

    public sealed class ParameterSpecimenBuilderTests
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
        public void ConstructorWithEmptyParameterNameThrows()
        {
            var ex = Record.Exception(() => new SutAlias(
                typeof(ParameterCustomizationTests),
                string.Empty));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Constructor2WithEmptyParameterNameThrows()
        {
            var fixture = new Fixture();

            var ex = Record.Exception(() => new SutAlias(
                typeof(ParameterCustomizationTests),
                string.Empty,
                fixture.Create<int>()));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers()
        {
            var fixture = new Fixture();
            var assertion = new ConstructorInitializedMemberAssertion(fixture);

            assertion.Verify(
                SutType.GetProperty(nameof(SutAlias.DeclaringType)),
                SutType.GetProperty(nameof(SutAlias.ParameterName)),
                SutType.GetProperty(nameof(SutAlias.SpecimenValue)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutAlias(
                typeof(SutAlias),
                fixture.Create<string>()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutAlias(
                typeof(SutAlias),
                fixture.Create<string>(),
                fixture.Create<int>()));

            Assert.Null(ex);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Hello")]
        public void CreateWithNonParameterInfoRequestReturnsNoSpecimen(object request)
        {
            var fixture = new Fixture();
            var contextMock = new Mock<ISpecimenContext>();
            var sut = new SutAlias(
                typeof(ParameterSpecimenBuilderTests),
                fixture.Create<string>());

            var actual = sut.Create(request, contextMock.Object);

            Assert.IsType<NoSpecimen>(actual);
            Assert.NotSame(sut.SpecimenValue, actual);
        }

        [Theory]
        [InlineData(typeof(string), "stringParameter", "Hello")]
        [InlineData(typeof(ParameterSpecimenBuilderTests), "wrongParameter", "Hello")]
        [InlineData(typeof(ParameterSpecimenBuilderTests), "stringParameter", 50)]
        public void CreateWithRequestThatDoesNotMatchParametersReturnsNoSpecimen(
            Type declaringType,
            string parameterName,
            object specimenValue)
        {
            var parameterInfo = typeof(ParameterSpecimenBuilderTests)
                .GetMethod(nameof(TestMethodStub), BindingFlags.Static | BindingFlags.NonPublic)
                .GetParameters()
                .First();
            var contextMock = new Mock<ISpecimenContext>();
            var sut = new SutAlias(
                declaringType,
                parameterName,
                specimenValue);

            var actual = sut.Create(parameterInfo, contextMock.Object);

            Assert.IsType<NoSpecimen>(actual);
            Assert.NotSame(sut.SpecimenValue, actual);
        }

        [Fact]
        public void CreateWithRequestThatMatchesParametersReturnsSpecimenValue()
        {
            var fixture = new Fixture();
            var specimenValue = fixture.Create<string>();
            var parameterInfo = typeof(ParameterSpecimenBuilderTests)
                .GetMethod(nameof(TestMethodStub), BindingFlags.Static | BindingFlags.NonPublic)
                .GetParameters()
                .First();
            var contextMock = new Mock<ISpecimenContext>();
            var sut = new SutAlias(
                typeof(ParameterSpecimenBuilderTests),
                parameterInfo.Name,
                specimenValue);

            var actual = sut.Create(parameterInfo, contextMock.Object);

            Assert.Equal(specimenValue, actual);
        }

        private static string TestMethodStub(string stringParameter)
        {
            return stringParameter;
        }
    }
}
