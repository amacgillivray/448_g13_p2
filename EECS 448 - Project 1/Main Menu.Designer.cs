
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
            System.Windows.Forms.Button NetworkGameButton;
            this.LocalGameButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            NetworkGameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LocalGameButton
            // 
            this.LocalGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.LocalGameButton.Location = new System.Drawing.Point(69, 54);
            this.LocalGameButton.Name = "LocalGameButton";
            this.LocalGameButton.Size = new System.Drawing.Size(399, 103);
            this.LocalGameButton.TabIndex = 0;
            this.LocalGameButton.Text = "Local Game";
            this.LocalGameButton.UseVisualStyleBackColor = true;
            this.LocalGameButton.Click += new System.EventHandler(this.LocalGameButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            this.button1.Location = new System.Drawing.Point(69, 297);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(399, 103);
            this.button1.TabIndex = 1;
            this.button1.Text = "Credits";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // NetworkGameButton
            // 
            NetworkGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24.25F);
            NetworkGameButton.Location = new System.Drawing.Point(69, 176);
            NetworkGameButton.Name = "NetworkGameButton";
            NetworkGameButton.Size = new System.Drawing.Size(399, 103);
            NetworkGameButton.TabIndex = 2;
            NetworkGameButton.Text = "Network Game ";
            NetworkGameButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 450);
            this.Controls.Add(NetworkGameButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LocalGameButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LocalGameButton;
        private System.Windows.Forms.Button button1;
    }
}

