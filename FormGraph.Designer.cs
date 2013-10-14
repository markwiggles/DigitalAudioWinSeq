namespace DigitalAudioConsole
    {
    partial class FormGraph
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
         this.panelGraph = new System.Windows.Forms.Panel();
         this.SuspendLayout();
         // 
         // panelGraph
         // 
         this.panelGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.panelGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.panelGraph.Location = new System.Drawing.Point(0, 0);
         this.panelGraph.Name = "panelGraph";
         this.panelGraph.Size = new System.Drawing.Size(850, 518);
         this.panelGraph.TabIndex = 0;
         // 
         // FormGraph
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(850, 518);
         this.Controls.Add(this.panelGraph);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "FormGraph";
         this.Text = "Audio Graph";
         this.ResumeLayout(false);

            }

        #endregion

        private System.Windows.Forms.Panel panelGraph;
        }
    }