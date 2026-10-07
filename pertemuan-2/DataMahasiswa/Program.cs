using System.Globalization;
using DataMahasiswa;

var mahasiswa = new List<Mahasiswa>();
Console.WriteLine("SISTEM DATA MAHASISWA | PBKK Pertemuan 2");
Console.WriteLine("Data disimpan dalam memori selama program berjalan.");
while (true)
{
    Console.WriteLine("\n1. Tambah mahasiswa    2. Tampilkan mahasiswa");
    Console.WriteLine("3. Cari mahasiswa      4. Hapus mahasiswa    0. Keluar");
    Console.Write("Pilih menu: ");
    string? pilihan = Console.ReadLine();
    if (pilihan is null || pilihan == "0") break;
    try
    {
        switch (pilihan)
        {
            case "1": Tambah(); break;
            case "2": Tampilkan(mahasiswa); break;
            case "3":
                string kata = BacaTeks("NIM/NRP atau nama yang dicari");
                Tampilkan(mahasiswa.Where(m => m.Nim.Contains(kata, StringComparison.OrdinalIgnoreCase)
                    || m.Nama.Contains(kata, StringComparison.OrdinalIgnoreCase)));
                break;
            case "4": Hapus(); break;
            default: Console.WriteLine("Menu tidak tersedia."); break;
        }
    }
    catch (EndOfStreamException) { break; }
}
Console.WriteLine("Program selesai. Terima kasih!");

void Tambah()
{
    string nim = BacaTeks("NIM/NRP");
    if (!nim.All(char.IsAsciiDigit))
    { Console.WriteLine("NIM/NRP harus berupa angka."); return; }
    if (mahasiswa.Any(m => m.Nim == nim))
    { Console.WriteLine("NIM/NRP sudah terdaftar."); return; }
    string nama = BacaTeks("Nama");
    string prodi = BacaTeks("Prodi");
    decimal ipk;
    while (true)
    {
        Console.Write("IPK (0-4): ");
        if (decimal.TryParse(BacaBaris().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out ipk) && ipk >= 0 && ipk <= 4) break;
        Console.WriteLine("IPK tidak valid. Masukkan angka antara 0 dan 4.");
    }
    mahasiswa.Add(new Mahasiswa(nim, nama, prodi, ipk));
    Console.WriteLine($"Data {nama} berhasil ditambahkan.");
}

void Hapus()
{
    string nim = BacaTeks("NIM/NRP yang dihapus");
    var ditemukan = mahasiswa.FirstOrDefault(m => m.Nim == nim);
    if (ditemukan is null) { Console.WriteLine("Mahasiswa tidak ditemukan."); return; }
    Console.Write($"Hapus {ditemukan.Nama}? (y/t): ");
    if (!BacaBaris().Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
    { Console.WriteLine("Penghapusan dibatalkan."); return; }
    mahasiswa.Remove(ditemukan);
    Console.WriteLine("Data mahasiswa berhasil dihapus.");
}

static void Tampilkan(IEnumerable<Mahasiswa> data)
{
    var daftar = data.ToList();
    if (daftar.Count == 0) { Console.WriteLine("Tidak ada data mahasiswa."); return; }
    Console.WriteLine("\nNIM/NRP       Nama                      Prodi              IPK");
    Console.WriteLine(new string('-', 68));
    foreach (var m in daftar)
        Console.WriteLine($"{m.Nim,-13} {m.Nama,-25} {m.Prodi,-18} {m.Ipk.ToString("F2", CultureInfo.InvariantCulture)}");
    Console.WriteLine($"Jumlah mahasiswa: {daftar.Count}");
}

static string BacaBaris() => Console.ReadLine() ?? throw new EndOfStreamException();
static string BacaTeks(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        string nilai = BacaBaris().Trim();
        if (nilai.Length > 0) return nilai;
        Console.WriteLine("Isian tidak boleh kosong.");
    }
}
