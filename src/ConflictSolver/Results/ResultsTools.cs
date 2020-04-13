// <copyright file="ResultsTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A helper class for processing the collected results.
    /// </summary>
    internal static class ResultsTools
    {
        /// <summary>
        /// Converts the content of a snapshot to a readable text representation.
        /// </summary>
        /// <param name="snapshot">A collection of the <see cref="MonitoredMod"/> instances
        /// representing a snapshot.</param>
        /// <returns>A string that contains the text representation of the snapshot data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="snapshot"/> is null.</exception>
        public static string SnapshotToString(IEnumerable<MonitoredMod> snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var sb = new StringBuilder();

            foreach (var mod in snapshot)
            {
                sb.Append("Monitored mod '")
                    .Append(mod.ModName)
                    .AppendLine("'");

                foreach (string queriedMember in mod.QueriedMembers)
                {
                    sb.Append(" -> Queried following members: ")
                        .AppendLine(queriedMember);
                }

                foreach (var conflict in mod.Conflicts)
                {
                    sb.Append(" ** Possible conflict with mod '")
                        .Append(conflict.ModName)
                        .AppendLine("'");

                    foreach (string member in conflict.MemberNames)
                    {
                        sb.Append("    - ")
                            .AppendLine(member);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
