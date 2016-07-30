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
            this.SuspendLayout();
            // 
            // pokemonListView
            // 
            this.pokemonListView.BackColor = System.Drawing.SystemColors.Control;
            this.pokemonListView.GridLines = true;
            this.pokemonListView.Location = new System.Drawing.Point(12, 12);
            this.pokemonListView.Name = "pokemonListView";
            this.pokemonListView.Size = new System.Drawing.Size(598, 356);
            this.pokemonListView.TabIndex = 0;
            this.pokemonListView.UseCompatibleStateImageBehavior = false;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(429, 374);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(181, 25);
            this.btnTransfer.TabIndex = 1;
            this.btnTransfer.Text = "Transfer Selected Pokemon";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // PokemonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(622, 411);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.pokemonListView);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PokemonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Pokemon List";
            this.Load += new System.EventHandler(this.PokemonForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView pokemonListView;
        private System.Windows.Forms.Button btnTransfer;
    }
}