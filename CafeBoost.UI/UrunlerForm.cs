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
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri db;
        BindingList<Urun> blUrunler;
        public UrunlerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            blUrunler = new BindingList<Urun>(db.Urunler);
            InitializeComponent();
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();
            if (urunAd == string.Empty)
            {
                errorProvider1.SetError(txtUrunAd, "Ürün adı girmediniz.");
                //MessageBox.Show("Ürün adı giriniz.");
                return;
            }

            if (UrunVarMi(urunAd))
            {
                errorProvider1.SetError(txtUrunAd, "Ürün zaten tanımlı.");
                return;
            }
            errorProvider1.SetError(txtUrunAd, "");


            blUrunler.Add(new Urun()
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });

            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
        }

        private void txtUrunAd_Validating(object sender, CancelEventArgs e)
        {
            //e.Cancel = true; Kutudan çıkamamak için
            if (txtUrunAd.Text.Trim() != "") //if (txtUrunAd.Text.Trim() == "")
            {
                errorProvider1.SetError(txtUrunAd, "");//errorProvider1.SetError(txtUrunAd, "Ürün adı girmediniz.");
            }
            //else
            //{
            //    errorProvider1.SetError(txtUrunAd, "");
            //}
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            Urun urun = (Urun)dgvUrunler.Rows[e.RowIndex].DataBoundItem;
            string mevcutDeger = dgvUrunler.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (!dgvUrunler.IsCurrentCellDirty || e.FormattedValue.ToString() == mevcutDeger)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString() == "")
                {
                    MessageBox.Show("Ürün adı boş girilemez.");
                    e.Cancel = true;
                }
                if (BaskaUrunVarMi(e.FormattedValue.ToString(), urun))
                {
                    MessageBox.Show("Ürün zaten mevcut.");
                    e.Cancel = true;
                }
                //    if (UrunVarMi(e.FormattedValue.ToString()))
                //    {
                //        MessageBox.Show("Ürün zaten mevcut.");
                //        e.Cancel = true;
                //    }
            }
            else if (e.ColumnIndex == 1)
            {
                decimal birimFiyat;
                bool gecerliMi = decimal.TryParse(e.FormattedValue.ToString(), out birimFiyat);
                if (!gecerliMi || birimFiyat <= 0)
                {
                    MessageBox.Show("Geçersiz fiyat.");
                    e.Cancel = true;
                }

            }
        }
        private bool UrunVarMi(string urunAd)
        {
            return db.Urunler.Any(
                x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase));
        }
        //private bool UrunVarMiBuyukKucukHarfDuyarsiz(string urunAd)
        //{
        //    return db.Urunler.Any(
        //        x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase));
        //}

        private bool BaskaUrunVarMi(string UrunAd, Urun urun)
        {
            return db.Urunler.Any(
                x => x.UrunAd.Equals(UrunAd, StringComparison.CurrentCultureIgnoreCase) && x != urun);
        }
    }
}
