﻿namespace WorldSmith.Panels
{
    partial class DotaObjectBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Heroes");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Units");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Abilities");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Items");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DotaObjectBrowser));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.assetTreeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.assetTreeView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(245, 568);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(245, 593);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // assetTreeView
            // 
            this.assetTreeView.ContextMenuStrip = this.contextMenuStrip;
            this.assetTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetTreeView.ImageIndex = 0;
            this.assetTreeView.ImageList = this.imageList;
            this.assetTreeView.Location = new System.Drawing.Point(0, 0);
            this.assetTreeView.Name = "assetTreeView";
            treeNode1.Name = "heroes";
            treeNode1.Text = "Heroes";
            treeNode2.Name = "units";
            treeNode2.Text = "Units";
            treeNode3.Name = "abilities";
            treeNode3.Text = "Abilities";
            treeNode4.Name = "items";
            treeNode4.Text = "Items";
            this.assetTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.assetTreeView.SelectedImageIndex = 0;
            this.assetTreeView.Size = new System.Drawing.Size(245, 568);
            this.assetTreeView.TabIndex = 0;
            this.assetTreeView.DoubleClick += new System.EventHandler(this.assetTreeView_DoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "dotaicon16x16.png");
            this.imageList.Images.SetKeyName(2, "WSIcon32.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(66, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(54, 22);
            this.toolStripDropDownButton1.Text = "Create";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // DotaObjectBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 593);
            this.Controls.Add(this.toolStripContainer1);
            this.DockAreas = ((DigitalRune.Windows.Docking.DockAreas)((((((DigitalRune.Windows.Docking.DockAreas.Float | DigitalRune.Windows.Docking.DockAreas.Left) 
            | DigitalRune.Windows.Docking.DockAreas.Right) 
            | DigitalRune.Windows.Docking.DockAreas.Top) 
            | DigitalRune.Windows.Docking.DockAreas.Bottom) 
            | DigitalRune.Windows.Docking.DockAreas.Document)));
            this.Name = "DotaObjectBrowser";
            this.TabText = "DotaObjectBrowser";
            this.Text = "DotaObjectBrowser";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TreeView assetTreeView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    }
}