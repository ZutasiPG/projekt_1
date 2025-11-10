using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace SanctumVitae{
    public class szoba
    {
        public int szobaId { get; set; }
        public int agy { get; set; }
        public int potAgy { get; set; }
        public szoba(int szobaId, int agy, int potAgy)
        {
            this.szobaId = szobaId;
            this.agy = agy;
            this.potAgy = potAgy;
        }
    }
    public class foglalas
    {
        public int sorszam { get; set; }
        public int vendegAz { get; set; }
        public szoba szoba { get; set; }
        public DateTime tol { get; set; }
        public DateTime ig { get; set; }
        public int fo { get; set; }
        public int reggeli { get; set; }
        public int teljesEll { get; set; }
        public int fizetve { get; set; }
        public foglalas(int foglalasAz, int vendegAz, int szobaID, DateTime tol, DateTime ig, int fo, int reggeli, int teljesEll, int fizetve, List<szoba> szobak)
        {
            this.sorszam = foglalasAz;
            foreach (var Szoba in szobak)
                if (Szoba.szobaId == szobaID) szoba = Szoba;
            this.tol = tol;
            this.ig = ig;
            this.fo = fo;
            this.reggeli = reggeli;
            this.teljesEll = teljesEll;
            this.fizetve = fizetve;
        }
    }
    public partial class Form1 : Form{
        #region változók
        public string localSzerverKornyezet = "AMPPS"; //AMPPS vagy XAMPP
        public string dataConnectionString = "";
        private TableLayoutPanel tlpJelentes;
        private Button btnBrowse;
        public Button exit = new Button();
        public Button foglal = new Button();
        public Button foglalasMenu = new Button();
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
        public Label lbl10 = new Label();
        public Label lbl11 = new Label();
        public Label lbl12 = new Label();
        public Label noRoomLabel = new Label();
        public CheckBox reggeli = new CheckBox();
        public CheckBox teljesEllatas = new CheckBox();
        public CheckBox fizetve = new CheckBox();
        public static string tolIg = "";
        public DateTime? kezdodatum = null;
        public List<szoba> szobak = new List<szoba>();
        public List<foglalas> foglalasok = new List<foglalas>();
        public List<szoba> szabadSzobak = new List<szoba>();
        public szoba kivalasztottSzoba = new szoba(-1, -1, -1);
        private Button btnJelentesek = new Button();
        private Panel panelJelentes = new Panel();
        private RadioButton rbDatum = new RadioButton();
        private RadioButton rbHonap = new RadioButton();
        private RadioButton rbAktualis = new RadioButton();
        private DateTimePicker dpDatum = new DateTimePicker();
        private ComboBox cbEv = new ComboBox();
        private ComboBox cbHonap = new ComboBox();
        private TextBox txtPdfUt = new TextBox();
        private Button btnPdf = new Button();
        private Button btnMegjelenit = new Button();
        private DataGridView grid = new DataGridView();
        #endregion
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
        private void InitializeDatabase()
        {
            try
            {
                string masterConnectionString = "Server=localhost;Database=;User ID=root;Password=mysql;";
                string kezdoFoglalasokSqlContent = File.ReadAllText("kezdoFoglalasok.sql");
                string databaseSql = File.ReadAllText("database.sql");
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
                    ExecuteSqlScript(kezdoFoglalasokSqlContent, connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az adatbázis inicializálásakor! Kérjük, ellenőrizze a MySQL szerver futását, vagy az internetkapcsolatot (URL-ek elérhetőségét)." + Environment.NewLine + "Részletes hiba: " + ex.Message);
                Application.Exit();
            }
        }
        public void FrissitVendegAktivitas()
        {
            try
            {
                using (var conn = new MySqlConnection(dataConnectionString))
                {
                    conn.Open();
                    string inaktivalSql = "UPDATE vendegek SET aktivE = 0;";
                    using (var cmd1 = new MySqlCommand(inaktivalSql, conn))
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    string aktivalSql = @"
            UPDATE vendegek 
            SET aktivE = 1
            WHERE vsorsz IN (
                SELECT DISTINCT vendeg 
                FROM foglalasok 
                WHERE NOW() BETWEEN erk AND tav
            );";
                    using (var cmd2 = new MySqlCommand(aktivalSql, conn))
                    {
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
            
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            switch (localSzerverKornyezet)
            {
                case "XAMPP":
                    dataConnectionString = "Server=localhost;Database=projekt1;User ID=root;Password=;";
                    break;
                case "AMPPS":
                    dataConnectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
                    break;
            }
            InitializeDatabase();
            FrissitVendegAktivitas();
            naptar.BackColor = System.Drawing.SystemColors.HotTrack;
            naptar.CalendarDimensions = new System.Drawing.Size(1, 2);
            naptar.Location = new System.Drawing.Point(604, 75);
            naptar.Name = "naptar";
            naptar.TabIndex = 0;
            naptar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(naptar_DateChanged);
            naptar.MaxSelectionCount = 100;
            naptar.MinDate = DateTime.Now;
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
            vendegNeve.TextChanged += new System.EventHandler(vizsgal);
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
            vendegIrsz.TextChanged += new System.EventHandler(vizsgal);
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
            vendegKoztTipusa.TextChanged += new System.EventHandler(vizsgal);
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
            vendegHazszam.TextChanged += new System.EventHandler(vizsgal);
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
            vendegTel.TextChanged += new System.EventHandler(vizsgal);
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
            vendegKozteruletNeve.TextChanged += new System.EventHandler(vizsgal);
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
            hanyFo.TextChanged += new System.EventHandler(vizsgal);
            reggeli.Checked = false;
            reggeli.Location = new System.Drawing.Point(hanyFo.Location.X, 280);
            reggeli.Name = "reggeli";
            lbl10.AutoSize = true;
            lbl10.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl10.Location = new System.Drawing.Point(reggeli.Location.X - 80, 282);
            lbl10.Name = "lbl10";
            lbl10.Size = new System.Drawing.Size(112, 13);
            lbl10.Text = "Kér e reggelit?";
            teljesEllatas.Checked = false;
            teljesEllatas.Location = new System.Drawing.Point(hanyFo.Location.X, 310);
            teljesEllatas.Name = "teljesEllatas";
            teljesEllatas.CheckedChanged += new System.EventHandler(TeljesEll_CheckedChanged);
            lbl11.AutoSize = true;
            lbl11.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl11.Location = new System.Drawing.Point(teljesEllatas.Location.X - 105, 312);
            lbl11.Name = "lbl11";
            lbl11.Size = new System.Drawing.Size(112, 13);
            lbl11.Text = "Kér e teljes ellátást?";
            fizetve.Checked = false;
            fizetve.Location = new System.Drawing.Point(hanyFo.Location.X, 340);
            fizetve.Name = "fizetve";
            lbl12.AutoSize = true;
            lbl12.ForeColor = System.Drawing.SystemColors.ControlLight;
            lbl12.Location = new System.Drawing.Point(fizetve.Location.X - 84, 342);
            lbl12.Name = "lbl12";
            lbl12.Size = new System.Drawing.Size(112, 13);
            lbl12.Text = "Kifizeti e előre?";
            foglalasMenu.Location = new System.Drawing.Point(exit.Location.X + exit.Width + 10, exit.Location.Y + 11);
            foglalasMenu.Name = "jelentesek";
            foglalasMenu.Size = new System.Drawing.Size(75, 23);
            foglalasMenu.Text = "Jelentések";
            foglalasMenu.ForeColor = Color.White;
            foglalasMenu.FlatStyle = FlatStyle.Flat;
            foglalasMenu.Click += new EventHandler(foglalasMenu_Click);
            foglalasMenu.Visible = false;
            btnJelentesek.Location = new System.Drawing.Point(exit.Location.X + exit.Width + 10, exit.Location.Y+12);
            btnJelentesek.Name = "btnJelentesek";
            btnJelentesek.Size = new System.Drawing.Size(90, 23);
            btnJelentesek.Text = "Jelentések";
            btnJelentesek.ForeColor = Color.White;
            btnJelentesek.FlatStyle = FlatStyle.Flat;
            btnJelentesek.Click += new EventHandler(btnJelentesek_Click);
            Controls.Add(btnJelentesek);
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
            Controls.Add(fizetve);
            Controls.Add(reggeli);
            Controls.Add(teljesEllatas);
            Controls.Add(lbl10);
            Controls.Add(lbl11);
            Controls.Add(lbl12);
            Controls.Add(noRoomLabel);
            Controls.Add(foglalasMenu);
            MaximumSize = new System.Drawing.Size(820, 489);
            MinimumSize = new System.Drawing.Size(820, 489);
            Text = "Sanctum Vitae - Szállásfoglaló rendszer";
            exit.Width = 39;
            exit.Height = 39;
            exit.FlatStyle = FlatStyle.Flat;
            exit.FlatAppearance.BorderSize = 0;
            exit.BackColor = Color.Transparent;
            exit.Text = "";
            exit.BackgroundImage = System.Drawing.Image.FromFile("switch.png");
            exit.BackgroundImageLayout = ImageLayout.Zoom;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, exit.Width, exit.Height);
            exit.Region = new Region(gp);
            try
            {
                var myConnection = new MySqlConnection(dataConnectionString);
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"SELECT * FROM szobak ;";
                var myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var id = myReader.GetInt32("szobaAz");
                    var agy = myReader.GetInt32("agy");
                    var potagy = myReader.GetInt32("potagy");
                    szoba Szoba = new szoba(id, agy, potagy);
                    szobak.Add(Szoba);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                var myConnection = new MySqlConnection(dataConnectionString);
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"SELECT * FROM foglalasok ;";
                var myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var id = myReader.GetInt32("fsorsz");
                    var vendegID = myReader.GetInt32("vendeg");
                    var szoba = myReader.GetInt32("szoba");
                    var tol = myReader.GetDateTime("erk");
                    var ig = myReader.GetDateTime("tav");
                    var hanyFo = myReader.GetInt32("fo");
                    var reggeli = myReader.GetInt32("reggeli");
                    var teljesEllatas = myReader.GetInt32("teljesEll");
                    var fizetve = myReader.GetInt32("fizetve");
                    foglalas Foglalas = new foglalas(id, vendegID, szoba, tol, ig, hanyFo, reggeli, teljesEllatas, fizetve, szobak);
                    foglalasok.Add(Foglalas);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void foglalasMenu_Click(object sender, EventArgs e)
        {
        }
        private void btnJelentesek_Click(object sender, EventArgs e)
        {
            var torlendo = new List<Control>();
            foreach (Control c in Controls) if (c != exit && c != btnJelentesek) torlendo.Add(c);
            foreach (var c in torlendo) Controls.Remove(c);

            tlpJelentes = new TableLayoutPanel();
            tlpJelentes.Dock = DockStyle.Fill;
            tlpJelentes.ColumnCount = 1;
            tlpJelentes.RowCount = 2;
            tlpJelentes.RowStyles.Clear();

            tlpJelentes.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));
            tlpJelentes.RowStyles.Add(new RowStyle(SizeType.Absolute, 489 - 180));

            tlpJelentes.Padding = new Padding(8, 0, 8, 8);

            Controls.Add(tlpJelentes);

            var top = new TableLayoutPanel();
            top.Dock = DockStyle.Fill;
            top.ColumnCount = 2;
            top.RowCount = 1;
            top.ColumnStyles.Clear();
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 816 - 300));

            tlpJelentes.Controls.Add(top, 0, 0);

            var left = new FlowLayoutPanel();
            left.Dock = DockStyle.Fill;
            left.FlowDirection = FlowDirection.TopDown;
            left.WrapContents = false;
            left.Padding = new Padding(16, 24, 0, 0);


            rbDatum.Text = "Dátum";
            rbDatum.AutoSize = true;
            rbDatum.Checked = true;
            rbDatum.ForeColor = Color.White;
            dpDatum.Margin = new Padding(36, 4, 0, 8);


            dpDatum.Format = DateTimePickerFormat.Short;
            dpDatum.Width = 120;

            rbHonap.Text = "Hónap";
            rbHonap.AutoSize = true;
            rbHonap.ForeColor = Color.White;

            cbEv.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHonap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEv.Items.Clear();
            for (int ev = DateTime.Now.Year - 5; ev <= DateTime.Now.Year + 1; ev++) cbEv.Items.Add(ev);
            if (cbEv.Items.Count > 0) cbEv.SelectedItem = DateTime.Now.Year;
            cbHonap.Items.Clear();
            for (int h = 1; h <= 12; h++) cbHonap.Items.Add(h);
            cbHonap.SelectedItem = DateTime.Now.Month;

            var honapSor = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true, Margin = new Padding(36, 4, 0, 8) };
            cbEv.Width = 90;
            cbHonap.Width = 70;
            honapSor.Controls.Add(cbEv);
            honapSor.Controls.Add(cbHonap);

            rbAktualis.Text = "Aktuálisan itt tartózkodók";
            rbAktualis.AutoSize = true;
            rbAktualis.ForeColor = Color.White;

            left.Controls.Add(new Label() { Height = 18 });
            left.Controls.Add(rbDatum);
            left.Controls.Add(dpDatum);
            left.Controls.Add(new Label() { Height = 10 });
            left.Controls.Add(rbHonap);
            left.Controls.Add(honapSor);
            left.Controls.Add(rbAktualis);

            var right = new FlowLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.FlowDirection = FlowDirection.TopDown;
            right.WrapContents = false;

            var lblHova = new Label() { Text = "Hova mentené:", AutoSize = true, Margin = new Padding(8, 8, 0, 6) };

            var pathSor = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true, Margin = new Padding(-8, 4, 0, 8) };
            txtPdfUt.Width = 430;
            if (string.IsNullOrWhiteSpace(txtPdfUt.Text)) txtPdfUt.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "jelentes.pdf");
            btnBrowse = new Button() { Text = "...", Width = 36 };
            btnBrowse.Click -= btnBrowse_Click;
            btnBrowse.Click += btnBrowse_Click;
            btnBrowse.ForeColor = Color.White;
            pathSor.Controls.Add(txtPdfUt);
            pathSor.Controls.Add(btnBrowse);

            var gombSor = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true, Margin = new Padding(8, 6, 0, 0) };
            btnMegjelenit.Text = "Megjelenít";
            btnMegjelenit.Width = 120;
            btnMegjelenit.Click -= btnMegjelenit_Click;
            btnMegjelenit.Click += btnMegjelenit_Click;
            btnMegjelenit.ForeColor = Color.White;
            btnPdf.Text = "PDF";
            btnPdf.Width = 90;
            btnPdf.Click -= btnPdf_Click;
            btnPdf.Click += btnPdf_Click;
            btnPdf.ForeColor = Color.White;
            gombSor.Controls.Add(btnMegjelenit);
            gombSor.Controls.Add(btnPdf);

            right.Controls.Add(lblHova);
            right.Controls.Add(pathSor);
            right.Controls.Add(gombSor);

            top.Controls.Add(left, 0, 0);
            top.Controls.Add(right, 1, 0);

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.Margin = new Padding(8, 0, 8, 8);

            tlpJelentes.Controls.Add(grid, 0, 1);

            rbDatum.CheckedChanged -= (s, ea) => FrissitBeviteliLathatosag();
            rbHonap.CheckedChanged -= (s, ea) => FrissitBeviteliLathatosag();
            rbAktualis.CheckedChanged -= (s, ea) => FrissitBeviteliLathatosag();
            rbDatum.CheckedChanged += (s, ea) => FrissitBeviteliLathatosag();
            rbHonap.CheckedChanged += (s, ea) => FrissitBeviteliLathatosag();
            rbAktualis.CheckedChanged += (s, ea) => FrissitBeviteliLathatosag();

            dpDatum.ValueChanged -= (s, ea) => { if (rbDatum.Checked) grid.Visible = false; };
            cbEv.SelectedIndexChanged -= (s, ea) => { if (rbHonap.Checked) grid.Visible = false; };
            cbHonap.SelectedIndexChanged -= (s, ea) => { if (rbHonap.Checked) grid.Visible = false; };
            dpDatum.ValueChanged += (s, ea) => { if (rbDatum.Checked) grid.Visible = false; };
            cbEv.SelectedIndexChanged += (s, ea) => { if (rbHonap.Checked) grid.Visible = false; };
            cbHonap.SelectedIndexChanged += (s, ea) => { if (rbHonap.Checked) grid.Visible = false; };

            FrissitBeviteliLathatosag();
            grid.Visible = false;
        }
        private void FrissitBeviteliLathatosag()
        {
            dpDatum.Visible = rbDatum.Checked;
            cbEv.Visible = rbHonap.Checked;
            cbHonap.Visible = rbHonap.Checked;
        }
        private void btnMegjelenit_Click(object sender, EventArgs e)
        {
            ToltElonezet();
            grid.Visible = true;
        }
        private void ToltElonezet()
        {
            try
            {
                using (var conn = new MySqlConnection(dataConnectionString))
                {
                    conn.Open();
                    DataTable dt = new DataTable();
                    if (rbDatum.Checked)
                    {
                        string sql = @"
                    SELECT 
                        s.szobaAz AS Szoba,
                        (SELECT v.vnev 
                           FROM foglalasok f 
                           JOIN vendegek v ON v.vsorsz = f.vendeg
                          WHERE f.szoba = s.szobaAz AND @d BETWEEN f.erk AND f.tav
                          LIMIT 1) AS Vendég,
                        CASE 
                          WHEN EXISTS (SELECT 1 FROM foglalasok f WHERE f.szoba = s.szobaAz AND @d BETWEEN f.erk AND f.tav)
                               THEN (SELECT IF(f.fizetve, 'Kifizetve', 'Foglalt') 
                                       FROM foglalasok f 
                                      WHERE f.szoba = s.szobaAz AND @d BETWEEN f.erk AND f.tav LIMIT 1)
                          ELSE 'Üres'
                        END AS Állapot
                    FROM szobak s
                    ORDER BY s.szobaAz;";
                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@d", dpDatum.Value.Date);
                            using (var da = new MySqlDataAdapter(cmd)) da.Fill(dt);
                        }
                    }
                    else if (rbHonap.Checked)
                    {
                        int ev = Convert.ToInt32(cbEv.SelectedItem);
                        int honap = Convert.ToInt32(cbHonap.SelectedItem);
                        DateTime start = new DateTime(ev, honap, 1, 0, 0, 0);
                        DateTime end = start.AddMonths(1).AddSeconds(-1);
                        string sql = @"
                    SELECT 
                        v.vnev AS Vendég, 
                        v.telepules AS Település,
                        f.szoba AS Szoba, 
                        f.fo AS Fő, 
                        f.erk AS Érkezés, 
                        f.tav AS Távozás,
                        IF(f.fizetve, 'Kifizetve', 'Nincs kifizetve') AS Fizetés
                    FROM foglalasok f
                    JOIN vendegek v ON v.vsorsz = f.vendeg
                    WHERE f.erk <= @end AND f.tav >= @start
                    ORDER BY f.erk;";
                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@start", start);
                            cmd.Parameters.AddWithValue("@end", end);
                            using (var da = new MySqlDataAdapter(cmd)) da.Fill(dt);
                        }
                    }
                    else
                    {
                        string sql = @"
                    SELECT 
                        v.vnev AS Vendég,
                        v.telepules AS Település,
                        f.szoba AS Szoba,
                        f.erk AS Érkezés,
                        f.tav AS Távozás
                    FROM foglalasok f
                    JOIN vendegek v ON v.vsorsz = f.vendeg
                    WHERE v.aktivE = 1 AND NOW() BETWEEN f.erk AND f.tav
                    ORDER BY f.erk;";
                        using (var cmd = new MySqlCommand(sql, conn))
                        using (var da = new MySqlDataAdapter(cmd)) da.Fill(dt);
                    }
                    grid.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az előnézet betöltésekor: " + ex.Message);
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = Path.GetFileName(string.IsNullOrWhiteSpace(txtPdfUt.Text) ? "jelentes.pdf" : txtPdfUt.Text);
                sfd.InitialDirectory = Path.GetDirectoryName(string.IsNullOrWhiteSpace(txtPdfUt.Text) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : txtPdfUt.Text);
                if (sfd.ShowDialog() == DialogResult.OK) txtPdfUt.Text = sfd.FileName;
            }
        }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            var dt = grid.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Nincs megjelenített adat.");
                return;
            }
            string cel = txtPdfUt.Text;
            try
            {
                ExportDataTableToPdf(dt, rbDatum.Checked ? "Napi jelentés" : rbHonap.Checked ? "Havi jelentés" : "Aktuális vendégek", cel);
                MessageBox.Show("PDF sikeresen létrehozva:\n" + cel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF hiba: " + ex.Message);
            }
        }
        private void ExportDataTableToPdf(DataTable dt, string cim, string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4.Rotate(), 36, 36, 36, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                var fontCim = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var fontFejlec = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                Paragraph p = new Paragraph(cim, fontCim);
                p.Alignment = Element.ALIGN_LEFT;
                doc.Add(p);
                doc.Add(new Paragraph("Generálás: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                doc.Add(new Paragraph(" "));
                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(dt.Columns[i].ColumnName, fontFejlec));
                    cell.BackgroundColor = new BaseColor(230, 230, 230);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        string txt = dt.Rows[r][c] == null ? "" : Convert.ToString(dt.Rows[r][c]);
                        table.AddCell(new Phrase(txt, fontCell));
                    }
                }
                doc.Add(table);
                doc.Close();
                writer.Close();
            }
        }
        private void TeljesEll_CheckedChanged(object sender, EventArgs e)
        {
            reggeli.Checked = false;
            reggeli.Enabled = !teljesEllatas.Checked;
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
            JoE();
        }
        private void foglal_Click(object sender, EventArgs e)
        {
            if (kivalasztottSzoba.szobaId == -1)
            {
                MessageBox.Show("Kérem, válasszon szobát!");
                return;
            }
            string connectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO vendegek (vnev, telepules, koztNeve, koztTipusa, hazSz, telefonSz, hanyFo) VALUES (@Nev, @Telepules, @KoztNeve, @KoztTipusa, @Hazszam, @Telefon, @HanyFo )";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nev", vendegNeve.Text);
                    command.Parameters.AddWithValue("@Telepules", vendegIrsz.Text);
                    command.Parameters.AddWithValue("@KoztNeve", vendegKozteruletNeve.Text);
                    command.Parameters.AddWithValue("@KoztTipusa", vendegKoztTipusa.Text);
                    command.Parameters.AddWithValue("@Hazszam", vendegHazszam.Text);
                    command.Parameters.AddWithValue("@Telefon", vendegTel.Text);
                    command.Parameters.AddWithValue("@HanyFo", hanyFo.Text);
                    command.ExecuteNonQuery();
                }
                sql = "INSERT INTO foglalasok (vendeg, szoba, erk, tav, fo, reggeli, teljesEll, fizetve) VALUES (@vendeg, @szoba, @erk, @tav, @fo, @reggeli, @teljesEll, @fizetve )";
                var myConnection = new MySqlConnection(dataConnectionString);
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = @"
                SELECT vendegek.vsorsz as ID 
                FROM vendegek 
                WHERE vnev = @vnev 
                AND telepules = @telepules 
                AND koztNeve = @koztNeve 
                AND koztTipusa = @koztTipusa 
                AND hazSz = @hazSz 
                AND telefonSz = @telefonSz 
                AND hanyFo = @hanyFo 
                AND aktivE;";
                myCommand.Parameters.AddWithValue("@vnev", vendegNeve.Text);
                myCommand.Parameters.AddWithValue("@telepules", vendegIrsz.Text);
                myCommand.Parameters.AddWithValue("@koztNeve", vendegKozteruletNeve.Text);
                myCommand.Parameters.AddWithValue("@koztTipusa", vendegKoztTipusa.Text);
                myCommand.Parameters.AddWithValue("@hazSz", vendegHazszam.Text);
                myCommand.Parameters.AddWithValue("@telefonSz", vendegTel.Text);
                myCommand.Parameters.AddWithValue("@hanyFo", hanyFo.Text);
                var myReader = myCommand.ExecuteReader();
                int id = -1;
                while (myReader.Read())
                {
                    id = myReader.GetInt32("ID");
                }
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@vendeg", id);
                    command.Parameters.AddWithValue("@szoba", kivalasztottSzoba.szobaId);
                    command.Parameters.AddWithValue("@erk", naptar.SelectionStart);
                    command.Parameters.AddWithValue("@tav", naptar.SelectionEnd);
                    command.Parameters.AddWithValue("@fo", hanyFo.Text);
                    command.Parameters.AddWithValue("@reggeli", reggeli.Checked);
                    command.Parameters.AddWithValue("@teljesEll", teljesEllatas.Checked);
                    command.Parameters.AddWithValue("@fizetve", fizetve.Checked);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            MessageBox.Show("A foglalás sikeres!");
            ertekTorol();
        }
        private void klikkSzoba(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string text = button.Text;
            string[] parts = text.Split(':');
            int szobaId = int.Parse(parts[1].Substring(0, 2).Trim());
            int agy = int.Parse(parts[2].Substring(0, 2).Trim());
            int potAgy = int.Parse(parts[3].Trim().TrimEnd(')'));
            kivalasztottSzoba = new szoba(szobaId, agy, potAgy);
        }
        public bool JoE()
        {
            List<Control> toRemove = new List<Control>();
            foreach (Control ctrl in Controls)
                if (ctrl is Button && ctrl.Text.Contains("Szoba")) toRemove.Add(ctrl);
            for (int i = 0; i < toRemove.Count; i++) Controls.Remove(toRemove[i]);
            noRoomLabel.Text = "";
            if (vendegNeve.Text != string.Empty && vendegIrsz.Text != string.Empty && vendegKoztTipusa.Text != string.Empty && vendegTel.Text != string.Empty && int.TryParse(vendegHazszam.Text, out int b) && tolIg != "" && vendegKozteruletNeve.Text != "" && int.TryParse(hanyFo.Text, out int c) && c > 0)
            {
                foglal.Enabled = true;
                foglal.BackColor = Color.Green;
                int fo;
                szabadSzobak.Clear();
                if (int.TryParse(hanyFo.Text, out fo))
                {
                    DateTime kezd = naptar.SelectionStart;
                    DateTime veg = naptar.SelectionEnd;
                    for (int i = 0; i < szobak.Count; i++)
                    {
                        szoba aktualisSzoba = szobak[i];
                        if ((aktualisSzoba.agy + aktualisSzoba.potAgy) < fo)
                            continue;
                        bool foglalt = false;
                        for (int j = 0; j < foglalasok.Count; j++)
                        {
                            foglalas aktualisFoglalas = foglalasok[j];
                            if (aktualisFoglalas.szoba.szobaId == aktualisSzoba.szobaId)
                            {
                                DateTime foglTol = aktualisFoglalas.tol;
                                DateTime foglIg = aktualisFoglalas.ig;
                                if (kezd <= foglIg && veg >= foglTol)
                                {
                                    foglalt = true;
                                    break;
                                }
                            }
                        }
                        if (!foglalt)
                        {
                            szabadSzobak.Add(aktualisSzoba);
                        }
                    }
                }
                if (szabadSzobak.Count != 0)
                {
                    for (int i = 0; i < szabadSzobak.Count; i++)
                    {
                        Button tmp = new Button();
                        tmp.Location = new System.Drawing.Point(300, 75 + (i * 30));
                        tmp.Size = new System.Drawing.Size(200, 23);
                        tmp.Text = $"Szoba: {szabadSzobak[i].szobaId} (Ágyak: {szabadSzobak[i].agy}, Pótágyak: {szabadSzobak[i].potAgy})";
                        tmp.BackColor = Color.Black;
                        tmp.ForeColor = Color.White;
                        tmp.Click += new System.EventHandler(klikkSzoba);
                        Controls.Add(tmp);
                    }
                }
                else
                {
                    noRoomLabel.Text = "Sajnos a keresett feltételek mellett nincs szabad szoba.";
                    noRoomLabel.ForeColor = Color.Red;
                    noRoomLabel.Location = new System.Drawing.Point(300, 75);
                    noRoomLabel.AutoSize = true;
                }
                return true;
            }
            else
            {
                foglal.Enabled = false;
                foglal.BackColor = Color.Red;
                return false;
            }
        }
        private void vizsgal(object sender, EventArgs e)
        {
            JoE();
        }
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
            hanyFo.Text = "";
            reggeli.Checked = false;
            teljesEllatas.Checked = false;
            fizetve.Checked = false;
        }
        private void exit_Click(object sender, EventArgs e)
        {
            string file = @"kezdoFoglalasok.sql";
            using (var conn = new MySqlConnection(dataConnectionString))
            {
                conn.Open();
                var sb = new StringBuilder();
                sb.AppendLine(beszuras(conn, "szobak", "agy, potagy"));
                sb.AppendLine();
                sb.AppendLine(beszuras(conn, "vendegek", "vnev, telepules, koztNeve, koztTipusa, hazSz, telefonSz, hanyFo, aktivE", true));
                sb.AppendLine();
                sb.AppendLine(beszuras(conn, "foglalasok", "vendeg, szoba, erk, tav, fo, reggeli, teljesEll, fizetve", true));
                File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
            }
            Application.Exit();
        }
        private string beszuras(MySqlConnection conn, string table, string cols, bool boolAsText = false)
        {
            var list = cols.Split(',');
            for (int i = 0; i < list.Length; i++) list[i] = list[i].Trim();
            string q = $"SELECT {string.Join(", ", list)} FROM {table}";
            var cmd = new MySqlCommand(q, conn);
            var r = cmd.ExecuteReader();
            var lines = new List<string>();
            while (r.Read())
            {
                var vals = new List<string>();
                foreach (var c in list)
                {
                    object v = r[c];
                    vals.Add(Format(v, boolAsText));
                }
                lines.Add("(" + string.Join(", ", vals) + ")");
            }
            r.Close();
            return $"INSERT INTO {table} ({string.Join(", ", list)}) VALUES\n{string.Join(",\n", lines)};";
        }
        private string Format(object v, bool boolAsText)
        {
            if (v == DBNull.Value) return "NULL";
            if (v is bool b) return boolAsText ? (b ? "TRUE" : "FALSE") : (b ? "1" : "0");
            if (v is DateTime d) return $"'{d:yyyy-MM-dd HH:mm:ss}'";
            if (v is int || v is double || v is decimal || v is float)
                return Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture);
            return $"'{v.ToString().Replace("'", "''")}'";
        }
    }
}
