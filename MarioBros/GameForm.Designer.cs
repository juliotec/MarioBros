namespace Game
{
    partial class Game
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
            Canvas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(Canvas)).BeginInit();
            SuspendLayout();
            // 
            // Canvas
            // 
            Canvas.BackColor = System.Drawing.Color.White;
            Canvas.Location = new System.Drawing.Point(12, 12);
            Canvas.Name = "Canvas";
            Canvas.Size = new System.Drawing.Size(776, 426);
            Canvas.TabIndex = 1;
            Canvas.TabStop = false;
            Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(pcCanvas_MouseUp);
            // 
            // Game
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(Canvas);
            KeyPreview = true;
            Name = "Game";
            Text = "Games";
            Load += new System.EventHandler(Game_Load);
            KeyDown += new System.Windows.Forms.KeyEventHandler(Game_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(Canvas)).EndInit();
            ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.PictureBox Canvas;
    }
}