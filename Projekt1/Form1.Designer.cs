namespace Projekt1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.naptar = new System.Windows.Forms.MonthCalendar();
            this.lbl1 = new System.Windows.Forms.Label();
            this.exit = new System.Windows.Forms.Button();
            this.vendegNeve = new System.Windows.Forms.TextBox();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.vendegIrsz = new System.Windows.Forms.TextBox();
            this.lbl5 = new System.Windows.Forms.Label();
            this.vendegKoztTipusa = new System.Windows.Forms.TextBox();
            this.lbl6 = new System.Windows.Forms.Label();
            this.vendegHazszam = new System.Windows.Forms.TextBox();
            this.lbl7 = new System.Windows.Forms.Label();
            this.vendegTel = new System.Windows.Forms.TextBox();
            this.foglal = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.vendegKozteruletNeve = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hanyFo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // naptar
            // 
            this.naptar.BackColor = System.Drawing.SystemColors.HotTrack;
            this.naptar.CalendarDimensions = new System.Drawing.Size(1, 2);
            this.naptar.Location = new System.Drawing.Point(604, 75);
            this.naptar.Name = "naptar";
            this.naptar.TabIndex = 0;
            this.naptar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.naptar_DateChanged);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl1.Location = new System.Drawing.Point(601, 53);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(147, 13);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "Mettől meddig kíván maradni:";
            // 
            // exit
            // 
            this.exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exit.Location = new System.Drawing.Point(12, 12);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 2;
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // vendegNeve
            // 
            this.vendegNeve.Location = new System.Drawing.Point(157, 102);
            this.vendegNeve.MaxLength = 171;
            this.vendegNeve.Name = "vendegNeve";
            this.vendegNeve.Size = new System.Drawing.Size(100, 20);
            this.vendegNeve.TabIndex = 3;
            this.vendegNeve.TextChanged += new System.EventHandler(this.vendegNeve_TextChanged);
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl2.Location = new System.Drawing.Point(12, 75);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(79, 13);
            this.lbl2.TabIndex = 4;
            this.lbl2.Text = "Vendég adatai:";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl3.Location = new System.Drawing.Point(77, 102);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(74, 13);
            this.lbl3.TabIndex = 5;
            this.lbl3.Text = "Vendég neve:";
            // 
            // lbl4
            // 
            this.lbl4.AutoSize = true;
            this.lbl4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl4.Location = new System.Drawing.Point(99, 128);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(52, 13);
            this.lbl4.TabIndex = 7;
            this.lbl4.Text = "település:";
            // 
            // vendegIrsz
            // 
            this.vendegIrsz.Location = new System.Drawing.Point(157, 127);
            this.vendegIrsz.MaxLength = 255;
            this.vendegIrsz.Name = "vendegIrsz";
            this.vendegIrsz.Size = new System.Drawing.Size(100, 20);
            this.vendegIrsz.TabIndex = 6;
            this.vendegIrsz.TextChanged += new System.EventHandler(this.vendegIrsz_TextChanged);
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl5.Location = new System.Drawing.Point(62, 180);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(89, 13);
            this.lbl5.TabIndex = 9;
            this.lbl5.Text = "közterület típusa:";
            // 
            // vendegKoztTipusa
            // 
            this.vendegKoztTipusa.Location = new System.Drawing.Point(157, 177);
            this.vendegKoztTipusa.MaxLength = 60;
            this.vendegKoztTipusa.Name = "vendegKoztTipusa";
            this.vendegKoztTipusa.Size = new System.Drawing.Size(100, 20);
            this.vendegKoztTipusa.TabIndex = 8;
            this.vendegKoztTipusa.TextChanged += new System.EventHandler(this.vendegUtca_TextChanged);
            // 
            // lbl6
            // 
            this.lbl6.AutoSize = true;
            this.lbl6.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl6.Location = new System.Drawing.Point(100, 206);
            this.lbl6.Name = "lbl6";
            this.lbl6.Size = new System.Drawing.Size(51, 13);
            this.lbl6.TabIndex = 11;
            this.lbl6.Text = "házszám:";
            // 
            // vendegHazszam
            // 
            this.vendegHazszam.Location = new System.Drawing.Point(157, 202);
            this.vendegHazszam.MaxLength = 10;
            this.vendegHazszam.Name = "vendegHazszam";
            this.vendegHazszam.Size = new System.Drawing.Size(100, 20);
            this.vendegHazszam.TabIndex = 10;
            this.vendegHazszam.TextChanged += new System.EventHandler(this.vendegHazszam_TextChanged);
            // 
            // lbl7
            // 
            this.lbl7.AutoSize = true;
            this.lbl7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbl7.Location = new System.Drawing.Point(39, 232);
            this.lbl7.Name = "lbl7";
            this.lbl7.Size = new System.Drawing.Size(112, 13);
            this.lbl7.TabIndex = 13;
            this.lbl7.Text = "Vendég telefonszáma:";
            // 
            // vendegTel
            // 
            this.vendegTel.Location = new System.Drawing.Point(157, 227);
            this.vendegTel.MaxLength = 20;
            this.vendegTel.Name = "vendegTel";
            this.vendegTel.Size = new System.Drawing.Size(100, 20);
            this.vendegTel.TabIndex = 12;
            this.vendegTel.TextChanged += new System.EventHandler(this.vendegTel_TextChanged);
            // 
            // foglal
            // 
            this.foglal.BackColor = System.Drawing.Color.Red;
            this.foglal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.foglal.Enabled = false;
            this.foglal.Location = new System.Drawing.Point(12, 415);
            this.foglal.Margin = new System.Windows.Forms.Padding(0);
            this.foglal.Name = "foglal";
            this.foglal.Size = new System.Drawing.Size(75, 23);
            this.foglal.TabIndex = 14;
            this.foglal.Text = "Lefoglal";
            this.foglal.UseVisualStyleBackColor = false;
            this.foglal.Click += new System.EventHandler(this.foglal_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(68, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "közterület neve:";
            // 
            // vendegKozteruletNeve
            // 
            this.vendegKozteruletNeve.Location = new System.Drawing.Point(157, 152);
            this.vendegKozteruletNeve.MaxLength = 60;
            this.vendegKozteruletNeve.Name = "vendegKozteruletNeve";
            this.vendegKozteruletNeve.Size = new System.Drawing.Size(100, 20);
            this.vendegKozteruletNeve.TabIndex = 15;
            this.vendegKozteruletNeve.TextChanged += new System.EventHandler(this.vendegKozteruletNeve_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(49, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Hány fővel érkezne:";
            // 
            // hanyFo
            // 
            this.hanyFo.Location = new System.Drawing.Point(157, 253);
            this.hanyFo.MaxLength = 20;
            this.hanyFo.Name = "hanyFo";
            this.hanyFo.Size = new System.Drawing.Size(100, 20);
            this.hanyFo.TabIndex = 17;
            this.hanyFo.TextChanged += new System.EventHandler(this.hanyFo_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hanyFo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vendegKozteruletNeve);
            this.Controls.Add(this.foglal);
            this.Controls.Add(this.lbl7);
            this.Controls.Add(this.vendegTel);
            this.Controls.Add(this.lbl6);
            this.Controls.Add(this.vendegHazszam);
            this.Controls.Add(this.lbl5);
            this.Controls.Add(this.vendegKoztTipusa);
            this.Controls.Add(this.lbl4);
            this.Controls.Add(this.vendegIrsz);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.vendegNeve);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.naptar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Projekt1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar naptar;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.TextBox vendegNeve;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.TextBox vendegIrsz;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.TextBox vendegKoztTipusa;
        private System.Windows.Forms.Label lbl6;
        private System.Windows.Forms.TextBox vendegHazszam;
        private System.Windows.Forms.Label lbl7;
        private System.Windows.Forms.TextBox vendegTel;
        private System.Windows.Forms.Button foglal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox vendegKozteruletNeve;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hanyFo;
    }
}

