namespace MLifter.Controls
{
    partial class AudioRecordControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonRecord = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRemove
            // 
            this.buttonRemove.Image = global::MLifter.Controls.Properties.Resources.process_stop;
            this.buttonRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonRemove.Location = new System.Drawing.Point(160, 0);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 28);
            this.buttonRemove.TabIndex = 21;
            this.buttonRemove.TabStop = false;
            this.buttonRemove.Text = "&Delete";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonRecord
            // 
            this.buttonRecord.Image = global::MLifter.Controls.Properties.Resources.mediaRecord;
            this.buttonRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonRecord.Location = new System.Drawing.Point(80, 0);
            this.buttonRecord.Name = "buttonRecord";
            this.buttonRecord.Size = new System.Drawing.Size(75, 28);
            this.buttonRecord.TabIndex = 19;
            this.buttonRecord.Text = "&Record";
            this.buttonRecord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonRecord.UseVisualStyleBackColor = true;
            this.buttonRecord.Click += new System.EventHandler(this.buttonRecord_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Image = global::MLifter.Controls.Properties.Resources.mediaPlaybackStart;
            this.buttonPlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonPlay.Location = new System.Drawing.Point(0, 0);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 28);
            this.buttonPlay.TabIndex = 20;
            this.buttonPlay.Text = "&Play";
            this.buttonPlay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // AudioRecordControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonRecord);
            this.Controls.Add(this.buttonPlay);
            this.Name = "AudioRecordControl";
            this.Size = new System.Drawing.Size(238, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonRecord;
        private System.Windows.Forms.Button buttonPlay;
    }
}
