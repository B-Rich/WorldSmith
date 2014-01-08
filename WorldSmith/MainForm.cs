﻿using KVLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldSmith.DataClasses;
using WorldSmith.Dialogs;

namespace WorldSmith
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadFromData();
        }

        private void LoadFromData()
        {
            TreeNode n = unitTreeView.Nodes.Find("defaultUnits", false)[0];

            foreach(DotaUnit unit in DotaData.DefaultUnits)
            {
                TreeNode newnode = new TreeNode()
                {
                    Name = unit.ClassName,
                    Text = unit.ClassName,
                    Tag = "Item"
                };
                n.Nodes.Add(newnode);
            }

            n = unitTreeView.Nodes.Find("defaultHeroes", false)[0];
            foreach (DotaHero unit in DotaData.DefaultHeroes)
            {
                TreeNode newnode = new TreeNode()
                {
                    Name = unit.ClassName,
                    Text = unit.ClassName,
                    Tag = "Item"
                };
                n.Nodes.Add(newnode);
            }

        }     
     

        private void unitTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            unitPropertyGrid.SelectedObject = DotaData.AllUnits.FirstOrDefault(x => x.ClassName == e.Node.Name);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void unitPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            TreeNode n = unitTreeView.SelectedNode;
            //Grab the unit out of the default list and put it in the Override list
            DotaBaseUnit b = DotaData.AllDefaultUnits.FirstOrDefault(x => x.ClassName == n.Name);
            if (b == null) return;
            if (b is DotaUnit)
            {                
                TreeNode root = unitTreeView.Nodes.Find("overrideUnits", false)[0];
                root.Nodes.Add(n);

                DotaData.DefaultUnits.Remove(b as DotaUnit);
                DotaData.OverriddenUnits.Add(b as DotaUnit);
            }
            if(b is DotaHero)
            {
                //Remove the custom hero from the unit list
                n.Remove();
                DotaData.DefaultUnits.Remove(b as DotaUnit);

                //Get a new classname for this overriden hero               
                TextPrompt prompt = new TextPrompt();
                prompt.Text = "Create New Hero Classname";
                prompt.PromptText = b.ClassName + "_custom";

                string newClassName = prompt.ShowDialog();

                //Create a new Dota hero for this guy
                DotaHero newHero = b.Clone() as DotaHero;
                newHero.ClassName = newClassName;
                newHero.override_hero = b.ClassName;

                //Put him in the hero override list
                DotaData.OverridenHeroes.Add(newHero);

                //Add him to the tree view
                TreeNode root = unitTreeView.Nodes.Find("overrideHero", false)[0];
                TreeNode newNode = new TreeNode()
                    {
                        Name = newHero.ClassName,
                        Text = newHero.ClassName,
                        Tag = "Item",
                    };
                root.Nodes.Add(newNode);     
                unitTreeView.SelectedNode = newNode;

                //Switch to the Override view for this unit
                unitTreeView.CollapseAll();
                newNode.Parent.ExpandAll();
            }
            
        }

        private void addonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Create the new Addon Wizard
            NewAddonWizard wizard = new NewAddonWizard();
            if (wizard.ShowDialog() != DialogResult.OK) return; //They didnt want to make a new addon.  Whatever...


            KeyValue doc = new KeyValue("AddonInfo");
            doc += new KeyValue("addonSteamAppID") + 816;
            doc += new KeyValue("addontitle") + wizard.addonTitleBox.Text;
            doc += new KeyValue("addonversion") + wizard.addonVersionBox.Text;
            doc += new KeyValue("addontagline") + wizard.addonTagLineBox.Text;
            doc += new KeyValue("addonauthor") + wizard.addonTagLineBox.Text;
            doc += new KeyValue("addonSteamGroupName") + wizard.addonSteamGroupName.Text;
            doc += new KeyValue("addonauthorSteamID") + wizard.addonSteamGroupName.Text;
            doc += new KeyValue("addonContent_Campaign") + 0;
            doc += new KeyValue("addonURL0") + wizard.addonURLBox.Text;
            doc += new KeyValue("addonDescription") + wizard.addonDescriptionBox.Text;

            string addonPath = Properties.Settings.Default.dotadir + Path.DirectorySeparatorChar
                + "dota" + Path.DirectorySeparatorChar + "addons" + Path.DirectorySeparatorChar
                + wizard.addonNameBox.Text + Path.DirectorySeparatorChar;

            Directory.CreateDirectory(addonPath);
            Directory.CreateDirectory(addonPath + "scripts");
            Directory.CreateDirectory(addonPath + "maps");
            Directory.CreateDirectory(addonPath + "materials");
            Properties.Settings.Default.AddonPath = addonPath;
            Properties.Settings.Default.Save();
            
            File.WriteAllText(addonPath + "addoninfo.txt", DotaData.KVHeader + doc.ToString());
        }
    }
}
