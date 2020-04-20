// <copyright file="MemberInfoExtensions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace ConflictSolver.Tools
{
    /// <summary>
    /// A static class containing extension methods for the <see cref="MemberInfo"/> class.
    /// </summary>
    internal static class MemberInfoExtensions
    {
        /// <summary>Returns a string representation of the <see cref="MemberInfo"/> object including
        /// the member's assembly and class.</summary>
        /// <param name="member">The class member to get a string representation of.</param>
        /// <returns>A string representation of the class member.</returns>
        public static string ToFullString(this MemberInfo member)
        {
            string result = member.ToString();
            int spaceIndex = result.IndexOf(' ');
            if (spaceIndex >= 0)
            {
                string prefix = GetAssemblyPrefix(member.ReflectedType) + member.ReflectedType.Name + ".";
                return result.Insert(spaceIndex + 1, prefix);
            }
            else
            {
                return GetAssemblyPrefix(member.ReflectedType) + result;
            }
        }

        private static string GetAssemblyPrefix(Type type) => type.Assembly.GetName().Name + "::";
    }
}
