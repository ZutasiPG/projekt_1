using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace Projekt1
{
    public partial class Form1 : Form
    {
        private const string DatabaseSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/refs/heads/main/database.sql?token=GHSAT0AAAAAADMNT2LLOYS56UUAKJ4R3YXK2HSL3TQ";
        private const string KezdoFoglalasokSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/refs/heads/main/kezdoFoglalasok.sql?token=GHSAT0AAAAAADMNT2LLBHMZKCVMJRJU3N6U2HSL4BA";

        public static bool bad = false;

        public Form1()
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
            exit.BackgroundImage = Image.FromFile("switch.png");
            exit.BackgroundImageLayout = ImageLayout.Zoom;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, exit.Width, exit.Height);
            exit.Region = new Region(gp);

            Task.Run(() => InitializeDatabaseFromWeb()).Wait();
        }

        private async Task InitializeDatabaseFromWeb()
        {
            try
            {
                string connectionString = "Server=localhost;Database=;User ID=root;Password=mysql;";
                string databaseSql;

                using (HttpClient client = new HttpClient())
                {
                    databaseSql = await client.GetStringAsync(DatabaseSqlUrl);
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(databaseSql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                connectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
                string kezdoFoglalasokSql;

                using (HttpClient client = new HttpClient())
                {
                    kezdoFoglalasokSql = await client.GetStringAsync(KezdoFoglalasokSqlUrl);
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string clearSql = @"
                                    SET FOREIGN_KEY_CHECKS = 0;
                                    TRUNCATE TABLE foglalasok;
                                    TRUNCATE TABLE vendegek;
                                    TRUNCATE TABLE szobak;
                                    TRUNCATE TABLE iranyitoszamok;
                                    SET FOREIGN_KEY_CHECKS = 1;
                                ";
                    using (MySqlCommand clearCommand = new MySqlCommand(clearSql, connection))
                    {
                        await clearCommand.ExecuteNonQueryAsync();
                    }

                    using (MySqlCommand command = new MySqlCommand(kezdoFoglalasokSql, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Exit();
                MessageBox.Show("Hiba az adatbázis inicializálásakor! Az adatbázis nem elérhető vagy a szkript letöltése sikertelen." + Environment.NewLine + ex.Message);
            }
        }

        public void joE()
        {
            if (vendegNeve.Text != string.Empty && vendegUtca.Text != string.Empty && vendegTel.Text != string.Empty && int.TryParse(vendegIrsz.Text, out int a) && int.TryParse(vendegHazszam.Text, out int b) && tolIg != "")
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

        DateTime? kezdodatum = null;
        public static string tolIg = "";

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

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ertekTorol()
        {
            vendegNeve.Text = "";
            vendegIrsz.Text = "";
            vendegUtca.Text = "";
            vendegHazszam.Text = "";
            vendegTel.Text = "";
            naptar.SelectionStart = DateTime.Now;
            naptar.SelectionEnd = DateTime.Now;
            tolIg = "";
        }

        private void foglal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A foglalás sikeres!");
            string connectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO vendegek (vnev, irsz, utca, hazSz, telefonSz) " +
                             "VALUES (@Nev, @Irsz, @Utca, @Hazszam, @Telefon)";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nev", vendegNeve.Text);
                    command.Parameters.AddWithValue("@Irsz", vendegIrsz.Text);
                    command.Parameters.AddWithValue("@Utca", vendegUtca.Text);
                    command.Parameters.AddWithValue("@Hazszam", vendegHazszam.Text);
                    command.Parameters.AddWithValue("@Telefon", vendegTel.Text);
                    command.ExecuteNonQuery();
                }
            }
            ertekTorol();
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}