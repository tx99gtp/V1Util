using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;
using ReleaseNoteGenerator.Properties;
using VersionOne.SDK.ObjectModel;
using System.IO;

namespace ReleaseNoteGenerator {
    public partial class Menu : Form {
        public const int yesCheck = 0;
        public const int EXCELCheck = 1;

        public Menu() {
            InitializeComponent();
            listMultipleProjects.DisplayMember = "Text";
            if (String.IsNullOrEmpty(Settings.Default.XMLLocation.ToString())) {
                ShowAllItems(false);
                XMLLocationLabel.Text = "Please Import XML Template File!";
            }
            else {
                ShowAllItems(true);
                XMLLocationLabel.Text = string.Format("XML Location: {0}", Settings.Default.XMLLocation.ToString());
            }
        }

        private void GenerateReport_Click(object sender, EventArgs e) {
            if (AllItemsSet()) {
                int size = listMultipleProjects.Items.Count;
                for (int i = 0; i < size; i++) {
                    SetReportParameters();
                    RemoveAfterGenerate();
                }
            }
        }

        private void MenuLoad(object sender, EventArgs e) {
            LoadTreeView(new ReleaseNoteController.ReleaseNoteController().GetProjects() as ICollection);
        }

        private void LoadTreeView(ICollection projectsCollection) {
            treeView.BeginUpdate();
            TreeNode parentNode = new TreeNode();

            foreach (Project project in projectsCollection) {
                if (!project.Name.StartsWith("-")) {
                    if (!String.IsNullOrEmpty(parentNode.Text)) {
                        parentNode = new TreeNode();
                    }

                    parentNode.Text = project.Name;
                    parentNode.Tag = project;
                    treeView.Nodes.Add(parentNode);
                }
                else {
                    AddChildren(project, parentNode);
                }
            }
            treeView.Sort();
            treeView.ExpandAll();
            treeView.EndUpdate();
        }

        private void AddChildren(Project project, TreeNode parentNode) {
            project.Name = project.Name.Substring(1, project.Name.Length - 1);

            if (project.Name.StartsWith("-")) {
                parentNode = parentNode.LastNode;
                AddChildren(project, parentNode);
            }
            else {
                TreeNode myNode = new TreeNode();
                myNode.Text = project.Name;
                myNode.Tag = project;
                parentNode.Nodes.Add(myNode);
            }
        }

        private Boolean GetFileName(ref string fileName) {
            fileName = GetSelectedItemName();
            string output = Regex.Replace(fileName, @"[^\w]+", " ");
            saveFileDialog.FileName = output;
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                fileName = saveFileDialog.FileName;
                return true;
            }
            else {
                return false;
            }
        }

        private IEnumerable GetReportTypes() {
            if (checkedListBoxrReportType.CheckedItems.Count == 0) {
                throw new Exception("You must Select a Report Type.");
            }
            return checkedListBoxrReportType.CheckedItems;
        }
        private IEnumerable GetRequiredCheckboxEnumerable() {
            if (checkedListBoxReleaseNoteReq.CheckedItems.Count == 0) {
                throw new Exception("You must select what type of Release note you Require.");
            }
            return checkedListBoxReleaseNoteReq.CheckedItems;
        }

        async void SetReportParameters() {
            string fileName = null;

            if (GetFileName(ref fileName)) {
                string selectedListItem = GetSelectedItem();
                string xmlLocation = Settings.Default.XMLLocation;
                IEnumerable reportType = GetReportTypes();
                IEnumerable checkBox = checkBoxPrintAllAssets.Checked ? null : GetRequiredCheckboxEnumerable();

                await System.Threading.Tasks.Task.Run(() => new ReleaseNoteController.ReleaseNoteController().WriteReport(fileName, selectedListItem, xmlLocation,
                        reportType, checkBox));
            }
        }

        private void ShowAllItems(bool hide) {
            treeView.Enabled = hide;
            checkedListBoxReleaseNoteReq.Enabled = hide;
            checkedListBoxReleaseNoteReq.SetItemCheckState(yesCheck, CheckState.Checked);
            checkedListBoxrReportType.Enabled = hide;
            checkedListBoxrReportType.SetItemCheckState(EXCELCheck, CheckState.Checked);
            checkBoxPrintAllAssets.Enabled = hide;
            generateReportButton.Enabled = hide;
        }

        private void CheckBoxPrintAllAssets_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxPrintAllAssets.Checked) {
                checkedListBoxReleaseNoteReq.ClearSelected();
                checkedListBoxReleaseNoteReq.Enabled = false;
            }
            else {
                checkedListBoxReleaseNoteReq.Refresh();
                checkedListBoxReleaseNoteReq.Enabled = true;
            }
        }

        private void ImportXMLCategoryToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialogXMLCategory.ShowDialog() == DialogResult.OK) {
                Settings.Default.XMLLocation = openFileDialogXMLCategory.FileName;
                Settings.Default.Save();
            }
        }

        private void SaveOriginalCategoryFileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saveFileDialogOriginalIdentifierFile.ShowDialog() == DialogResult.OK) {
                new ReleaseNoteController.ReleaseNoteController().GetDefaultIdentifiersFile(saveFileDialogOriginalIdentifierFile.FileName);
            }
        }

        private void ImportXMLTemplateFile_Click(object sender, EventArgs e) {
            if (openFileDialogXMLCategory.ShowDialog() == DialogResult.OK) {
                Settings.Default.XMLLocation = openFileDialogXMLCategory.FileName;
                Settings.Default.Save();
                if (String.IsNullOrEmpty(Settings.Default.XMLLocation.ToString())) {
                    ShowAllItems(false);
                    XMLLocationLabel.Text = "Please Import XML Template File!";
                }
                else {
                    ShowAllItems(true);
                    XMLLocationLabel.Text = string.Format("XML Location: {0}", Settings.Default.XMLLocation.ToString());
                }
            }
        }

        private void checkedListBoxReleaseNoteReq_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void addListbutton_Click(object sender, EventArgs e) {
            if (!listMultipleProjects.Items.Contains(treeView.SelectedNode)) {
                listMultipleProjects.Items.Add(treeView.SelectedNode);
            }
        }

        private void removeListbutton_Click(object sender, EventArgs e) {
            listMultipleProjects.Items.Remove(listMultipleProjects.SelectedItem);
        }

        private void clearListbutton_Click(object sender, EventArgs e) {
            listMultipleProjects.Items.Clear();
        }
        private void RemoveAfterGenerate() {
            listMultipleProjects.Items.RemoveAt(0);
        }
        private string GetSelectedItem() {
            TreeNode selectedNode = (TreeNode)listMultipleProjects.Items[0];
            return selectedNode.Tag.ToString();
        }

        private Boolean AllItemsSet() {
            if (String.IsNullOrEmpty(Settings.Default.XMLLocation)) {
                MessageBox.Show("Please import XML Template.", "No XML Template Imported", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
            if (!String.IsNullOrEmpty(Settings.Default.XMLLocation) && !File.Exists(Settings.Default.XMLLocation)) {
                MessageBox.Show("No XML Template was found at the specified path. Please import an XML Template.", "No XML Template Found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                XMLLocationLabel.Text = "Please Import XML Template File!";
                Settings.Default.XMLLocation = null;
                return false;
            }
            try {
                GetRequiredCheckboxEnumerable();
            }
            catch {
                if (!checkBoxPrintAllAssets.Checked) {
                    MessageBox.Show("Please Select a Release Note Requirement.", "No Requirement Selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
            }
            try {
                GetReportTypes();
            }
            catch {
                MessageBox.Show("Please select a Report Output type.", "No Report Type Selected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
            return true;
        }

        private string GetSelectedItemName()
        {
            TreeNode selectedNode = (TreeNode)listMultipleProjects.Items[0];
            return selectedNode.Text.ToString();
        }
    }
}
