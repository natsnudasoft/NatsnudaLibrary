// <copyright file="IRandom.cs" company="natsnudasoft">
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

namespace Natsnudasoft.NatsnudaLibrary
{
    /// <summary>
    /// Provides an interface describing the actions of a pseudo-random number generator.
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than
        /// <see cref="int.MaxValue"/>.</returns>
        int Next();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than
        /// <paramref name="maxValue"/>.</returns>
        int Next(int maxValue);

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.
        /// </param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.
        /// </param>
        /// <returns>A 32-bit signed integer that is greater than or equal to
        /// <paramref name="minValue"/>, and less than <paramref name="maxValue"/>.</returns>
        int Next(int minValue, int maxValue);

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to fill with random numbers.</param>
        void NextBytes(byte[] buffer);

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less
        /// than 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0,
        /// and less than 1.0.</returns>
        double NextDouble();
    }
}