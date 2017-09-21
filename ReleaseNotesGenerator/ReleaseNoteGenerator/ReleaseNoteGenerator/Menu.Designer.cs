namespace ReleaseNoteGenerator {
    partial class Menu {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ProjectGroup = new System.Windows.Forms.GroupBox();
            this.treeView = new System.Windows.Forms.TreeView();
            this.generateReportButton = new System.Windows.Forms.Button();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setXMLCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getOriginalCategoryFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.checkedListBoxrReportType = new System.Windows.Forms.CheckedListBox();
            this.groupBoxReportTypeCheckBox = new System.Windows.Forms.GroupBox();
            this.groupBoxReleaseNotesReq = new System.Windows.Forms.GroupBox();
            this.checkedListBoxReleaseNoteReq = new System.Windows.Forms.CheckedListBox();
            this.checkBoxPrintAllAssets = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialogXMLCategory = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogOriginalIdentifierFile = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.XMLLocationLabel = new System.Windows.Forms.Label();
            this.listMultipleProjects = new System.Windows.Forms.ListBox();
            this.addListbutton = new System.Windows.Forms.Button();
            this.removeListbutton = new System.Windows.Forms.Button();
            this.clearListbutton = new System.Windows.Forms.Button();
            this.ProjectGroup.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.groupBoxReportTypeCheckBox.SuspendLayout();
            this.groupBoxReleaseNotesReq.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectGroup
            // 
            this.ProjectGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectGroup.Controls.Add(this.treeView);
            this.ProjectGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectGroup.Location = new System.Drawing.Point(22, 46);
            this.ProjectGroup.Name = "ProjectGroup";
            this.ProjectGroup.Size = new System.Drawing.Size(480, 589);
            this.ProjectGroup.TabIndex = 37;
            this.ProjectGroup.TabStop = false;
            this.ProjectGroup.Text = "Project List:";
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(14, 19);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(457, 557);
            this.treeView.TabIndex = 25;
            // 
            // generateReportButton
            // 
            this.generateReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateReportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.generateReportButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generateReportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateReportButton.Location = new System.Drawing.Point(514, 592);
            this.generateReportButton.Name = "generateReportButton";
            this.generateReportButton.Size = new System.Drawing.Size(169, 43);
            this.generateReportButton.TabIndex = 38;
            this.generateReportButton.Text = "Generate Report";
            this.generateReportButton.UseVisualStyleBackColor = true;
            this.generateReportButton.Click += new System.EventHandler(this.GenerateReport_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(701, 24);
            this.menuStrip.TabIndex = 39;
            this.menuStrip.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setXMLCategoryToolStripMenuItem,
            this.getOriginalCategoryFileToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.optionsToolStripMenuItem.Text = "File";
            // 
            // setXMLCategoryToolStripMenuItem
            // 
            this.setXMLCategoryToolStripMenuItem.Name = "setXMLCategoryToolStripMenuItem";
            this.setXMLCategoryToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.setXMLCategoryToolStripMenuItem.Text = "Import XML Category File";
            this.setXMLCategoryToolStripMenuItem.Click += new System.EventHandler(this.ImportXMLCategoryToolStripMenuItem_Click);
            // 
            // getOriginalCategoryFileToolStripMenuItem
            // 
            this.getOriginalCategoryFileToolStripMenuItem.Name = "getOriginalCategoryFileToolStripMenuItem";
            this.getOriginalCategoryFileToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.getOriginalCategoryFileToolStripMenuItem.Text = "Save Original Category File";
            this.getOriginalCategoryFileToolStripMenuItem.Click += new System.EventHandler(this.SaveOriginalCategoryFileToolStripMenuItem_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "ReleaseNotes";
            this.saveFileDialog.InitialDirectory = "Desktop\\";
            // 
            // checkedListBoxrReportType
            // 
            this.checkedListBoxrReportType.CheckOnClick = true;
            this.checkedListBoxrReportType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkedListBoxrReportType.FormattingEnabled = true;
            this.checkedListBoxrReportType.Items.AddRange(new object[] {
            "XML",
            "EXCEL"});
            this.checkedListBoxrReportType.Location = new System.Drawing.Point(6, 20);
            this.checkedListBoxrReportType.Name = "checkedListBoxrReportType";
            this.checkedListBoxrReportType.Size = new System.Drawing.Size(163, 49);
            this.checkedListBoxrReportType.TabIndex = 41;
            // 
            // groupBoxReportTypeCheckBox
            // 
            this.groupBoxReportTypeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxReportTypeCheckBox.Controls.Add(this.checkedListBoxrReportType);
            this.groupBoxReportTypeCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBoxReportTypeCheckBox.Location = new System.Drawing.Point(514, 217);
            this.groupBoxReportTypeCheckBox.Name = "groupBoxReportTypeCheckBox";
            this.groupBoxReportTypeCheckBox.Size = new System.Drawing.Size(175, 77);
            this.groupBoxReportTypeCheckBox.TabIndex = 43;
            this.groupBoxReportTypeCheckBox.TabStop = false;
            this.groupBoxReportTypeCheckBox.Text = "Report Output Type:";
            // 
            // groupBoxReleaseNotesReq
            // 
            this.groupBoxReleaseNotesReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxReleaseNotesReq.Controls.Add(this.checkedListBoxReleaseNoteReq);
            this.groupBoxReleaseNotesReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBoxReleaseNotesReq.Location = new System.Drawing.Point(514, 75);
            this.groupBoxReleaseNotesReq.Name = "groupBoxReleaseNotesReq";
            this.groupBoxReleaseNotesReq.Size = new System.Drawing.Size(175, 100);
            this.groupBoxReleaseNotesReq.TabIndex = 44;
            this.groupBoxReleaseNotesReq.TabStop = false;
            this.groupBoxReleaseNotesReq.Text = "Release Notes Required As:";
            // 
            // checkedListBoxReleaseNoteReq
            // 
            this.checkedListBoxReleaseNoteReq.CheckOnClick = true;
            this.checkedListBoxReleaseNoteReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.checkedListBoxReleaseNoteReq.FormattingEnabled = true;
            this.checkedListBoxReleaseNoteReq.Items.AddRange(new object[] {
            "Yes",
            "No",
            "AssetsWithNull"});
            this.checkedListBoxReleaseNoteReq.Location = new System.Drawing.Point(6, 23);
            this.checkedListBoxReleaseNoteReq.Name = "checkedListBoxReleaseNoteReq";
            this.checkedListBoxReleaseNoteReq.Size = new System.Drawing.Size(163, 64);
            this.checkedListBoxReleaseNoteReq.TabIndex = 0;
            this.checkedListBoxReleaseNoteReq.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxReleaseNoteReq_SelectedIndexChanged);
            // 
            // checkBoxPrintAllAssets
            // 
            this.checkBoxPrintAllAssets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPrintAllAssets.AutoSize = true;
            this.checkBoxPrintAllAssets.Location = new System.Drawing.Point(520, 194);
            this.checkBoxPrintAllAssets.Name = "checkBoxPrintAllAssets";
            this.checkBoxPrintAllAssets.Size = new System.Drawing.Size(95, 17);
            this.checkBoxPrintAllAssets.TabIndex = 1;
            this.checkBoxPrintAllAssets.Text = "Print All Assets";
            this.checkBoxPrintAllAssets.UseVisualStyleBackColor = true;
            this.checkBoxPrintAllAssets.CheckedChanged += new System.EventHandler(this.CheckBoxPrintAllAssets_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(511, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "This Checkbox Overrides the above:";
            // 
            // openFileDialogXMLCategory
            // 
            this.openFileDialogXMLCategory.FileName = "openFileDialogXMLCategory";
            this.openFileDialogXMLCategory.Title = "XML Category File";
            // 
            // saveFileDialogOriginalIdentifierFile
            // 
            this.saveFileDialogOriginalIdentifierFile.Filter = "XML Files|*.xml";
            this.saveFileDialogOriginalIdentifierFile.Title = "Save Xml File";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(514, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 23);
            this.button1.TabIndex = 46;
            this.button1.Text = "Import XML Template File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ImportXMLTemplateFile_Click);
            // 
            // XMLLocationLabel
            // 
            this.XMLLocationLabel.AutoSize = true;
            this.XMLLocationLabel.Location = new System.Drawing.Point(12, 30);
            this.XMLLocationLabel.Name = "XMLLocationLabel";
            this.XMLLocationLabel.Size = new System.Drawing.Size(190, 13);
            this.XMLLocationLabel.TabIndex = 47;
            this.XMLLocationLabel.Text = "Please Import XML Template Location!";
            // 
            // listMultipleProjects
            // 
            this.listMultipleProjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listMultipleProjects.FormattingEnabled = true;
            this.listMultipleProjects.Location = new System.Drawing.Point(514, 386);
            this.listMultipleProjects.Name = "listMultipleProjects";
            this.listMultipleProjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listMultipleProjects.Size = new System.Drawing.Size(169, 199);
            this.listMultipleProjects.TabIndex = 48;
            // 
            // addListbutton
            // 
            this.addListbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addListbutton.Location = new System.Drawing.Point(514, 300);
            this.addListbutton.Name = "addListbutton";
            this.addListbutton.Size = new System.Drawing.Size(169, 23);
            this.addListbutton.TabIndex = 49;
            this.addListbutton.Text = "-> Add to List";
            this.addListbutton.UseVisualStyleBackColor = true;
            this.addListbutton.Click += new System.EventHandler(this.addListbutton_Click);
            // 
            // removeListbutton
            // 
            this.removeListbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeListbutton.Location = new System.Drawing.Point(514, 329);
            this.removeListbutton.Name = "removeListbutton";
            this.removeListbutton.Size = new System.Drawing.Size(169, 23);
            this.removeListbutton.TabIndex = 50;
            this.removeListbutton.Text = "<- Remove from List";
            this.removeListbutton.UseVisualStyleBackColor = true;
            this.removeListbutton.Click += new System.EventHandler(this.removeListbutton_Click);
            // 
            // clearListbutton
            // 
            this.clearListbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearListbutton.Location = new System.Drawing.Point(514, 358);
            this.clearListbutton.Name = "clearListbutton";
            this.clearListbutton.Size = new System.Drawing.Size(169, 23);
            this.clearListbutton.TabIndex = 51;
            this.clearListbutton.Text = "Clear List";
            this.clearListbutton.UseVisualStyleBackColor = true;
            this.clearListbutton.Click += new System.EventHandler(this.clearListbutton_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 647);
            this.Controls.Add(this.clearListbutton);
            this.Controls.Add(this.removeListbutton);
            this.Controls.Add(this.addListbutton);
            this.Controls.Add(this.listMultipleProjects);
            this.Controls.Add(this.XMLLocationLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxPrintAllAssets);
            this.Controls.Add(this.groupBoxReleaseNotesReq);
            this.Controls.Add(this.groupBoxReportTypeCheckBox);
            this.Controls.Add(this.generateReportButton);
            this.Controls.Add(this.ProjectGroup);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(450, 420);
            this.Name = "Menu";
            this.Text = "Release Note Generator";
            this.Load += new System.EventHandler(this.MenuLoad);
            this.ProjectGroup.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxReportTypeCheckBox.ResumeLayout(false);
            this.groupBoxReleaseNotesReq.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ProjectGroup;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button generateReportButton;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.CheckedListBox checkedListBoxrReportType;
        private System.Windows.Forms.GroupBox groupBoxReportTypeCheckBox;
        private System.Windows.Forms.GroupBox groupBoxReleaseNotesReq;
        private System.Windows.Forms.CheckedListBox checkedListBoxReleaseNoteReq;
        private System.Windows.Forms.CheckBox checkBoxPrintAllAssets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem setXMLCategoryToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogXMLCategory;
        private System.Windows.Forms.ToolStripMenuItem getOriginalCategoryFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialogOriginalIdentifierFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label XMLLocationLabel;
        private System.Windows.Forms.ListBox listMultipleProjects;
        private System.Windows.Forms.Button addListbutton;
        private System.Windows.Forms.Button removeListbutton;
        private System.Windows.Forms.Button clearListbutton;
    }
}

