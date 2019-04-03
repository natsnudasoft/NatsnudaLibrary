// <copyright file="ExceptionBehaviorExpectationTests.cs" company="natsnudasoft">
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
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Xunit;

    public sealed class ExceptionBehaviorExpectationTests
    {
        private static readonly Guid ThrowCorrectGuid =
            new Guid("0567E3F7-5AEC-496F-BA10-36FFD9C7E1E0");

        private static readonly Guid ThrowIncorrectGuid =
            new Guid("BD7A8F47-EA65-4D51-AD96-FA775CED9391");

        private static readonly Type SutType = typeof(ExceptionBehaviorExpectation<Exception>);

        private Mock<IGuardClauseCommand> stubMethodCommandMock;

        [Fact]
        public void ConstructorHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetConstructors());
        }

        [Fact]
        public void ConstructorEmptyParameterNameThrows()
        {
            var fixture = new Fixture();

            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<Exception>(
                new Mock<ISpecimenBuilder>().Object,
                string.Empty,
                fixture.Create<object[]>()));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Constructor2EmptyParameterNameThrows()
        {
            var fixture = new Fixture();

            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<Exception>(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IGuardClauseExtensions>().Object,
                string.Empty,
                fixture.Create<object[]>()));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ConstructorEmptyValuesThrows()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<Exception>(
                new Mock<ISpecimenBuilder>().Object,
                fixture.Create<string>(),
                Array.Empty<object>()));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Constructor2EmptyValuesThrows()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<Exception>(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IGuardClauseExtensions>().Object,
                fixture.Create<string>(),
                Array.Empty<object>()));

            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var assertion = new ConstructorInitializedMemberAssertion(fixture);

            assertion.Verify(
                SutType.GetProperty(nameof(ExceptionBehaviorExpectation<Exception>.ParameterName)));
        }

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers2()
        {
            var fixture = new Fixture();
            var expected = new object[] { fixture.Create<string>(), fixture.Create<int>() };

            var sut = new ExceptionBehaviorExpectation<ArgumentException>(
                new Mock<ISpecimenBuilder>().Object,
                fixture.Create<string>(),
                expected);

            Assert.Equal(expected, sut.Values);
        }

        [Fact]
        public void Constructor2SetsCorrectInitializedMembers2()
        {
            var fixture = new Fixture();
            var expected = new object[] { fixture.Create<string>(), fixture.Create<int>() };

            var sut = new ExceptionBehaviorExpectation<ArgumentException>(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IGuardClauseExtensions>().Object,
                fixture.Create<string>(),
                expected);

            Assert.Equal(expected, sut.Values);
        }

        [Fact]
        public void Constructor1DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<ArgumentException>(
                new Mock<ISpecimenBuilder>().Object,
                fixture.Create<string>(),
                fixture.Create<object[]>()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new ExceptionBehaviorExpectation<ArgumentException>(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IGuardClauseExtensions>().Object,
                fixture.Create<string>(),
                fixture.Create<object[]>()));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(nameof(ExceptionBehaviorExpectation<Exception>.Verify)));
        }

        [Fact]
        public void VerifyWithCommandThatThrowsIncorrectThrowsCreateException()
        {
            var sut = this.CreateSut("guidParameter", ThrowIncorrectGuid);

            var ex = Record.Exception(() => sut.Verify(new Mock<IGuardClauseCommand>().Object));

            this.stubMethodCommandMock.Verify(
                m => m.CreateException(It.IsAny<string>(), It.IsAny<UnauthorizedAccessException>()),
                Times.Once());
            Assert.IsType<InvalidOperationException>(ex);
            Assert.IsType<UnauthorizedAccessException>(ex.InnerException);
        }

        [Fact]
        public void VerifyWithCommandThatDoesNotThrowThrowsCreateException()
        {
            var sut = this.CreateSut("guidParameter", Guid.Empty);

            var ex = Record.Exception(() => sut.Verify(new Mock<IGuardClauseCommand>().Object));

            this.stubMethodCommandMock
                .Verify(m => m.CreateException(It.IsAny<string>()), Times.Once());
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Null(ex.InnerException);
        }

        [Fact]
        public void VerifyWithCommandThatThrowsCorrectDoesNotThrow()
        {
            var sut = this.CreateSut("guidParameter", ThrowCorrectGuid);

            var ex = Record.Exception(() => sut.Verify(new Mock<IGuardClauseCommand>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyWithCommandThatHasNoMethodInvokeCommandDoesNotThrow()
        {
            var fixture = new Fixture();
            var specimenBuilderMock = new Mock<ISpecimenBuilder>();
            var guardClauseExtensionsMock = new Mock<IGuardClauseExtensions>();
            MethodInvokeCommand methodInvokeCommand = null;
            guardClauseExtensionsMock
                .Setup(e => e.TryGetMethodInvokeCommand(
                    It.IsAny<IGuardClauseCommand>(),
                    out methodInvokeCommand))
                .Returns(false);
            var sut = new ExceptionBehaviorExpectation<InvalidOperationException>(
                specimenBuilderMock.Object,
                guardClauseExtensionsMock.Object,
                fixture.Create<string>(),
                fixture.Create<Guid>());

            var ex = Record.Exception(() => sut.Verify(new Mock<IGuardClauseCommand>().Object));

            Assert.Null(ex);
        }

        private static void StubMethodTest(Guid guidParameter)
        {
            if (guidParameter == ThrowCorrectGuid)
            {
                throw new InvalidOperationException();
            }

            if (guidParameter == ThrowIncorrectGuid)
            {
                throw new UnauthorizedAccessException();
            }
        }

        private static MethodInvokeCommand CreateMethodInvokeCommand()
        {
            return new MethodInvokeCommand(
                new Mock<IMethod>().Object,
                new Mock<IExpansion<object>>().Object,
                typeof(ExceptionBehaviorExpectationTests)
                    .GetMethod(nameof(StubMethodTest), BindingFlags.Static | BindingFlags.NonPublic)
                    .GetParameters()
                    .First());
        }

        private ExceptionBehaviorExpectation<InvalidOperationException> CreateSut(
            string parameterName,
            Guid guidParameter)
        {
            var specimenBuilderMock = new Mock<ISpecimenBuilder>();
            var guardClauseExtensionsMock = new Mock<IGuardClauseExtensions>();
            var methodInvokeCommand = CreateMethodInvokeCommand();
            guardClauseExtensionsMock
                .Setup(e => e.TryGetMethodInvokeCommand(
                    It.IsAny<IGuardClauseCommand>(),
                    out methodInvokeCommand))
                .Returns(true);
            this.stubMethodCommandMock = new Mock<IGuardClauseCommand>();
            this.stubMethodCommandMock
                .Setup(m => m.Execute(It.IsAny<Guid>()))
                .Callback<object>(g => StubMethodTest((Guid)g));
            this.stubMethodCommandMock
                .Setup(m => m.CreateException(It.IsAny<string>(), It.IsAny<Exception>()))
                .Returns<string, Exception>(
                    (s, ex) => new InvalidOperationException(string.Empty, ex));
            this.stubMethodCommandMock
                .Setup(m => m.CreateException(It.IsAny<string>()))
                .Returns(new InvalidOperationException());
            guardClauseExtensionsMock
                .Setup(e => e.CreateExtendedCommand(
                    It.IsAny<ISpecimenBuilder>(),
                    It.IsAny<IGuardClauseCommand>()))
                .Returns(this.stubMethodCommandMock.Object);
            return new ExceptionBehaviorExpectation<InvalidOperationException>(
                specimenBuilderMock.Object,
                guardClauseExtensionsMock.Object,
                parameterName,
                guidParameter);
        }
    }
}