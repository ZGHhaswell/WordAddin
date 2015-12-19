namespace TestProject
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
            this.button1 = new System.Windows.Forms.Button();
            this.rch_ReceiveContent = new System.Windows.Forms.RichTextBox();
            this.rch_SendContent = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_p = new System.Windows.Forms.TextBox();
            this.txt_c = new System.Windows.Forms.TextBox();
            this.dd = new System.Windows.Forms.RadioButton();
            this.xd = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "链接服务器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rch_ReceiveContent
            // 
            this.rch_ReceiveContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rch_ReceiveContent.Location = new System.Drawing.Point(24, 41);
            this.rch_ReceiveContent.Name = "rch_ReceiveContent";
            this.rch_ReceiveContent.Size = new System.Drawing.Size(541, 126);
            this.rch_ReceiveContent.TabIndex = 1;
            this.rch_ReceiveContent.Text = "";
            // 
            // rch_SendContent
            // 
            this.rch_SendContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rch_SendContent.Location = new System.Drawing.Point(24, 239);
            this.rch_SendContent.Name = "rch_SendContent";
            this.rch_SendContent.Size = new System.Drawing.Size(541, 69);
            this.rch_SendContent.TabIndex = 2;
            this.rch_SendContent.Text = "";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(469, 323);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "发送数据";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "协议：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "命令：";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "发送内容";
            // 
            // txt_p
            // 
            this.txt_p.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_p.Location = new System.Drawing.Point(64, 183);
            this.txt_p.Name = "txt_p";
            this.txt_p.Size = new System.Drawing.Size(135, 21);
            this.txt_p.TabIndex = 7;
            // 
            // txt_c
            // 
            this.txt_c.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txt_c.Location = new System.Drawing.Point(254, 183);
            this.txt_c.Name = "txt_c";
            this.txt_c.Size = new System.Drawing.Size(149, 21);
            this.txt_c.TabIndex = 8;
            // 
            // dd
            // 
            this.dd.AutoSize = true;
            this.dd.Location = new System.Drawing.Point(180, 15);
            this.dd.Name = "dd";
            this.dd.Size = new System.Drawing.Size(71, 16);
            this.dd.TabIndex = 9;
            this.dd.Text = "大端模式";
            this.dd.UseVisualStyleBackColor = true;
            // 
            // xd
            // 
            this.xd.AutoSize = true;
            this.xd.Checked = true;
            this.xd.Location = new System.Drawing.Point(282, 15);
            this.xd.Name = "xd";
            this.xd.Size = new System.Drawing.Size(71, 16);
            this.xd.TabIndex = 10;
            this.xd.TabStop = true;
            this.xd.Text = "小端模式";
            this.xd.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 358);
            this.Controls.Add(this.xd);
            this.Controls.Add(this.dd);
            this.Controls.Add(this.txt_c);
            this.Controls.Add(this.txt_p);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rch_SendContent);
            this.Controls.Add(this.rch_ReceiveContent);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "客户端模拟程序";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rch_ReceiveContent;
        private System.Windows.Forms.RichTextBox rch_SendContent;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_p;
        private System.Windows.Forms.TextBox txt_c;
        private System.Windows.Forms.RadioButton dd;
        private System.Windows.Forms.RadioButton xd;
    }
}

