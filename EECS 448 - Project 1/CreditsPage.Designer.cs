namespace EECS_448___Project_1
{
    partial class CreditsPage
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
            int[] px = {166, 101, 191, 80};
            int[] py = {53, 108, 163, 218};
            int[] sx = {292, 423, 241, 479};
            int[] sy = {55, 55, 55, 55};
            string[] contributors = {
                "Drake Pham",
                "James Hanselman",
                "Derek Ruf",
                "Matthew McManness"
            };

            this.labels = new System.Windows.Forms.Label[4];
            this.SuspendLayout();

            for (int i = 0; i < 4; i++)
            {
                this.labels[i] = new System.Windows.Forms.Label();
                this.labels[i].AutoSize = true;
                this.labels[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.labels[i].ForeColor = System.Drawing.SystemColors.ControlLightLight;
                this.labels[i].Location = new System.Drawing.Point(px[i], py[i]);
                this.labels[i].Name = "label"+(i+1);
                this.labels[i].Size = new System.Drawing.Size(sx[i], sy[i]);
                this.labels[i].TabIndex = i;
                this.labels[i].Text = contributors[i];
                this.labels[i].Click += new System.EventHandler(this.label_Click);
                this.Controls.Add(this.labels[i]);
            }
            
            // 
            // CreditsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(614, 337);
            this.Name = "CreditsPage";
            this.Text = "Battleship";
            this.Load += new System.EventHandler(this.CreditsPage_Load);
            this.Click += new System.EventHandler(this.CreditsPage_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label[] labels;

        //private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.Label label3;
        //private System.Windows.Forms.Label label4;
    }
}