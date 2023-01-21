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
namespace LinqDeneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         LinqDbEntities db = new LinqDbEntities();

        private void Form1_Load(object sender, EventArgs e)
        { 
            //burası ado.net gibi uzun uzun 
            SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=LinqDb;Integrated Security=True");
            SqlCommand komut = new SqlCommand("select *from tbl_Dersler",baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void btnListele_Click(object sender, EventArgs e)
        { //model aracılığıyla nesne üretip data gride aktardık. EF ü yukarıya göre daha az kodla yazdık.
            dataGridView1.DataSource = db.tbl_Ogrenciler.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void btnNotL_Click(object sender, EventArgs e)
        {
            var query = from item in db.tbl_Notlar
                        select new
                        {
                            item.notid,
                            item.tbl_Ogrenciler.ad,
                            item.tbl_Ogrenciler.soyad,
                            item.tbl_Dersler.dersad,
                            item.sinav1,item.sinav2,item.sinav3,
                            item.ortalama,
                            item.durum
                        };
            dataGridView1.DataSource = query.ToList();

           // dataGridView1.DataSource = db.tbl_Notlar.ToList();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            tbl_Ogrenciler t = new tbl_Ogrenciler();
            t.ad = txtAd.Text;
            t.soyad = txtSoyad.Text;
            db.tbl_Ogrenciler.Add(t);
            db.SaveChanges();
            MessageBox.Show("Öğrenci listeye eklenmiştir.");

           
        }

        private void btnDersL_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.tbl_Dersler.ToList();
            dataGridView1.Columns[2].Visible = false;
        }

        private void btnDersKaydet_Click(object sender, EventArgs e)
        {
            tbl_Dersler d = new tbl_Dersler();
            d.dersad = txtDers.Text;
            db.tbl_Dersler.Add(d);
            db.SaveChanges();
            MessageBox.Show("Yeni Ders kaydı eklenmiştir.");

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            var x = db.tbl_Ogrenciler.Find(id);
            db.tbl_Ogrenciler.Remove(x);
            db.SaveChanges();
            MessageBox.Show("öğrenci silindi..");
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            var x = db.tbl_Ogrenciler.Find(id);
            x.ad = txtAd.Text;
            x.soyad = txtSoyad.Text;
            x.fotograf = txtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci bilgileri başarıyla güncellendi..");
        }

        private void btnProc_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NotListesi();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.tbl_Ogrenciler.Where(x => x.ad == txtAd.Text | x.soyad==txtSoyad.Text).ToList();
        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtAd.Text;
            var degerler = from item in db.tbl_Ogrenciler
                           where item.ad.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();
        }

        private void btnLinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked==true)
            {
                List<tbl_Ogrenciler> liste1 = db.tbl_Ogrenciler.OrderBy(p => p.ad).ToList();
                dataGridView1.DataSource = liste1;
            }
            if (radioButton2.Checked==true)
            {
                List<tbl_Ogrenciler> liste2 = db.tbl_Ogrenciler.OrderByDescending(p => p.ad).ToList();
                dataGridView1.DataSource = liste2;
            }
            if (radioButton3.Checked==true)
            {
                List<tbl_Ogrenciler> liste3 = db.tbl_Ogrenciler.OrderBy(p => p.ad).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }
            if (radioButton4.Checked==true)
            {
                List<tbl_Ogrenciler> liste4 = db.tbl_Ogrenciler.Where(p => p.ad.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste4;
            }
            if (radioButton5.Checked==true)
            {
                List<tbl_Ogrenciler> liste5 = db.tbl_Ogrenciler.Where(p => p.ad.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton6.Checked==true)
            {
                bool liste6 = db.tbl_Ogrenciler.Any();
                MessageBox.Show(liste6.ToString());
            }
            if (radioButton7.Checked==true)
            {
                int toplam = db.tbl_Ogrenciler.Count();
                MessageBox.Show(toplam.ToString(),"Toplam Öğrenci Sayısı:",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            if (radioButton8.Checked==true)
            {
                var toplam = db.tbl_Notlar.Sum(p =>p.sinav1);
                MessageBox.Show("Toplam Sınav1 Puanı:" +toplam.ToString());
            }
            if (radioButton10.Checked==true)
            {
                var ortalama = db.tbl_Notlar.Average(p => p.sinav1);
                MessageBox.Show("Sınav1 Ortalama Puanı:" + ortalama.ToString());
            }

            if (radioButton9.Checked == true)
            {
                var ortalamapuan = db.tbl_Notlar.Average(p => p.sinav1);
                List<tbl_Notlar> yukseknotlar = db.tbl_Notlar.Where(p => p.sinav1 > ortalamapuan).ToList();
                dataGridView1.DataSource = yukseknotlar;
            }
            if (radioButton11.Checked==true)
            {
                var enyuksek = db.tbl_Notlar.Max(p => p.sinav1);
                var kimalmis = from item in db.NotListesi()
                               where item.sinav1 == enyuksek
                               select new
                               {
                                   item.AD_SOYAD,
                                   item.dersad,
                                   item.sinav1
                               };
                dataGridView1.DataSource = kimalmis.ToList();
                MessageBox.Show("1. Sınavın en yüksek notu:" + enyuksek);

            }
            if (radioButton12.Checked==true)
            {
                var endusuk = db.tbl_Notlar.Min(p => p.sinav1);
                MessageBox.Show("1. Sınavın en düşük puanı:"+endusuk);
            }

        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            var sorgu = from d1 in db.tbl_Notlar
                        join d2 in db.tbl_Ogrenciler
                        on d1.ogr equals d2.id
                        select new
                        {
                           ogrenci=d2.ad + " " +d2.soyad,
                           //soyad=d2.soyad,
                           sinav1=d1.sinav1,
                           sinav2=d1.sinav2,
                           sinav3=d1.sinav3
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}
