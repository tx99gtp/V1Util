using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLExtractor.Utility
{
    static class Utils
    {
        internal static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> collectionOfCollection)
        {
            IEnumerable<IEnumerable<T>> empty = new[] { Enumerable.Empty<T>() };
            return collectionOfCollection.Aggregate(
                empty,
                (accumulator, sequence) =>
                    from accessQuery in accumulator
                    from item in sequence
                    select accessQuery.Concat(new[] { item })
                );
        }
    }
}
