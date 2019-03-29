// <copyright file="ParameterNullReferenceBehaviorExpectationTests.cs" company="natsnudasoft">
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
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Idioms;
    using AutoFixture.Kernel;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Xunit;
    using SutAlias =
        Natsnudasoft.NatsnudaLibrary.TestExtensions.ParameterNullReferenceBehaviorExpectation;

    public sealed class ParameterNullReferenceBehaviorExpectationTests
    {
        private static readonly Type SutType = typeof(SutAlias);

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
        public void Constructor1DoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias(new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IGuardClauseExtensions>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(nameof(SutAlias.Verify)));
        }

        [Fact]
        public void VerifyWithCommandThatThrowsIncorrectThrowsCreateException()
        {
            var sut = this.CreateSut(() => { throw new UnauthorizedAccessException(); });

            var ex = Record.Exception(() => sut.Verify(CreateCommandMock().Object));

            this.stubMethodCommandMock.Verify(
                m => m.CreateException(It.IsAny<string>(), It.IsAny<UnauthorizedAccessException>()),
                Times.Once());
            Assert.IsType<InvalidOperationException>(ex);
            Assert.IsType<UnauthorizedAccessException>(ex.InnerException);
        }

        [Fact]
        public void VerifyWithCommandThatDoesNotThrowThrowsCreateException()
        {
            var sut = this.CreateSut(() => { });

            var ex = Record.Exception(() => sut.Verify(CreateCommandMock().Object));

            this.stubMethodCommandMock
                .Verify(m => m.CreateException(It.IsAny<string>()), Times.Once());
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Null(ex.InnerException);
        }

        [Fact]
        public void VerifyWithCommandThatThrowsCorrectDoesNotThrow()
        {
            var sut = this.CreateSut(() => { throw new ArgumentNullException(string.Empty); });

            var ex = Record.Exception(() => sut.Verify(CreateCommandMock().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyWithCommandThatIsNotClassOrInterfaceDoesNotThrow()
        {
            var sut = new SutAlias(
                new Mock<ISpecimenBuilder>().Object);
            var commandMock = new Mock<IGuardClauseCommand>();
            commandMock.SetupGet(c => c.RequestedType).Returns(typeof(int));

            var ex = Record.Exception(() => sut.Verify(commandMock.Object));

            Assert.Null(ex);
        }

        private static Mock<IGuardClauseCommand> CreateCommandMock()
        {
            var commandMock = new Mock<IGuardClauseCommand>();
            commandMock
                .Setup(c => c.RequestedType)
                .Returns(typeof(ParameterNullReferenceBehaviorExpectationTests));
            return commandMock;
        }

        private SutAlias CreateSut(
            Action stubMethodCommandMockAction)
        {
            var specimenBuilderMock = new Mock<ISpecimenBuilder>();
            var guardClauseExtensionsMock = new Mock<IGuardClauseExtensions>();
            this.stubMethodCommandMock = new Mock<IGuardClauseCommand>();
            this.stubMethodCommandMock
                .Setup(m => m.Execute(null))
                .Callback(stubMethodCommandMockAction);
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
            return new SutAlias(
                specimenBuilderMock.Object,
                guardClauseExtensionsMock.Object);
        }
    }
}