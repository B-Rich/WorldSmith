﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldSmith.DataClasses;
using System.Reflection;
using WorldSmith.Dialogs.Actions;
using WorldSmith.Dialogs;

namespace WorldSmith.Panels
{
    public partial class ObjectLinkEdit : UserControl
    {
        object action;
        public IEnumerable<BaseActionVariable> Variables;
        public string ActionContext;

        public event LinkClickedEventHandler LinkClicked;

        public object Object
        {
            get
            {
                return action;
            }
            set
            {
                action = value;
                BuildActionPanel();
            }
        }

        public string Grammer
        {
            get;
            set;
        }

        public ObjectLinkEdit()
        {
            InitializeComponent();
        }


        private void BuildActionPanel()
        {
            if (Object == null)
            {
                Grammer = "No Object Selected";
                linkLabel1.LinkArea = new LinkArea(0, 0);
                return; //Skip if object is null
            }


            linkLabel1.Links.Clear();
            if (Grammer == null)
            {
                EditorGrammarAttribute attrib = Object.GetType().GetCustomAttribute<EditorGrammarAttribute>();
                if(attrib != null)
                {
                    Grammer = attrib.Grammar;
                }
                else
                {
                    Grammer = "No Grammer Set";
                }                
            }
                
            //Find each % and get the positions to create links
            int ind = Grammer.IndexOf('%', 0);
            int count = 0;
            while(ind != -1)
            {
                int start = ind;
                int end = Grammer.IndexOf(' ', start);
                int len = end == -1 ? Grammer.Length - start : end - start;

                linkLabel1.Links.Add(start - count, len - 1, Grammer.Substring(start+1, len - 1)); //Shif the region back one because we remove the % signs.

                ind = Grammer.IndexOf('%', start + len);

                count++;
            }


            linkLabel1.Text = Grammer.Replace("%", "");

            UpdateLinkTexts();
        }


        private void UpdateLinkTexts()
        {
            for (int j = 0; j < linkLabel1.Links.Count; j++ )
            {

                LinkLabel.Link link = linkLabel1.Links[j];
                string property = link.LinkData as string;

                PropertyInfo info = action.GetType().GetProperty(property);

                if(info == null)
                {
                    MessageBox.Show("Error: Tried to look up " + property + " but couldn't find it");
                    linkLabel1.Text = "Error when reading editor grammar";
                    linkLabel1.LinkArea = new LinkArea(0, 0);
                    return;
                }
                object value = info.GetMethod.Invoke(action, new object[] { });

                string valueText = property;
                if (value != null)
                {
                    valueText = value.ToString();
                }
                if (valueText.Length > 20)
                {
                    valueText = "...";
                }

                string display = "(" + valueText + ")";



                //Get the old display.
                string old = linkLabel1.Text.Substring(link.Start, link.Length);

                //Replace it with the new one
                linkLabel1.Text = linkLabel1.Text.Replace(old, display);

                int diff = link.Length - display.Length; //Get the difference in chars so we can move the other links back

                link.Length = display.Length; //Adjust this link to that it takes up the new area

                //pull back all of the other links so they adjust to the new area, starting with the editing link
                int editedIndex = linkLabel1.Links.IndexOf(link);
                for (int i = editedIndex + 1; i < linkLabel1.Links.Count; i++)
                {
                    LinkLabel.Link l = linkLabel1.Links[i];
                    l.Start -= diff;
                }


            }


        }




        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {           

            string Property = e.Link.LinkData as string;

            PropertyInfo info = action.GetType().GetProperty(Property);
            if(info == null)
            {
                MessageBox.Show("Property " + Property + " Does not exist for " + action.GetType().Name);
                return;
            }

            DialogResult result = DialogResult.Cancel;

            string valueResult = "";

            if(info.PropertyType == typeof(NumberValue))
            {
                VariableEditor editor = new VariableEditor();
                if(Variables != null)
                {
                    editor.SetVariable(Variables.Select(x => x.Name));
                }
                else
                {
                    editor.SetVariable(new string[] { });
                }
                NumberValue val = info.GetMethod.Invoke(action, new object[] { }) as NumberValue;
                editor.SetDefault(val == null ? "" : val.ToString());
                editor.Text = "Variable Editor - " + Property;

                result = editor.ShowDialog();

                if (result == DialogResult.OK)
                {                    
                    info.SetMethod.Invoke(action, new object[] { new NumberValue(valueResult) });
                }
            }
            if(info.PropertyType == typeof(TargetKey))
            {
                TargetKeyEditor editor = new TargetKeyEditor();
                editor.SetPresets(ActionContext);
                editor.VariableList = Variables;

                TargetKey t = info.GetMethod.Invoke(action, new object[] { }) as TargetKey;
                if (t == null) t = new TargetKey();
                editor.Target = t;

                result = editor.ShowDialog();

                if(result == DialogResult.OK)
                {
                    TargetKey target = editor.Target;
                    info.SetMethod.Invoke(action, new object[] { target });
                }

            }
            //It's an Enum!
            if(typeof(Enum).IsAssignableFrom(info.PropertyType))
            {
                Enum enumValue = info.GetMethod.Invoke(action, new object[] { }) as Enum;


                //Find out if it's a flag and open the flag editor
                if(info.PropertyType.GetCustomAttribute<FlagsAttribute>() != null)
                {
                    FlagCheckBoxEditor editor = new FlagCheckBoxEditor();
                    editor.EnumValue = enumValue;

                    result = editor.ShowDialog();

                    enumValue = editor.EnumValue;
                }
                else
                {
                    EnumEditor editor = new EnumEditor();
                    editor.EnumValue = enumValue;

                    result = editor.ShowDialog();

                    enumValue = editor.EnumValue;
                }

                if (result == DialogResult.OK)
                {
                    info.SetMethod.Invoke(action, new object[] { enumValue });

                    valueResult = enumValue.ToString();
                    
                }


            }

            if(typeof(bool) == info.PropertyType)
            {
                bool val = (bool)info.GetMethod.Invoke(action, new object[] { });

                BoolEditor editor = new BoolEditor();
                editor.Value = val;

                result = editor.ShowDialog();

                if(result == DialogResult.OK)
                {
                    info.SetMethod.Invoke(action, new object[] { editor.Value });                    
                }
            }
            if(typeof(string) == info.PropertyType)
            {
                string val = (string)info.GetMethod.Invoke(action, new object[] { });

                TextPrompt editor = new TextPrompt();

                editor.Text = Property;
                editor.PromptText = val;

                result = editor.ShowDialog();
                if(result == DialogResult.OK)
                {
                    info.SetMethod.Invoke(action, new object[] { editor.PromptText });     
                }
            }
            if(typeof(ActionCollection) == info.PropertyType)
            {
                ActionCollection actions = info.GetMethod.Invoke(action, new object[] { }) as ActionCollection;

                ActionListEditor editor = new ActionListEditor();
                if (actions == null) actions = new ActionCollection(Property);

                editor.ActionContext = ActionContext;
                editor.Variables = Variables;
                editor.Actions = actions;

                result = editor.ShowDialog();
                
                if(result == DialogResult.OK)
                {
                    info.SetMethod.Invoke(action, new object[] { editor.Actions });  
                }

            }
            

            if(result == DialogResult.OK)
            {
                UpdateLinkTexts();
            }

            if (LinkClicked != null)
            {
                LinkClicked.Invoke(this, new LinkClickedEventArgs(e.Link.LinkData as string));
            }
            
            
        }

        
    }
}
