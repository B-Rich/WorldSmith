﻿using System;
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

namespace WorldSmith.Dialogs
{
    public delegate void Task();
    public partial class AssetLoadingDialog : Form
    {

        public static Dictionary<string, Task> InitialLoad = new Dictionary<string, Task>()
        {
            {"Building Action Factory", () => { DotaActionFactory.BuildFactory(); } }, 
            {"Opening pak01_dir.vpk",  () => { DotaData.LoadFromVPK(Properties.Settings.Default.DotaDir); } },
            {"npc_units.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultUnitsFile, DotaData.AllUnits); } },
            {"npc_heroes.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultHeroesFile, DotaData.AllHeroes); } },
            {"npc_abilities.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultAbilitiesFile, DotaData.AllAbilities); } },
            {"items.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultItemsFile, DotaData.AllItems); } },
            {"Init Lua", () => { LuaHelper.Init();  } }
        };

        public static Dictionary<string, Task> AddonLoadTasks = new Dictionary<string, Task>()
        {
            {"npc_units_custom.txt", () => {
                
                DotaData.ReadCustom(DotaData.CustomUnitsFile, DotaData.AllUnits); 
            } },
            {"npc_heroes_custom.txt", () => {
                
                DotaData.ReadCustom(DotaData.CustomHeroesFile, DotaData.AllHeroes, true); 
            } },
            {"npc_abilities_custom.txt", () => {
                
                DotaData.ReadCustom(DotaData.CustomAbilityFile, DotaData.AllAbilities); 
            } },
            {"npc_items_custom.txt", () => {
                ;
                DotaData.ReadCustom(DotaData.CustomItemsFile, DotaData.AllItems); 
            } },
            { "npc_abilities_override.txt", () => {
                DotaData.ReadCustom(DotaData.OverrideAbilityFile, DotaData.AllAbilities, true);
            }},
            { "npc_items_override.txt", () => {
                DotaData.ReadCustom(DotaData.OverrideItemsFile, DotaData.AllItems, true);
            }},
            { "npc_units_override.txt", () => {
                DotaData.ReadCustom(DotaData.OverrideUnitsFile, DotaData.AllUnits, true);
            }},              
        };

        public static Dictionary<string, Task> AddonSaveTasks = new Dictionary<string, Task>()
        {
            {"Saving npc_units_custom.txt", () => { DotaData.SaveList(DotaData.CustomUnits, "DOTAUnits", "npc_units_custom.txt");} },
            {"Saving npc_heroes_custom.txt", () => { DotaData.SaveList(DotaData.OverridenHeroes, "DOTAHeroes", "npc_heroes_custom.txt");} },
            {"Saving npc_abilities_custom.txt", () => { DotaData.SaveList(DotaData.CustomAbilities, "DOTAAbilities", "npc_abilities_custom.txt");} },
            {"Saving npc_items_custom.txt", () => { DotaData.SaveList(DotaData.CustomItems, "DOTAAbilities", "npc_items_custom.txt"); } },
            {"Saving npc_abilities_override.txt", () => { DotaData.SaveList(DotaData.OverridenAbilities, "DOTAAbilities", "npc_abilities_override.txt"); } },
            {"Saving npc_items_override.txt", () => { DotaData.SaveList(DotaData.OverridenItems, "DOTAAbilities", "npc_items_override.txt"); } },

            
        };

        public AssetLoadingDialog()
        {
            InitializeComponent();
        }

        private Dictionary<string, Task> job;
        

        public void Start(Dictionary<string, Task> job)
        {
            this.job = job;
            backgroundWorker1.RunWorkerAsync();
        }

        public DialogResult ShowDialog(Dictionary<string, Task> job)
        {
            Start(job);
            return base.ShowDialog();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            int Total = job.Count;

            int count = 0;
            foreach(KeyValuePair<string, Task> kv in job)
            {
                int progress = (int)((count / (float)Total) * 100);
                backgroundWorker1.ReportProgress(progress, kv.Key);
                kv.Value();
                count++;
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            loadingLabel.Text = (string)e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
