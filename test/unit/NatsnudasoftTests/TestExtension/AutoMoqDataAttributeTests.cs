// <copyright file="AutoMoqDataAttributeTests.cs" company="natsnudasoft">
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
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Xunit;

    public sealed class AutoMoqDataAttributeTests
    {
        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new AutoMoqDataAttribute());

            Assert.Null(ex);
        }
    }
}
