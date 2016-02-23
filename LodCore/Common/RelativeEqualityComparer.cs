using System;
using System.Collections.Generic;
using System.Linq;
using Journalist;

namespace Common
{
    public class RelativeEqualityComparer : IEqualityComparer<string>
    {
        private readonly int _appropriateEditDistance;

        public RelativeEqualityComparer(int editDistanceConsideredEqual)
        {
            Require.Positive(editDistanceConsideredEqual, nameof(editDistanceConsideredEqual));

            _appropriateEditDistance = editDistanceConsideredEqual;
        }

        public bool Equals(string original, string compared)
        {
            Require.NotNull(original, nameof(original));
            Require.NotNull(compared, nameof(compared));

            var editDistance = EditDistance(original, compared);
            return editDistance <= _appropriateEditDistance;
        }

        public int GetHashCode(string obj)
        {
            Require.NotNull(obj, nameof(obj));

            return string.Empty.GetHashCode();
        }

        private static int EditDistance(string original, string modified)
        {
            var lenOrig = original.Length;
            var lenDiff = modified.Length;

            var matrix = new int[lenOrig + 1, lenDiff + 1];
            for (var i = 0; i <= lenOrig; i++)
                matrix[i, 0] = i;
            for (var j = 0; j <= lenDiff; j++)
                matrix[0, j] = j;

            for (var i = 1; i <= lenOrig; i++)
            {
                for (var j = 1; j <= lenDiff; j++)
                {
                    var cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                    var vals = new[]
                    {
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost
                    };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[lenOrig, lenDiff];
        }
    }
}