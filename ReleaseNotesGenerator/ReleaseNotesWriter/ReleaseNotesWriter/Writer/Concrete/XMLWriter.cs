using System;
using ReleaseNotesWriter.Writer.Utility;
using System.Collections.Generic;
using System.Linq;
using AssetDataStructures.FCAsset.Concrete;
using RefinedReleaseNotesDictionary.Dictionary;
using ReleaseNotesWriter.Writer.Abstract;

namespace ReleaseNotesWriter.Writer.Concrete
{
    public class XmlWriter : BaseWriter
    {
        public XmlWriter(Dictionary<CReleaseNoteAsset,Dictionary<int,string>> refineNotes)
        {
            Dictionary = new CReleaseNoteDictionary(refineNotes);
        }

        public override void WriteAllReleaseNotes(string fileName)
        {
            try
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(fileName.Contains(".xml") ? fileName : fileName + ".xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("AllReleaseNotes");

                    foreach (CReleaseNoteAsset cReleaseNoteAsset in Dictionary.RefinedDictionary.Keys)
                    {
                        WriteReleaseNoteElement(writer, cReleaseNoteAsset);
                        WriteReleaseNotePipesElement(writer, cReleaseNoteAsset);

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void WriteReleaseNoteElement(System.Xml.XmlWriter writer, CReleaseNoteAsset cReleaseNoteAsset)
        {
            try
            {
                writer.WriteStartElement("ReleaseNote");
                writer.WriteElementString("ID", cReleaseNoteAsset.Id.Equals(null) ? String.Empty : cReleaseNoteAsset.Id.ToString());
                writer.WriteElementString("Name", cReleaseNoteAsset.Name.Equals(null) ? String.Empty : cReleaseNoteAsset.Name.ToString());
                writer.WriteElementString("Description",cReleaseNoteAsset.Description.Equals(null)? String.Empty:cReleaseNoteAsset.Description.ToString());
                writer.WriteElementString("Source",cReleaseNoteAsset.SourceName.Equals(null)? String.Empty: cReleaseNoteAsset.SourceName.ToString());
                writer.WriteElementString("Status",cReleaseNoteAsset.Status.Equals(null)?String.Empty:cReleaseNoteAsset.Status.ToString());
                writer.WriteElementString("ReleaseNoteRequired", cReleaseNoteAsset.ReleaseNoteRequired.ToString());
                writer.WriteElementString("URL", cReleaseNoteAsset.URL.ToString());

                if (cReleaseNoteAsset.SuperName != null)
                {
                    writer.WriteElementString("Associated_SuperName", cReleaseNoteAsset.SuperName.ToString());    
                }
                if (cReleaseNoteAsset.SuperNumber != null)
                {
                    writer.WriteElementString("Associated_SuperNumber", cReleaseNoteAsset.SuperNumber.ToString());
                }
            }
            catch
            {
                // ignored
            }
        }
        private void WriteReleaseNotePipesElement(System.Xml.XmlWriter writer, CReleaseNoteAsset cReleaseNoteAsset)
        {
            writer.WriteStartElement("Categories");
            foreach (int i in Dictionary.RefinedDictionary[cReleaseNoteAsset].Keys)
            {
                writer.WriteElementString("Category" + i, Dictionary.RefinedDictionary[cReleaseNoteAsset][i]);
            }
        }

        public bool WriteRequiredReleasNotes(bool? releaseNoteRequired, string fileName)
        {
            var assets = LinqReleaseNotes.GetAssetsWithReleaseNoteRequired(
                Dictionary.RefinedDictionary, releaseNoteRequired);

            IEnumerable<CReleaseNoteAsset> cReleaseNoteAssets = 
                assets as CReleaseNoteAsset[] ?? assets.ToArray();
            if (!UtilityFunctions.IsAny(cReleaseNoteAssets))
            {
                NoAssetsAvailableToPrint(fileName);
                return false;
            }

            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(fileName + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("AllReleaseNotes");

                foreach (CReleaseNoteAsset cReleaseNoteAsset in cReleaseNoteAssets)
                {
                    writer.WriteStartElement("ReleaseNote");
                    writer.WriteElementString("ID", cReleaseNoteAsset.Id.ToString());
                    writer.WriteElementString("Name", cReleaseNoteAsset.Name.ToString());
                    writer.WriteElementString("URL", cReleaseNoteAsset.URL.ToString());
                    writer.WriteElementString("ReleaseNoteRequired", cReleaseNoteAsset.ReleaseNoteRequired.ToString());

                    writer.WriteStartElement("Categories");

                    foreach (int i in Dictionary.RefinedDictionary[cReleaseNoteAsset].Keys)
                    {
                        writer.WriteElementString("Category", Dictionary.RefinedDictionary[cReleaseNoteAsset][i]);
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return true;
        }

        private void NoAssetsAvailableToPrint(string fileName)
        {
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(fileName+".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Exception");
                writer.WriteElementString("TheException", "There are no assets available");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
