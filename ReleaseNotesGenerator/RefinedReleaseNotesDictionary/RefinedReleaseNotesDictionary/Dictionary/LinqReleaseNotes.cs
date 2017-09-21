using System.Collections.Generic;
using System.Linq;
using AssetDataStructures.FCAsset.Concrete;

namespace RefinedReleaseNotesDictionary.Dictionary
{
    public static class LinqReleaseNotes
    {
        public static IEnumerable<CReleaseNoteAsset> GetAssetsWithPipeCount(Dictionary<CReleaseNoteAsset, Dictionary<int, string>> dic,
            int pipeCount)
        {
            IEnumerable<CReleaseNoteAsset> getAssetswithPipeCount = from keys in dic.Keys
                                                    where dic[keys].Keys.Count == pipeCount
                                                    select keys;
            return getAssetswithPipeCount;
        }

        public static IEnumerable<CReleaseNoteAsset> GetAssetsWithReleaseNoteRequired(
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> dic, bool? rnReq)
        {
            IEnumerable<CReleaseNoteAsset> assetsWithReleaseNoteReq = from keys in dic.Keys
                                                    where keys.ReleaseNoteRequired == rnReq
                                                    select keys;
            return assetsWithReleaseNoteReq;
        }

        public static IEnumerable<CReleaseNoteAsset> GetAssetsWithValueEqualTo(
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> dic, int pipe, string term)
        {
            IEnumerable<CReleaseNoteAsset> assetsWithPipeAsString = from keys in dic.Keys
                                                    where (dic[keys]).ContainsKey(pipe) && term == (dic[keys])[pipe]
                                                    select keys;
            return assetsWithPipeAsString;
        }

        public static IEnumerable<T> Intersect<T>(int enumerableCount, params IEnumerable<T>[] enumerables)
        {
            if (enumerableCount > 0)
            {
                return (from item in enumerables[enumerableCount - 1]
                        select item).Intersect(Intersect(enumerableCount - 1, enumerables));
            }
            else return enumerables[enumerableCount];
        }
        public static IEnumerable<T> Intersect<T>(IEnumerable<T> enumerables1, IEnumerable<T> enumerables2)
        {
                return (from item in enumerables1
                        select item).Intersect(enumerables2);
        }

        public static IEnumerable<T> Union<T>(int enumerableCount, params IEnumerable<T>[] enumerables)
        {
            if (enumerableCount > 0)
            {
                return (from item in enumerables[enumerableCount - 1]
                        select item).Union(Union(enumerableCount - 1, enumerables));
            }
            else return enumerables[enumerableCount];
        }
    }
}
