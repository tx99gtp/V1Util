using System.Collections.Generic;
using RefinedReleaseNotesDictionary.Dictionary;

namespace ReleaseNotesWriter.Writer.Abstract
{
    public abstract class BaseWriter
    {
        internal CReleaseNoteDictionary Dictionary;
        internal Dictionary<int, Dictionary<int, string>> ExtractedXml;
        public abstract void WriteAllReleaseNotes(string fileName);
    }
}
