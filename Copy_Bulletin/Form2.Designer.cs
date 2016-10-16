namespace Copy_Bulletin
{
    partial class Form2
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
            this.show_text = new System.Windows.Forms.RichTextBox();
            this.return_ok = new System.Windows.Forms.Button();
            this.return_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // show_text
            // 
            this.show_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.show_text.Location = new System.Drawing.Point(13, 13);
            this.show_text.Name = "show_text";
            this.show_text.Size = new System.Drawing.Size(959, 602);
            this.show_text.TabIndex = 0;
            this.show_text.Text = "";
            this.show_text.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // return_ok
            // 
            this.return_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.return_ok.Font = new System.Drawing.Font("標楷體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.return_ok.Location = new System.Drawing.Point(13, 627);
            this.return_ok.Name = "return_ok";
            this.return_ok.Size = new System.Drawing.Size(75, 23);
            this.return_ok.TabIndex = 1;
            this.return_ok.Text = "record";
            this.return_ok.UseVisualStyleBackColor = true;
            this.return_ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // return_cancel
            // 
            this.return_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.return_cancel.Font = new System.Drawing.Font("標楷體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.return_cancel.Location = new System.Drawing.Point(896, 627);
            this.return_cancel.Name = "return_cancel";
            this.return_cancel.Size = new System.Drawing.Size(75, 23);
            this.return_cancel.TabIndex = 2;
            this.return_cancel.Text = "Cancel";
            this.return_cancel.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.return_cancel);
            this.Controls.Add(this.return_ok);
            this.Controls.Add(this.show_text);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox show_text;
        private System.Windows.Forms.Button return_ok;
        private System.Windows.Forms.Button return_cancel;
    }
}