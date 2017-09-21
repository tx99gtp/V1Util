using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReleaseNotesWriter.Writer.Concrete;
using V1Query.RunQuery.Abstract;
using VersionOne.SDK.ObjectModel;
using AssetDataStructures.FCAsset.Concrete;
using ReleaseNoteController.Properties;
using ReleaseNotesProcessor.ProcessReleaseNotes.Concrete;
using V1Query.RunQuery.Concrete;
using XMLExtractor;
using System;

namespace ReleaseNoteController
{
    public class ReleaseNoteController
    {
        public ICollection<Project> GetProjects()
        {
            ICollection<Project> releaseNoteProjects = new CReleaseNoteQueryFactory().GetProjects();
            return releaseNoteProjects;
        }

        public bool WriteReport(string fileName, string oid, string xmlFileDir, IEnumerable fileTypesEnumerator, IEnumerable releaseNoteReqEnumerable = null)
        {
            Boolean WrongFormatItemsOrReportFailure = true;
            foreach (string fileType in fileTypesEnumerator)
            {
                if (fileType.Contains("XML")) 
                {
                    if (releaseNoteReqEnumerable == null)
                    {
                        Dictionary<CReleaseNoteAsset, Dictionary<int, string>> refinedReleaseNotes = GetRefinedReleaseNotesDictionary(oid);
                        XmlWriter xml = new XmlWriter(refinedReleaseNotes);
                        xml.WriteAllReleaseNotes(fileName);
                    }
                    else
                    {
                        XmlWriteRequiredReleasenotes(fileName, oid, releaseNoteReqEnumerable);
                    }
                }
                else if (fileType.Contains("EXCEL"))
                {
                    if (releaseNoteReqEnumerable == null)
                    {
                        Dictionary<CReleaseNoteAsset, Dictionary<int, string>> refinedReleaseNotes = GetRefinedReleaseNotesDictionary(oid);
                        Dictionary<int,Dictionary<int,string>> extractedXmlIdentifierCollection = new ExtractXML().ExtractXMLFile(xmlFileDir);

                        ExcelWriter excel = new ExcelWriter(refinedReleaseNotes, extractedXmlIdentifierCollection);
                        excel.WriteAllReleaseNotes(fileName);
                    }
                    else
                    {
                        WrongFormatItemsOrReportFailure = ExcelWriteRequiredReleasenotes(fileName, oid, xmlFileDir, releaseNoteReqEnumerable);
                    }
                }
            }
            return WrongFormatItemsOrReportFailure;
        }

        private void XmlWriteRequiredReleasenotes(string fileName, string oid, IEnumerable releaseNoteReqEnumerable)
        {
            foreach (string required in releaseNoteReqEnumerable)
            {
                Dictionary<CReleaseNoteAsset,Dictionary<int,string>> refinedReleaseNotes = GetRefinedReleaseNotesDictionary(oid);
                XmlWriter xml = new XmlWriter(refinedReleaseNotes);
                if (required.Equals("Yes"))
                {
                    WriteRequiredXmlReleaseNotes(fileName + "Yes", true, xml);
                }
                if (required.Equals("YesCmd")) 
                {
                    WriteRequiredXmlReleaseNotes(fileName, true, xml);
                }
                else if (required.Equals("No"))
                {
                    WriteRequiredXmlReleaseNotes(fileName + "No", false, xml);
                }
                else if (required.Equals("AssetsWithNull"))
                {
                    WriteRequiredXmlReleaseNotes(fileName + "WithNull", null, xml);
                }
            }
        }

        private Boolean ExcelWriteRequiredReleasenotes(string fileName, string oid, string xmlFileDir, IEnumerable releaseNoteReqEnumerable)
        {
            Boolean WrongFormatItems = false;
            foreach (string required in releaseNoteReqEnumerable)
            {
                Dictionary<CReleaseNoteAsset, Dictionary<int, string>> refinedReleaseNotes = GetRefinedReleaseNotesDictionary(oid);
                var extractedXmlIdentifierCollection = new ExtractXML().ExtractXMLFile(xmlFileDir);

                ExcelWriter excel = new ExcelWriter(refinedReleaseNotes, extractedXmlIdentifierCollection);
                if (required.Equals("Yes"))
                {
                    WrongFormatItems = (WriteRequiredExcelReleaseNotes(fileName + " Yes", true, excel) || WrongFormatItems);
                }
                if (required.Equals("YesCmd")) 
                {
                    excel.setCmdHide(true);
                    WriteRequiredExcelReleaseNotes(fileName, true, excel);
                }
                else if (required.Equals("No"))
                {
                    WrongFormatItems = (WriteRequiredExcelReleaseNotes(fileName + " No", false, excel) || WrongFormatItems);
                }
                else if (required.Equals("AssetsWithNull"))
                {
                    WrongFormatItems = (WriteRequiredExcelReleaseNotes(fileName + " WithNull", null, excel) || WrongFormatItems);
                }
            }
            return WrongFormatItems;
        }

        private static void WriteRequiredXmlReleaseNotes(string fileName, bool? req, XmlWriter xml)
        {
            xml.WriteRequiredReleasNotes(req, fileName);
        }
        private static Boolean WriteRequiredExcelReleaseNotes(string fileName, bool? req, ExcelWriter excel)
        {
            return excel.WriteRequiredReleaseNotes(req, fileName);
        }

        private Dictionary<CReleaseNoteAsset, Dictionary<int, string>> GetRefinedReleaseNotesDictionary(string oid)
        {
            AbstractQueryFactory qfFactory = new CReleaseNoteQueryFactory(oid);

            ProcessRawReleaseNotes rawReleaseNotes = new ProcessRawReleaseNotes();
            
            rawReleaseNotes.RunProcess(qfFactory.GetAssets().Cast<CReleaseNoteAsset>());

            return rawReleaseNotes.RefinedReleaseNoteDictionary;
        }

        public bool GetDefaultIdentifiersFile(string saveFileLocation)
        {
            File.WriteAllText(saveFileLocation, Resources.AvailableReleaseNoteIdentifiers);
            return true;
        }
    }
}
