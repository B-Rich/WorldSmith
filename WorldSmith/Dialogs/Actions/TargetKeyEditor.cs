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
using WorldSmith.Panels;

namespace WorldSmith.Dialogs.Actions
{
    public partial class TargetKeyEditor : Form
    {

        Dictionary<string, string[]> ContextTargets = new Dictionary<string, string[]>()
        {
            { "Default", new string[] { "CASTER", "TARGET"} },

            //ABILITY EVENT CONTEXTS
            { "OnSpellStart", new string[] { "CASTER", "TARGET", "POINT" } },
            { "OnChannelSucceeded", new string[] { "CASTER", "TARGET", "POINT" } },
            { "OnChannelInterrupted", new string[] { "CASTER", "TARGET", "POINT" } },
            { "OnChannelFinish", new string[] { "CASTER", "TARGET", "POINT" } },
            { "OnAbilityPhaseStart", new string[] { "CASTER", "TARGET", "POINT" } },
          
            
            { "OnOwnerDied", new string[] { "UNIT" } },
            { "OnOwnerSpawned", new string[] { "UNIT" } },
            { "OnUpgrade", new string[] { "UNIT" } },
            { "OnToggleOn", new string[] { "UNIT" } },
            { "OnToggleOff", new string[] { "UNIT" } },

            { "OnProjectileHitUnit", new string[] { "CASTER", "TARGET", "POINT", "PROJECTILE" } },
            { "OnProjectileFinish", new string[] { "CASTER", "TARGET", "POINT", "PROJECTILE" } },

            //MODIFER EVENT CONTEXTS
            { "OnAttacked", new string[] { "ATTACKER", "UNIT" } },
            { "OnIntervalThink", new string[] { "UNIT", "TARGET", "CASTER" } },
            { "OnCreated", new string[] { "CASTER", "TARGET" } },
            { "OnAttackLanded", new string[] { "ATTACKER", "UNIT", "TARGET", "CASTER"} },
            { "OnTakeDamage", new string[] { "ATTACKER", "TARGET", "CASTER" } },
            { "OnDealDamage", new string[] { "ATTACKER", "TARGET", "CASTER" } },
            { "OnDeath", new string[] { "UNIT", "CASTER" } },
            { "OnKill", new string[] { "TARGET", "CASTER" } },
            { "OnAttackStart", new string[] { "CASTER", "TARGET" } },
            { "OnDestroy", new string[] { "CASTER", "TARGET" } },
        };

        public IEnumerable<BaseActionVariable> VariableList;

        private TargetKey target;

        public TargetKey Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                InitLinks();
            }
        }

        public TargetKeyEditor(TargetKey target)
        {

            InitializeComponent();
            Target = target;
            InitLinks();
        }

        public TargetKeyEditor()
            : this(new TargetKey())
        {
            
        }

        public void SetPresets(string category)
        {            
            if (category == null || !ContextTargets.ContainsKey(category)) category = "Default";

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(ContextTargets[category]);
            comboBox1.SelectedIndex = 0;
        }

        private void InitLinks()
        {
            linkHeader.Grammer = "Create a %Shape centered around %Center";
            linkHeader.Object = Target;
            linkHeader.Variables = VariableList;
            linkHeader.LinkClicked += linkHeader_LinkedClicked;
            CenterType(true);
            linkTargetFilter.Grammer = "That targets %UnitTypes on %Teams with %Flags";
            linkTargetFilter.Object = Target;
            linkTargetFilter.Variables = VariableList;
            linkChance.Grammer = "and selects %MaxTargets and %Random additional units";
            linkChance.Object = Target;
            linkChance.Variables = VariableList;

            if (target.Preset != TargetKey.PresetType.NONE)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
            comboBox1.SelectedItem = target.Preset.ToString();
        }

        void linkHeader_LinkedClicked(object sender, LinkClickedEventArgs e)
        {
            if(e.LinkText == "Shape")
            {
                CenterType(target.Shape == TargetKey.ShapeE.CIRCLE);
            }
        }



        private void CenterType(bool type)
        {

            if (type)
            {
                linkLineOrCircle.Grammer = "With radius %Radius";
            }
            else
            {
                linkLineOrCircle.Grammer = "With length of %Length units and thickness of %Thickness";
              
            }
            linkLineOrCircle.Object = Target;
            linkLineOrCircle.Variables = VariableList;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                radioButton2.Checked = false;
                linkHeader.Enabled = false;
                linkLineOrCircle.Enabled = false;
                linkTargetFilter.Enabled = false;
                linkChance.Enabled = false;

                comboBox1.Enabled = true;                
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                linkHeader.Enabled = true;
                linkLineOrCircle.Enabled = true;
                linkTargetFilter.Enabled = true;
                linkChance.Enabled = true;

                comboBox1.Enabled = false;
                target.Preset = TargetKey.PresetType.NONE;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            target.Preset = (TargetKey.PresetType)Enum.Parse(typeof(TargetKey.PresetType), comboBox1.SelectedItem as string);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
