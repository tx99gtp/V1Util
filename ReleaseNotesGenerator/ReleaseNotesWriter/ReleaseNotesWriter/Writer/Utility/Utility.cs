using System.Collections.Generic;
using System.Linq;

namespace ReleaseNotesWriter.Writer.Utility
{
    public static class UtilityFunctions
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
