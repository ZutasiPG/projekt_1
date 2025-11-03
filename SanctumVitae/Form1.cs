using MySql.Data.MySqlClient;
using SanctumVitae.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SanctumVitae{
    public partial class Form1 : Form{
        private const string DatabaseSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/main/database.sql";
        private const string KezdoFoglalasokSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/main/kezdoFoglalasok.sql";

        public Button exit = new Button();
        public Button foglal = new Button();

        public MonthCalendar naptar = new MonthCalendar();

        public TextBox vendegNeve = new TextBox();
        public TextBox vendegIrsz = new TextBox();
        public TextBox vendegKoztTipusa = new TextBox();
        public TextBox vendegHazszam = new TextBox();
        public TextBox vendegTel = new TextBox();
        public TextBox vendegKozteruletNeve = new TextBox();
        public TextBox hanyFo = new TextBox();

        public Label lbl1 = new Label();
        public Label lbl2 = new Label();
        public Label lbl3 = new Label();
        public Label lbl4 = new Label();
        public Label lbl5 = new Label();
        public Label lbl6 = new Label();
        public Label lbl7 = new Label();
        public Label lbl8 = new Label();
        public Label lbl9 = new Label();

        public static string tolIg = "";
        DateTime? kezdodatum = null;
        #region Web github elérés
        private void ExecuteSqlScript(string sqlScript, MySqlConnection connection)
        {
            string[] commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commands)
            {
                string trimmedCommand = commandText.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedCommand))
                {
                    using (MySqlCommand command = new MySqlCommand(trimmedCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private bool InitializeDatabaseFromWeb()
        {
            try
            {
                string masterConnectionString = "Server=localhost;Database=;User ID=root;Password=mysql;";
                string dataConnectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
                string databaseSql;
                string kezdoFoglalasokSqlContent;

                using (HttpClient client = new HttpClient())
                {
                    databaseSql = client.GetStringAsync(DatabaseSqlUrl).Result;
                    kezdoFoglalasokSqlContent = client.GetStringAsync(KezdoFoglalasokSqlUrl).Result;
                }

                using (MySqlConnection masterConnection = new MySqlConnection(masterConnectionString))
                {
                    masterConnection.Open();

                    string setupScript = "DROP DATABASE IF EXISTS projekt1; CREATE DATABASE IF NOT EXISTS projekt1;";
                    ExecuteSqlScript(setupScript, masterConnection);
                }

                using (MySqlConnection connection = new MySqlConnection(dataConnectionString))
                {
                    connection.Open();

                    ExecuteSqlScript(databaseSql, connection);

                    string clearSql = @"
                                    SET FOREIGN_KEY_CHECKS = 0;
                                    TRUNCATE TABLE foglalasok;
                                    TRUNCATE TABLE vendegek;
                                    TRUNCATE TABLE szobak;
                                    SET FOREIGN_KEY_CHECKS = 1;
                                ";
                    ExecuteSqlScript(clearSql, connection);

                    ExecuteSqlScript(kezdoFoglalasokSqlContent, connection);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az adatbázis inicializálásakor! Kérjük, ellenőrizze a MySQL szerver futását, vagy az internetkapcsolatot (URL-ek elérhetőségét)." + Environment.NewLine + "Részletes hiba: " + ex.Message);
                return false;
            }
        }
        #endregion
        public Form1(){

            naptar.BackColor = System.Drawing.SystemColors.HotTrack;
            naptar.CalendarDimensions = new System.Drawing.Size(1, 2);
            naptar.Location = new System.Drawing.Point(604, 75);
            naptar.Name = "naptar";
            naptar.TabIndex = 0;
            naptar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(naptar_DateChanged);

            lbl1.AutoSize = true;
            lbl1.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl1.Location = new System.Drawing.Point(601, 53);
            lbl1.Name = "lbl1";
            lbl1.Size = new System.Drawing.Size(147, 13);
            lbl1.TabIndex = 1;
            lbl1.Text = "Mettől meddig kíván maradni:";

            exit.Cursor = System.Windows.Forms.Cursors.Hand;
            exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            exit.Location = new System.Drawing.Point(12, 12);
            exit.Name = "exit";
            exit.Size = new System.Drawing.Size(75, 23);
            exit.TabIndex = 2;
            exit.UseVisualStyleBackColor = true;
            exit.Click += new System.EventHandler(exit_Click);
 
            vendegNeve.Location = new System.Drawing.Point(157, 102);
            vendegNeve.MaxLength = 171;
            vendegNeve.Name = "vendegNeve";
            vendegNeve.Size = new System.Drawing.Size(100, 20);
            vendegNeve.TabIndex = 3;
            vendegNeve.TextChanged += new System.EventHandler(vendegNeve_TextChanged);

            lbl2.AutoSize = true;
            lbl2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            lbl2.Location = new System.Drawing.Point(12, 75);
            lbl2.Name = "lbl2";
            lbl2.Size = new System.Drawing.Size(79, 13);
            lbl2.TabIndex = 4;
            lbl2.Text = "Vendég adatai:";

            lbl3.AutoSize = true;
            lbl3.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl3.Location = new System.Drawing.Point(77, 102);
            lbl3.Name = "lbl3";
            lbl3.Size = new System.Drawing.Size(74, 13);
            lbl3.TabIndex = 5;
            lbl3.Text = "Vendég neve:";
 
            lbl4.AutoSize = true;
            lbl4.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl4.Location = new System.Drawing.Point(99, 128);
            lbl4.Name = "lbl4";
            lbl4.Size = new System.Drawing.Size(52, 13);
            lbl4.TabIndex = 7;
            lbl4.Text = "település:";
 
            vendegIrsz.Location = new System.Drawing.Point(157, 127);
            vendegIrsz.MaxLength = 255;
            vendegIrsz.Name = "vendegIrsz";
            vendegIrsz.Size = new System.Drawing.Size(100, 20);
            vendegIrsz.TabIndex = 6;
            vendegIrsz.TextChanged += new System.EventHandler(vendegIrsz_TextChanged);

            lbl5.AutoSize = true;
            lbl5.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl5.Location = new System.Drawing.Point(62, 180);
            lbl5.Name = "lbl5";
            lbl5.Size = new System.Drawing.Size(89, 13);
            lbl5.TabIndex = 9;
            lbl5.Text = "közterület típusa:";

            vendegKoztTipusa.Location = new System.Drawing.Point(157, 177);
            vendegKoztTipusa.MaxLength = 60;
            vendegKoztTipusa.Name = "vendegKoztTipusa";
            vendegKoztTipusa.Size = new System.Drawing.Size(100, 20);
            vendegKoztTipusa.TabIndex = 8;
            vendegKoztTipusa.TextChanged += new System.EventHandler(vendegUtca_TextChanged);

            lbl6.AutoSize = true;
            lbl6.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl6.Location = new System.Drawing.Point(100, 206);
            lbl6.Name = "lbl6";
            lbl6.Size = new System.Drawing.Size(51, 13);
            lbl6.TabIndex = 11;
            lbl6.Text = "házszám:";

            vendegHazszam.Location = new System.Drawing.Point(157, 202);
            vendegHazszam.MaxLength = 10;
            vendegHazszam.Name = "vendegHazszam";
            vendegHazszam.Size = new System.Drawing.Size(100, 20);
            vendegHazszam.TabIndex = 10;
            vendegHazszam.TextChanged += new System.EventHandler(vendegHazszam_TextChanged);

            lbl7.AutoSize = true;
            lbl7.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl7.Location = new System.Drawing.Point(39, 232);
            lbl7.Name = "lbl7";
            lbl7.Size = new System.Drawing.Size(112, 13);
            lbl7.TabIndex = 13;
            lbl7.Text = "Vendég telefonszáma:";

            vendegTel.Location = new System.Drawing.Point(157, 227);
            vendegTel.MaxLength = 20;
            vendegTel.Name = "vendegTel";
            vendegTel.Size = new System.Drawing.Size(100, 20);
            vendegTel.TabIndex = 12;
            vendegTel.TextChanged += new System.EventHandler(vendegTel_TextChanged);

            foglal.BackColor = System.Drawing.Color.Red;
            foglal.Cursor = System.Windows.Forms.Cursors.Hand;
            foglal.Enabled = false;
            foglal.Location = new System.Drawing.Point(12, 415);
            foglal.Margin = new System.Windows.Forms.Padding(0);
            foglal.Name = "foglal";
            foglal.Size = new System.Drawing.Size(75, 23);
            foglal.TabIndex = 14;
            foglal.Text = "Lefoglal";
            foglal.UseVisualStyleBackColor = false;
            foglal.Click += new System.EventHandler(foglal_Click);

            lbl8.AutoSize = true;
            lbl8.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl8.Location = new System.Drawing.Point(68, 154);
            lbl8.Name = "lbl8";
            lbl8.Size = new System.Drawing.Size(83, 13);
            lbl8.TabIndex = 16;
            lbl8.Text = "közterület neve:";
 
            vendegKozteruletNeve.Location = new System.Drawing.Point(157, 152);
            vendegKozteruletNeve.MaxLength = 60;
            vendegKozteruletNeve.Name = "vendegKozteruletNeve";
            vendegKozteruletNeve.Size = new System.Drawing.Size(100, 20);
            vendegKozteruletNeve.TabIndex = 15;
            vendegKozteruletNeve.TextChanged += new System.EventHandler(vendegKozteruletNeve_TextChanged);

            lbl9.AutoSize = true;
            lbl9.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl9.Location = new System.Drawing.Point(49, 257);
            lbl9.Name = "lbl9";
            lbl9.Size = new System.Drawing.Size(102, 13);
            lbl9.TabIndex = 18;
            lbl9.Text = "Hány fővel érkezne:";

            hanyFo.Location = new System.Drawing.Point(157, 253);
            hanyFo.MaxLength = 20;
            hanyFo.Name = "hanyFo";
            hanyFo.Size = new System.Drawing.Size(100, 20);
            hanyFo.TabIndex = 17;
            hanyFo.TextChanged += new System.EventHandler(hanyFo_TextChanged);

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.InfoText;
            CancelButton = exit;
            ClientSize = new System.Drawing.Size(800, 450);
            ControlBox = false;
            Controls.Add(lbl9);
            Controls.Add(hanyFo);
            Controls.Add(lbl8);
            Controls.Add(vendegKozteruletNeve);
            Controls.Add(foglal);
            Controls.Add(lbl7);
            Controls.Add(vendegTel);
            Controls.Add(lbl6);
            Controls.Add(vendegHazszam);
            Controls.Add(lbl5);
            Controls.Add(vendegKoztTipusa);
            Controls.Add(lbl4);
            Controls.Add(vendegIrsz);
            Controls.Add(lbl3);
            Controls.Add(lbl2);
            Controls.Add(vendegNeve);
            Controls.Add(exit);
            Controls.Add(lbl1);
            Controls.Add(naptar);
            MaximumSize = new System.Drawing.Size(816, 489);
            MinimumSize = new System.Drawing.Size(816, 489);    

            bool initializationSuccess = InitializeDatabaseFromWeb();

            if (initializationSuccess)
            {
                InitializeComponent();
                naptar.MaxSelectionCount = 100;
                naptar.MinDate = DateTime.Now;
                exit.Width = 39;
                exit.Height = 39;
                exit.FlatStyle = FlatStyle.Flat;
                exit.FlatAppearance.BorderSize = 0;
                exit.BackColor = Color.Transparent;
                exit.Text = "";
                try
                {
                    exit.BackgroundImage = Image.FromFile("switch.png");
                    exit.BackgroundImageLayout = ImageLayout.Zoom;
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    gp.AddEllipse(0, 0, exit.Width, exit.Height);
                    exit.Region = new Region(gp);
                }
                catch (FileNotFoundException)
                {
                }
            }
            else
            {
                Application.Exit();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void joE()
        {
            if (vendegNeve.Text != string.Empty && vendegIrsz.Text != string.Empty && vendegKoztTipusa.Text != string.Empty && vendegTel.Text != string.Empty && int.TryParse(vendegHazszam.Text, out int b) && tolIg != "" && vendegKozteruletNeve.Text != "" && int.TryParse(hanyFo.Text, out int c))
            {
                foglal.Enabled = true;
                foglal.BackColor = Color.Green;
            }
            else
            {
                foglal.Enabled = false;
                foglal.BackColor = Color.Red;
            }
        }
        private void naptar_DateChanged(object sender, DateRangeEventArgs e)
        {
            if (kezdodatum == null) kezdodatum = e.Start;
            else
            {
                DateTime kezd = (kezdodatum < e.Start) ? (DateTime)kezdodatum : e.Start;
                DateTime veg = (kezdodatum > e.Start) ? (DateTime)kezdodatum : e.Start;
                if (kezd < naptar.MinDate) kezd = naptar.MinDate;
                naptar.SelectionStart = kezd;
                naptar.SelectionEnd = veg;
                tolIg = $"{kezd.ToShortDateString()} - {veg.ToShortDateString()}";
                kezdodatum = null;
            }
            joE();
        }
        private void foglal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A foglalás sikeres!");
            string connectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO vendegek (vnev, telepules, koztNeve, koztTipusa, hazSz, telefonSz, hanyFo) " +
                             "VALUES (@Nev, @Telepules, @KoztNeve, @KoztTipusa, @Hazszam, @Telefon, @HanyFo )";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nev", vendegNeve.Text);
                    command.Parameters.AddWithValue("@Telepules", vendegIrsz.Text);
                    command.Parameters.AddWithValue("@KoztNeve", vendegKoztTipusa.Text);
                    command.Parameters.AddWithValue("@KoztTipusa", vendegKoztTipusa.Text);
                    command.Parameters.AddWithValue("@Hazszam", vendegHazszam.Text);
                    command.Parameters.AddWithValue("@Telefon", vendegTel.Text);
                    command.Parameters.AddWithValue("@HanyFo", hanyFo.Text);
                    command.ExecuteNonQuery();
                }
            }
            ertekTorol();
        }
        #region joE ellenőrzés
        private void vendegNeve_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void vendegIrsz_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void vendegUtca_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void vendegHazszam_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void vendegTel_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void vendegKozteruletNeve_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        private void hanyFo_TextChanged(object sender, EventArgs e)
        {
            joE();
        }
        #endregion
        public void ertekTorol()
        {
            vendegNeve.Text = "";
            vendegIrsz.Text = "";
            vendegKoztTipusa.Text = "";
            vendegHazszam.Text = "";
            vendegKozteruletNeve.Text = "";
            vendegTel.Text = "";
            naptar.SelectionStart = DateTime.Now;
            naptar.SelectionEnd = DateTime.Now;
            tolIg = "";
        }
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
