// <copyright file="EqualsOverrideAssertionTests.cs" company="natsnudasoft">
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
    using System.Reflection;
    using AutoFixture;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;
    using Moq;
    using Moq.Protected;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Natsnudasoft.NatsnudasoftTests.Helper;
    using Xunit;

    public sealed class EqualsOverrideAssertionTests
    {
        private static readonly Type SutType = typeof(SutStub);
        private static readonly Type TestHelperType = typeof(EqualsOverrideAssertionVerifyHelper);

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
            const BindingFlags nonPublicFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            assertion.Verify(
                SutType.GetProperty(nameof(EqualsOverrideAssertion.SpecimenBuilder)),
                SutType.GetProperty("SuccessiveCount", nonPublicFlags));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new SutStub(
                new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutStub(
                new Mock<ISpecimenBuilder>().Object,
                fixture.Create<int>()));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyMethodsHaveCorrectGuardClauses()
        {
            const string VerifyMethodName = nameof(EqualsOverrideAssertion.Verify);
            var fixture = new Fixture();
            fixture.Inject((MethodInfo)null);
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetMethod(VerifyMethodName, new[] { typeof(MethodInfo) }));
        }

        [Fact]
        public void VerifyWithMethodThatIsNotEqualsMethodIsNotInvoked()
        {
            var sutMock = new Mock<EqualsOverrideAssertion>(new Mock<ISpecimenBuilder>().Object)
            {
                CallBase = true,
            };
            var sut = sutMock.Object;

            sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.EqualsWrongName)));

            sutMock.Protected()
                .Verify("VerifyEqualsMethod", Times.Never(), ItExpr.IsAny<MethodInfo>());
        }

        [Fact]
        public void VerifyWithMethodThatIsEqualsMethodIsInvoked()
        {
            var sutMock = new Mock<EqualsOverrideAssertion>(new Mock<ISpecimenBuilder>().Object)
            {
                CallBase = true,
            };
            var sut = sutMock.Object;

            sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) }));

            sutMock.Protected()
                .Verify("VerifyEqualsMethod", Times.Once(), ItExpr.IsAny<MethodInfo>());
        }

        [Fact]
        public void IsEqualsMethodWithNonEqualsMethodReturnsFalse()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsMethodTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.EqualsWrongName)));

            Assert.False(result);
        }

        [Fact]
        public void IsEqualsMethodWithEqualsMethodReturnsTrue()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsMethodTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(object) }));

            Assert.True(result);
        }

        [Fact]
        public void IsEqualsParameterObjectTypeWithNonObjectTypeParameterReturnsFalse()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsParameterObjectTypeTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) }));

            Assert.False(result);
        }

        [Fact]
        public void IsEqualsParameterObjectTypeWithObjectTypeParameterReturnsTrue()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsParameterObjectTypeTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(object) }));

            Assert.True(result);
        }

        [Fact]
        public void IsEqualsParameterNullableTypeWithNonNullableTypeReturnsFalse()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsParameterNullableTypeTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(bool) }));

            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(EqualsOverrideAssertionVerifyHelper))]
        public void IsEqualsParameterNullableTypeWithNullableTypeReturnsTrue(Type parameterType)
        {
            var fixture = new Fixture();
            var sut = fixture.Create<SutStub>();

            var result = sut.IsEqualsParameterNullableTypeTest(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { parameterType }));

            Assert.True(result);
        }

        [Fact]
        public void CreateSpecimenCallsCreate()
        {
            var fixture = new Fixture();
            var specimenBuilderMock = new Mock<ISpecimenBuilder>();
            fixture.Inject(specimenBuilderMock.Object);
            var sut = fixture.Create<SutStub>();

            sut.CreateSpecimenTest(typeof(string));

            specimenBuilderMock
                .Verify(b => b.Create(typeof(string), It.IsAny<ISpecimenContext>()), Times.Once());
        }

        private sealed class SutStub : EqualsOverrideAssertion
        {
            public SutStub(ISpecimenBuilder specimenBuilder)
                : base(specimenBuilder)
            {
            }

            public SutStub(
                ISpecimenBuilder specimenBuilder,
                int successiveCount)
                : base(specimenBuilder, successiveCount)
            {
            }

#pragma warning disable CC0091 // Use static method
            public bool IsEqualsMethodTest(MethodInfo methodInfo)
            {
                return IsEqualsMethod(methodInfo);
            }

            public bool IsEqualsParameterObjectTypeTest(MethodInfo methodInfo)
            {
                return IsEqualsParameterObjectType(methodInfo);
            }

            public bool IsEqualsParameterNullableTypeTest(MethodInfo methodInfo)
            {
                return IsEqualsParameterNullableType(methodInfo);
            }
#pragma warning restore CC0091 // Use static method

            public object CreateSpecimenTest(Type type)
            {
                return this.CreateSpecimen(type);
            }

            protected override void VerifyEqualsMethod(MethodInfo methodInfo)
            {
            }
        }
    }
}