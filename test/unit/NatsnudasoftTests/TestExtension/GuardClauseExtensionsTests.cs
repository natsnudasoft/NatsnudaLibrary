// <copyright file="GuardClauseExtensionsTests.cs" company="natsnudasoft">
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
    using System.Threading.Tasks;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class GuardClauseExtensionsTests
    {
        private static readonly Type SutType = typeof(GuardClauseExtensions);
        private static readonly Type TaskReturnType = typeof(GuardClauseAssertion).Assembly.GetType(
            "Ploeh.AutoFixture.Idioms.GuardClauseAssertion+TaskReturnMethodInvokeCommand");

        [Fact]
        public void CreateExtendedCommandHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(nameof(GuardClauseExtensions.CreateExtendedCommand)));
        }

        [Fact]
        public void TryGetMethodInvokeCommandHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Inject<MethodInvokeCommand>(null);
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(
                SutType.GetMethod(nameof(GuardClauseExtensions.TryGetMethodInvokeCommand)));
        }

        [Fact]
        public void CreateExtendedCommandInvalidBaseCommandReturnsOriginalCommand()
        {
            var sut = new GuardClauseExtensions();
            var command = CreateWrappedMethodInvokeCommand(
                new Mock<IMethod>().Object,
                new Mock<IExpansion<object>>().Object);

            var actual = sut.CreateExtendedCommand(new Mock<ISpecimenBuilder>().Object, command);

            Assert.Same(command, actual);
        }

        [Fact]
        public void CreateExtendedCommandWithStaticMethodReturnsMethodInvokeCommand()
        {
            var sut = new GuardClauseExtensions();
            var methodInfo = typeof(GuardClauseExtensionsTests).GetMethod(
                nameof(StaticCommandHelperMethod),
                BindingFlags.Static | BindingFlags.NonPublic);
            var method = new StaticMethod(methodInfo);
            var command = CreateWrappedMethodInvokeCommand(
                method,
                new IndexedReplacement<object>(0));

            var actual = sut.CreateExtendedCommand(new Mock<ISpecimenBuilder>().Object, command);

            Assert.IsType<MethodInvokeCommand>(
                (actual as ReflectionExceptionUnwrappingCommand)?.Command);
        }

        [Fact]
        public void CreateExtendedCommandWithStaticAsyncMethodReturnsMethodInvokeCommand()
        {
            var sut = new GuardClauseExtensions();
            var methodInfo = typeof(GuardClauseExtensionsTests).GetMethod(
                nameof(StaticCommandHelperMethodAsync),
                BindingFlags.Static | BindingFlags.NonPublic);
            var method = new StaticMethod(methodInfo);
            var command = CreateWrappedMethodInvokeCommand(
                method,
                new IndexedReplacement<object>(0));

            var actual = sut.CreateExtendedCommand(new Mock<ISpecimenBuilder>().Object, command);

            Assert.IsType<AsyncMethodInvokeCommand>(
                (actual as ReflectionExceptionUnwrappingCommand)?.Command);
        }

        [Fact]
        public void CreateExtendedCommandWithInstanceMethodReturnsMethodInvokeCommand()
        {
            var sut = new GuardClauseExtensions();
            var methodInfo = typeof(GuardClauseExtensionsTests).GetMethod(
                nameof(this.InstanceCommandHelperMethod),
                BindingFlags.Instance | BindingFlags.NonPublic);
            var method = new InstanceMethod(methodInfo, this);
            var command = CreateWrappedMethodInvokeCommand(
                method,
                new IndexedReplacement<object>(0));

            var actual = sut.CreateExtendedCommand(new Mock<ISpecimenBuilder>().Object, command);

            Assert.IsType<MethodInvokeCommand>(
                (actual as ReflectionExceptionUnwrappingCommand)?.Command);
        }

        [Fact]
        public void CreateExtendedCommandWithInstanceAsyncMethodReturnsMethodInvokeCommand()
        {
            var sut = new GuardClauseExtensions();
            var methodInfo = typeof(GuardClauseExtensionsTests).GetMethod(
                nameof(this.InstanceCommandHelperMethodAsync),
                BindingFlags.Instance | BindingFlags.NonPublic);
            var method = new InstanceMethod(methodInfo, this);
            var command = CreateWrappedMethodInvokeCommand(
                method,
                new IndexedReplacement<object>(0));

            var actual = sut.CreateExtendedCommand(new Mock<ISpecimenBuilder>().Object, command);

            Assert.IsType<AsyncMethodInvokeCommand>(
                (actual as ReflectionExceptionUnwrappingCommand)?.Command);
        }

        [Fact]
        public void TryGetMethodInvokeCommandDoesNotContainMethodInvokeCommandReturnsFalse()
        {
            var sut = new GuardClauseExtensions();
            MethodInvokeCommand methodInvokeCommand;

            var actual = sut.TryGetMethodInvokeCommand(
                new Mock<IGuardClauseCommand>().Object,
                out methodInvokeCommand);

            Assert.False(actual);
            Assert.Null(methodInvokeCommand);
        }

        [Fact]
        public void TryGetMethodInvokeCommandHasMethodInvokeCommandReturnsCommand()
        {
            var sut = new GuardClauseExtensions();
            MethodInvokeCommand methodInvokeCommand;
            var command = CreateWrappedMethodInvokeCommand(
                new Mock<IMethod>().Object,
                new Mock<IExpansion<object>>().Object);

            var actual = sut.TryGetMethodInvokeCommand(command, out methodInvokeCommand);

            Assert.True(actual);
            Assert.NotNull(methodInvokeCommand);
        }

        [Fact]
        public void TryGetMethodInvokeCommandHasTaskTypeMethodInvokeCommandReturnsCommand()
        {
            var sut = new GuardClauseExtensions();
            var testHelperMethod = typeof(GuardClauseExtensionsTests)
                .GetMethod(nameof(TestHelperMethod), BindingFlags.Static | BindingFlags.NonPublic);
            var innerCommand = new ReflectionExceptionUnwrappingCommand(new MethodInvokeCommand(
                new Mock<IMethod>().Object,
                new Mock<IExpansion<object>>().Object,
                testHelperMethod.GetParameters().First()));
            var command =
                (IGuardClauseCommand)Activator.CreateInstance(TaskReturnType, innerCommand);
            MethodInvokeCommand methodInvokeCommand;

            var actual = sut.TryGetMethodInvokeCommand(command, out methodInvokeCommand);

            Assert.True(actual);
            Assert.NotNull(methodInvokeCommand);
        }

        private static IGuardClauseCommand CreateWrappedMethodInvokeCommand(
            IMethod method,
            IExpansion<object> expansion)
        {
            var testHelperMethod = typeof(GuardClauseExtensionsTests)
                .GetMethod(nameof(TestHelperMethod), BindingFlags.Static | BindingFlags.NonPublic);
            return new ReflectionExceptionUnwrappingCommand(new MethodInvokeCommand(
                method,
                expansion,
                testHelperMethod.GetParameters().First()));
        }

        private static string TestHelperMethod(string stringParameter)
        {
            return stringParameter;
        }

        private static void StaticCommandHelperMethod()
        {
        }

        private static async Task StaticCommandHelperMethodAsync()
        {
            await Task.Yield();
        }

#pragma warning disable CC0091 // Use static method
        private void InstanceCommandHelperMethod()
        {
        }

        private async Task InstanceCommandHelperMethodAsync()
        {
            await Task.Yield();
        }
#pragma warning restore CC0091 // Use static method
    }
}
