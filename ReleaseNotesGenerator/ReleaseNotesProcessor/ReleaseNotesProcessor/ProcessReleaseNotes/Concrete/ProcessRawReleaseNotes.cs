using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetDataStructures.FCAsset.Concrete;

namespace ReleaseNotesProcessor.ProcessReleaseNotes.Concrete
{
    public class ProcessRawReleaseNotes
    {
        public Dictionary<CReleaseNoteAsset, Dictionary<int, string>> RefinedReleaseNoteDictionary;

        public ProcessRawReleaseNotes()
        {
            RefinedReleaseNoteDictionary = new Dictionary<CReleaseNoteAsset, Dictionary<int, string>>();
        }

        public void RunProcess(IEnumerable<CReleaseNoteAsset> releaseNotes)
        {
            IEnumerable availableNotes = from rNote in releaseNotes
                where rNote.ReleaseNoteString != null
                select rNote;
            foreach (CReleaseNoteAsset note in availableNotes)
            {
                Dictionary<int, string> refinedReleaseNoteDictionary =
                    SetReleaseNotesInDictionary(note.ReleaseNoteString.ToString());

                note.IsReleaseNoteCorrectlyFormatted = IsReferenceNull(note);
                if (note.IsReleaseNoteCorrectlyFormatted)
                {
                    note.IsReleaseNoteCorrectlyFormatted = IsThereMin5pipes(refinedReleaseNoteDictionary);
                }

               
                RefinedReleaseNoteDictionary.Add(note, refinedReleaseNoteDictionary);
            }
        }

        protected Dictionary<int, string> SetReleaseNotesInDictionary(string releaseNoteString)
        {
            Dictionary<int, string> pipeDictionary = new Dictionary<int, string>();

            if (releaseNoteString.Contains('|'))
            {
                string[] piped = releaseNoteString.Split('|');
                List<string> pipeList = piped.ToList();
                pipeList.RemoveAt(0);

                int key = 1;

                foreach (string pipedString in pipeList)
                {
                    if (!String.IsNullOrWhiteSpace(pipedString))
                    {
                        pipeDictionary.Add(key, pipedString);
                    }
                    key++;
                }
            }
            else
            {
                pipeDictionary.Add(5, releaseNoteString);
            }

            return pipeDictionary;
        }

        private bool IsThereMin5pipes(Dictionary<int, string> refinedReleaseNoteDictionary)
        {
            return refinedReleaseNoteDictionary.Keys.Count >= 5;
        }

        private bool IsReferenceNull(CReleaseNoteAsset note)
        {
            return note.Reference != null;
        }
    }
}
