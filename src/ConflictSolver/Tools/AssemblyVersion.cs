// <copyright file="AssemblyVersion.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.Tools
{
    /// <summary>
    /// A static class that provides access to the assembly version.
    /// </summary>
    internal static class AssemblyVersion
    {
        /// <summary>
        /// Gets the current assembly version in the format 'major.minor.patch'.
        /// </summary>
        public static string SemVer
        {
            get
            {
                var fullVersion = typeof(AssemblyVersion).Assembly.GetName().Version;
                return fullVersion == null
                    ? Strings.UnknownVersion
                    : fullVersion.Major + "." + fullVersion.Minor + "." + fullVersion.Build;
            }
        }
    }
}
