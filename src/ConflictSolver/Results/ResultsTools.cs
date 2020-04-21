// <copyright file="ResultsTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConflictSolver.Monitor;

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
        /// <param name="includeOwnModQueries">A value indicating whether the <see cref="AccessTarget.OwnMod"/>
        /// items should be included.</param>
        /// <returns>A string that contains the text representation of the snapshot data.</returns>
        public static string SnapshotToString(IEnumerable<MonitoredMod> snapshot, bool includeOwnModQueries)
        {
            var sb = new StringBuilder();

            foreach (var mod in snapshot)
            {
                sb.Append("Monitored mod '")
                    .Append(mod.ModName)
                    .AppendLine("'");

                var queries = includeOwnModQueries ? mod.QueriedMembers : mod.QueriedMembers.Where(m => m.AccessTarget != AccessTarget.OwnMod);
                if (queries.Any())
                {
                    sb.AppendLine(" -> Queried following members:");
                }

                foreach (var queriedMember in queries)
                {
                    sb.AppendLine(queriedMember.ToString());
                }

                foreach (var conflict in mod.Conflicts)
                {
                    sb.Append(" ** Possible conflict with mod '")
                        .Append(conflict.ModName)
                        .AppendLine("'");

                    var members = includeOwnModQueries
                        ? conflict.ConflictingMembers
                        : conflict.ConflictingMembers.Where(m => m.AccessTarget != AccessTarget.OwnMod);

                    foreach (var member in members)
                    {
                        sb.Append("    - ")
                            .AppendLine(member.ToString());
                    }
                }
            }

            return sb.ToString();
        }
    }
}
