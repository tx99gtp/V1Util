using ReleaseNoteGenerator.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VersionOne.SDK.ObjectModel;

namespace ReleaseNoteGenerator {
    class CmdRun {

        public Boolean Run(string[] args) {

            Boolean invalidItemsExist = false;

            string xmlLocation = args[0];
            string saveFilePath = args[1];
            string prefix = args[2];
            List<string> projectList = new List<string>();
            for (int i = 3; i < args.Length; i++) {
                projectList.Add(args[i]);
            }

            if (xmlLocationExists(xmlLocation) && saveFilePathExists(saveFilePath)) {
                IEnumerable reqYes = stringCollectionBuilder("YesCmd");
                IEnumerable reportType = stringCollectionBuilder("EXCEL");
                string projectFilePath;

                ICollection ProjectCollection = new ReleaseNoteController.ReleaseNoteController().GetProjects() as ICollection;
                for(int i = 0; i < projectList.Count; i++){
                    string oid = GetOid(ProjectCollection, projectList[i]);

                    if (!String.IsNullOrEmpty(oid))
                    {
                        projectFilePath = GetFile(saveFilePath, prefix, projectList[i]);
                        try
                        {
                            invalidItemsExist = ((new ReleaseNoteController.ReleaseNoteController().WriteReport(projectFilePath, oid, xmlLocation,
                                    reportType, reqYes)) || invalidItemsExist);
                        }
                        catch
                        {
                            invalidItemsExist = true;
                        }
                    }
                    else invalidItemsExist = true;
                }
            }
            else invalidItemsExist = true;
            return invalidItemsExist;
        }

        private string GetFile(string saveFilePath, string prefix, string projectName){
            string cleanProjectName = Regex.Replace(projectName, @"[^\w]+", " ");
            saveFilePath = saveFilePath + prefix + cleanProjectName;
            return saveFilePath;
        }

        private Boolean xmlLocationExists(string xmlLocation) 
        {
            if (!File.Exists(xmlLocation)) 
            {
                 Console.WriteLine("ERROR: XML Template File not found!");
                 return false;
            }
            else if (System.IO.Path.GetExtension(xmlLocation) != ".xml")
            {
                 Console.WriteLine("ERROR: XML Template File not .xml file type!");
                 return false;
			}
            return true;
        }

        private string GetOid(ICollection projectsCollection, string projectName) {
            string currentProjectName;

            foreach (Project project in projectsCollection) {
                currentProjectName = project.Name.ToString();
                while (currentProjectName[0] == '-')
                    currentProjectName = currentProjectName.Trim('-');
                if (currentProjectName.Equals(projectName)) {
                    return project.ToString();
                }
            }
            Console.WriteLine("ERROR: Project " + projectName + " was not found.");
            return null;
        }

        private string[] stringCollectionBuilder(string targetString) {
            string[] builtCollection = new string[1] { targetString };
            return builtCollection;
        }

        private Boolean saveFilePathExists(string path) {
            if (Directory.Exists(path) && path.EndsWith("\\"))
            {
                return true;
            }
            else if (!path.EndsWith("\\"))
            {
                Console.WriteLine("ERROR: Save Filepath does not end with a backslash! (" + path + ")");
                return false;
            }
            else
            {
                Console.WriteLine("ERROR: Save Filepath is not valid! (" + path + ")");
                return false;
            }
        }
    }
}
