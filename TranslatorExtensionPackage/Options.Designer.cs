namespace JeanAzzopardi.TranslatorExtensionPackage
{
    partial class FormOptions
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
            this.CmbFrom = new System.Windows.Forms.ComboBox();
            this.CmbTo = new System.Windows.Forms.ComboBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.LblTranslateFrom = new System.Windows.Forms.Label();
            this.LblTo = new System.Windows.Forms.Label();
            this.BtnSwitch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CmbFrom
            // 
            this.CmbFrom.FormattingEnabled = true;
            this.CmbFrom.Location = new System.Drawing.Point(93, 53);
            this.CmbFrom.Name = "CmbFrom";
            this.CmbFrom.Size = new System.Drawing.Size(121, 21);
            this.CmbFrom.TabIndex = 0;
            // 
            // CmbTo
            // 
            this.CmbTo.FormattingEnabled = true;
            this.CmbTo.Location = new System.Drawing.Point(241, 53);
            this.CmbTo.Name = "CmbTo";
            this.CmbTo.Size = new System.Drawing.Size(121, 21);
            this.CmbTo.TabIndex = 1;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(287, 99);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LblTranslateFrom
            // 
            this.LblTranslateFrom.AutoSize = true;
            this.LblTranslateFrom.Location = new System.Drawing.Point(12, 56);
            this.LblTranslateFrom.Name = "LblTranslateFrom";
            this.LblTranslateFrom.Size = new System.Drawing.Size(74, 13);
            this.LblTranslateFrom.TabIndex = 3;
            this.LblTranslateFrom.Text = "Translate from";
            // 
            // LblTo
            // 
            this.LblTo.AutoSize = true;
            this.LblTo.Location = new System.Drawing.Point(220, 56);
            this.LblTo.Name = "LblTo";
            this.LblTo.Size = new System.Drawing.Size(16, 13);
            this.LblTo.TabIndex = 4;
            this.LblTo.Text = "to";
            // 
            // BtnSwitch
            // 
            this.BtnSwitch.Location = new System.Drawing.Point(206, 99);
            this.BtnSwitch.Name = "BtnSwitch";
            this.BtnSwitch.Size = new System.Drawing.Size(75, 23);
            this.BtnSwitch.TabIndex = 5;
            this.BtnSwitch.Text = "Switch";
            this.BtnSwitch.UseVisualStyleBackColor = true;
            this.BtnSwitch.Click += new System.EventHandler(this.BtnSwitch_Click);
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 134);
            this.Controls.Add(this.BtnSwitch);
            this.Controls.Add(this.LblTo);
            this.Controls.Add(this.LblTranslateFrom);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.CmbTo);
            this.Controls.Add(this.CmbFrom);
            this.Name = "FormOptions";
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbFrom;
        private System.Windows.Forms.ComboBox CmbTo;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Label LblTranslateFrom;
        private System.Windows.Forms.Label LblTo;
        private System.Windows.Forms.Button BtnSwitch;
    }
}