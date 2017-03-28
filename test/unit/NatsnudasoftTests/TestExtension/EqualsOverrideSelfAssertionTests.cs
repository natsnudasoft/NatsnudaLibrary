// <copyright file="EqualsOverrideSelfAssertionTests.cs" company="natsnudasoft">
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
    using Helper;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class EqualsOverrideSelfAssertionTests
    {
        private static readonly Type SutType = typeof(EqualsOverrideSelfAssertion);
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

            assertion.Verify(SutType.GetProperty(nameof(EqualsOverrideAssertion.SpecimenBuilder)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new EqualsOverrideSelfAssertion(
                new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyEqualsMethodViolatesEqualsSelfContractThrows()
        {
            var fixture = new Fixture();
            var verifyHelperMock = new Mock<EqualsOverrideAssertionVerifyHelper>(false, false, true)
            {
                CallBase = true
            };
            fixture.Inject(verifyHelperMock.Object);
            var sut = fixture.Create<EqualsOverrideSelfAssertion>();

            var ex = Record.Exception(() => sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) })));

            Assert.IsType<EqualsOverrideException>(ex);
        }

        [Fact]
        public void VerifyEqualsMethodCorrectBehaviourInvokesEquals()
        {
            var fixture = new Fixture();
            var verifyHelperMock = new Mock<EqualsOverrideAssertionVerifyHelper>(false, true, true)
            {
                CallBase = true
            };
            fixture.Inject(verifyHelperMock.Object);
            var sut = fixture.Create<EqualsOverrideSelfAssertion>();

            sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) }));

            verifyHelperMock.Verify(
                h => h.Equals(It.IsAny<EqualsOverrideAssertionVerifyHelper>()),
                Times.Once());
        }
    }
}
