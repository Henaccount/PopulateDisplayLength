//
//////////////////////////////////////////////////////////////////////////////
//
//  Copyright 2015 Autodesk, Inc.  All rights reserved.
//
//  Use of this software is subject to the terms of the Autodesk license 
//  agreement provided at the time of installation or download, or which 
//  otherwise accompanies this software in either electronic or hard copy form.   
//
//////////////////////////////////////////////////////////////////////////////
// if just one type of hose exists, shortdescription should be "HOSE"


using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;


using Autodesk.ProcessPower.DataLinks;
using Autodesk.ProcessPower.ProjectManager;
using Autodesk.ProcessPower.PlantInstance;
using Autodesk.AutoCAD.EditorInput;

using System;
using PlantApp = Autodesk.ProcessPower.PlantInstance.PlantApplication;

using System.Collections.Generic;
using Autodesk.ProcessPower.PnP3dObjects;
using Autodesk.ProcessPower.PnP3dDataLinks;

namespace pssCommands
{
    /// <summary>
    /// Helper class including some static helper functions.
    /// </summary>
    /// 


    public class Helper
    {
        public static Project currentProject { get; set; }
        public static Document ActiveDocument { get; set; }
        public static DataLinksManager ActiveDataLinksManager { get; set; }
        public static DataLinksManager3d dlm3d { get; set; }
        public static Database db { get; set; }
        public static Editor ed { get; set; }
        public static PipingObjectAdder pipeObjAdder { get; set; }


        public static bool Initialize()
        {
            if (PlantApplication.CurrentProject == null)
                return false;


            currentProject = PlantApp.CurrentProject.ProjectParts["Piping"];
            ActiveDataLinksManager = currentProject.DataLinksManager;
            dlm3d = DataLinksManager3d.Get3dManager(ActiveDataLinksManager);
            pipeObjAdder = new PipingObjectAdder(dlm3d, db);
            ActiveDocument = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            db = ActiveDocument.Database;
            ed = ActiveDocument.Editor;
            return true;
        }

        public static void Terminate()
        {
            currentProject = null;
            ActiveDataLinksManager = null;
            ActiveDocument = null;
            db = null;
            ed = null;
        }

        public static IDictionary<string, double> ReadInDict(string thestring, bool fillvalues)
        {
            string whole_file = thestring;

            IDictionary<string, double> dict = new Dictionary<string, double>();

            // Split into lines.
            string[] lines = whole_file.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;

            // Load the dictionary.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(new char[] { '=' });

                if (fillvalues)
                    dict[line_r[0]] = Convert.ToDouble(line_r[1]);
                else
                    dict[line_r[0]] = 0.0;
            }

            // Return the values.
            return dict;
        }

    }

    // Helper class to workaround a Hashtable issue: 
    // Can't change values in a foreach loop or enumerator
    class CBoolClass
    {
        public CBoolClass(bool val) { this.val = val; }
        public bool val;
        public override string ToString() { return (val.ToString()); }
    }
}


