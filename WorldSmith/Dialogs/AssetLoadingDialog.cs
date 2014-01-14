﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            {"Opening pak01_dir.vpk",  () => { DotaData.LoadFromVPK(Properties.Settings.Default.dotadir); } },
            {"npc_units.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultUnitsFile, DotaData.DefaultUnits); } },
            {"npc_heroes.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultHeroesFile, DotaData.DefaultHeroes); } },
            {"npc_abilities.txt", () => { DotaData.ReadScriptFromVPK(DotaData.DefaultAbilitiesFile, DotaData.DefaultAbilities); } },
        };

        public static Dictionary<string, Task> AddonLoadTasks = new Dictionary<string, Task>()
        {
            {"npc_units_custom.txt", () => { DotaData.ReadOverride(DotaData.CustomUnitsFile, DotaData.CustomUnits); } },
            {"npc_heroes_custom.txt", () => { DotaData.ReadOverride(DotaData.CustomHeroesFile, DotaData.OverridenHeroes); } },
            {"npc_abilities_custom.txt", () => { DotaData.ReadOverride(DotaData.CustomAbilityFile, DotaData.CustomAbilities); } },
            {"Cleaning HeroOverrideList", () => {
                foreach(DotaHero hero in DotaData.OverridenHeroes)
                {
                    DotaHero baseHero = DotaData.DefaultHeroes.FirstOrDefault(h => h.ClassName == hero.override_hero);
                    if (baseHero != null) DotaData.DefaultHeroes.Remove(baseHero);
                }
            } },           
        };

        public static Dictionary<string, Task> AddonSaveTasks = new Dictionary<string, Task>()
        {
            {"Saving npc_units_custom.txt", () => { DotaData.SaveList(DotaData.CustomUnits, "DOTAUnits", "npc_units_custom.txt");} },
            {"Saving npc_heroes_custom.txt", () => { DotaData.SaveList(DotaData.OverridenHeroes, "DOTAHeroes", "npc_heroes_custom.txt");} },
            {"Saving npc_abilities_custom.txt", () => { DotaData.SaveList(DotaData.CustomAbilities, "DOTAAbilities", "npc_abilities_custom.txt");} },
            
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
