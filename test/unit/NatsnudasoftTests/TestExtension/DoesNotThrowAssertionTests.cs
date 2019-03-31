// <copyright file="DoesNotThrowAssertionTests.cs" company="natsnudasoft">
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
    using SutAlias = Natsnudasoft.NatsnudaLibrary.TestExtensions.DoesNotThrowAssertion;

    public sealed class DoesNotThrowAssertionTests
    {
        private static readonly Type SutType = typeof(SutAlias);
        private static readonly Type TestHelperType = typeof(VerifyHelper);

        private Mock<ISpecimenBuilder> specimenBuilderMock;
        private Mock<IVerifyHelper> verifyHelperMock;

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1034:NestedTypesShouldNotBeVisible",
            Justification = "Needs to be public for dynamic proxy.")]
        public interface IVerifyHelper
        {
            Guid PublicGetProperty { get; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Design",
                "CA1044:PropertiesShouldNotBeWriteOnly",
                Justification = "Even though this is bad design we still need to test for it.")]
            Guid PublicSetProperty { set; }

            Guid PublicProperty { get; set; }
        }

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

            assertion.Verify(SutType.GetProperty(nameof(SutAlias.SpecimenBuilder)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias(new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyMethodsHaveCorrectGuardClauses()
        {
            const string VerifyMethodName = nameof(SutAlias.Verify);
            var fixture = new Fixture();
            fixture.Inject((ConstructorInfo)null);
            fixture.Inject((MethodInfo)null);
            fixture.Inject((PropertyInfo)null);
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(VerifyMethodName, new[] { typeof(ConstructorInfo) }),
                SutType.GetMethod(VerifyMethodName, new[] { typeof(MethodInfo) }),
                SutType.GetMethod(VerifyMethodName, new[] { typeof(PropertyInfo) }));
        }

        [Fact]
        public void VerifyWithConstructorInfoInvokesConstructor()
        {
            var fixture = new Fixture();
            var expectedGuid = fixture.Create<Guid>();
            var sut = this.CreateSut(expectedGuid);

            sut.Verify(TestHelperType.GetConstructors().First());

            Assert.Equal(expectedGuid, VerifyHelper.TestGuid);
        }

        [Fact]
        public void VerifyWithInstanceMethodInfoInvokesMethod()
        {
            var fixture = new Fixture();
            var expectedGuid = fixture.Create<Guid>();
            var sut = this.CreateSut(expectedGuid);

            sut.Verify(TestHelperType.GetMethod(nameof(VerifyHelper.InstanceMethodTest)));

            Assert.Equal(expectedGuid, VerifyHelper.TestGuid);
        }

        [Fact]
        public void VerifyWithStaticMethodInfoInvokesMethod()
        {
            var fixture = new Fixture();
            var expectedGuid = fixture.Create<Guid>();
            var sut = this.CreateSut(expectedGuid);

            sut.Verify(TestHelperType.GetMethod(nameof(VerifyHelper.StaticMethodTest)));

            Assert.Equal(expectedGuid, VerifyHelper.TestGuid);
        }

        [Fact]
        public void VerifyWithPropertyInfoWithNoPublicGetOrSetThrows()
        {
            var sut = this.CreateSut(Guid.Empty);

            var ex = Record.Exception(() => sut.Verify(TestHelperType.GetProperty(
                "PrivateProperty",
                BindingFlags.Instance | BindingFlags.NonPublic)));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void VerifyWithPropertyInfoWithPublicGet()
        {
            var sut = this.CreateSut(Guid.Empty);

            sut.Verify(this.verifyHelperMock.Object.GetType().GetProperty(
                nameof(IVerifyHelper.PublicGetProperty)));

            this.verifyHelperMock.VerifyGet(v => v.PublicGetProperty, Times.Once());
        }

        [Fact]
        public void VerifyWithPropertyInfoWithPublicSet()
        {
            var sut = this.CreateSut(Guid.Empty);

            sut.Verify(this.verifyHelperMock.Object.GetType().GetProperty(
                nameof(IVerifyHelper.PublicSetProperty)));

            this.verifyHelperMock
                .VerifySet(v => v.PublicSetProperty = It.IsAny<Guid>(), Times.Once());
        }

        [Fact]
        public void VerifyWithPropertyInfoWithPublicGetAndSet()
        {
            var sut = this.CreateSut(Guid.Empty);

            sut.Verify(this.verifyHelperMock.Object.GetType().GetProperty(
                nameof(IVerifyHelper.PublicProperty)));

            this.verifyHelperMock.VerifyGet(v => v.PublicProperty, Times.Once());
            this.verifyHelperMock
                .VerifySet(v => v.PublicProperty = It.IsAny<Guid>(), Times.Once());
        }

        private SutAlias CreateSut(Guid guidSpecimen)
        {
            this.verifyHelperMock = new Mock<IVerifyHelper>();
            this.specimenBuilderMock = new Mock<ISpecimenBuilder>();
            this.specimenBuilderMock
                .Setup(b => b.Create(It.IsAny<ParameterInfo>(), It.IsAny<SpecimenContext>()))
                .Returns(guidSpecimen);
            this.specimenBuilderMock
                .Setup(b => b.Create(It.IsAny<Type>(), It.IsAny<SpecimenContext>()))
                .Returns(this.verifyHelperMock.Object);
            this.specimenBuilderMock
                .Setup(b => b.Create(TestHelperType, It.IsAny<SpecimenContext>()))
                .Returns(new VerifyHelper(Guid.Empty));
            return new SutAlias(this.specimenBuilderMock.Object);
        }

        private class VerifyHelper
        {
            public VerifyHelper(Guid testGuid)
            {
                TestGuid = testGuid;
            }

            public static Guid TestGuid { get; set; }

            private object PrivateProperty { get; set; }

            public static void StaticMethodTest(Guid testGuid)
            {
                TestGuid = testGuid;
            }

#pragma warning disable CC0091 // Use static method
            public void InstanceMethodTest(Guid testGuid)
            {
                TestGuid = testGuid;
            }
#pragma warning restore CC0091 // Use static method
        }
    }
}