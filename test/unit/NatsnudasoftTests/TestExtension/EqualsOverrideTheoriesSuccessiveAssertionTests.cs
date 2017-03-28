﻿// <copyright file="EqualsOverrideTheoriesSuccessiveAssertionTests.cs" company="natsnudasoft">
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
    using Helper;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class EqualsOverrideTheoriesSuccessiveAssertionTests
    {
        private static readonly Type SutType = typeof(EqualsOverrideTheoriesSuccessiveAssertion);
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
            var ex = Record.Exception(() => new EqualsOverrideTheoriesSuccessiveAssertion(
                new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyEqualsMethodWithTheoryWithLeftThatDoesNotMatchReflectedTypeThrows()
        {
            var fixture = new Fixture();
            fixture.Inject(new EqualsOverrideAssertionVerifyHelper(false, true, false));
            var equalsOverrideTheory = new EqualsOverrideTheory(
                true,
                fixture.Create<EqualsOverrideAssertionVerifyHelper>(),
                false);
            var sut = new EqualsOverrideTheoriesSuccessiveAssertion(fixture, equalsOverrideTheory);

            var ex = Record.Exception(() => sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) })));

            Assert.IsType<EqualsOverrideException>(ex);
            Assert.Contains("owner method", ex.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void VerifyEqualsMethodWithTheoryWithRightThatCannotBeAssignedDoesNotInvokeEquals()
        {
            var fixture = new Fixture();
            var verifyHelperMock = new Mock<EqualsOverrideAssertionVerifyHelper>(false, true, false)
            {
                CallBase = true
            };
            var equalsOverrideTheory = new EqualsOverrideTheory(
                verifyHelperMock.Object,
                true,
                false);
            var sut = new EqualsOverrideTheoriesSuccessiveAssertion(fixture, equalsOverrideTheory);

            sut.Verify(verifyHelperMock.Object.GetType().GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) }));

            verifyHelperMock.Verify(
                h => h.Equals(It.IsAny<EqualsOverrideAssertionVerifyHelper>()),
                Times.Never());
        }

        [Fact]
        public void VerifyEqualsMethodViolatesEqualsSuccessiveContractThrows()
        {
            var fixture = new Fixture();
            var equalsResultQueue = new Queue<bool>(new[] { true, false, true });
            var differentSuccessiveTheory = new EqualsOverrideAssertionVerifyHelper(
                false,
                false,
                () => equalsResultQueue.Dequeue());
            var equalsOverrideTheory = new EqualsOverrideTheory(
                new EqualsOverrideAssertionVerifyHelper(false, false, false),
                differentSuccessiveTheory,
                true);
            var sut = new EqualsOverrideTheoriesSuccessiveAssertion(fixture, equalsOverrideTheory);

            var ex = Record.Exception(() => sut.Verify(TestHelperType.GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) })));

            Assert.IsType<EqualsOverrideException>(ex);
            Assert.Contains("multiple times", ex.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void VerifyEqualsMethodCorrectBehaviourInvokesEquals()
        {
            var fixture = new Fixture();
            var verifyHelperMock = new Mock<EqualsOverrideAssertionVerifyHelper>(false, false, true)
            {
                CallBase = true
            };
            var verifyHelperQueue = new Queue<EqualsOverrideAssertionVerifyHelper>(new[]
            {
                verifyHelperMock.Object,
                new EqualsOverrideAssertionVerifyHelper(false, false, true)
            });
            fixture.Register(() => verifyHelperQueue.Dequeue());
            var equalsOverrideTheory = new EqualsOverrideTheory(
                fixture.Create<EqualsOverrideAssertionVerifyHelper>(),
                fixture.Create<EqualsOverrideAssertionVerifyHelper>(),
                true);
            var sut = new EqualsOverrideTheoriesSuccessiveAssertion(fixture, equalsOverrideTheory);

            sut.Verify(verifyHelperMock.Object.GetType().GetMethod(
                nameof(EqualsOverrideAssertionVerifyHelper.Equals),
                new[] { typeof(EqualsOverrideAssertionVerifyHelper) }));

            verifyHelperMock.Verify(
                h => h.Equals(It.IsAny<EqualsOverrideAssertionVerifyHelper>()),
                Times.Exactly(3));
        }
    }
}
