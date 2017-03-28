// <copyright file="EqualsOverrideAssertionVerifyHelper.cs" company="natsnudasoft">
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

namespace Natsnudasoft.NatsnudasoftTests.Helper
{
    using System;

    public class EqualsOverrideAssertionVerifyHelper :
        IEquatable<EqualsOverrideAssertionVerifyHelper>,
        IEquatable<bool>
    {
        private readonly bool nullResult;
        private readonly bool sameReferenceResult;
        private readonly Func<bool> equalsResultFactory;

        public EqualsOverrideAssertionVerifyHelper(
            bool nullResult,
            bool sameReferenceResult,
            bool equalsResult)
        {
            this.nullResult = nullResult;
            this.sameReferenceResult = sameReferenceResult;
            this.equalsResultFactory = () => equalsResult;
        }

        public EqualsOverrideAssertionVerifyHelper(
            bool nullResult,
            bool sameReferenceResult,
            Func<bool> equalsResultFactory)
        {
            this.nullResult = nullResult;
            this.sameReferenceResult = sameReferenceResult;
            this.equalsResultFactory = equalsResultFactory;
        }

        public virtual bool Equals(bool other)
        {
            return other == this.equalsResultFactory();
        }

        public virtual bool Equals(bool? other)
        {
            bool result;
            if (other.HasValue)
            {
                result = other.Value == this.equalsResultFactory();
            }
            else
            {
                result = false;
            }

            return result;
        }

        public virtual bool Equals(EqualsOverrideAssertionVerifyHelper other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = this.nullResult;
            }
            else if (ReferenceEquals(other, this))
            {
                result = this.sameReferenceResult;
            }
            else
            {
                result = this.equalsResultFactory() || other.equalsResultFactory();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as EqualsOverrideAssertionVerifyHelper);
        }

        public virtual bool EqualsWrongName(object other)
        {
            return this.Equals(other as EqualsOverrideAssertionVerifyHelper);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
