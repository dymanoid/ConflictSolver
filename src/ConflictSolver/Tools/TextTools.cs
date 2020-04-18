// <copyright file="TextTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.Tools
{
    /// <summary>
    /// A helper class for text transforms.
    /// </summary>
    internal static class TextTools
    {
        /// <summary>
        /// Gets a description text containing a number and a correctly pluralized
        /// description of this number.
        /// </summary>
        /// <param name="number">The number of items to create the description text for.</param>
        /// <param name="singularDescription">The description of a single item (in singular).</param>
        /// <returns>A correctly pluralized description string.</returns>
        public static string GetNumberDescriptionText(int number, string singularDescription)
        {
            string description = "(" + number + " " + singularDescription;
            if (number != 1)
            {
                description += "s";
            }

            description += ")";
            return description;
        }
    }
}
