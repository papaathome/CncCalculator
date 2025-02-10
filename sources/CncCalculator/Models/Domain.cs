using System.Collections.ObjectModel;

namespace As.Applications.Models
{
    public static class Domain
    {
        /// <summary>
        /// Lengths.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Lengths = new(
            ["[mm]", "[cm]", "[in]", "[ft]"]
        );

        /// <summary>
        /// Counts.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Counts = new(
            ["[tooth]"]
        );

        /// <summary>
        /// Lengths over counts.
        /// </summary>
        public static readonly ReadOnlyCollection<string> LengthsOverCounts = new(
            ["[mm/tooth]", "[in/tooth]"]
        );

        /// <summary>
        /// Speeds (or lengths over times)
        /// </summary>
        public static readonly ReadOnlyCollection<string> Speeds = new(
          ["[mm/s]", "[mm/min]", "[cm/min]", "[in/s]", "[in/min]", "[ft/min]"]
        );

        /// <summary>
        /// Rotations (or one over times)
        /// </summary>
        public static readonly ReadOnlyCollection<string> Rotations = new(
            ["[rps]", "[rpm]"]
        );
    }
}
