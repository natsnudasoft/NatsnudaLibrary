// <copyright file="ParameterAttributeTests.cs" company="natsnudasoft">
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
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Xunit;

    public sealed class ParameterAttributeTests
    {
        private static readonly Type SutType = typeof(ParameterAttribute);

        [Fact]
        public void ConstructorHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetConstructors());
        }

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers()
        {
            var fixture = new Fixture();
            var assertion = new ConstructorInitializedMemberAssertion(fixture);

            assertion.Verify(
                SutType.GetProperty(nameof(ParameterAttribute.ParameterName)),
                SutType.GetProperty(nameof(ParameterAttribute.SpecimenValue)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new ParameterAttribute(fixture.Create<string>()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() =>
                new ParameterAttribute(fixture.Create<string>(), fixture.Create<int>()));

            Assert.Null(ex);
        }

        [Fact]
        public void GetCustomizationHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            var testMethod = typeof(ParameterAttributeTests).GetMethod(
                nameof(MethodWithParameterTestStub),
                BindingFlags.Static | BindingFlags.NonPublic);
            fixture.Inject(testMethod.GetParameters().First());
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetMethod(nameof(ParameterAttribute.GetCustomization)));
        }

        [Fact]
        public void GetCustomizationWithoutSpecimenValueReturnsCustomization()
        {
            var fixture = new Fixture();
            var sut = new ParameterAttribute(fixture.Create<string>());

            var testMethod = typeof(ParameterAttributeTests).GetMethod(
                nameof(MethodWithParameterTestStub),
                BindingFlags.Static | BindingFlags.NonPublic);
            var customization = sut.GetCustomization(testMethod.GetParameters().First());

            Assert.NotNull(customization);
        }

        [Fact]
        public void GetCustomizationWithSpecimenValueReturnsCustomization()
        {
            var fixture = new Fixture();
            var sut = new ParameterAttribute(fixture.Create<string>(), fixture.Create<int>());

            var testMethod = typeof(ParameterAttributeTests).GetMethod(
                nameof(MethodWithParameterTestStub),
                BindingFlags.Static | BindingFlags.NonPublic);
            var customization = sut.GetCustomization(testMethod.GetParameters().First());

            Assert.NotNull(customization);
        }

        private static Guid MethodWithParameterTestStub(Guid guidParameter)
        {
            return guidParameter;
        }
    }
}
