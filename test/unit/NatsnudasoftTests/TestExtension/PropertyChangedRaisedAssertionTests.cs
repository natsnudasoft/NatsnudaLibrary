// <copyright file="PropertyChangedRaisedAssertionTests.cs" company="natsnudasoft">
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
    using System.ComponentModel;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public sealed class PropertyChangedRaisedAssertionTests
    {
        private static readonly Type SutType = typeof(PropertyChangedRaisedAssertion);

        [Fact]
        public void ConstructorHasCorrectGuardClauses()
        {
            var fixture = new Fixture();
            fixture.Inject<IEqualityComparer<object>>(EqualityComparer<object>.Default);
            var assertion = new GuardClauseAssertion(fixture);

            assertion.Verify(SutType.GetConstructors());
        }

        [Fact]
        public void ConstructorSetsCorrectInitializedMembers()
        {
            var fixture = new Fixture();
            fixture.Inject<IEqualityComparer<object>>(EqualityComparer<object>.Default);
            var assertion = new ConstructorInitializedMemberAssertion(fixture);

            assertion.Verify(
                SutType.GetProperty(nameof(PropertyChangedRaisedAssertion.SpecimenBuilder)));
        }

        [Fact]
        public void ConstructorDoesNotThrow()
        {
            var ex = Record.Exception(() => new PropertyChangedRaisedAssertion(
                new Mock<ISpecimenBuilder>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void Constructor2DoesNotThrow()
        {
            var ex = Record.Exception(() => new PropertyChangedRaisedAssertion(
                new Mock<ISpecimenBuilder>().Object,
                new Mock<IEqualityComparer<object>>().Object));

            Assert.Null(ex);
        }

        [Fact]
        public void VerifyWithDoesNotImplementInterfacePropertyChangedDoesNotRaisePropertyChanged()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<PropertyChangedRaisedAssertion>();
            var testType = typeof(DoesNotImplementInterfacePropertyChangedTest);
            var testHelper = new DoesNotImplementInterfacePropertyChangedTest();
            var propertyChangedRaised = false;
            testHelper.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };
            fixture.Inject(testHelper);

            sut.Verify(testType.GetProperty(
                nameof(DoesNotImplementInterfacePropertyChangedTest.GuidProperty)));

            Assert.False(propertyChangedRaised);
        }

        [Fact]
        public void VerifyWithPrivateSetPropertyDoesNotRaisePropertyChanged()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<PropertyChangedRaisedAssertion>();
            var testType = typeof(PropertyChangedPrivateSetTest);
            var testHelper = new PropertyChangedPrivateSetTest();
            var propertyChangedRaised = false;
            testHelper.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };
            fixture.Inject(testHelper);

            sut.Verify(testType.GetProperty(
                nameof(PropertyChangedPrivateSetTest.GuidProperty)));

            Assert.False(propertyChangedRaised);
        }

        [Fact]
        public void VerifyWithDoesNotRaisePropertyChangedThrows()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<PropertyChangedRaisedAssertion>();
            var testType = typeof(PropertyChangedDoesNotRaiseTest);
            var testHelper = new PropertyChangedDoesNotRaiseTest();
            var propertyChangedRaised = false;
            testHelper.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };
            fixture.Inject(testHelper);

            var ex = Record.Exception(() => sut.Verify(testType.GetProperty(
                nameof(PropertyChangedDoesNotRaiseTest.GuidProperty))));

            Assert.IsType<PropertyChangedRaisedException>(ex);
            Assert.False(propertyChangedRaised);
        }

        [Fact]
        public void VerifyWithValueThatCannotBeUniqueThrows()
        {
            var specimenBuilderMock = new Mock<ISpecimenBuilder>();
            specimenBuilderMock
                .Setup(b => b.Create(typeof(Guid), It.IsAny<ISpecimenContext>()))
                .Returns(Guid.Empty);
            specimenBuilderMock
                .Setup(b => b.Create(typeof(PropertyChangedTest), It.IsAny<ISpecimenContext>()))
                .Returns(new PropertyChangedTest());
            var sut = new PropertyChangedRaisedAssertion(specimenBuilderMock.Object);
            var testType = typeof(PropertyChangedTest);

            var ex = Record.Exception(() => sut.Verify(testType.GetProperty(
                nameof(PropertyChangedTest.GuidProperty))));

            Assert.IsType<PropertyChangedRaisedException>(ex);
        }

        [Fact]
        public void VerifyWithExplicitPropertyChangedRaisesPropertyChanged()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<PropertyChangedRaisedAssertion>();
            var testType = typeof(ExplicitPropertyChangedTest);
            var testHelper = new ExplicitPropertyChangedTest();
            var propertyChangedRaised = false;
            testHelper.InternalPropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };
            fixture.Inject(testHelper);

            sut.Verify(testType.GetProperty(nameof(ExplicitPropertyChangedTest.GuidProperty)));

            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public void VerifyWithPropertyChangedRaisesPropertyChanged()
        {
            var fixture = new Fixture();
            var sut = fixture.Create<PropertyChangedRaisedAssertion>();
            var testType = typeof(PropertyChangedTest);
            var testHelper = new PropertyChangedTest();
            var propertyChangedRaised = false;
            testHelper.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };
            fixture.Inject(testHelper);

            sut.Verify(testType.GetProperty(nameof(PropertyChangedTest.GuidProperty)));

            Assert.True(propertyChangedRaised);
        }

        private sealed class DoesNotImplementInterfacePropertyChangedTest
        {
            public event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    this.InternalPropertyChanged += value;
                }

                remove
                {
                    this.InternalPropertyChanged -= value;
                }
            }

            private event PropertyChangedEventHandler InternalPropertyChanged;

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Usage",
                "CA1801:ReviewUnusedParameters",
                MessageId = "value",
                Justification = "Test stub, we don't want to use value.")]
            public Guid GuidProperty
            {
                get
                {
                    return Guid.Empty;
                }

                set
                {
                    this.InternalPropertyChanged
                        ?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GuidProperty)));
                }
            }
        }

        private sealed class PropertyChangedPrivateSetTest : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Usage",
                "CA1801:ReviewUnusedParameters",
                MessageId = "value",
                Justification = "Test stub, we don't want to use value.")]
            public Guid GuidProperty
            {
                get
                {
                    return Guid.Empty;
                }

                private set
                {
                    this.PropertyChanged
                        ?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GuidProperty)));
                }
            }
        }

        private sealed class PropertyChangedDoesNotRaiseTest : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged
            {
                add { }
                remove { }
            }

            public Guid GuidProperty { get; set; }
        }

        private sealed class ExplicitPropertyChangedTest : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler InternalPropertyChanged;

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
            {
                add
                {
                    this.InternalPropertyChanged += value;
                }

                remove
                {
                    this.InternalPropertyChanged -= value;
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Usage",
                "CA1801:ReviewUnusedParameters",
                MessageId = "value",
                Justification = "Test stub, we don't want to use value.")]
            public Guid GuidProperty
            {
                get
                {
                    return Guid.Empty;
                }

                set
                {
                    this.InternalPropertyChanged
                        ?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GuidProperty)));
                }
            }
        }

        private sealed class PropertyChangedTest : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Usage",
                "CA1801:ReviewUnusedParameters",
                MessageId = "value",
                Justification = "Test stub, we don't want to use value.")]
            public Guid GuidProperty
            {
                get
                {
                    return Guid.Empty;
                }

                set
                {
                    this.PropertyChanged
                        ?.Invoke(this, new PropertyChangedEventArgs(nameof(this.GuidProperty)));
                }
            }
        }
    }
}
