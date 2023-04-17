namespace MarioBros
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)_pictureBox).BeginInit();
            SuspendLayout();
            // 
            // _pictureBox
            // 
            _pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _pictureBox.Location = new System.Drawing.Point(0, 0);
            _pictureBox.Margin = new System.Windows.Forms.Padding(0);
            _pictureBox.Name = "_pictureBox";
            _pictureBox.Size = new System.Drawing.Size(882, 448);
            _pictureBox.TabIndex = 0;
            _pictureBox.TabStop = false;
            // 
            // MainForm
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(882, 448);
            Controls.Add(_pictureBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Mario Bros";
            Load += MainFormLoad;
            KeyDown += MainFormKeyDown;
            KeyUp += MainFormKeyUp;
            ((System.ComponentModel.ISupportInitialize)_pictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        protected System.Windows.Forms.PictureBox _pictureBox;
    }
}
