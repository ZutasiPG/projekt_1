using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Projekt1{
    public partial class Form1 : Form{
        public Form1(){
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
        public void joE(){
            if (vendegNeve.Text != null && vendegUtca.Text != null && vendegTel.Text != null && int.TryParse(vendegIrsz.Text, out int a) && int.TryParse(vendegHazszam.Text, out int b) && tolIg != ""){
                foglal.Enabled = true;
                foglal.BackColor = Color.Green;
            }
            else{
                foglal.Enabled = false;
                foglal.BackColor = Color.Red;
            }
        }
        DateTime? kezdodatum = null;
        bool programbolValtozik = false;
        public static string tolIg = "";
        private void naptar_DateChanged(object sender, DateRangeEventArgs e){
            if (programbolValtozik) return;
            if (kezdodatum == null) kezdodatum = e.Start;
            else{
                DateTime kezd = (kezdodatum < e.Start) ? (DateTime)kezdodatum : e.Start;
                DateTime veg = (kezdodatum > e.Start) ? (DateTime)kezdodatum : e.Start;
                programbolValtozik = true;
                naptar.SelectionStart = kezd;
                naptar.SelectionEnd = veg;
                programbolValtozik = false;
                //MessageBox.Show($"Kijelölt intervallum: {kezd.ToShortDateString()} - {veg.ToShortDateString()}");
                tolIg = $"{kezd.ToShortDateString()} - {veg.ToShortDateString()}";
                kezdodatum = null;
            }
            joE();
        }
        private void exit_Click(object sender, EventArgs e){
            Application.Exit();
        }
        private void foglal_Click(object sender, EventArgs e){
            MessageBox.Show("A foglalás sikeres!");
            
        }
        private void vendegNeve_TextChanged(object sender, EventArgs e){
            joE();
        }
        

        private void vendegIrsz_TextChanged(object sender, EventArgs e){
            joE() ;
        }

        private void vendegUtca_TextChanged(object sender, EventArgs e){
            joE();
        }

        private void vendegHazszam_TextChanged(object sender, EventArgs e){
            joE();
        }

        private void vendegTel_TextChanged(object sender, EventArgs e){
            joE();
        }
    }
}
