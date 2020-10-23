namespace CafeBoost.Data
{
    public class SiparisDetay
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }
        public int Adet { get; set; }
        public string TutarTL => string.Format("{0:00}₺", Tutar());
        //public string TutarTL { get { return $"{Tutar():0.00}TL"; } }
        public decimal Tutar() => Adet * BirimFiyat;


    }
}
