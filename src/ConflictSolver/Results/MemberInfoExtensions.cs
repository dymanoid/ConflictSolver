// <copyright file="MemberInfoExtensions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Reflection;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A static class containing extension methods for the <see cref="MemberInfo"/> class.
    /// </summary>
    internal static class MemberInfoExtensions
    {
        /// <summary>Returns a string representation of the <see cref="MemberInfo"/> object including the member's class.</summary>
        /// <param name="member">The class member to get a string representation of.</param>
        /// <returns>A string representation of the class member.</returns>
        public static string ToFullString(this MemberInfo member)
        {
            string result = member.ToString();
            int spaceIndex = result.IndexOf(' ');
            return spaceIndex < 0 ? result : result.Insert(spaceIndex + 1, member.ReflectedType.Name + ".");
        }
    }
}
