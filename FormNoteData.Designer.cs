namespace DigitalAudioConsole
    {
    partial class FormNoteData
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose( disposing );
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
         this.listViewNoteData = new System.Windows.Forms.ListView();
         this.columnHeaderNote = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderFrequency = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeaderDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.SuspendLayout();
         // 
         // listViewNoteData
         // 
         this.listViewNoteData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNote,
            this.columnHeaderFrequency,
            this.columnHeaderDuration});
         this.listViewNoteData.Dock = System.Windows.Forms.DockStyle.Fill;
         this.listViewNoteData.Location = new System.Drawing.Point(0, 0);
         this.listViewNoteData.Name = "listViewNoteData";
         this.listViewNoteData.Size = new System.Drawing.Size(648, 382);
         this.listViewNoteData.TabIndex = 0;
         this.listViewNoteData.UseCompatibleStateImageBehavior = false;
         this.listViewNoteData.View = System.Windows.Forms.View.Details;
         // 
         // columnHeaderNote
         // 
         this.columnHeaderNote.Text = "Note";
         // 
         // columnHeaderFrequency
         // 
         this.columnHeaderFrequency.Text = "Frequency";
         this.columnHeaderFrequency.Width = 130;
         // 
         // columnHeaderDuration
         // 
         this.columnHeaderDuration.Text = "Duration";
         this.columnHeaderDuration.Width = 200;
         // 
         // FormNoteData
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(648, 382);
         this.Controls.Add(this.listViewNoteData);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "FormNoteData";
         this.Text = "Note Data";
         this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.ListView listViewNoteData;
        private System.Windows.Forms.ColumnHeader columnHeaderNote;
        private System.Windows.Forms.ColumnHeader columnHeaderFrequency;
        private System.Windows.Forms.ColumnHeader columnHeaderDuration;
        }
    }