// <copyright file="ParameterCustomizationTests.cs" company="natsnudasoft">
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
    using System.Collections.Generic;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class ParameterCustomizationTests
    {
        private static readonly Type SutType = typeof(ParameterCustomization);

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
            var ex = Record.Exception(() => new ParameterCustomization(
                typeof(ParameterCustomizationTests),
                string.Empty));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Constructor2WithEmptyParameterNameThrows()
        {
            var fixture = new Fixture();

            var ex = Record.Exception(() => new ParameterCustomization(
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
                SutType.GetProperty(nameof(ParameterCustomization.DeclaringType)),
                SutType.GetProperty(nameof(ParameterCustomization.ParameterName)),
                SutType.GetProperty(nameof(ParameterCustomization.SpecimenValue)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new ParameterCustomization(
                typeof(ParameterCustomizationTests),
                fixture.Create<string>()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new ParameterCustomization(
                typeof(ParameterCustomizationTests),
                fixture.Create<string>(),
                fixture.Create<int>()));

            Assert.Null(ex);
        }

        [Fact]
        public void CustomizeWithoutSpecimenValueAddsCustomizationToFixture()
        {
            var fixture = new Fixture();
            var fixtureMock = new Mock<IFixture>();
            var customizationsMock = new Mock<IList<ISpecimenBuilder>>();
            var declaringType = typeof(ParameterCustomizationTests);
            var parameterName = fixture.Create<string>();
            var sut = new ParameterCustomization(declaringType, parameterName);
            fixtureMock.SetupGet(f => f.Customizations).Returns(customizationsMock.Object);

            sut.Customize(fixtureMock.Object);

            customizationsMock.Verify(
                c => c.Add(It.Is<ParameterSpecimenBuilder>(p =>
                    p.DeclaringType == declaringType &&
                    p.ParameterName == parameterName &&
                    p.SpecimenValue == null)),
                Times.Once());
        }

        [Fact]
        public void CustomizeWithSpecimenValueAddsCustomizationToFixture()
        {
            var fixture = new Fixture();
            var fixtureMock = new Mock<IFixture>();
            var customizationsMock = new Mock<IList<ISpecimenBuilder>>();
            var declaringType = typeof(ParameterCustomizationTests);
            var parameterName = fixture.Create<string>();
            object specimenValue = fixture.Create<int>();
            var sut = new ParameterCustomization(declaringType, parameterName, specimenValue);
            fixtureMock.SetupGet(f => f.Customizations).Returns(customizationsMock.Object);

            sut.Customize(fixtureMock.Object);

            customizationsMock.Verify(
                c => c.Add(It.Is<ParameterSpecimenBuilder>(p =>
                    p.DeclaringType == declaringType &&
                    p.ParameterName == parameterName &&
                    p.SpecimenValue == specimenValue)),
                Times.Once());
        }
    }
}
