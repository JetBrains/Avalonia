namespace WindowsInteropTest
{
    partial class SelectorForm
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
            this.btnEmbedToWinForms = new System.Windows.Forms.Button();
            this.btnEmbedToWpf = new System.Windows.Forms.Button();
            this.btnEmbedToWpfThroughWinForms = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEmbedToWinForms
            // 
            this.btnEmbedToWinForms.Location = new System.Drawing.Point(12, 12);
            this.btnEmbedToWinForms.Name = "btnEmbedToWinForms";
            this.btnEmbedToWinForms.Size = new System.Drawing.Size(201, 86);
            this.btnEmbedToWinForms.TabIndex = 0;
            this.btnEmbedToWinForms.Text = "Embed to WinForms";
            this.btnEmbedToWinForms.UseVisualStyleBackColor = true;
            this.btnEmbedToWinForms.Click += new System.EventHandler(this.btnEmbedToWinForms_Click);
            // 
            // btnEmbedToWpf
            // 
            this.btnEmbedToWpf.Location = new System.Drawing.Point(219, 12);
            this.btnEmbedToWpf.Name = "btnEmbedToWpf";
            this.btnEmbedToWpf.Size = new System.Drawing.Size(201, 86);
            this.btnEmbedToWpf.TabIndex = 1;
            this.btnEmbedToWpf.Text = "Embed to WPF";
            this.btnEmbedToWpf.UseVisualStyleBackColor = true;
            this.btnEmbedToWpf.Click += new System.EventHandler(this.btnEmbedToWpf_Click);
            // 
            // btnEmbedToWpfThroughWinForms
            // 
            this.btnEmbedToWpfThroughWinForms.Location = new System.Drawing.Point(426, 12);
            this.btnEmbedToWpfThroughWinForms.Name = "btnEmbedToWpfThroughWinForms";
            this.btnEmbedToWpfThroughWinForms.Size = new System.Drawing.Size(201, 86);
            this.btnEmbedToWpfThroughWinForms.TabIndex = 2;
            this.btnEmbedToWpfThroughWinForms.Text = "Embed to WPF (WinForms)";
            this.btnEmbedToWpfThroughWinForms.UseVisualStyleBackColor = true;
            this.btnEmbedToWpfThroughWinForms.Click += new System.EventHandler(this.btnEmbedToWpfThroughWinForms_Click);
            // 
            // SelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 284);
            this.Controls.Add(this.btnEmbedToWpf);
            this.Controls.Add(this.btnEmbedToWinForms);
            this.Controls.Add(this.btnEmbedToWpfThroughWinForms);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SelectorForm";
            this.Text = "Interop";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEmbedToWpfThroughWinForms;
        private System.Windows.Forms.Button btnEmbedToWinForms;
        private System.Windows.Forms.Button btnEmbedToWpf;
    }
}

