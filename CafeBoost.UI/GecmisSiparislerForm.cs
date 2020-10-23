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
    public partial class GecmisSiparislerForm : Form
    {
        private readonly KafeVeri kafeVeri;
        //private readonly KafeVeri kafeVeri db; ikinci yol

        public GecmisSiparislerForm(KafeVeri kafeVeri)
        {
            InitializeComponent();
            dgvSiparisler.DataSource = kafeVeri.GecmisSiparisler;
            //db = kafeVeri;
            this.kafeVeri = kafeVeri;
        }

        private void dgvSiparisDetayları_SelectionChanged(object sender, EventArgs e)
        {
            // en az bir seçili satır varsa
            if (dgvSiparisler.SelectedRows.Count > 0)
            {
                //seçili satırların ilkinin üzerinde sipariş nesnesi
                Siparis seciliSiparis = (Siparis)dgvSiparisler.SelectedRows[0].DataBoundItem;
                dgvSiparisDetayları.DataSource = seciliSiparis.SiparisDetaylar;
            }
        }
    }
}
