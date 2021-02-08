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
    public partial class frmYapilmisListele : Form
    {
        public frmYapilmisListele()
        {
            InitializeComponent();
        }

        //degiskenler atandi
        public int Proje_ID;

        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;");

        DataSet set3 = new DataSet();
        private void frmYapilmisListele_Load(object sender, EventArgs e)
        {
           //fonksiyonlari baslatiyoruz
            Task_Goster();
            Set_DataGridView();
        }

        private void Set_DataGridView()
        {
            // datadaki veriler goruntulendi
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Task_Goster()
        {
            // ID si verilen Projenin verilerini cektik 
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from [yapilmis] where projeid ='" + Proje_ID + "'", baglanti);
            adapter.Fill(set3, "yapilmis");
            dataGridView1.DataSource = set3.Tables["yapilmis"];
            baglanti.Close();
        }

        private void btnToDoGüncelle_Click(object sender, EventArgs e)
        {
            // girilen degerleri databaseye yoladik
            baglanti.Open();
            SqlCommand Baglanti_Guncel = new SqlCommand("update [yapilmis] set ad=@ad, kisi=@kisi, tarih=@tarih, aciklama=@aciklama where kartno=@kartno", baglanti);
            Baglanti_Guncel.Parameters.AddWithValue("@kartno", kartnosu.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@tarih", txtTarih.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@ad", txtAd.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@kisi", txtKisi.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
            Baglanti_Guncel.ExecuteNonQuery();
            baglanti.Close();
            set3.Tables["yapilmis"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş güncellendi.");

            // txtboxlardan verileri temizledik
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        //secili verileri txtboxlara yazdik
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAd.Text = dataGridView1.CurrentRow.Cells["ad"].Value.ToString();
            txtKisi.Text = dataGridView1.CurrentRow.Cells["kisi"].Value.ToString();
            txtTarih.Text = dataGridView1.CurrentRow.Cells["tarih"].Value.ToString();
            txtAciklama.Text = dataGridView1.CurrentRow.Cells["aciklama"].Value.ToString();
            kartnosu.Text = dataGridView1.CurrentRow.Cells["kartno"].Value.ToString();
        }

        // veriyi data baseden sildirme fonksiyonu 
        private void btnKaldır_Click(object sender, EventArgs e)
        {
            
            baglanti.Open();
            SqlCommand sil = new SqlCommand("delete from [yapilmis] where kartno='" + dataGridView1.CurrentRow.Cells["kartno"].Value.ToString() + "'", baglanti);
            sil.ExecuteNonQuery();
            baglanti.Close();
            set3.Tables["yapilmis"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş kaldırıldı.");
        }

        private void txtKartNoAra_TextChanged(object sender, EventArgs e)
        {
            //kart numarsindan isleri ariyor
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter ara = new SqlDataAdapter("select *from [yapilmis] where kartno like'%" + txtKartNoAra.Text + "%'", baglanti);
            ara.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
