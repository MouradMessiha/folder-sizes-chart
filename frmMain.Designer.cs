namespace FolderSizesChart
{
    partial class frmMain
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
            this.scrScroll = new System.Windows.Forms.VScrollBar();
            this.btnScaleUp = new System.Windows.Forms.Button();
            this.btnScaleDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scrScroll
            // 
            this.scrScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scrScroll.Location = new System.Drawing.Point(1213, 2);
            this.scrScroll.Name = "scrScroll";
            this.scrScroll.Size = new System.Drawing.Size(19, 768);
            this.scrScroll.TabIndex = 0;
            this.scrScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrScroll_Scroll);
            // 
            // btnScaleUp
            // 
            this.btnScaleUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScaleUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleUp.Location = new System.Drawing.Point(1167, 21);
            this.btnScaleUp.Name = "btnScaleUp";
            this.btnScaleUp.Size = new System.Drawing.Size(27, 29);
            this.btnScaleUp.TabIndex = 1;
            this.btnScaleUp.Text = "+";
            this.btnScaleUp.UseVisualStyleBackColor = true;
            this.btnScaleUp.Click += new System.EventHandler(this.btnScaleUp_Click);
            // 
            // btnScaleDown
            // 
            this.btnScaleDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScaleDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleDown.Location = new System.Drawing.Point(1133, 21);
            this.btnScaleDown.Name = "btnScaleDown";
            this.btnScaleDown.Size = new System.Drawing.Size(27, 29);
            this.btnScaleDown.TabIndex = 2;
            this.btnScaleDown.Text = "-";
            this.btnScaleDown.UseVisualStyleBackColor = true;
            this.btnScaleDown.Click += new System.EventHandler(this.btnScaleDown_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 775);
            this.Controls.Add(this.btnScaleDown);
            this.Controls.Add(this.btnScaleUp);
            this.Controls.Add(this.scrScroll);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder sizes chart";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseWheel);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseClick);
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar scrScroll;
        private System.Windows.Forms.Button btnScaleUp;
        private System.Windows.Forms.Button btnScaleDown;
    }
}

