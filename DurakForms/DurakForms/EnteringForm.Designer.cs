namespace DurakForms
{
    partial class EnteringForm
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
            this.components = new System.ComponentModel.Container();
            this.HostingButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.IpInputBox = new System.Windows.Forms.TextBox();
            this.NickNameBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.EndTurn = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // HostingButton
            // 
            this.HostingButton.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.HostingButton.FlatAppearance.BorderSize = 15;
            this.HostingButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow;
            this.HostingButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.HostingButton.Location = new System.Drawing.Point(487, 112);
            this.HostingButton.Name = "HostingButton";
            this.HostingButton.Size = new System.Drawing.Size(354, 221);
            this.HostingButton.TabIndex = 0;
            this.HostingButton.Tag = "Enter";
            this.HostingButton.Text = "hosting";
            this.HostingButton.UseVisualStyleBackColor = true;
            this.HostingButton.Click += new System.EventHandler(this.HostingButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(357, 214);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 1;
            this.ConnectButton.Tag = "Enter";
            this.ConnectButton.Text = "connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // IpInputBox
            // 
            this.IpInputBox.Location = new System.Drawing.Point(262, 284);
            this.IpInputBox.Name = "IpInputBox";
            this.IpInputBox.PlaceholderText = "ip adress";
            this.IpInputBox.Size = new System.Drawing.Size(100, 23);
            this.IpInputBox.TabIndex = 3;
            this.IpInputBox.Tag = "Enter";
            this.IpInputBox.TextChanged += new System.EventHandler(this.IpInputBox_TextChanged);
            // 
            // NickNameBox
            // 
            this.NickNameBox.Location = new System.Drawing.Point(314, 113);
            this.NickNameBox.Name = "NickNameBox";
            this.NickNameBox.PlaceholderText = "nickname";
            this.NickNameBox.Size = new System.Drawing.Size(100, 23);
            this.NickNameBox.TabIndex = 3;
            this.NickNameBox.Tag = "Enter";
            this.NickNameBox.TextChanged += new System.EventHandler(this.NickNameBox_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(242, 487);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // EndTurn
            // 
            this.EndTurn.Location = new System.Drawing.Point(538, 432);
            this.EndTurn.Name = "EndTurn";
            this.EndTurn.Size = new System.Drawing.Size(75, 23);
            this.EndTurn.TabIndex = 5;
            this.EndTurn.Text = "EndTurn";
            this.EndTurn.UseVisualStyleBackColor = true;
            this.EndTurn.Click += new System.EventHandler(this.EndTurn_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(839, 387);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // EnteringForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 612);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.EndTurn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.NickNameBox);
            this.Controls.Add(this.IpInputBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.HostingButton);
            this.Name = "EnteringForm";
            this.Text = "Durak";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button HostingButton;
        private Button ConnectButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ContextMenuStrip contextMenuStrip1;
        private TextBox IpInputBox;
        private TextBox NickNameBox;
        private PictureBox pictureBox1;
        private Button EndTurn;
        private Button StartButton;
    }
}