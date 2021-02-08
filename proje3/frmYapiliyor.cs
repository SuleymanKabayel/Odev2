using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proje3
{
    public partial class frmYapiliyor : Form
    {
        public frmYapiliyor()
        {
            InitializeComponent();
        }
        /**değişkenlerimizi belirledik*/
        public string Proje_Kart_Adi;
        public int Proje_ID;
        public frmMain frmmm;
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;");
        private void frmYapiliyor_Load(object sender, EventArgs e)
        {
            //random ID gibi random gorev numarasi atadik
            Random rastgele = new Random();
            int sayi = rastgele.Next(10000);
            kartnosu.Text = Convert.ToString(sayi);
            P_Adilabel.Text = Proje_Kart_Adi;

        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            //girilen verileri veri tabanina yazdiriyoruz 
            baglanti.Open();
            SqlCommand Baglanti_is = new SqlCommand("insert into [yapiliyor](projeid,P_Adi,kartno,tarih,ad,kisi,aciklama) values(@projeid,@P_Adi,@kartno,@tarih,@ad,@kisi,@aciklama)", baglanti);
            Baglanti_is.Parameters.AddWithValue("@projeid",Proje_ID );
            Baglanti_is.Parameters.AddWithValue("@P_Adi", Proje_Kart_Adi);
            Baglanti_is.Parameters.AddWithValue("@kartno", kartnosu.Text);
            Baglanti_is.Parameters.AddWithValue("@tarih", txtTarih.Text);
            Baglanti_is.Parameters.AddWithValue("@ad", txtAd.Text);
            Baglanti_is.Parameters.AddWithValue("@kisi", txtKisi.Text);
            Baglanti_is.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
            Baglanti_is.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Yeni iş eklendi.");

            //Eski yazilari siliyoruz(temizliyoruz)
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }

            
            //frmmm.topZaman += 1;


            this.Close();
        }
    }
}
