using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReleaseNotesProcessor.ProcessReleaseNotes.Concrete;

namespace ReleaseNotesProcessorTest
{
    [TestClass]
    public class UnitTestReleaseNoteStringToDictionary : ProcessRawReleaseNotes
    {
        [TestMethod]
        public void ReleaseNotesInDictionaryWithCorrectFormat()
        {
            // arrange
            const string beginningString = "|Old|Generate|Functionality|Testing|Some actual release note here|";
            Dictionary<int, string> expectedDictionary = new Dictionary<int, string>()
            {
               {1,"Old"},
               {2,"Generate"},
               {3,"Functionality"},
               {4,"Testing"},
               {5,"Some actual release note here"}
            };

            // act
            Dictionary<int, string>  actualDictionary = SetReleaseNotesInDictionary(beginningString);

            // assert
            Assert.AreEqual(true, AreEqual(expectedDictionary, actualDictionary));
        }

        [TestMethod]
        public void ReleaseNotesInDictionaryWithIncorrectFormat()
        {
            // arrange
            const string beginningString = "Some actual release note here that is not formatted";
            Dictionary<int, string> expectedDictionary = new Dictionary<int, string>()
            {
               {5,"Some actual release note here that is not formatted"}
            };

            // act
            Dictionary<int, string> actualDictionary = SetReleaseNotesInDictionary(beginningString);

            // assert
            Assert.AreEqual(true, AreEqual(expectedDictionary, actualDictionary));
        }

        private static bool AreEqual(Dictionary<int, string> expectedDictionary, Dictionary<int, string> actualDictionary)
        {
            return actualDictionary.Values.SequenceEqual(expectedDictionary.Values) &&
                 actualDictionary.Keys.SequenceEqual(expectedDictionary.Keys);

        }
    }
}
