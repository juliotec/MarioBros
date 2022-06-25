namespace MarioBros
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ((System.ComponentModel.ISupportInitialize)(Canvas)).BeginInit();
            SuspendLayout();
            // 
            // Canvas
            // 
            Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            Canvas.Image = ((System.Drawing.Image)(resources.GetObject("Canvas.Image")));
            Canvas.Location = new System.Drawing.Point(0, 0);
            Canvas.Size = new System.Drawing.Size(788, 450);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(788, 450);
            Name = "MainForm";
            Text = "Form1";
            KeyDown += new System.Windows.Forms.KeyEventHandler(Demo_KeyDown);
            KeyUp += new System.Windows.Forms.KeyEventHandler(Demo_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(Canvas)).EndInit();
            ResumeLayout(false);

        }

        #endregion
    }
}

