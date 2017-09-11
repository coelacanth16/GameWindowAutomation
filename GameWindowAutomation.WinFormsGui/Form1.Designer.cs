namespace GameWindowAutomation.WinFormsGui
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.StartBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.endBtn = new System.Windows.Forms.Button();
            this.xLbl = new System.Windows.Forms.Label();
            this.yLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.keyPressedLogTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(171, 12);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // endBtn
            // 
            this.endBtn.Location = new System.Drawing.Point(252, 12);
            this.endBtn.Name = "endBtn";
            this.endBtn.Size = new System.Drawing.Size(75, 23);
            this.endBtn.TabIndex = 2;
            this.endBtn.Text = "End";
            this.endBtn.UseVisualStyleBackColor = true;
            this.endBtn.Click += new System.EventHandler(this.endBtn_Click);
            // 
            // xLbl
            // 
            this.xLbl.AutoSize = true;
            this.xLbl.Location = new System.Drawing.Point(12, 17);
            this.xLbl.Name = "xLbl";
            this.xLbl.Size = new System.Drawing.Size(14, 13);
            this.xLbl.TabIndex = 3;
            this.xLbl.Text = "X";
            // 
            // yLbl
            // 
            this.yLbl.AutoSize = true;
            this.yLbl.Location = new System.Drawing.Point(12, 30);
            this.yLbl.Name = "yLbl";
            this.yLbl.Size = new System.Drawing.Size(14, 13);
            this.yLbl.TabIndex = 4;
            this.yLbl.Text = "Y";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(1199, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(205, 287);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // keyPressedLogTB
            // 
            this.keyPressedLogTB.Location = new System.Drawing.Point(12, 57);
            this.keyPressedLogTB.Multiline = true;
            this.keyPressedLogTB.Name = "keyPressedLogTB";
            this.keyPressedLogTB.Size = new System.Drawing.Size(74, 242);
            this.keyPressedLogTB.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1416, 311);
            this.Controls.Add(this.keyPressedLogTB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.yLbl);
            this.Controls.Add(this.xLbl);
            this.Controls.Add(this.endBtn);
            this.Controls.Add(this.StartBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button endBtn;
        private System.Windows.Forms.Label xLbl;
        private System.Windows.Forms.Label yLbl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox keyPressedLogTB;
    }
}

