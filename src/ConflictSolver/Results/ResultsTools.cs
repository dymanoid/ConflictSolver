// <copyright file="ResultsTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

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
        public static string SnapshotToString(IEnumerable<MonitoredMod> snapshot)
        {
            var sb = new StringBuilder();

            foreach (var mod in snapshot)
            {
                sb.Append("Monitored mod '")
                    .Append(mod.ModName)
                    .AppendLine("'");

                sb.Append(" -> Queried following members: ");

                foreach (var queriedMember in mod.QueriedMembers)
                {
                    sb.Append(queriedMember.AccessTarget)
                        .Append(" - ")
                        .Append(queriedMember.AccessTypes)
                        .Append(" - ")
                        .AppendLine(queriedMember.MemberName);
                }

                foreach (var conflict in mod.Conflicts)
                {
                    sb.Append(" ** Possible conflict with mod '")
                        .Append(conflict.ModName)
                        .AppendLine("'");

                    foreach (var member in conflict.ConflictingMembers)
                    {
                        sb.Append("    - ")
                            .Append(member.AccessTarget)
                            .Append(" - ")
                            .Append(member.AccessTypes)
                            .Append(" - ")
                            .AppendLine(member.MemberName);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
