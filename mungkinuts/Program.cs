using System;
using System.Collections.Generic;

namespace UTS_PBO_TipeB
{
    class Agen
    {
        private string idAgen;
        private string namaAgen;
        private int stokAgen;

        public string IdAgen
        {
            get { return idAgen; }
            set { idAgen = value; }
        }

        public string NamaAgen
        {
            get { return namaAgen; }
            set { namaAgen = value; }
        }

        public int StokAgen
        {
            get { return stokAgen; }
            private set
            {
                if (value >= 0) stokAgen = value;
                else Console.WriteLine("Stok agen tidak boleh negatif!");
            }
        }

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            IdAgen = idAgen;
            NamaAgen = namaAgen;
            StokAgen = stokAgen;
        }

        public void TambahStok(int jumlah)
        {
            if (jumlah > 0)
            {
                StokAgen = stokAgen + jumlah;
            }
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID Agen     : {IdAgen}");
            Console.WriteLine($"Nama Agen   : {NamaAgen}");
            Console.WriteLine($"Stok Agen   : {StokAgen} galon");
            Console.WriteLine("------------------------");
        }
    }


    abstract class ProdukGalon
    {

        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }


        public int StokGudang
        {
            get { return stokGudang; }
            private set
            {
                if (value >= 0) stokGudang = value;
                else Console.WriteLine("Stok gudang tidak boleh negatif!");
            }
        }

        public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            IdProduk = idProduk;
            NamaProduk = namaProduk;
            StokGudang = stokGudang;
        }


        public void KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah pengurangan harus lebih dari 0!");
            }
            else if (jumlah > stokGudang)
            {
                Console.WriteLine("Stok gudang tidak mencukupi!");
            }
            else
            {
                StokGudang = stokGudang - jumlah;   
            }
        }


        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfo()
        {
            Console.WriteLine($"ID Produk   : {IdProduk}");
            Console.WriteLine($"Nama Produk : {NamaProduk}");
            Console.WriteLine($"Stok Gudang : {StokGudang} galon");
        }
    }


    class GalonAkua : ProdukGalon
    {
        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang)
        {
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }

        public override void TampilInfo()
        {
            Console.WriteLine("[Galon Akua]");
            base.TampilInfo();
            Console.WriteLine("Harga       : Rp20.000/galon");
            Console.WriteLine("------------------------");
        }
    }

    class GalonBerkeran : ProdukGalon
    {

        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            JenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 12000;
        }

        public override void TampilInfo()
        {
            Console.WriteLine("[Galon Berkeran]");
            base.TampilInfo();
            Console.WriteLine($"Jenis Keran : {JenisKeran}");
            Console.WriteLine("Harga       : Rp12.000/galon");
            Console.WriteLine("------------------------");
        }
    }


    interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    class DistribusiGalon : DapatDidistribusikan
    {
        public Agen AgenTujuan { get; set; }
        public ProdukGalon Produk { get; set; }
        public int Jumlah { get; set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
        {
            AgenTujuan = agenTujuan;
            Produk = produk;
            Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"Mengirim {Jumlah} galon {Produk.NamaProduk} ke {AgenTujuan.NamaAgen}...");

            if (Jumlah <= 0)
            {
                Console.WriteLine("Pengiriman GAGAL: jumlah galon harus lebih dari 0.");
                Console.WriteLine("Stok gudang dan stok agen tidak berubah.");
            }

            else if (Jumlah > Produk.StokGudang)
            {
                Console.WriteLine($"Pengiriman GAGAL: jumlah ({Jumlah}) melebihi stok gudang ({Produk.StokGudang}).");
                Console.WriteLine("Stok gudang dan stok agen tidak berubah.");
            }

            else
            {
                Produk.KurangiStok(Jumlah);        
                AgenTujuan.TambahStok(Jumlah);    
                Console.WriteLine($"Paket berhasil dikirimkan ke agen {AgenTujuan.NamaAgen}! Operasi aman, Bos."); 
                Console.WriteLine($"Biaya pengiriman : Rp{Produk.HitungBiaya(Jumlah):N0}");
                Console.WriteLine($"Sisa stok gudang : {Produk.StokGudang} galon");
                Console.WriteLine($"Stok agen kini   : {AgenTujuan.StokAgen} galon");
            }

            Console.WriteLine("------------------------");
        }
    }


    class SindikatGalon
    {
        public string NamaSindikat { get; set; }
        public List<Agen> DaftarAgen { get; set; }
        public List<ProdukGalon> DaftarProduk { get; set; }

        public SindikatGalon(string namaSindikat)
        {
            NamaSindikat = namaSindikat;
            DaftarAgen = new List<Agen>();
            DaftarProduk = new List<ProdukGalon>();
        }

        public void TambahAgen(Agen agen)
        {
            DaftarAgen.Add(agen);
            Console.WriteLine($"Agen {agen.NamaAgen} berhasil direkrut.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            DaftarProduk.Add(produk);
            Console.WriteLine($"Produk {produk.NamaProduk} masuk ke gudang pusat.");
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine($"\n===== ASET SINDIKAT {NamaSindikat.ToUpper()} =====");

            Console.WriteLine("\n--- Daftar Agen ---");
            foreach (Agen a in DaftarAgen)
            {
                a.TampilInfo();
            }

            Console.WriteLine("\n--- Daftar Produk ---");
            foreach (ProdukGalon p in DaftarProduk)
            {
                p.TampilInfo();  
            }
        }
    }


    class Program
    {
        static void Main()
        {

            Console.WriteLine("===== NO. 1: DATA AGEN =====");
            Agen agen1 = new Agen("AG01", "Markas Utara", 10);
            Agen agen2 = new Agen("AG02", "Markas Selatan", 5);
            agen1.TampilInfo();
            agen2.TampilInfo();


            Console.WriteLine("\n===== NO. 2, 3, 5: DATA PRODUK =====");


            ProdukGalon akua = new GalonAkua("PR01", "Galon Akua Murni", 50);
            ProdukGalon berkeran = new GalonBerkeran("PR02", "Galon Berkeran Impor", 30, "Keran Emas");
            akua.TampilInfo();
            berkeran.TampilInfo();

 
            Console.WriteLine("\n===== NO. 4: POLYMORPHISM =====");

            List<ProdukGalon> daftarProduk = new List<ProdukGalon> { akua, berkeran };
            foreach (ProdukGalon p in daftarProduk)
            {
                Console.WriteLine($"Biaya 5 galon {p.NamaProduk} : Rp{p.HitungBiaya(5):N0}");
            }


            Console.WriteLine("\n===== NO. 6: DISTRIBUSI =====");
            DistribusiGalon kirim1 = new DistribusiGalon(agen1, akua, 20);       
            DistribusiGalon kirim2 = new DistribusiGalon(agen2, berkeran, 0);    
            DistribusiGalon kirim3 = new DistribusiGalon(agen2, berkeran, 100);  

            kirim1.KirimPaket();
            kirim2.KirimPaket();
            kirim3.KirimPaket();


            Console.WriteLine("\n===== NO. 7: SINDIKAT =====");
            SindikatGalon sindikat = new SindikatGalon("Mafia Galon");
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(akua);
            sindikat.TambahProduk(berkeran);
            sindikat.TampilkanSindikat();

            Console.ReadKey();
        }
    }
}