using KitapMagzasiClassLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KitapMagazasi
{
    public partial class Form1 : Form
    {//Bu projede oluşturulan fake databasedeki veriler üzerinde LINQ sorgulamaları yapılıyor.
        public Form1()
        {
            InitializeComponent();
            FakeDb.FakeDataOlustur();
            CbKitapTur.DataSource = Enum.GetNames(typeof(KitapTuru));
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
             //Kullanici adi ve şifre FakeDB'de var ise mbox'a Hoşgeldin Adi + " " + Soyadi
            //Kullanici adi ve şifre FakeDB'de yok ise mbox'a Ka ve veya Şifre Hatalı
            var kullanici = FakeDb.Kullaniciler.Where(x => x.KullaniciAdi == TbKullaniciAdi.Text && x.Sifre == TbSifre.Text).Select(x => x.Adi).ToList(); //seçilen kullanıcı adına ait select içindekiki belirtilen alanları getirir.


            if (kullanici.Any()) // kullanicinin içi dolu mu diye kontrol ediyoruz.
            {
                foreach (var item in kullanici)
                {
                    MessageBox.Show("Hoşgeldin " + item.ToString());
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.\nLütfen kontrol edip tekrar deneyiniz.");
            }
                       
        }

        private void BtnSifremiUnuttum_Click(object sender, EventArgs e)
        {
            //Kullanici adi FakeDB'de var ise mbox'a Şifreyi Yaz
            //Kullanici adi FakeDb'de yok ise mbox'a Böyle Bir Kullanici Yok Yaz
            var kullanici = FakeDb.Kullaniciler.Where(x => x.KullaniciAdi == TbKullaniciAdi.Text); //seçilen kullanıcı adına ait tüm bilgileri getirir.

            if (kullanici.Any())
            {
                foreach (var item in kullanici)
                {
                    MessageBox.Show(item.Sifre);
                }
                               
            }
            else
            {
                MessageBox.Show("Böyle bir kullanıcı yok.");
            }
        }

        private void BtnYazarKitapGetir_Click(object sender, EventArgs e)
        {
            // TbYazarAdi'na girilen Yazarın Tüm kitaplarını getir ve LbSonuc'a Kitabın adını ve Fiyatını Yaz
           var kitaplar = FakeDb.Kitaplar.Where(x => x.Yazar.AdiSoyadi == TbYazarAdi.Text);
            foreach (var item in kitaplar)
            {
                LbSonuc.Items.Add(item.Adi + " " +item.KitapTuru + " " + item.SayfaSayisi.ToString() + " " + item.YayinEvi + " " + item.BaskiYili);
            }
        }

        private void BtnTumKitapSatisAded_Click(object sender, EventArgs e)
        {
            //Tüm Kitap Türlerinin ayrı ayrı kaç adet sattığını ve KitapTürünü LbSonuc'ta listeleyin
            var satis = FakeDb.Satislar.GroupBy(x => x.AlınanKitap.KitapTuru);
            foreach (var item in satis)
            {
                LbSonuc.Items.Add(item.Count() + " " + item.Key); // key denilen hangi alan için gruplandığını gösteriyor.

            }
        }
        private void BtnYazarKitapSayfa_Click(object sender, EventArgs e)
        {
            // TbYazarAdi'na girilen Yazarın Tüm kitaplarının ortalama sayfa adedini mbox'a Yazdır
            var kitaplar = FakeDb.Kitaplar.Where(x => x.Yazar.AdiSoyadi == TbYazarAdi.Text).Average(x => x.SayfaSayisi);
            MessageBox.Show(kitaplar.ToString());
            //UZUN YOL
            //var kitaplar = FakeDb.Kitaplar.Where(x => x.Yazar.AdiSoyadi == TbYazarAdi.Text).Select(x => x.SayfaSayisi).ToList();
            //int ort=0,sayac=0;
            //foreach (var item in kitaplar)
            //{
            //    ort += item;
            //    sayac++;
            //}
            //ort = ort / sayac;
            //MessageBox.Show(ort.ToString());
        }

        private void BtnYazarKitapTur_Click(object sender, EventArgs e)
        {
            // TbYazarAdi'na girilen Yazarın hangi türde kaç adet kitabı olduğunu LbSonuc'ta listeleyin

            var tur = FakeDb.Kitaplar.Where(x => x.Yazar.AdiSoyadi == TbYazarAdi.Text).GroupBy(x => x.KitapTuru);
            foreach (var item in tur)
            {
                LbSonuc.Items.Add(item.Count() + " " + item.Key);
            }
        }

        private void BtnKitapTurOrtalamaFiyat_Click(object sender, EventArgs e)
        {
            KitapTuru seciliKitapTuru = (KitapTuru)CbKitapTur.SelectedIndex;
            //Secili Kitap Turuna Ait Kitaplarin ortalama fiyatını bulun
            var kitapTurFiyatOrt = FakeDb.Kitaplar.Where(x=>x.KitapTuru == seciliKitapTuru).Average(x=>x.Fiyat);
            MessageBox.Show(kitapTurFiyatOrt.ToString());
        }

        private void BtnEnPahalıKitap_Click(object sender, EventArgs e)
        {
            // FakeDb'deki En Pahalı Kitabın satis fiyatını ve adını mbox'a yazdırın
            var kitap = FakeDb.Kitaplar.OrderByDescending(x => x.Fiyat).Take(1);

            foreach (var item in kitap)
            {
                MessageBox.Show(item.Adi + " - " +item.SayfaSayisi);
            }
        }
            

        private void BtnFiyatOrtalamaFazla_Click(object sender, EventArgs e)
        {
            // FakeDb'deki Fiyatı Ortalama Kitap Fiyatından Fazla olan Kitapları LbSonuc'ta listeleyin
            var ort = FakeDb.Kitaplar.Average(x=>x.Fiyat);
            MessageBox.Show(ort.ToString());
            var kitaplar = FakeDb.Kitaplar.Where( x => x.Fiyat>ort ).ToList();
            foreach (var item in kitaplar)
            {
                LbSonuc.Items.Add(item.Adi +" - "+item.Fiyat);
            }
        }

        private void TbFiyatAralık_Click(object sender, EventArgs e)
        {
            // FakeDb'deki Fiyatı TbMin ile TbMax arasında olan Kitapları Fiyata göre azalan şekilde
            // LbSonuc'ta listeleyin

            LbSonuc.Items.Clear();
            var kitaplar = FakeDb.Kitaplar.Where(x=>x.Fiyat > Convert.ToDouble(TbMin.Text) && x.Fiyat < Convert.ToDouble(TbMax.Text)).ToList();
            foreach (var item in kitaplar)
            {
                LbSonuc.Items.Add(item.Adi +" - "+item.Fiyat);
            }
        }

        private void BtnKısadanUzuna_Click(object sender, EventArgs e)
        {
            //Kitapları sayfa sayısına göre kısadan uzuna sıralayın,
            //kitapların isimlerini ve satış fiyatlarını LbSonuc'ta listeleyin

            var kitapSayfa = FakeDb.Kitaplar.OrderBy(x=>x.SayfaSayisi).ToList();

            foreach (var item in kitapSayfa)
            {
                LbSonuc.Items.Add(item.Adi+"-  -  -"+item.SayfaSayisi+"-  -  -"+item.Fiyat);
            }
        }

        private void BtnYılSatis_Click(object sender, EventArgs e)
        {
            // Her Yıl kaç adet kitap satıldığını ve bu kitaplardan elde edilen geliri LbSonuc'ta listeleyin
            var kitapAdet = FakeDb.Satislar.GroupBy(x => new {
                x.SatisTarihi.Year
            }).Select(x => new {
                tarih = x.Key.Year,
                toplamKitap = x.Count(),
                toplamFiyat = x.Sum(y => y.AlınanKitap.Fiyat)
            });

            foreach (var item in kitapAdet)
            {
                LbSonuc.Items.Add(item.tarih + " - - " +item.toplamKitap + " - - " +item.toplamFiyat+"TL");
            }
        }

        private void BtnKinKacKitap_Click(object sender, EventArgs e)
        {
            //Tüm kullanıcıların kaç kitap aldığını LbSonuc'ta listeleyin (KullanıcıAdi, KitapSayisi)
            var kacKitapAlinmis = FakeDb.Satislar.GroupBy(x => new
            {
                x.SatinAlanKullanici

            }).Select(x=> new
            {
                kullaniciAdi= x.Key.SatinAlanKullanici.KullaniciAdi,
                kitapSayisi = x.Count()
            }).ToList();

            foreach (var item in kacKitapAlinmis)
            {
                LbSonuc.Items.Add(item.kullaniciAdi+" - - "+item.kitapSayisi);
            }

        }

        private void BtnAralikTurKitap_Click(object sender, EventArgs e)
        {
            KitapTuru seciliKitapTuru = (KitapTuru)CbKitapTur.SelectedIndex;
            //Girilen seciliKitapTurun'deki (en ucuz) kitabın adi ve Yazarının adini mbox ile ekrana yazın
            var kitap = FakeDb.Kitaplar.Where(x=>x.KitapTuru == seciliKitapTuru).OrderBy(x => x.Fiyat).Take(1);

            foreach (var item in kitap)
            {
                MessageBox.Show(item.Adi + " - " + item.Yazar.AdiSoyadi);
            }
        }

        private void BtnKitapHediye_Click(object sender, EventArgs e)
        {
            //Tüm kullanıcıların  kaç tane hediye kitap aldığını LbSonuc'ta listeleyin (KullanıcıAdi, HediyeKitapSayisi)
            lvSonuc.Clear();
            lvSonuc.Columns.Add("Hediye Eden Kullanici");
            lvSonuc.Columns.Add("Hediye Kitap Sayisi");



            lvSonuc.View = View.Details;
            lvSonuc.GridLines = true;
            ListViewItem items = new ListViewItem();

            var a = FakeDb.Satislar.Where(x => x.HediyeMi == true).GroupBy(x => new { x.SatinAlanKullanici.KullaniciAdi }).
                Select(x => new { K = x.Key.KullaniciAdi, toplamkitap = x.Count() });


            foreach (var item in a)
            {
                items = lvSonuc.Items.Add(item.K);
                items.SubItems.Add(item.toplamkitap.ToString());

            }

            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);



        }

        private void BtnKitapHediyeMi_Click(object sender, EventArgs e)
        {
            //TbKitapAdi'na adı girilen kitabın hediye edilip edilmediğini mbox  ile ekrana yazın
            var a = FakeDb.Satislar.Where(x => x.AlınanKitap.Adi == TbKitapAdi.Text).Select(x => x.HediyeMi);

            foreach (var item in a)
            {
                if (item == true)
                {
                    MessageBox.Show("Hediye");
                    break;
                }
                else
                {
                    MessageBox.Show("NO");
                    break;

                }
            }
        }

        private void BtnKitapHediyeEdilmeSayisi_Click(object sender, EventArgs e)
        {
            // TbKitapAdi'na adı girilen kitabın kaç adet hediye edildiğini mbox ile ekrana yazın

            lvSonuc.Clear();
            lvSonuc.Columns.Add("Kitap Adı");
            lvSonuc.Columns.Add("Hediye Adet");



            lvSonuc.View = View.Details;
            lvSonuc.GridLines = true;
            ListViewItem items = new ListViewItem();


            var a = FakeDb.Satislar.Where(x => x.AlınanKitap.Adi == TbKitapAdi.Text && x.HediyeMi == true).
                GroupBy(x => new { x.AlınanKitap.Adi }).Select(x => new { Kitap = x.Key.Adi, HediyeAdet = x.Count() });


            foreach (var item in a)
            {


                items = lvSonuc.Items.Add(item.Kitap);
                items.SubItems.Add(item.HediyeAdet.ToString());


            }

            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }

        private void BtnYayinEviYazar_Click(object sender, EventArgs e)
        {
            //Hangi yayınevi hangi yazar'ın kaç adet kitabını başmıs LbSonuc'ta listeleyin (YayınEviAdı, YazarAdı, KitapAdedi)
            var yazarKitap = FakeDb.Kitaplar.GroupBy(x => new
            {
                x.YayinEvi.YayinEviAdi,
                x.Yazar.AdiSoyadi,
            }).Select(x => new {
                yayinEviAdi = x.Key.YayinEviAdi,
                yazarAdi=x.Key.AdiSoyadi,
                count = x.Count()
            });

            foreach (var item in yazarKitap)
            {
                LbSonuc.Items.Add(item.yayinEviAdi + " - - " +item.yazarAdi + " - - " + item.count);             

            }

        }

        private void BtnYazarKitapİlk_Click(object sender, EventArgs e)
        {
            //TbYazarAdi'na girilen Yazarın ilk basılan kitabının adını ve Basım tarihini mbox ile ekrana yazın
            var ilkKitap = FakeDb.Kitaplar.Where(x => x.Yazar.AdiSoyadi == TbYazarAdi.Text).Min(x=>x.BaskiYili);
            MessageBox.Show(ilkKitap.ToString());
        }

        private void BtnYayinEviFiyat_Click(object sender, EventArgs e)
        {
            //Tüm Yayınevlerinin Toplam Sattığı kitap adedini ve Kaç adet kitap sattığını LbSonuc'ta listeleyin
            lvSonuc.Clear();
            lvSonuc.Columns.Add("Yayın Evi");
            lvSonuc.Columns.Add("Satilan Kitap Sayisi");
            lvSonuc.Columns.Add("Total Fiyat");

            lvSonuc.View = View.Details;
            lvSonuc.GridLines = true;
            ListViewItem items = new ListViewItem();

            var a = FakeDb.Satislar.GroupBy(x => new { x.AlınanKitap.YayinEvi.YayinEviAdi }).
                Select(y => new { YayinEvi = y.Key.YayinEviAdi, Toplam = y.Key.YayinEviAdi.Count(), ToplamFiyat = y.Sum(x => x.AlınanKitap.Fiyat) });



            foreach (var item in a)
            {


                items = lvSonuc.Items.Add(item.YayinEvi);
                items.SubItems.Add(item.Toplam.ToString());
                items.SubItems.Add(item.ToplamFiyat.ToString() + "TL");




            }

            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvSonuc.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }
    }
}
