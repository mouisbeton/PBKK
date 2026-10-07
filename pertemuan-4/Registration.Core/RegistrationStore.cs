using System.Collections.ObjectModel;

namespace Registration.Core;

public sealed class RegistrationStore
{
    public static readonly string[] Programs = ["Teknik Informatika", "Sistem Informasi", "Manajemen", "Akuntansi"];
    public ObservableCollection<Student> Students { get; } = [];

    public void Add(string nim, string nama, string? prodi, string? gender)
    {
        nim = nim.Trim();
        nama = nama.Trim();
        if (nim.Length == 0) throw new ArgumentException("NIM harus diisi.");
        if (nim.Length < 8 || nim.Length > 12 || !nim.All(char.IsAsciiDigit))
            throw new ArgumentException("NIM harus terdiri dari 8 sampai 12 digit angka.");
        if (nama.Length == 0) throw new ArgumentException("Nama harus diisi.");
        if (prodi is null || !Programs.Contains(prodi)) throw new ArgumentException("Pilih program studi.");
        if (gender != "Laki-laki" && gender != "Perempuan") throw new ArgumentException("Pilih jenis kelamin.");
        if (Students.Any(s => s.Nim == nim)) throw new ArgumentException("NIM sudah terdaftar.");
        Students.Add(new Student(nim, nama, prodi, gender));
    }

    public void Remove(Student? student)
    {
        if (student is null || !Students.Contains(student))
            throw new ArgumentException("Pilih mahasiswa yang ingin dihapus.");
        Students.Remove(student);
    }
}
