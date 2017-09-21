using System.Collections.Generic;
using System.Linq;
using AssetDataStructures.FCAsset.Concrete;
using RefinedReleaseNotesDictionary.Dictionary;

namespace ReleaseNoteOutputOrdering
{
    public class ClientOrdering
    {
        public Dictionary<string, List<CReleaseNoteAsset>> GetAssetOrdering(
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> rawReleaseNotes,
            Dictionary<int, Dictionary<int, string>> identifierCollection)
        {
            Dictionary<string, List<CReleaseNoteAsset>> orderedAssetsWithHeaders = new Dictionary<string, List<CReleaseNoteAsset>>();
            Dictionary<CReleaseNoteAsset,Dictionary<int, string>> removeOkPipes = RemoveOKPipes(rawReleaseNotes);
            foreach (var key in identifierCollection.Keys)
            {
                List<CReleaseNoteAsset> allOrderedAssets = new List<CReleaseNoteAsset>();
                Dictionary<int,string> combinationDictionary = identifierCollection[key];
                int keyCount = combinationDictionary.Keys.Count;

                IEnumerable<string> categoryCombination = combinationDictionary.Values;
                IEnumerable<CReleaseNoteAsset> assetsWithCombinations = GetAssetsWithCombination(removeOkPipes,
                    keyCount - 1, categoryCombination.ToArray());

                if (assetsWithCombinations != null && assetsWithCombinations.Any())
                {
                    IEnumerable<CReleaseNoteAsset> orderedByEpics = assetsWithCombinations
                        .OrderByDescending(item => item.SuperScope.ToString());

                    allOrderedAssets.AddRange((orderedByEpics.ToList()));
                    var headerTitle = string.Join(",", categoryCombination.ToArray());
                    orderedAssetsWithHeaders.Add(headerTitle, allOrderedAssets);
                }
            }

            var assetsSetToIncorrect = SetAssetsToInCorrect(rawReleaseNotes, orderedAssetsWithHeaders);

            return assetsSetToIncorrect;
        }

        private Dictionary<string, List<CReleaseNoteAsset>> SetAssetsToInCorrect(
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> rawReleaseNotes,
            Dictionary<string, List<CReleaseNoteAsset>> orderedAssetsWithHeaders)
        {
            foreach (var cReleaseNoteAsset in rawReleaseNotes.Keys)
            {
                bool contained = true;
                foreach (var asset in orderedAssetsWithHeaders.Values)
                {
                    if (asset.Contains(cReleaseNoteAsset))
                    {
                        contained = true;
                        break;
                    }
                    contained = false;
                }
                if (!contained)
                {
                    cReleaseNoteAsset.IsReleaseNoteCorrectlyFormatted = false;
                }
            }
            return orderedAssetsWithHeaders;
        }

        private Dictionary<CReleaseNoteAsset, Dictionary<int, string>> RemoveOKPipes(
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> rawReleaseNotes)
        {
            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> removedOKs = new Dictionary<CReleaseNoteAsset, Dictionary<int, string>>();

            foreach (var keyValue in rawReleaseNotes)
            {
                string pipeValue;
                if (keyValue.Value.TryGetValue(1, out pipeValue) && pipeValue == "OK")
                {
                    keyValue.Value.Remove(1);
                    Dictionary<int, string> tempDictionary = new Dictionary<int, string>();
                    foreach (var key in keyValue.Value.Keys)
                    {
                        tempDictionary.Add(key - 1, keyValue.Value[key]);
                    }
                    removedOKs.Add(keyValue.Key, tempDictionary);
                }
                else
                {
                    removedOKs.Add(keyValue.Key, keyValue.Value);
                }
            }
            return removedOKs;
        }

        private IEnumerable<CReleaseNoteAsset> GetAssetsWithCombination(Dictionary<CReleaseNoteAsset,
            Dictionary<int, string>> allReleaseNoteDictionary, int count, params string[] combination)
        {
            if (count == 0)
            {
                return LinqReleaseNotes.GetAssetsWithValueEqualTo(allReleaseNoteDictionary, count + 1,
                    combination[count]);
            }
            return LinqReleaseNotes.Intersect(GetAssetsWithCombination(
                allReleaseNoteDictionary, count - 1, combination),
                LinqReleaseNotes.GetAssetsWithValueEqualTo(allReleaseNoteDictionary,
                    count + 1, combination[count]));
        }
    }
}