
namespace EECS_448___Project_1 {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.LocalGameButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LocalGameButton
            // 
            this.LocalGameButton.BackColor = System.Drawing.SystemColors.Desktop;
            this.LocalGameButton.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocalGameButton.ForeColor = System.Drawing.Color.Lime;
            this.LocalGameButton.Location = new System.Drawing.Point(25, 86);
            this.LocalGameButton.Name = "LocalGameButton";
            this.LocalGameButton.Size = new System.Drawing.Size(399, 103);
            this.LocalGameButton.TabIndex = 0;
            this.LocalGameButton.Text = "Play";
            this.LocalGameButton.UseVisualStyleBackColor = false;
            this.LocalGameButton.Click += new System.EventHandler(this.LocalGameButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Desktop;
            this.button1.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(25, 307);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(399, 103);
            this.button1.TabIndex = 1;
            this.button1.Text = "Credits";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.button2.Location = new System.Drawing.Point(25, 300);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(399, 103);
            this.button2.TabIndex = 1;
            this.button2.Text = "Play vs. AI";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(32, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(382, 75);
            this.label1.TabIndex = 3;
            this.label1.Text = "Battleship";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Desktop;
            this.button3.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Lime;
            this.button3.Location = new System.Drawing.Point(25, 196);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(399, 103);
            this.button3.TabIndex = 4;
            this.button3.Text = "Play vs. AI";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(449, 437);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LocalGameButton);
            this.Name = "Form1";
            this.Text = "Battleship";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LocalGameButton;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
    }
}

