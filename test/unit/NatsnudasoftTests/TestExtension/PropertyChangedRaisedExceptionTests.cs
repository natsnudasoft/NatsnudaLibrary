// <copyright file="PropertyChangedRaisedExceptionTests.cs" company="natsnudasoft">
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
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.SemanticComparison.Fluent;
    using Xunit;
    using SutAlias = Natsnudasoft.NatsnudaLibrary.TestExtensions.PropertyChangedRaisedException;

    public sealed class PropertyChangedRaisedExceptionTests
    {
        private static readonly Type SutType = typeof(SutAlias);

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers()
        {
            var fixture = new Fixture();
            var propertyInfo =
                SutType.GetProperty(nameof(SutAlias.PropertyInfo));
            fixture.Inject(propertyInfo);
            var assertion = new ConstructorInitializedMemberAssertion(fixture);

            assertion.Verify(
                SutType.GetProperty(nameof(SutAlias.PropertyInfo)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias());

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutAlias(fixture.Create<string>()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor3DoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias(
                SutType.GetProperty(nameof(SutAlias.PropertyInfo))));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor4DoesNotThrow()
        {
            var fixture = new Fixture();
            var ex = Record.Exception(() => new SutAlias(
                fixture.Create<string>(),
                new InvalidOperationException()));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor5DoesNotThrow()
        {
            var ex = Record.Exception(() => new SutAlias(
                SutType.GetProperty(nameof(SutAlias.PropertyInfo)),
                new InvalidOperationException()));

            Assert.Null(ex);
        }

        [Fact]
        public void ExceptionSerializationCopiesAllMembers()
        {
            var propertyInfo =
                SutType.GetProperty(nameof(SutAlias.PropertyInfo));
            var sutExpected = new SutAlias(propertyInfo);
            SutAlias sutActual;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, sutExpected);
                memoryStream.Position = 0;
                sutActual =
                    (SutAlias)binaryFormatter.Deserialize(memoryStream);
            }

            Assert.NotSame(sutExpected, sutActual);
            sutActual
                .AsSource()
                .OfLikeness<SutAlias>()
                .Without(ex => ex.Data)
                .Without(ex => ex.HelpLink)
                .Without(ex => ex.HResult)
                .Without(ex => ex.Source)
                .Without(ex => ex.StackTrace)
                .Without(ex => ex.TargetSite)
                .ShouldEqual(sutExpected);
        }
    }
}