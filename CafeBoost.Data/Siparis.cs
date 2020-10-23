using System;
using System.Collections.Generic;
using System.Linq;

namespace CafeBoost.Data
{
    public class Siparis
    {
        public int MasaNo { get; set; }
        public List<SiparisDetay> SiparisDetaylar { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; } // {get=>SiparisDurum.Aktif; set => 1 = 1;}
        public decimal OdenenTutar { get; set; }
        public string ToplamTutarTL => string.Format("{0:00}₺",ToplamTutar());
        public Siparis()
        {
            SiparisDetaylar = new List<SiparisDetay>();
            AcilisZamani = DateTime.Now;
        }
        public decimal ToplamTutar()
        {
            //decimal toplam = 0;
            //foreach (var item in SiparisDetaylar)
            //{                                             
            //    toplam += item.Adet * item.BirimFiyat;
            //}
            //return toplam;
            return SiparisDetaylar.Sum(x => x.Tutar());
            //return SiparisDetaylar.Sum(x => x.Adet * x.BirimFiyat)(x=>x.Tutar());
        }
        //public decimal ToplamTutar() => SiparisDetaylar.Sum(x => x.Tutar());
    }
}
