using CafeBoost.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBoost.UI
{
    public partial class SiparisForm : Form
    {
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly AnaForm anaForm;
        private readonly BindingList<SiparisDetay> blsiparisDetaylar;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis, AnaForm anaForm)
        {

            //Constructor parametresi olarak gelen bu nesneleri daha sonra da erişebileceğimiz fieldlara aktarıyoruz.
            db = kafeVeri;
            this.anaForm = anaForm;
            this.siparis = siparis;
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;
            MasalariListele();
            UrunleriListele();
            MasaNoGuncelle();
            lblOdemeTutariGuncelle();
            blsiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            //blsiparisDetaylar.ListChanged += BlsiparisDetaylar_ListChanged;
            dgvSiparisDetaylar.DataSource = blsiparisDetaylar;
            //dgvSiparisDetaylar.Columns[0].HeaderText = "Ürün Adı";
            //dgvSiparisDetaylar.Columns[1].HeaderText = "Birim Fiyat";
            //dgvSiparisDetaylar.Columns[2].HeaderText = "Adet";
            //dgvSiparisDetaylar.Columns[3].HeaderText = "Tutar";

            Object SelectedItem = 1;
            int sayi = (int)SelectedItem;
            int karesi = sayi * sayi;
        }

        private void MasalariListele()
        {
            cboMasalar.Items.Clear();
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x=>x.MasaNo==i))
                {
                    cboMasalar.Items.Add(i);
                }
                //cboMasalar.Items.RemoveAt(i);
            }
        }

        //silerken liste güncellemek için ikinci yol
        //private void BlsiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    lblOdemeTutariGuncelle();
        //}

        private void lblOdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = db.Urunler;
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları (Açılış: {siparis.AcilisZamani.Value.ToShortTimeString()})";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun secilenUrun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            // Eklenen siparişleri birbiri üstüne ekle
            //SiparisDetay detay = blsiparisDetaylar.FirstOrDefault(x => x.UrunAd == secilenUrun.UrunAd);

            //if (detay!=null)
            //{
            //    detay.Adet += adet;
            //    blsiparisDetaylar.ResetBindings();
            //}
            //else
            //{
            //    detay = new SiparisDetay()
            //    {
            //        UrunAd = secilenUrun.UrunAd,
            //        BirimFiyat = secilenUrun.BirimFiyat,
            //        Adet = adet
            //    };
            //    blsiparisDetaylar.Add(detay);
            //}

            SiparisDetay detay = new SiparisDetay()
            {
                UrunAd = secilenUrun.UrunAd,
                BirimFiyat = secilenUrun.BirimFiyat,
                Adet = adet
            };
            blsiparisDetaylar.Add(detay);
            lblOdemeTutariGuncelle();

            //dgvSiparisDetaylar.DataSource = null;
            //dgvSiparisDetaylar.DataSource = siparis.SiparisDetaylar;

        }

        private void dgvSiparisDetaylar_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblOdemeTutariGuncelle();
        }

        private void dgvSiparisDetaylar_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seçili detayları silmek istediğinize emin misiniz?", "Silme Olayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Sipariş iptal edilerek kapatılacaktır. Emin misiniz?", "Ödeme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Iptal);
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ödeme alındıysa sipariş kapatılacaktır. Emin misiniz?", "Ödeme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr==DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Odendi, siparis.ToplamTutar());
            }
        }

        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar = 0)
        {
            siparis.OdenenTutar = odenenTutar;
            siparis.KapanisZamani = DateTime.Now;
            siparis.Durum = siparisDurum;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasalar.SelectedIndex < 0) return;
            int kaynak = siparis.MasaNo;
            int hedef = (int)cboMasalar.SelectedItem;
            siparis.MasaNo = hedef;
            anaForm.MasaTasi(kaynak, hedef);
            MasaNoGuncelle();
            MasalariListele();
        }
    }
}
