using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;

namespace Projekt1
{
    public partial class Form1 : Form
    {
        private const string DatabaseSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/main/database.sql";
        private const string KezdoFoglalasokSqlUrl = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/main/kezdoFoglalasok.sql";
        private const string iranyitoSzamokSql = "https://raw.githubusercontent.com/ZutasiPG/projekt_1/main/addIrsz.sql";

        public static bool bad = false;

        public Form1()
        {
            if (Task.Run(() => InitializeDatabaseFromWeb()).Result)
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
            }
            else
            {
                bad = true;
                Application.Exit(); // Alkalmazás bezárása hiba esetén
            }
        }

        private async Task<bool> InitializeDatabaseFromWeb()
        {
            try
            {
                string masterConnectionString = "Server=localhost;Database=;User ID=root;Password=mysql;";
                string dataConnectionString = "Server=localhost;Database=projekt1;User ID=root;Password=mysql;";
                string databaseSql;
                string kezdoFoglalasokSqlContent;
                string iranyitoSzamokSqlContent;

                using (HttpClient client = new HttpClient())
                {
                    databaseSql = await client.GetStringAsync(DatabaseSqlUrl);
                    kezdoFoglalasokSqlContent = await client.GetStringAsync(KezdoFoglalasokSqlUrl);
                    iranyitoSzamokSqlContent = await client.GetStringAsync(iranyitoSzamokSql);
                }

                using (MySqlConnection masterConnection = new MySqlConnection(masterConnectionString))
                {
                    await masterConnection.OpenAsync();

                    string dropSql = "DROP DATABASE IF EXISTS projekt1; CREATE DATABASE IF NOT EXISTS projekt1;"; // biztos létrehozás
                    using (MySqlCommand dropCommand = new MySqlCommand(dropSql, masterConnection))
                    {
                        await dropCommand.ExecuteNonQueryAsync();
                    }

                    using (MySqlCommand command = new MySqlCommand(databaseSql, masterConnection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                using (MySqlConnection connection = new MySqlConnection(dataConnectionString))
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

                    // HELEYES SORREND: ELŐSZÖR AZ IRÁNYÍTÓSZÁMOK (Parent) BESZÚRÁSA!
                    using (MySqlCommand irszCommand = new MySqlCommand(iranyitoSzamokSqlContent, connection))
                    {
                        // A MySqlCommand automatikusan elválasztja a pontosvesszővel elválasztott parancsokat.
                        // Ha a hiba ismétlődik, manuális szétbontásra lesz szükség.
                        await irszCommand.ExecuteNonQueryAsync();
                    }

                    // MÁSODSZOR A VENDÉGEK/FOGLALÁSOK (Child) BESZÚRÁSA
                    using (MySqlCommand command = new MySqlCommand(kezdoFoglalasokSqlContent, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az adatbázis inicializálásakor! Az adatbázis nem elérhető vagy a szkript letöltése sikertelen." + Environment.NewLine + ex.Message);
                return false;
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