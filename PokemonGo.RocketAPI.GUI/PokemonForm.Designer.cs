namespace PokemonGo.RocketAPI.GUI
{
    partial class PokemonForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PokemonForm));
            this.pokemonListView = new System.Windows.Forms.ListView();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.pokemonListMenuStrip = new System.Windows.Forms.MenuStrip();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByCPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByIVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pokemonListMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pokemonListView
            // 
            this.pokemonListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pokemonListView.BackColor = System.Drawing.SystemColors.Control;
            this.pokemonListView.GridLines = true;
            this.pokemonListView.Location = new System.Drawing.Point(12, 28);
            this.pokemonListView.Name = "pokemonListView";
            this.pokemonListView.Size = new System.Drawing.Size(598, 340);
            this.pokemonListView.TabIndex = 0;
            this.pokemonListView.UseCompatibleStateImageBehavior = false;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransfer.Location = new System.Drawing.Point(429, 374);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(181, 25);
            this.btnTransfer.TabIndex = 1;
            this.btnTransfer.Text = "Transfer Selected Pokemon";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // pokemonListMenuStrip
            // 
            this.pokemonListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortToolStripMenuItem});
            this.pokemonListMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.pokemonListMenuStrip.Name = "pokemonListMenuStrip";
            this.pokemonListMenuStrip.Size = new System.Drawing.Size(622, 24);
            this.pokemonListMenuStrip.TabIndex = 10;
            this.pokemonListMenuStrip.Text = "PokemonForm";
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByCPToolStripMenuItem,
            this.sortByIVToolStripMenuItem,
            this.sortByNameToolStripMenuItem});
            this.sortToolStripMenuItem.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // sortByCPToolStripMenuItem
            // 
            this.sortByCPToolStripMenuItem.Name = "sortByCPToolStripMenuItem";
            this.sortByCPToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sortByCPToolStripMenuItem.Text = "By CP";
            this.sortByCPToolStripMenuItem.Click += new System.EventHandler(this.sortByCPToolStripMenuItem_Click);
            // 
            // sortByIVToolStripMenuItem
            // 
            this.sortByIVToolStripMenuItem.Name = "sortByIVToolStripMenuItem";
            this.sortByIVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sortByIVToolStripMenuItem.Text = "By IV";
            this.sortByIVToolStripMenuItem.Click += new System.EventHandler(this.sortByIVToolStripMenuItem_Click);
            // 
            // sortByNameToolStripMenuItem
            // 
            this.sortByNameToolStripMenuItem.Name = "sortByNameToolStripMenuItem";
            this.sortByNameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sortByNameToolStripMenuItem.Text = "By Name";
            this.sortByNameToolStripMenuItem.Click += new System.EventHandler(this.sortByNameToolStripMenuItem_Click);
            // 
            // PokemonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(622, 411);
            this.Controls.Add(this.pokemonListMenuStrip);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.pokemonListView);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "PokemonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Pokemon List";
            this.Load += new System.EventHandler(this.PokemonForm_Load);
            this.pokemonListMenuStrip.ResumeLayout(false);
            this.pokemonListMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView pokemonListView;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.MenuStrip pokemonListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByCPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByIVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByNameToolStripMenuItem;
    }
}