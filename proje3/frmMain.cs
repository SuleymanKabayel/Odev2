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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //degiskenler tanimlaniyor
         public string P_Adi;
         public int Yeni_Proje_Adi;
       
         public int yapiliyorZaman ;


        // data  baglanti kodu
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-FVKJQ22D\\MSSQLSERVER01;Initial Catalog=task_takip;Integrated Security=True");
       
        //data setleri daha rahat kulanmak  icin atam
        DataSet set1 = new DataSet();
        DataSet set2 = new DataSet();
        DataSet set3 = new DataSet();

        


        private void btnToDoEkle_Click(object sender, EventArgs e)
        {
            // yapilacklara veri ekleniyor
            frmToDo ekle = new frmToDo(); 
            ekle.Proje_Kart_Adi = P_Adi;
            ekle.Proje_ID = Yeni_Proje_Adi;
            ekle.frm = this;
            ekle.ShowDialog();

        }

        private void btnToDoList_Click(object sender, EventArgs e)
        {
            // yapilacaklar listesine  yolaniyor
            frmToDoList listele = new frmToDoList();
            listele.Proje_ID = Yeni_Proje_Adi;
            listele.ShowDialog();
          
        }

        private void btnEkleYapiliyor_Click(object sender, EventArgs e)
        {
            // yapiliyora veri ekleniyor
            frmYapiliyor yapiliyor = new frmYapiliyor();
            yapiliyor.Proje_Kart_Adi = P_Adi;
            yapiliyor.Proje_ID = Yeni_Proje_Adi;
            yapiliyor.frmmm = this;
            yapiliyor.ShowDialog();
          
        }

        private void btninprogress_Click(object sender, EventArgs e)
        {
            // yapiliyor listesine yolaniyor
            frmYapiliyorListele yapiliyorlistele = new frmYapiliyorListele();
            yapiliyorlistele.Proje_ID = Yeni_Proje_Adi;
            yapiliyorlistele.ShowDialog();
            
        }

        private void btnEkleYapilmis_Click(object sender, EventArgs e)
        {
            //yapilmis islere is ekleniyor
            frmYapilmis yapilmis = new frmYapilmis();
            yapilmis.Proje_Kart_Adi = P_Adi;
            yapilmis.Proje_ID = Yeni_Proje_Adi;
            yapilmis.frmm = this;
            yapilmis.ShowDialog();
          
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            // yapilmis olanlar listesine yoluyor 
            frmYapilmisListele yapilmislistele = new frmYapilmisListele();
            yapilmislistele.Proje_ID = Yeni_Proje_Adi;
            yapilmislistele.ShowDialog();
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // form acildiginda fonksiyonlari calitiriyor
            
            Task_Goster();
            Set_DataGridView();
            projeid.Text = Convert.ToString(Yeni_Proje_Adi);
            P_Adinew.Text = P_Adi;

        }


        private void Set_DataGridView()
        {

            // tablolari gosteriyor
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void Task_Goster()
        {
            int i = -2; 
            
            // ID si erilen projedeki datalari gosteren fonksiyon
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from [is_takip_table] where projeid ='" +Yeni_Proje_Adi+"'", baglanti);            
            adapter.Fill(set1, "is_takip_table");
            dataGridView1.DataSource = set1.Tables["is_takip_table"];
            i += dataGridView1.RowCount;
            SqlDataAdapter adapter2 = new SqlDataAdapter("select *from [yapiliyor] where projeid ='" + Yeni_Proje_Adi + "'", baglanti);   
            adapter2.Fill(set2, "yapiliyor");
            dataGridView2.DataSource = set2.Tables["yapiliyor"];
            i += dataGridView2.RowCount;
            SqlDataAdapter adapter3 = new SqlDataAdapter("select *from [yapilmis] where projeid ='" + Yeni_Proje_Adi + "'", baglanti);
            adapter3.Fill(set3, "yapilmis");
            dataGridView3.DataSource = set3.Tables["yapilmis"];
            baglanti.Close();

            
        }

        

        
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            //drag and drop fonksiyonun baslangici 
            
            int SourceRow;
            SourceRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            dataGridView1.DoDragDrop(SourceRow, DragDropEffects.Copy);

        }

        private void dataGridView2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dataGridView2_DragDrop(object sender, DragEventArgs e)
        {
            
            int SourceRow = Convert.ToInt32(e.Data.GetData(Type.GetType("System.Int32")));
            Point clientPoint = dataGridView2.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = dataGridView2.HitTest(clientPoint.X, clientPoint.Y);
            if(hit.Type == DataGridViewHitTestType.Cell)
            {
                
                baglanti.Open();
                SqlCommand sil = new SqlCommand("delete from [is_takip_table] where kartno='" + dataGridView1.CurrentRow.Cells["kartno"].Value.ToString() + "'", baglanti);
                sil.ExecuteNonQuery();

               
                SqlCommand Baglanti_is = new SqlCommand("insert into [yapiliyor](projeid,P_Adi,kartno,tarih,ad,kisi,aciklama) values(@projeid,@P_Adi,@kartno,@tarih,@ad,@kisi,@aciklama)", baglanti);
                Baglanti_is.Parameters.AddWithValue("@P_Adi", dataGridView1.CurrentRow.Cells[1].Value);
                Baglanti_is.Parameters.AddWithValue("@projeid", dataGridView1.CurrentRow.Cells[0].Value);
                Baglanti_is.Parameters.AddWithValue("@kartno", dataGridView1.CurrentRow.Cells[2].Value);
                Baglanti_is.Parameters.AddWithValue("@tarih", dataGridView1.CurrentRow.Cells[3].Value);
                Baglanti_is.Parameters.AddWithValue("@ad", dataGridView1.CurrentRow.Cells[4].Value);
                Baglanti_is.Parameters.AddWithValue("@kisi", dataGridView1.CurrentRow.Cells[5].Value);
                Baglanti_is.Parameters.AddWithValue("@aciklama", dataGridView1.CurrentRow.Cells[6].Value);
                Baglanti_is.ExecuteNonQuery();
                baglanti.Close();
                int DestRow = hit.RowIndex;
                int DestCol = hit.ColumnIndex;

                for(int u=0; u<6; u++)
                {
                    dataGridView2.Rows[DestRow].Cells[u].Value = dataGridView1.Rows[SourceRow].Cells[u].Value;
                }
                
            }
        }

        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {
          
            int SourceRow;
            SourceRow = dataGridView2.HitTest(e.X, e.Y).RowIndex;
            dataGridView1.DoDragDrop(SourceRow, DragDropEffects.Copy);
        }

        private void dataGridView3_DragDrop(object sender, DragEventArgs e)
        {
            int SourceRow = Convert.ToInt32(e.Data.GetData(Type.GetType("System.Int32")));
            Point clientPoint = dataGridView3.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = dataGridView3.HitTest(clientPoint.X, clientPoint.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                
                baglanti.Open();
                SqlCommand sil = new SqlCommand("delete from [yapiliyor] where kartno='" + dataGridView2.CurrentRow.Cells["kartno"].Value.ToString() + "'", baglanti);
                sil.ExecuteNonQuery();

                
                SqlCommand Baglanti_is = new SqlCommand("insert into [yapilmis](projeid,P_Adi,kartno,tarih,ad,kisi,aciklama) values(@projeid,@P_Adi,@kartno,@tarih,@ad,@kisi,@aciklama)", baglanti);
                Baglanti_is.Parameters.AddWithValue("@P_Adi", dataGridView2.CurrentRow.Cells[1].Value);
                Baglanti_is.Parameters.AddWithValue("@projeid", dataGridView2.CurrentRow.Cells[0].Value);
                Baglanti_is.Parameters.AddWithValue("@kartno", dataGridView2.CurrentRow.Cells[2].Value);
                Baglanti_is.Parameters.AddWithValue("@tarih", dataGridView2.CurrentRow.Cells[3].Value);
                Baglanti_is.Parameters.AddWithValue("@ad", dataGridView2.CurrentRow.Cells[4].Value);
                Baglanti_is.Parameters.AddWithValue("@kisi", dataGridView2.CurrentRow.Cells[5].Value);
                Baglanti_is.Parameters.AddWithValue("@aciklama", dataGridView2.CurrentRow.Cells[6].Value);
                Baglanti_is.ExecuteNonQuery();
                baglanti.Close();

                int DestRow = hit.RowIndex;
                int DestCol = hit.ColumnIndex;

                for (int u = 0; u < 5; u++)
                {
                    dataGridView3.Rows[DestRow].Cells[u].Value = dataGridView2.Rows[SourceRow].Cells[u].Value;
                }

            }

        }

        private void dataGridView3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        //drag and  drop son
        
        private void button1_Click(object sender, EventArgs e)
        {
            // islemlerden sonr tablolari yeniliyoruz

            set1.Clear();
            set2.Clear();
            set3.Clear();
            Task_Goster();
            
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
