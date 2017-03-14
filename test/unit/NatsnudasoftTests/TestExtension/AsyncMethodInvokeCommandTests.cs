// <copyright file="AsyncMethodInvokeCommandTests.cs" company="natsnudasoft">
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
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class AsyncMethodInvokeCommandTests
    {
        private Mock<IMethod> methodMock;
        private Mock<IExpansion<object>> expansionMock;

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var parameterInfo = typeof(AsyncMethodInvokeCommandTests)
                .GetMethod(nameof(StubMethodAsync), BindingFlags.Static | BindingFlags.NonPublic)
                .GetParameters().First();
            var ex = Record.Exception(() => new AsyncMethodInvokeCommand(
                new Mock<IMethod>().Object,
                new Mock<IExpansion<object>>().Object,
                parameterInfo));

            Assert.Null(ex);
        }

        [Fact]
        public void ExecuteAsIGuardClauseCommandWithNoTaskThrows()
        {
            var sut = this.CreateSut(null);

#pragma warning disable IDE0004 // Cast is redundant
            var ex = Record.Exception(() => ((IGuardClauseCommand)sut).Execute(null));
#pragma warning restore IDE0004 // Cast is redundant

            Assert.IsType<GuardClauseException>(ex);
        }

        [Fact]
        public void ExecuteAsIGuardClauseCommandWithTaskWithExceptionThrows()
        {
            var sut = this.CreateSut(StubMethodAsync(true));

#pragma warning disable IDE0004 // Cast is redundant
            var ex = Record.Exception(() => ((IGuardClauseCommand)sut).Execute(null));
#pragma warning restore IDE0004 // Cast is redundant

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ExecuteAsIGuardClauseCommandWithTaskInvokesAndAwaitsMethod()
        {
            const int ExpansionValue = 16;
            var task = StubMethodAsync(false);
            var sut = this.CreateSut(task);

#pragma warning disable IDE0004 // Cast is redundant
            ((IGuardClauseCommand)sut).Execute(ExpansionValue);
#pragma warning restore IDE0004 // Cast is redundant

            this.VerifyMethodInvoked(ExpansionValue);
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        private static async Task StubMethodAsync(bool throwException)
        {
            if (throwException)
            {
                throw new InvalidOperationException();
            }

            await Task.Yield();
        }

        private AsyncMethodInvokeCommand CreateSut(Task methodReturnedTask)
        {
            this.methodMock = new Mock<IMethod>();
            this.methodMock
                .Setup(m => m.Invoke(It.IsAny<IEnumerable<object>>()))
                .Returns(methodReturnedTask);
            this.expansionMock = new Mock<IExpansion<object>>();
            var parameterInfo = typeof(AsyncMethodInvokeCommandTests)
                .GetMethod(nameof(StubMethodAsync), BindingFlags.Static | BindingFlags.NonPublic)
                .GetParameters().First();
            return new AsyncMethodInvokeCommand(
                this.methodMock.Object,
                this.expansionMock.Object,
                parameterInfo);
        }

        private void VerifyMethodInvoked(object expansionValue)
        {
            this.expansionMock.Verify(e => e.Expand(expansionValue), Times.Once());
            this.methodMock.Verify(m => m.Invoke(It.IsAny<IEnumerable<object>>()), Times.Once());
        }
    }
}
