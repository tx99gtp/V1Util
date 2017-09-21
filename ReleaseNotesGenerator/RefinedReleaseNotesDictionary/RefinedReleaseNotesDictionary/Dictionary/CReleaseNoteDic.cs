using System.Collections.Generic;
using AssetDataStructures.FCAsset.Concrete;

namespace RefinedReleaseNotesDictionary.Dictionary
{
    public class CReleaseNoteDictionary
    {
        public Dictionary<CReleaseNoteAsset, Dictionary<int, string>> RefinedDictionary { set; get; }
        public CReleaseNoteDictionary(Dictionary<CReleaseNoteAsset, Dictionary<int, string>> inputDictionary)
        {
            RefinedDictionary = inputDictionary;
        }
    }
}
