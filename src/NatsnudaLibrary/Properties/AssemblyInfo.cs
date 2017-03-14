// <copyright file="AssemblyInfo.cs" company="natsnudasoft">
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

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(true)]

// Internals visible to unit testing and Moq proxy.
#pragma warning disable MEN002 // Line is too long
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
[assembly: InternalsVisibleTo("Natsnudasoft.NatsnudasoftTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100652536ee8cd36e26a6192faf3cf6a3c19bc831746992d18a83d9df9cf9e70d431249181f7c926cadf6547013b219042b0a2e09914d7bbee366d08ab344f75b21a48718daecedde0262237fbc4763a043cfe10199e1fa671db1c8902ef11c4b355c8dc8ef0143fda52ad9d6c3c9c331c58adc24de0893c58938375a0eac4478df")]
#pragma warning restore MEN002 // Line is too long

[assembly: AssemblyTitle("NatsnudaLibrary")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("NatsnudaLibrary")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("9b0ad511-af08-4a36-9a68-edcbf2edf605")]
