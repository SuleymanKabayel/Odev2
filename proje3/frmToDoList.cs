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
    public partial class frmToDoList : Form
    {
        public frmToDoList()
        {
            InitializeComponent();
        }

        /**değişkenlerimizi belirledik*/
        public int Proje_ID;
        
        public frmMain frm;
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;Integrated Security=True");
        DataSet set1 = new DataSet();
        private void frmToDoList_Load(object sender, EventArgs e)
        {
            //fonksiyonlar cagiriliyor 
            Task_Goster();
            Set_DataGridView();
        }

        private void Set_DataGridView()
        {
            // verilerin  listeledik 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Task_Goster()
        {
            // olan projedeki isleri yazdirdik 
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from [is_takip_table] where projeid ='" + Proje_ID + "'", baglanti);
            adapter.Fill(set1, "is_takip_table");
            dataGridView1.DataSource = set1.Tables["is_takip_table"];
            baglanti.Close();
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
            SqlCommand Baglanti_Guncel = new SqlCommand("update [is_takip_table] set ad=@ad, kisi=@kisi, tarih=@tarih, aciklama=@aciklama where kartno=@kartno", baglanti);
            Baglanti_Guncel.Parameters.AddWithValue("@kartno", kartnosu.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@tarih", txtTarih.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@ad", txtAd.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@kisi", txtKisi.Text);
            Baglanti_Guncel.Parameters.AddWithValue("@aciklama", txtAciklama.Text);
            Baglanti_Guncel.ExecuteNonQuery();
            baglanti.Close();
            set1.Tables["is_takip_table"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş güncellendi.");
            
            //txtboxlari temizledik 
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }

        }

        public int delete_Data(string s)
        {
            baglanti.Open();
            SqlCommand sil = new SqlCommand("delete from [is_takip_table] where kartno='" + s + "'", baglanti);
            int i = 0;
            i = sil.ExecuteNonQuery();
            baglanti.Close();
            return i;
        }
        private void btnKaldır_Click(object sender, EventArgs e)
        {
            //secilen isin tablodan silindigi  yer 
            delete_Data(dataGridView1.CurrentRow.Cells["kartno"].Value.ToString());
            set1.Tables["is_takip_table"].Clear();
            Task_Goster();
            MessageBox.Show("Seçilen iş kaldırıldı.");


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //kart numarsindan isleri ariyor
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter ara = new SqlDataAdapter("select *from [is_takip_table] where kartno like'%"+txtKartNoAra.Text +"%'", baglanti);
            ara.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();

        }
    }
}
