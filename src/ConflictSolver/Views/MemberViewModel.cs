// <copyright file="MemberViewModel.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Results;

namespace ConflictSolver.Views
{
    /// <summary>
    /// A viewmodel for the <see cref="MemberAccessInfo"/> struct.
    /// </summary>
    internal sealed class MemberViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberViewModel"/> class.
        /// </summary>
        /// <param name="memberAccess">The <see cref="MemberAccessInfo"/> struct to create a viewmodel for.</param>
        public MemberViewModel(in MemberAccessInfo memberAccess)
        {
            AccessInfo = $"[{memberAccess.AccessTarget,10}] [{memberAccess.AccessTypes,18}] ";
            var memberClass = memberAccess.Member.DeclaringType;
            Assembly = memberClass.Assembly.GetName().Name + "::";
            Class = memberClass.Name + ".";
            string memberString = memberAccess.Member.ToString();
            int spaceIndex = memberString.IndexOf(' ') + 1;
            if (spaceIndex > 0)
            {
                Type = memberString.Remove(spaceIndex);
                Name = memberString.Substring(spaceIndex);
            }
            else
            {
                Type = string.Empty;
                Name = memberString;
            }
        }

        /// <summary>
        /// Gets the member access information string.
        /// </summary>
        public string AccessInfo { get; }

        /// <summary>
        /// Gets the member type or method return type string.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the member assembly name string.
        /// </summary>
        public string Assembly { get; }

        /// <summary>
        /// Gets the member declaring class name string.
        /// </summary>
        public string Class { get; }

        /// <summary>
        /// Gets the full member name string.
        /// </summary>
        public string Name { get; }
    }
}
