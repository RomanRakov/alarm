namespace alarm
{
    partial class AlarmListForm
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
            this.AddAlarmButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddAlarmButton
            // 
            this.AddAlarmButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AddAlarmButton.Location = new System.Drawing.Point(504, 12);
            this.AddAlarmButton.Name = "AddAlarmButton";
            this.AddAlarmButton.Size = new System.Drawing.Size(133, 94);
            this.AddAlarmButton.TabIndex = 0;
            this.AddAlarmButton.Text = "Добавить будильник";
            this.AddAlarmButton.UseVisualStyleBackColor = true;
            this.AddAlarmButton.Click += new System.EventHandler(this.AddAlarmButton_Click);
            // 
            // AlarmListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 450);
            this.Controls.Add(this.AddAlarmButton);
            this.Name = "AlarmListForm";
            this.Text = "AlarmListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddAlarmButton;
    }
}