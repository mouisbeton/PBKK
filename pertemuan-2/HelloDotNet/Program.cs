using System.Globalization;

Console.WriteLine("Hello .NET!");
Console.WriteLine("Selamat belajar C#");
Console.WriteLine("Pemrograman Framework\n");
string nama = BacaTeks("Nama");
string nim = BacaTeks("NIM/NRP");
string prodi = BacaTeks("Prodi");
int semester;
do { Console.Write("Semester (1-14): "); }
while (!int.TryParse(BacaBaris(), out semester) || semester < 1 || semester > 14);
decimal ipk;
do { Console.Write("IPK (0-4): "); }
while (!decimal.TryParse(BacaBaris().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
    CultureInfo.InvariantCulture, out ipk) || ipk < 0 || ipk > 4);

Console.WriteLine($"\nHalo, {nama}! Selamat belajar .NET dan C#.");
Console.WriteLine("=== BIODATA MAHASISWA ===");
Console.WriteLine($"Nama     : {nama}\nNIM/NRP  : {nim}\nProdi    : {prodi}");
Console.WriteLine($"Semester : {semester}\nIPK      : {ipk.ToString("F2", CultureInfo.InvariantCulture)}");
Console.WriteLine("Belajar framework, membangun karya!");

static string BacaBaris() => Console.ReadLine() ?? throw new EndOfStreamException("Input berakhir.");
static string BacaTeks(string label)
{
    string nilai;
    do { Console.Write($"{label}: "); nilai = BacaBaris().Trim(); }
    while (nilai.Length == 0);
    return nilai;
}
