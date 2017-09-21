using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using ReleaseNoteOutputOrdering;
using ReleaseNotesWriter.Writer.Abstract;
using ReleaseNotesWriter.Writer.Utility;
using AssetDataStructures.FCAsset.Concrete;
using RefinedReleaseNotesDictionary.Dictionary;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using V1ServicesConnector.Classes;
using V1ServicesConnector.Form;
using VersionOne.SDK.ObjectModel;

namespace ReleaseNotesWriter.Writer.Concrete {
    public class ExcelWriter : BaseWriter {
        private const string ReasonIncorrectFormat = "Spelling mistake/One of categories not in template XML";
        private const string ReasonFewerCategories = "Contains fewer categories than template XML minimum";
        private const string WhiteSpace = " ";
        private const string AndSpace = " & ";
        private const string ReferenceIsNull = "Reference is null";
        private const string TableFormat = "Contains table formatting";
        private const string ImageFlag = "Contains image";
        private const int MinimumReleaseNotePipeCount = 5;
        private const int MaximumReleaseNotePipeCount = 9;
        private const int FirstHeader = 0;
        private const int SecondHeader = 1;
        private const int ThirdHeader = 2;
        private const int FourthHeader = 3;
        private const int HeaderListLength = 4;
        private Boolean cmdHide = false;
        private static V1Services _mV1Services;

        public ExcelWriter(V1Services mV1Services) {
            _mV1Services = mV1Services;

        }

        public ExcelWriter(Dictionary<CReleaseNoteAsset, Dictionary<int, string>> refineNotes, Dictionary<int, Dictionary<int, string>> extractedXml) {
            Dictionary = new CReleaseNoteDictionary(refineNotes);
            ExtractedXml = extractedXml;
        }

        public override void WriteAllReleaseNotes(string fileName) {
            Tuple<Application, Workbook, Workbook> xLAppAndWorkbook = SetUpExcelWorkbooks();
            Application xLApp = xLAppAndWorkbook.Item1;
            Workbook AuditWorkbook = xLAppAndWorkbook.Item2;
            Workbook OutputWorkbook = xLAppAndWorkbook.Item3;
            Worksheet AuditWorksheet = AuditWorkbook.ActiveSheet;
            Worksheet OutputWorksheet = OutputWorkbook.ActiveSheet;

            var cReleaseNoteAssets = Dictionary.RefinedDictionary.Keys;
            WriteOutputSheet(Dictionary.RefinedDictionary, OutputWorksheet);
            Worksheet xLWorksheet = AddWorksheet(AuditWorkbook);
            WriteAuditSheet(cReleaseNoteAssets, AuditWorksheet);
            Boolean WrongFormatItems = WrongFormatItemsExist(cReleaseNoteAssets.Where(item => !item.IsReleaseNoteCorrectlyFormatted));
            if (WrongFormatItems) {
                List<string> wrongFormatText = WriteWrongFormatText(cReleaseNoteAssets);
                System.IO.File.WriteAllLines(fileName + " Errors.txt", wrongFormatText);
            }

            AuditWorkbook.SaveAs(fileName + " Audit.xlsx");
            OutputWorkbook.SaveAs(fileName + " Output.xlsx");
            if (cmdHide) xLApp.Quit();
            else xLApp.Visible = true;
        }

        public bool WriteRequiredReleaseNotes(bool? releaseNoteRequired, string fileName) {
            Tuple<Application, Workbook, Workbook> xLAppAndWorkbook = SetUpExcelWorkbooks();
            Application xLApp = xLAppAndWorkbook.Item1;
            Workbook AuditWorkbook = xLAppAndWorkbook.Item2;
            Workbook OutputWorkbook = xLAppAndWorkbook.Item3;
            Worksheet AuditWorksheet = AuditWorkbook.ActiveSheet;
            Worksheet OutputWorksheet = OutputWorkbook.ActiveSheet;

            IEnumerable<CReleaseNoteAsset> cReleaseNoteAssets = LinqReleaseNotes
                .GetAssetsWithReleaseNoteRequired(Dictionary.RefinedDictionary, releaseNoteRequired);

            Dictionary<CReleaseNoteAsset, Dictionary<int, string>> parsedDictionary =
                cReleaseNoteAssets.ToDictionary(cReleaseNoteAsset => cReleaseNoteAsset, cReleaseNoteAsset =>
                    Dictionary.RefinedDictionary[cReleaseNoteAsset]);
            WriteOutputSheet(parsedDictionary, OutputWorksheet);
            var cReleaseNoteAssetsAudit = Dictionary.RefinedDictionary.Keys;
            Boolean WrongFormatItems = WrongFormatItemsExist(cReleaseNoteAssets.Where(item => !item.IsReleaseNoteCorrectlyFormatted));
            if (WrongFormatItems){
            	List<string> wrongFormatText = WriteWrongFormatText(cReleaseNoteAssets);
				System.IO.File.WriteAllLines(fileName + " Errors.txt", wrongFormatText);
            }

            WriteAuditSheet(cReleaseNoteAssetsAudit, AuditWorksheet);

            AuditWorkbook.SaveAs(fileName + " Audit.xlsx");
            OutputWorkbook.SaveAs(fileName + " Output.xlsx");
            if (cmdHide) xLApp.Quit();
            else xLApp.Visible = true;
            return WrongFormatItems;
        }

        private Tuple<Application, Workbook, Workbook> SetUpExcelWorkbooks() {
            Application xLApp = new Application {
                Visible = false,
                DisplayAlerts = false
            };
            Workbook AuditWorkbook = xLApp.Workbooks.Add();
            Workbook OutputWorkbook = xLApp.Workbooks.Add();

            return Tuple.Create(xLApp, AuditWorkbook, OutputWorkbook);
        }

        private Worksheet AddWorksheet(Workbook xLWorkbook) {
            return (Worksheet)xLWorkbook.Worksheets.Add();
        }

        private void WriteAuditSheet(IEnumerable<CReleaseNoteAsset> cReleaseNoteAssets, Worksheet xLWorksheet) {
            DataTable auditTab = SetUpAuditDataTable(cReleaseNoteAssets);
            ExcelConvertDataTable(auditTab, xLWorksheet);
            FormatExcelWorksheet(xLWorksheet, "Audit");
        }

        private void WriteOutputSheet(Dictionary<CReleaseNoteAsset, Dictionary<int, string>> parsedDictionary, Worksheet xLWorksheet) {
            Dictionary<string, List<CReleaseNoteAsset>> outputCollection = new ClientOrdering()
                .GetAssetOrdering(parsedDictionary, ExtractedXml);
            DataTable outputTab = SetUpOutputDataTable(outputCollection);
            ExcelConvertDataTable(outputTab, xLWorksheet);
            FormatExcelWorksheet(xLWorksheet, "Output");
        }

        private DataTable SetUpOutputDataTable(Dictionary<string, List<CReleaseNoteAsset>> collection) {
            DataTable outputTab = LoadOutputTabColumns();

            LoadOutputTabRows(collection, outputTab);
            return outputTab;
        }

        private DataTable SetUpAuditDataTable(IEnumerable<CReleaseNoteAsset> collection) {
            DataTable auditTab = LoadAuditTabColumns();

            LoadAuditTabRows(collection, auditTab);
            return auditTab;

        }

        private void FormatHeaderTitles(IReadOnlyList<string> headerTitles, DataTable outputTab) {
            if (headerTitles.Count >= HeaderListLength) {
                string existencePipe = headerTitles[FirstHeader];
                string majorAreaPipe = headerTitles[SecondHeader];
                string categoryPipe = headerTitles[ThirdHeader];
                string changeTypePipe = headerTitles[FourthHeader];

                string typeDesignation = headerTitles[SecondHeader];
            }
        }

        private List<string> RemoveFirstFewPipesInReleaseNoteString(List<string> releaseNotePipes, List<string> headerTitles) {
            if (releaseNotePipes != null) {
                releaseNotePipes.RemoveAt(0);
                if (releaseNotePipes[0] == "OK" || releaseNotePipes[0].Contains("\n")) {
                    releaseNotePipes.RemoveAt(0);
                }
            }
            foreach (string item in headerTitles) {
                if (releaseNotePipes != null) {
                    releaseNotePipes.RemoveAt(0);
                }
            }
            return releaseNotePipes;
        }

        private void FormatExcelWorksheet(Worksheet xLWorksheet, string tabName) {
            xLWorksheet.Columns.EntireColumn.AutoFit();
            xLWorksheet.Rows.EntireRow.AutoFit();

            FreezeColumnTitles(xLWorksheet);
            xLWorksheet.Name = tabName;

            AddHyperLinkToItemsURLColumn(xLWorksheet, tabName);
        }

        private void FreezeColumnTitles(Worksheet xLWorksheet) {
            xLWorksheet.Application.ActiveWindow.SplitRow = 1;
            xLWorksheet.Application.ActiveWindow.FreezePanes = true;
        }

        private void AddHyperLinkToItemsURLColumn(Worksheet xLWorksheet, string tabName) {
            if (tabName == "Incorrectly Formatted" || tabName == "Audit") {
                int urlColumn = 0;
                for (int i = 1; i < xLWorksheet.Columns.Count; i++) {
                    Range range = xLWorksheet.Cells[1, i] as Range;
                    if (range != null && (string)range.Value == "URL") {
                        urlColumn = i;
                        break;
                    }
                }
                Range used = xLWorksheet.UsedRange;
                int lastUsedRow = used.Row + used.Rows.Count;
                for (int count = 2; count < lastUsedRow; count++) {
                    try {
                        string cellValue = String.Empty;
                        if (urlColumn != 0) {
                            Range range = xLWorksheet.Cells[count, urlColumn] as Range;
                            if (range != null)
                                cellValue = (string)range.Value;
                        }
                        if (cellValue != String.Empty) {
                            Range cellRange = ((Range)xLWorksheet.Cells[count, urlColumn]);
                            xLWorksheet.Hyperlinks.Add(cellRange, cellValue);
                        }
                    }
                    catch {
                        //ignore
                    }
                }
            }
        }

        private void ExcelConvertDataTable(DataTable dataTable, Worksheet xLWorksheet) {
            for (int i = 0; i < dataTable.Columns.Count; i++) {
                xLWorksheet.Cells[1, (i + 1)] = dataTable.Columns[i].ColumnName;
                xLWorksheet.Cells[1, (i + 1)].Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                xLWorksheet.Cells[1, (i + 1)].Font.Bold = true;
            }

            for (int i = 0; i < dataTable.Rows.Count; i++) {
                for (int j = 0; j < dataTable.Columns.Count; j++) {
                    xLWorksheet.Cells[(i + 2), (j + 1)] = dataTable.Rows[i][j];
                }
            }
        }

        private DataTable LoadIncorrectTabColumns() {
            DataTable incorrectTab = new DataTable();
            incorrectTab.Columns.Add("ID");
            incorrectTab.Columns.Add("Reference");
            incorrectTab.Columns.Add("Source");
            incorrectTab.Columns.Add("Epic");
            incorrectTab.Columns.Add("Release Note");
            incorrectTab.Columns.Add("URL");
            incorrectTab.Columns.Add("Reason incorrectly formatted");

            return incorrectTab;
        }

        private DataTable LoadOutputTabColumns() {
            DataTable outputTab = new DataTable();
            outputTab.Columns.Add("Existence");
            outputTab.Columns.Add("Major Area");
            outputTab.Columns.Add("Category");
            outputTab.Columns.Add("Change Type");
            outputTab.Columns.Add("Type Designation");
            outputTab.Columns.Add("Reference");
            outputTab.Columns.Add("Source");
            outputTab.Columns.Add("Epic");
            outputTab.Columns.Add("ID");
            outputTab.Columns.Add("Release Note");
            outputTab.Columns.Add("Top Summary");
            outputTab.Columns.Add("Usability Indicator");
            return outputTab;
        }

        private DataTable LoadAuditTabColumns() {
            DataTable auditTab = new DataTable();
            auditTab.Columns.Add("ID");
            auditTab.Columns.Add("Name");
            auditTab.Columns.Add("Split From ID");
            auditTab.Columns.Add("Epic");
            auditTab.Columns.Add("Issue");
            auditTab.Columns.Add("Reference");
            auditTab.Columns.Add("Goals");
            auditTab.Columns.Add("Source");
            auditTab.Columns.Add("Status");
            auditTab.Columns.Add("Release Notes Required");
            auditTab.Columns.Add("Release Notes");
            auditTab.Columns.Add("Change Date");
            auditTab.Columns.Add("Sprint");
            auditTab.Columns.Add("Project");
            auditTab.Columns.Add("Team");
            auditTab.Columns.Add("Owner");
            auditTab.Columns.Add("URL");

            return auditTab;
        }

        private void LoadOutputTabRows(Dictionary<string, List<CReleaseNoteAsset>> collection, DataTable outputTab) {
            foreach (KeyValuePair<string, List<CReleaseNoteAsset>> dictionary in collection) {
                List<string> headerTitles = dictionary.Key.Split(',').ToList();
                FormatHeaderTitles(headerTitles, outputTab);

                string existencePipe = headerTitles[0];
                string majorAreaPipe = headerTitles[1];
                string categoryPipe = headerTitles[2];
                string changeTypePipe = headerTitles[3];

                IEnumerable<CReleaseNoteAsset> correctlyFormatted = dictionary.Value
                    .Where(element => element.IsReleaseNoteCorrectlyFormatted
                        && (element.ReleaseNoteString.ToString().Split('|').Count() <= MaximumReleaseNotePipeCount));
                foreach (CReleaseNoteAsset asset in correctlyFormatted) {
                    try {
                        List<string> dataRow = new List<string>();

                        dataRow.Add(existencePipe);
                        dataRow.Add(majorAreaPipe);
                        dataRow.Add(categoryPipe);
                        dataRow.Add(changeTypePipe);
                        dataRow.Add(majorAreaPipe);
                        string reference = (asset.Reference == null) ? WhiteSpace : asset.Reference.ToString();
                        dataRow.Add(reference);
                        string source = (asset.SourceName == null) ? WhiteSpace : asset.SourceName.ToString();
                        dataRow.Add(source);
                        string super = (asset.SuperNumber == null) ? WhiteSpace : asset.SuperNumber.ToString();
                        dataRow.Add(super);
                        string id = (asset.Id == null) ? WhiteSpace : asset.Id.ToString();
                        dataRow.Add(id);
                        List<string> releaseNotePipes = (asset.ReleaseNoteString == null)
                            ? null
                            : asset.ReleaseNoteString.ToString().Split('|').ToList();

                        releaseNotePipes = RemoveFirstFewPipesInReleaseNoteString(releaseNotePipes, headerTitles);
                        for (int i = 0; i < releaseNotePipes.Count(); i++) {
                            releaseNotePipes[i] = ReplaceCharacters(releaseNotePipes[i]);
                        }
                        dataRow.AddRange(releaseNotePipes.ToArray());
                        outputTab.Rows.Add(dataRow.ToArray());
                    }
                    catch (Exception e) {
                        throw new Exception("Output Tab had an error: " + e.Message);
                    }
                }
            }
        }

        private void LoadAuditTabRows(IEnumerable<CReleaseNoteAsset> collection, DataTable auditTab) {
            PrimaryWorkitem item;
            foreach (CReleaseNoteAsset asset in collection) {
                try {
                    item = _mV1Services.ObjModel.ObjInstance.Get.PrimaryWorkitemByDisplayID(asset.Id.ToString());
                    ICollection<Issue> issues = item.Issues;
                    string id = (asset.Id == null) ? WhiteSpace : asset.Id.ToString();
                    string name = (asset.Name == null) ? WhiteSpace : asset.Name.ToString();
                    string splitFromId = (asset.SplitFromNumber == null) ? WhiteSpace : asset.SplitFromNumber.ToString();
                    string epic = (asset.SuperNumber == null) ? WhiteSpace : asset.SuperNumber.ToString();
                    string issueList = " ";
                    foreach (var issue in issues) {
                        issueList += issue.DisplayID + " ";
                    }
                    string reference = (asset.Reference == null) ? WhiteSpace : asset.Reference.ToString();
                    string goals = (asset.GoalsName == null) ? WhiteSpace : asset.GoalsName.ToString();
                    string source = (asset.SourceName == null) ? WhiteSpace : asset.SourceName.ToString();
                    string status = (asset.Status == null) ? WhiteSpace : asset.Status.ToString();
                    string releaseNotesRequired = (asset.ReleaseNoteRequired == null)
                        ? WhiteSpace
                        : asset.ReleaseNoteRequired.ToString();
                    string releaseNoteString = (asset.ReleaseNoteString == null)
                        ? WhiteSpace
                        : asset.ReleaseNoteString.ToString();
                    string changeDate = (asset.ChangeDate == null) ? WhiteSpace : asset.ChangeDate.ToString();
                    string sprint = (item.Iteration == null) ? WhiteSpace : item.Iteration.Name;
                    string project = (item.Project == null)? WhiteSpace : item.Project.Name;
                    string team = WhiteSpace;
                    try {
                        if (asset.TeamName != null) {
                            team = (asset.TeamName.IsAny() == false)
                            ? WhiteSpace
                            : String.Join("\n", asset.TeamName.ToArray());
                        }
                    }
                    catch {
                        team = "(No Team Assigned)";
                    }
                    string owner = WhiteSpace;
                    if (asset.OwnersName != null) {
                        owner = (asset.OwnersName.IsAny() == false)
                            ? WhiteSpace
                            : String.Join("\n", asset.OwnersName.ToArray());
                    }
                    string url = (asset.URL == null) ? WhiteSpace : asset.URL.ToString();

                    auditTab.Rows.Add(id, name, splitFromId, epic, issueList, reference, goals, source, status,
                        releaseNotesRequired, ReplaceCharacters(releaseNoteString), changeDate, sprint, project, team, owner, url);
                }
                catch (Exception e) {
                    throw new Exception("Audit Tab had an error: " + e.Message);
                }
            }
        }

        private List<string> WriteWrongFormatText(IEnumerable<CReleaseNoteAsset> collection) {
            IEnumerable<CReleaseNoteAsset> inCorrectlyFormatted = collection
                .Where(item => !item.IsReleaseNoteCorrectlyFormatted);


            List<string> wrongFormatText = new List<string>();
            foreach (CReleaseNoteAsset asset in inCorrectlyFormatted) {
                string nextEntry = (asset.Id == null) ? WhiteSpace : "ID: " + asset.Id.ToString();
                wrongFormatText.Add(nextEntry);
                nextEntry = (asset.URL == null) ? WhiteSpace : asset.URL.ToString();
                wrongFormatText.Add("URL: " + nextEntry);
                string releaseNotePipes = (asset.ReleaseNoteString == null)
                    ? WhiteSpace
                    : asset.ReleaseNoteString.ToString();
                string reason = WhiteSpace;
                if (IncorrectFormatCheck(releaseNotePipes)) {
                    reason = AddReason(reason, ReasonIncorrectFormat);
                }
                if (FewerCategoriesCheck(releaseNotePipes))
                {
                    reason = AddReason(reason, ReasonFewerCategories);
                }
                if (ReferenceNullCheck(asset)) {
                    reason = AddReason(reason, ReferenceIsNull);
                }
                if (TableCheck(releaseNotePipes)) {
                    reason = AddReason(reason, TableFormat);
                }
                if (ImageCheck(releaseNotePipes)) {
                    reason = AddReason(reason, ImageFlag);
                }
                wrongFormatText.Add("\tReason: " + reason);
                string team = WhiteSpace;
                try
                {
                    if (asset.TeamName != null)
                    {
                        team = (asset.TeamName.IsAny() == false)
                        ? WhiteSpace
                        : String.Join("\n", asset.TeamName.ToArray());
                    }
                }
                catch
                {
                    team = "(No Team Assigned)";
                }
                wrongFormatText.Add("\tTeam: " + team);
                string owner = WhiteSpace;
                if (asset.OwnersName != null)
                {
                    owner = (asset.OwnersName.IsAny() == false)
                        ? WhiteSpace
                        : String.Join("\n", asset.OwnersName.ToArray());
                }
                wrongFormatText.Add("\tOwner: " + owner);
                nextEntry = (asset.Reference == null) ? "\tReference: (NULL)" : "\tReference: " + asset.Reference.ToString();
                wrongFormatText.Add(nextEntry);
                nextEntry = (asset.SuperNumber == null) ? "\tEpic: (Unassigned)" : "\tEpic: " + asset.SuperNumber.ToString();
                wrongFormatText.Add(nextEntry);
                nextEntry = (asset.SourceName == null) ? "\tSource: (Unassigned)" : "\tSource: " + asset.SourceName.ToString();
                wrongFormatText.Add(nextEntry);
                wrongFormatText.Add("Release Note: " + ReplaceCharacters(releaseNotePipes));
                wrongFormatText.Add(WhiteSpace);
            }

            return wrongFormatText;
        }

        private Boolean WrongFormatItemsExist(IEnumerable<CReleaseNoteAsset> collection) {
            return collection != null && collection.Any();
        }

        public void setCmdHide(Boolean hideApp)
        {
            cmdHide = hideApp;
        }

        private string ReplaceCharacters(string releaseNote) {
            releaseNote = releaseNote.Replace("&gt;", ">");
            releaseNote = releaseNote.Replace("\n", "\n ");
            releaseNote = releaseNote.Replace("CONTAINSIMAGEFLAG", "");
            return releaseNote;
        }
        private bool IncorrectFormatCheck(string pipes) {
            if (pipes.Split('|').ToList().Count >= MinimumReleaseNotePipeCount)
                return true;
            else return false;
        }
        private bool FewerCategoriesCheck(string pipes)
        {
            if (pipes.Split('|').ToList().Count < MinimumReleaseNotePipeCount) return true;
            else return false; 
        }
        private bool ReferenceNullCheck(CReleaseNoteAsset asset) {
            if (asset.Reference == null)
                return true;
            else return false;
        }
        private bool TableCheck(string pipes) {
            if (pipes.Contains("\n\n\n"))
                return true;
            else return false;
            
        }
        private bool ImageCheck(string pipes) {
            if (pipes.Contains("CONTAINSIMAGEFLAG")) {
                return true;
            }
            else return false;
        }
        private string AddReason(string reason, string givenreason) {
            if (reason == WhiteSpace) {
                reason = reason + givenreason;
            }
            else{
                reason = reason + AndSpace + givenreason;
            }
            return reason;
        }
    }
}
