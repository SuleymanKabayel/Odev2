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
    public partial class frmYapiliyorListele : Form
    {
        public frmYapiliyorListele()
        {
            InitializeComponent();
        }

        //degiskenler atandi 
        public int Proje_ID;
        
        public frmMain frm1;
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;Integrated Security=True");
        DataSet set2 = new DataSet();

        private void frmYapiliyorListele_Load(object sender, EventArgs e)
        {
            Task_Goster();
            Set_DataGridView();
        }

        //Datagrideki verileri yazzdirdik 
        private void Set_DataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Task_Goster()
        {
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from [yapiliyor] where projeid ='" + Proje_ID + "'", baglanti);
            adapter.Fill(set2, "yapiliyor");
            dataGridView1.DataSource = set2.Tables["yapiliyor"];
            baglanti.Close();
            //projedeki var olan gorevleri yazdirdik
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
            txtAd.Text = dataGridView1.CurrentRow.Cells["ad"].Value.ToString();
            txtKisi.Text = dataGridView1.CurrentRow.Cells["kisi"].Value.ToString();
            txtTarih.Text = dataGridView1.CurrentRow.Cells["tarih"].Value.ToString();
            txtAciklama.Text = dataGridView1.CurrentRow.Cells["aciklama"].Value.ToString();
            kartnosu.Text = dataGridView1.CurrentRow.Cells["kartno"].Value.ToString();
            //secilen islerin ozeliklerini texboxlara yazdirdik 
        }

        private void btnToDoGüncelle_Click(object sender, EventArgs e)
        {
            // yapilan degisikleri texboxdan veritabanina yazdirdik 
            baglanti.Open();
            SqlCommand Baglanti_Guncel = new SqlCommand("update [yapiliyor] set ad=@ad, kisi=@kisi, tarih=@tarih, aciklama=@aciklama where kartno=@kartno", baglanti);
            Baglanti_Guncel.Parameters.AddWithValue("@kartno", kartnosu.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@tarih", txtTarih.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@ad", txtAd.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@kisi", txtKisi.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
            Baglanti_Guncel.ExecuteNonQuery();
            baglanti.Close();
            set2.Tables["yapiliyor"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş güncellendi.");

            //textboxlari temizledik
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnKaldır_Click(object sender, EventArgs e)
        {

            //secilen isin tablodan silindigi  yer 
            baglanti.Open();
            SqlCommand sil = new SqlCommand("delete from [yapiliyor] where kartno='" + dataGridView1.CurrentRow.Cells["kartno"].Value.ToString() + "'", baglanti);
            sil.ExecuteNonQuery();
            baglanti.Close();
            set2.Tables["yapiliyor"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş kaldırıldı.");

        }

        private void txtKartNoAra_TextChanged(object sender, EventArgs e)
        {

            //kart numarsindan isleri ariyor
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter ara = new SqlDataAdapter("select *from [yapiliyor] where kartno like'%" + txtKartNoAra.Text + "%'", baglanti);
            ara.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
