// <copyright file="EqualsOverrideTheoryTests.cs" company="natsnudasoft">
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
    using AutoFixture.Idioms;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Xunit;
    using SutAlias = Natsnudasoft.NatsnudaLibrary.TestExtensions.EqualsOverrideTheory;

    public sealed class EqualsOverrideTheoryTests
    {
        private static readonly Type SutType = typeof(SutAlias);

        [Theory]
        [AutoMoqData]
        public void ConstructorHasCorrectGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(SutType.GetConstructors());
        }

        [Theory]
        [AutoMoqData]
        public void ConstructorDoesNotThrow(DoesNotThrowAssertion assertion)
        {
            assertion.Verify(SutType.GetConstructors());
        }

        [Theory]
        [AutoMoqData]
        public void ConstructorSetsCorrectInitializedMembers(
            ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(
                SutType.GetProperty(nameof(SutAlias.Left)),
                SutType.GetProperty(nameof(SutAlias.Right)),
                SutType.GetProperty(nameof(SutAlias.ExpectedResult)));
        }
    }
}