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

namespace proje3
{
    public partial class proje : Form
    {
        //server baglantisi kuruluyor
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;");

        public int Yeni_proje_ID = 0;
        public proje()
        {
            InitializeComponent();
        }
       

        
        test pp = new test();

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;

            // yeni proje acildiginda onun adini acilan forma verdik 
            frmMain frm = new frmMain();
            frm.P_Adi = textBox2.Text;


            //yeni olusan projey random komutu ile ID ara
            Random rastgele = new Random();
            int sayi = rastgele.Next(10000);
            frm.Yeni_Proje_Adi = sayi;

            frm.Show();
            this.Hide();
            i++;
            Yeni_proje_ID = pp.projeHesap(i);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            //var olan proje Id gonderiliyor
            frmMain frm = new frmMain();
            frm.Yeni_Proje_Adi =Convert.ToInt32( textBox1.Text);
            baglanti.Open();
            
           // var olan projelerden ID ile proje adini cektik

            String[] dizi = { "is_takip_table", "yapiliyor", "yapilmis" };

            //diger toblolarin bos olma olasiligina karsi tum tablolardan proje adi aradik ve cektik
            for (int i = 0; i < dizi.Length; i++)
            {
                String s = "select * from  " + dizi[i];
                SqlCommand cmd1 = new SqlCommand(s, baglanti);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    frm.P_Adi = (string)dr1["P_Adi"];
                    break;

                }

                else
                {
                    frm.P_Adi = "";
                }
                dr1.Close();
            }
            frm.Show();
            this.Hide();

            baglanti.Close();
        }

      
        private void proje_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
