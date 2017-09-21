using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using XMLExtractor.Utility;

namespace XMLExtractor
{
    public class ExtractXML
    {
        public Dictionary<int, Dictionary<int, string>> ExtractXMLFile(string filePath)
        {
            XDocument xmlDocument = XDocument.Load(filePath);
            Dictionary<string, HashSet<string>> identifierDictionary = xmlDocument.Descendants("Identifier")
                .ToDictionary(
                identifier => identifier.Attribute("container").Value,
                identifier => new HashSet<string>(identifier.Descendants("Container_Value").Select(item => item.Value)));

            Dictionary<int, Dictionary<int, string>> combinationCollection = GetIdentifierCollection(identifierDictionary);

            return combinationCollection;
        }

        private Dictionary<int, Dictionary<int, string>> GetIdentifierCollection(Dictionary<string, HashSet<string>> identifierDictionary)
        {
            IEnumerable<string> stringCombinationCollection = identifierDictionary
                .Select(outerDictionary => outerDictionary.Value)
                .CartesianProduct().Select(hashSet => hashSet
                    .Aggregate((value, nextValue) => value + "," + nextValue));

            Dictionary<int, Dictionary<int, string>> dictionaryCombinationCollection = new Dictionary<int, Dictionary<int, string>>();

            int combinationKey = 1;
            foreach (string combination in stringCombinationCollection)
            {
                string[] stuff = combination.Split(',');
                Dictionary<int, string> dictionaryOfCombination = stuff.Select((stringValue, intKey) => 
                    new { s = stringValue, i = intKey + 1 })
                    .ToDictionary(key => key.i, value => value.s);

                dictionaryCombinationCollection.Add(combinationKey, dictionaryOfCombination);
                combinationKey++;
            }

            return dictionaryCombinationCollection;
        }
    }
}
