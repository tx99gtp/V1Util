using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLExtractor;

namespace XMLEctractorTest
{
    [TestClass]
    public class ExtractXmlTest
    {
        private static bool AreEqual(Dictionary<int, Dictionary<int, string>> actual,
            Dictionary<int, Dictionary<int, string>> expected)
        {
            bool isEqual = true;
            foreach (var expectedItem in expected)
            {
                Dictionary<int,string> currentDictionary;
                actual.TryGetValue(expectedItem.Key, out currentDictionary);
                if (currentDictionary != null)
                {
                    isEqual = currentDictionary.SequenceEqual(expectedItem.Value);
                }
                if (!isEqual)
                {
                    break;
                }
            }
            return isEqual;
        }
        [TestMethod]
        public void PracticeXmlDiagramCorrectOutput()
        {
            Dictionary<int, Dictionary<int, string>> expectedDictionary = new Dictionary<int, Dictionary<int, string>> 
            {
                {1, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "chipotle"}, {4, "cheese"} } },
                {2, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "chipotle"}, {4, "meat"} } },
                {3, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "mission burrito"}, {4, "cheese"} } },
                {4, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "mission burrito"}, {4, "meat"} } },
                {5, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "uburrito"}, {4, "cheese"} } },
                {6, new Dictionary<int,string> { {1, "hello"}, {2, "tom"}, {3, "uburrito"}, {4, "meat"} } },
                {7, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "chipotle"}, {4, "cheese"} } },
                {8, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "chipotle"}, {4, "meat"} } },
                {9, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "mission burrito"}, {4, "cheese"} } },
                {10, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "mission burrito"}, {4, "meat"} } },
                {11, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "uburrito"}, {4, "cheese"} } },
                {12, new Dictionary<int,string> { {1, "hello"}, {2, "jerry"}, {3, "uburrito"}, {4, "meat"} } },
                {13, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "chipotle"}, {4, "cheese"} } },
                {14, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "chipotle"}, {4, "meat"} } },
                {15, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "mission burrito"}, {4, "cheese"} } },
                {16, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "mission burrito"}, {4, "meat"} } },
                {17, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "uburrito"}, {4, "cheese"} } },
                {18, new Dictionary<int,string> { {1, "hello"}, {2, "john"}, {3, "uburrito"}, {4, "meat"} } },
                {19, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "chipotle"}, {4, "cheese"} } },
                {20, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "chipotle"}, {4, "meat"} } },
                {21, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "mission burrito"}, {4, "cheese"} } },
                {22, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "mission burrito"}, {4, "meat"} } },
                {23, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "uburrito"}, {4, "cheese"} } },
                {24, new Dictionary<int,string> { {1, "hey"}, {2, "tom"}, {3, "uburrito"}, {4, "meat"} } },
                {25, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "chipotle"}, {4, "cheese"} } },
                {26, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "chipotle"}, {4, "meat"} } },
                {27, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "mission burrito"}, {4, "cheese"} } },
                {28, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "mission burrito"}, {4, "meat"} } },
                {29, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "uburrito"}, {4, "cheese"} } },
                {30, new Dictionary<int,string> { {1, "hey"}, {2, "jerry"}, {3, "uburrito"}, {4, "meat"} } },
                {31, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "chipotle"}, {4, "cheese"} } },
                {32, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "chipotle"}, {4, "meat"} } },
                {33, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "mission burrito"}, {4, "cheese"} } },
                {34, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "mission burrito"}, {4, "meat"} } },
                {35, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "uburrito"}, {4, "cheese"} } },
                {36, new Dictionary<int,string> { {1, "hey"}, {2, "john"}, {3, "uburrito"}, {4, "meat"} } },
            };

            Dictionary<int, Dictionary<int, string>> actualDictionary = new ExtractXML().ExtractXMLFile(@"XMLFile1.xml");

            Assert.AreEqual(true, AreEqual(actualDictionary,expectedDictionary));
        }
    }
}
