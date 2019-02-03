#region Namespaces

using System;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

using System.Collections.Specialized;
using Autodesk.ProcessPower.ProjectManager;
using Autodesk.ProcessPower.PlantInstance;
using Autodesk.AutoCAD.Runtime;

#endregion

//V0
//v1:added DisplayElevation

namespace pssCommands
{
    public class Program
    {
        public static int precision = 4;


        public static void populateDisplayLength()
        {
            
            Helper.Initialize();

            try
            {
                if (!PnPProjectUtils.GetActiveDocumentType().Equals("Piping"))
                {
                    Helper.ed.WriteMessage("\n This tool works only on Plant 3D Piping drawings!");
                    return;
                }

                if (Autodesk.ProcessPower.AcPp3dObjectsUtils.ProjectUnits.CurrentLinearUnit == 1) //1=mm,2=in
                    precision = 0;
                else
                    precision = 4;

                Helper.ed.WriteMessage("\n AUTODESK PROVIDES THIS PROGRAM \"AS IS\" AND WITH ALL FAULTS.");
                Helper.ed.WriteMessage("\n AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF");
                Helper.ed.WriteMessage("\n MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.");
                Helper.ed.WriteMessage("\n DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE");
                Helper.ed.WriteMessage("\n UNINTERRUPTED OR ERROR FREE.");

                
                using (Transaction tr = Helper.db.TransactionManager.StartTransaction())
                {

                    TypedValue[] filterlist = new TypedValue[1];

                    filterlist[0] = new TypedValue(0, "ACPPPIPE");

                    SelectionFilter filter = new SelectionFilter(filterlist);

                    PromptSelectionResult selRes = Helper.ed.SelectAll(filter);

                    if (selRes.Status != PromptStatus.OK)
                    { return; }

                    ObjectId[] objIdArray = selRes.Value.GetObjectIds();

                    foreach (ObjectId id in objIdArray)
                    {
                        try
                        {
                            StringCollection theKeys = new StringCollection();
                            StringCollection theValues = new StringCollection();
                            Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                            PlantProject currentProj = PlantApplication.CurrentProject;

                            String length = "";
                            String positionz = "";
                            theKeys.Add("Length");
                            theKeys.Add("Position Z");
                            theValues = Helper.ActiveDataLinksManager.GetProperties(id, theKeys, true);
                            length = theValues[0];
                            positionz = theValues[1];

                            String displaylength = "";
                            String displayelevation = "";

                            displaylength = Converter.DistanceToString(Convert.ToDouble(length), DistanceUnitFormat.Current, precision);
                            if (Autodesk.ProcessPower.AcPp3dObjectsUtils.ProjectUnits.CurrentLinearUnit == 1)
                                displayelevation = Math.Round(Convert.ToDouble(positionz), precision).ToString();
                            else
                                displayelevation = Math.Round(Convert.ToDouble(positionz) / 12, 2).ToString() + "'";

                            try
                            {
                                theKeys = new StringCollection();
                                theValues = new StringCollection();
                                theKeys.Add("DisplayLength");
                                theValues.Add(displaylength);
                                Helper.ActiveDataLinksManager.SetProperties(id, theKeys, theValues);
                            }
                            catch (System.Exception) { }

                            try
                            {
                                theKeys = new StringCollection();
                                theValues = new StringCollection();
                                theKeys.Add("DisplayElevation");
                                theValues.Add(displayelevation);
                                Helper.ActiveDataLinksManager.SetProperties(id, theKeys, theValues);
                            }
                            catch (System.Exception) { }


                        }
                        catch (System.Exception e)
                        {
                            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                            Helper.ed.WriteMessage(trace.ToString());
                            Helper.ed.WriteMessage("\nLine: " + trace.GetFrame(0).GetFileLineNumber());
                            Helper.ed.WriteMessage("\npipe error: " + e.Message);
                        }


                    }

                    tr.Commit();
                }

                Helper.ed.WriteMessage("\nScript finished");
            }
            catch (System.Exception e)
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                Helper.ed.WriteMessage(trace.ToString());
                Helper.ed.WriteMessage("\nLine: " + trace.GetFrame(0).GetFileLineNumber());
                Helper.ed.WriteMessage("\nscript error: " + e.Message);
            }
            finally
            {
                Helper.Terminate();
            }
        }


    }




}

