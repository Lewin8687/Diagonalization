namespace PatternRecognition2
{
    partial class project
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
            this.btn_start = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.pn_qu_12before = new System.Windows.Forms.Panel();
            this.pn_fi_before = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pn_hk_before = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pn_nn_before = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.pn_quad_after = new System.Windows.Forms.Panel();
            this.pn_hk_after = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pn_fi_after = new System.Windows.Forms.Panel();
            this.pn_nn_after = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.txb_c1 = new System.Windows.Forms.TextBox();
            this.txb_c2 = new System.Windows.Forms.TextBox();
            this.txb_result = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(559, 9);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(65, 23);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 83;
            this.label8.Text = "Quad:";
            // 
            // pn_qu_12before
            // 
            this.pn_qu_12before.Location = new System.Drawing.Point(15, 28);
            this.pn_qu_12before.Name = "pn_qu_12before";
            this.pn_qu_12before.Size = new System.Drawing.Size(240, 230);
            this.pn_qu_12before.TabIndex = 82;
            // 
            // pn_fi_before
            // 
            this.pn_fi_before.Location = new System.Drawing.Point(275, 28);
            this.pn_fi_before.Name = "pn_fi_before";
            this.pn_fi_before.Size = new System.Drawing.Size(240, 230);
            this.pn_fi_before.TabIndex = 84;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 85;
            this.label1.Text = "Fisher:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 89;
            this.label2.Text = "HK:";
            // 
            // pn_hk_before
            // 
            this.pn_hk_before.Location = new System.Drawing.Point(275, 281);
            this.pn_hk_before.Name = "pn_hk_before";
            this.pn_hk_before.Size = new System.Drawing.Size(241, 260);
            this.pn_hk_before.TabIndex = 88;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 87;
            this.label3.Text = "NN:";
            // 
            // pn_nn_before
            // 
            this.pn_nn_before.Location = new System.Drawing.Point(16, 281);
            this.pn_nn_before.Name = "pn_nn_before";
            this.pn_nn_before.Size = new System.Drawing.Size(240, 260);
            this.pn_nn_before.TabIndex = 86;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(541, 585);
            this.tabControl1.TabIndex = 90;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.pn_qu_12before);
            this.tabPage1.Controls.Add(this.pn_hk_before);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.pn_fi_before);
            this.tabPage1.Controls.Add(this.pn_nn_before);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(533, 559);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Before diagonalid";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.pn_quad_after);
            this.tabPage2.Controls.Add(this.pn_hk_after);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.pn_fi_after);
            this.tabPage2.Controls.Add(this.pn_nn_after);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(533, 559);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "After diagolized";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 97;
            this.label4.Text = "HK:";
            // 
            // pn_quad_after
            // 
            this.pn_quad_after.Location = new System.Drawing.Point(17, 30);
            this.pn_quad_after.Name = "pn_quad_after";
            this.pn_quad_after.Size = new System.Drawing.Size(240, 230);
            this.pn_quad_after.TabIndex = 90;
            // 
            // pn_hk_after
            // 
            this.pn_hk_after.Location = new System.Drawing.Point(277, 283);
            this.pn_hk_after.Name = "pn_hk_after";
            this.pn_hk_after.Size = new System.Drawing.Size(241, 260);
            this.pn_hk_after.TabIndex = 96;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 91;
            this.label5.Text = "Quad:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 95;
            this.label6.Text = "NN:";
            // 
            // pn_fi_after
            // 
            this.pn_fi_after.Location = new System.Drawing.Point(277, 30);
            this.pn_fi_after.Name = "pn_fi_after";
            this.pn_fi_after.Size = new System.Drawing.Size(240, 230);
            this.pn_fi_after.TabIndex = 92;
            // 
            // pn_nn_after
            // 
            this.pn_nn_after.Location = new System.Drawing.Point(18, 283);
            this.pn_nn_after.Name = "pn_nn_after";
            this.pn_nn_after.Size = new System.Drawing.Size(240, 260);
            this.pn_nn_after.TabIndex = 94;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(275, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 93;
            this.label7.Text = "Fisher:";
            // 
            // txb_c1
            // 
            this.txb_c1.Location = new System.Drawing.Point(318, 13);
            this.txb_c1.Name = "txb_c1";
            this.txb_c1.Size = new System.Drawing.Size(50, 21);
            this.txb_c1.TabIndex = 91;
            this.txb_c1.Text = "0";
            // 
            // txb_c2
            // 
            this.txb_c2.Location = new System.Drawing.Point(427, 13);
            this.txb_c2.Name = "txb_c2";
            this.txb_c2.Size = new System.Drawing.Size(50, 21);
            this.txb_c2.TabIndex = 92;
            this.txb_c2.Text = "3";
            // 
            // txb_result
            // 
            this.txb_result.Location = new System.Drawing.Point(559, 38);
            this.txb_result.Multiline = true;
            this.txb_result.Name = "txb_result";
            this.txb_result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txb_result.Size = new System.Drawing.Size(294, 563);
            this.txb_result.TabIndex = 93;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(292, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 94;
            this.label9.Text = "D1:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(398, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 95;
            this.label10.Text = "D2:";
            // 
            // project
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 609);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txb_result);
            this.Controls.Add(this.txb_c2);
            this.Controls.Add(this.txb_c1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_start);
            this.Name = "project";
            this.Text = "PR Project";
            this.Load += new System.EventHandler(this.project_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pn_qu_12before;
        private System.Windows.Forms.Panel pn_fi_before;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pn_hk_before;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pn_nn_before;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pn_quad_after;
        private System.Windows.Forms.Panel pn_hk_after;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pn_fi_after;
        private System.Windows.Forms.Panel pn_nn_after;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txb_c1;
        private System.Windows.Forms.TextBox txb_c2;
        private System.Windows.Forms.TextBox txb_result;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}