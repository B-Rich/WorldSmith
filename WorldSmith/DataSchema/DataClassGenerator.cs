﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KVLib;

namespace WorldSmith.DataSchema
{
    static class DataClassGenerator
    {
        #region PropertyFinder
        public static void GenerateDataPropertiesFromKeyValues(string file, string baseKey, string outputFile)
        {
            KVLib.KeyValue kv = KVLib.KVParser.ParseKeyValueText(File.ReadAllText(file));
            kv = kv[baseKey];

            KVLib.KeyValue doc = new KVLib.KeyValue("Schema");


            foreach (KVLib.KeyValue k in kv.Children)
            {
                KVLib.KeyValue property = new KVLib.KeyValue(k.Key);
                property += new KVLib.KeyValue("Category") + "Misc";
                property += new KVLib.KeyValue("Description") + "No Description Set";
                string type = DetermineType(k);
                property += new KVLib.KeyValue("Type") + type;
                property += new KVLib.KeyValue("DefaultValue") + k.GetString();

                doc += property;


            }

            File.WriteAllText(outputFile, doc.ToString());
        }

        public static void FindPossibleValuesForKey(string file, string key)
        {
            KVLib.KeyValue srcDoc = KVLib.KVParser.ParseKeyValueText(File.ReadAllText(file));

            List<string> foundValues = new List<string>();


            foreach (KVLib.KeyValue k in srcDoc.Children)
            {
                KVLib.KeyValue val = k[key];
                if (val == null) continue;
                string sval = val.GetString();
                if (!foundValues.Contains(sval)) foundValues.Add(sval);
            }

            KVLib.KeyValue outDoc = new KVLib.KeyValue("PossibleValues");
            int count = 0;
            foreach (string s in foundValues)
            {
                outDoc += new KVLib.KeyValue(count.ToString()) + s;
                count++;
            }

            File.WriteAllText(key + ".txt", outDoc.ToString());
        }

        static void FindPossibleFlagsForKey(string file, string key)
        {

        }

        static string DetermineType(KVLib.KeyValue k)
        {

            //If they key contains 'Is', 'Can', 'Has', then it's probably a bool
            if (k.Key.IndexOf("Is", StringComparison.OrdinalIgnoreCase) != -1 ||
                k.Key.IndexOf("Can", StringComparison.OrdinalIgnoreCase) != -1 ||
                k.Key.IndexOf("Has", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return "bool";
            }

            //Attributes are always ints
            if (k.Key.IndexOf("Attribute", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return "int";
            }

            //Determine the type of the KV
            List<string> PossibleTypes = new List<string>() {
                    "float",
                    "string",
                    "bool",
                    "int",
                };

            float f;
            if (!k.TryGet(out f)) //Not a float
            {
                PossibleTypes.Remove("float");
            }
            int i;
            if (!k.TryGet(out i))
            {
                PossibleTypes.Remove("int");
            }
            bool b;
            if (!k.TryGet(out b))
            {
                PossibleTypes.Remove("bool");
            }
            if (PossibleTypes.Count > 1) //If we have more than one type, it's probably not a string
            {
                PossibleTypes.Remove("string");
            }

            if (PossibleTypes.Count == 1)
            {
                //Got it, return that one
                return PossibleTypes.First();
            }

            //If the possible types are float or int, default to int
            if (PossibleTypes.Contains("int") && PossibleTypes.Contains("float")
                && !PossibleTypes.Contains("bool"))
            {
                return "int";
            }

            if (k.GetInt() < 0 || k.GetInt() > 1) //If the number is less than 0 or greater than 1, assume int
            {
                return "int";
            }
            //If the value is '0' or '1', it could be float int or bool.  If the name didn't have 'is', lets default to
            //lets default to int


            return "CANTDETERMINE";
        }
        
        #endregion

        #region ClassGenerator
        public static void GenerateClassForSchema(string schemaFileName, string outputDir)
        {
            KeyValue doc = KVParser.ParseKeyValueText(File.ReadAllText(schemaFileName));
            StringBuilder csFile = new StringBuilder();
            csFile.AppendLine("/*****************************  NOTICE  ********************************************");
            csFile.AppendLine("* This file was autogenerated.  Do not edit.                                       *");
            csFile.AppendLine("* Instead, modify the schema in DataSchema related to this class and regenerate it *");
            csFile.AppendLine("***********************************************************************************/");
               
            csFile.AppendLine("using System;");
            csFile.AppendLine("using System.ComponentModel;");
            csFile.AppendLine("using WorldSmith.Panels;");
            csFile.AppendLine("using WorldSmith.Dialogs;");
            

            csFile.AppendLine();
            csFile.AppendLine("namespace WorldSmith.DataClasses");
            csFile.AppendLine("{");
            string baseclass = doc["BaseClass"].GetString();
            string classname = doc["ClassName"].GetString();

            if (doc["ActionAttribute"] != null && doc["ActionAttribute"].GetBool())
                csFile.AppendLine("\t[DotaAction]");

            csFile.AppendLine("\tpublic partial class " + classname + " : " + baseclass);
            csFile.AppendLine("\t{");

            foreach(KeyValue c in doc.Children)
            {               

                if (!c.HasChildren) continue; //Skip the ClassName and BaseClass keys

                if (c["DontWriteProperty"] != null && c["DontWriteProperty"].GetBool()) continue; //Skip do not write tag

                string type = c["Type"].GetString();
                if(type == "enum") //Create the Enum object for enum types
                {
                    type = c.Key + "Enum";
                    csFile.AppendLine("\t\tpublic enum " + c.Key + "Enum");
                    csFile.AppendLine("\t\t{");
                    foreach(KeyValue kv in c["PossibleValues"].Children)
                    {
                        csFile.AppendLine("\t\t\t" + kv.GetString() + ",");
                    }
                    csFile.AppendLine("\t\t}");
                    csFile.AppendLine();
                }
                if(type == "flags")
                {
                    type = c.Key + "Flags";
                    csFile.AppendLine("\t\t[Flags]");
                    csFile.AppendLine("\t\tpublic enum " + c.Key + "Flags");
                    csFile.AppendLine("\t\t{");
                    foreach(KeyValue kv in c["PossibleValues"].Children)
                    {
                        csFile.AppendLine("\t\t\t" + kv.Key + " = " + kv.GetString() + ",");
                    }
                    csFile.AppendLine("\t\t}");
                    csFile.AppendLine();

                    csFile.AppendLine("\t\t[Editor(typeof(FlagEnumDialogUIEditor), "
                        + "typeof(System.Drawing.Design.UITypeEditor))]");
                   
                }
                if (type == "AbilityActionCollection")
                {
                    csFile.AppendLine("\t\t[Editor(typeof(AbilityActionEditor), "
                        + "typeof(System.Drawing.Design.UITypeEditor))]");
                }

                

                csFile.AppendLine(string.Format("\t\t[Category(\"{0}\")]", c["Category"].GetString()));
                csFile.AppendLine(string.Format("\t\t[Description(\"{0}\")]", c["Description"].GetString()));
                if (c["ReadOnly"] != null && c["ReadOnly"].GetBool())
                    csFile.AppendLine("\t\t[ReadOnly(true)]");


                if (type != "AbilityActionCollection"
                    || type != "TargetKey"
                    || type != "ActionCollection"
                    || type != "ControlPointList")
                {
                    csFile.Append("\t\t[DefaultValue(");
                    if (type == "string")
                    {
                        csFile.Append("\"" + c["DefaultValue"].GetString().Replace(@"\", @"\\") + "\"");
                    }
                    if (c["Type"].GetString() == "enum")
                    {
                        csFile.Append(type + "." + c["DefaultValue"].GetString());
                    }
                    if (c["Type"].GetString() == "flags")
                    {
                        csFile.Append(type + "." + c["DefaultValue"].GetString());
                    }
                    if (type == "int")
                    {
                        csFile.Append(c["DefaultValue"].GetString());
                    }
                    if (type == "float")
                    {
                        csFile.Append(c["DefaultValue"].GetString() + "f");
                    }
                    if (type == "bool")
                    {
                        csFile.Append(c["DefaultValue"].GetBool().ToString().ToLower());
                    }
                    if (type == "PerLevel")
                    {
                        csFile.Append("typeof(PerLevel), \"" + c["DefaultValue"].GetString() + "\"");
                    }
                    if (type == "AbilityActionCollection")
                    {
                        csFile.Append("null");
                        //csFile.Append("typeof(AbilityActionCollection), \"\"");
                    }
                    if (type == "TargetKey")
                    {
                        csFile.Append("null");
                        //csFile.Append("typeof(TargetKey), \"" + c["DefaultValue"].GetString() + "\"");
                    }
                    if (type == "ActionCollection")
                    {
                        csFile.Append("null");
                        //csFile.Append("typeof(ActionCollection), \"\"");
                    }
                    if (type == "ControlPointList")
                    {
                        csFile.Append("null");
                        //csFile.Append("typeof(ControlPointList), \"" + c["DefaultValue"].GetString() + "\"");
                    }
                    csFile.AppendLine(")]");
                }

                csFile.AppendLine(string.Format("\t\tpublic {0} {1}", type, c.Key));
                csFile.AppendLine("\t\t{");
                csFile.AppendLine("\t\t\tget;");
                csFile.AppendLine("\t\t\tset;");
                csFile.AppendLine("\t\t}");
                csFile.AppendLine();

            }

            csFile.AppendLine("\t}");
            csFile.AppendLine("}");

            File.WriteAllText(outputDir + doc["ClassName"].GetString() + ".cs", csFile.ToString());

        }

        #endregion
    }
}
