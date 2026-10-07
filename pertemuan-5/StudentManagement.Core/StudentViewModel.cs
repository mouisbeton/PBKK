using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Data.SqlClient;

namespace StudentManagement.Core;

public sealed class StudentViewModel : INotifyPropertyChanged
{
    private readonly StudentRepository repository;
    private int editingId;
    private string nim = "", nama = "", email = "", searchText = "", status = "Siap", jurusan = "", gender = "";
    private Student? selectedStudent;
    private bool deletePending;
    private StudentStats stats = new(0,0,0,0,0);
    public ObservableCollection<Student> Students { get; } = [];
    public string[] Departments { get; } = ["Informatika", "Sistem Informasi"];
    public string[] Genders { get; } = ["Laki-laki", "Perempuan"];
    public ICommand SaveCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ConfirmDeleteCommand { get; }
    public ICommand CancelDeleteCommand { get; }

    public StudentViewModel(StudentRepository repository)
    {
        this.repository = repository;
        SaveCommand = new RelayCommand(() => Guard(Save));
        ResetCommand = new RelayCommand(Reset);
        SearchCommand = new RelayCommand(() => Guard(Load));
        DeleteCommand = new RelayCommand(RequestDelete);
        ConfirmDeleteCommand = new RelayCommand(() => Guard(Delete));
        CancelDeleteCommand = new RelayCommand(() => { DeletePending = false; Status = "Penghapusan dibatalkan."; });
        Guard(() => { repository.Initialize(); Load(); });
    }

    public string Nim { get => nim; set => Set(ref nim, value ?? ""); }
    public string Nama { get => nama; set => Set(ref nama, value ?? ""); }
    public string Email { get => email; set => Set(ref email, value ?? ""); }
    public string Jurusan { get => jurusan; set => Set(ref jurusan, value ?? ""); }
    public string Gender { get => gender; set => Set(ref gender, value ?? ""); }
    public string SearchText { get => searchText; set => Set(ref searchText, value ?? ""); }
    public string Status { get => status; private set => Set(ref status, value); }
    public bool DeletePending { get => deletePending; private set => Set(ref deletePending, value); }
    public string FormTitle => editingId == 0 ? "Tambah mahasiswa" : "Edit mahasiswa";
    public int TotalStudents => stats.Total;
    public int TotalInformatika => stats.Informatika;
    public int TotalSistemInformasi => stats.SistemInformasi;
    public int TotalLakiLaki => stats.LakiLaki;
    public int TotalPerempuan => stats.Perempuan;

    public Student? SelectedStudent
    {
        get => selectedStudent;
        set
        {
            if (!Set(ref selectedStudent, value)) return;
            DeletePending = false;
            if (value is null) return;
            editingId = value.Id;
            Nim = value.Nim; Nama = value.Nama; Jurusan = value.Jurusan; Gender = value.Gender; Email = value.Email;
            OnChanged(nameof(FormTitle));
        }
    }

    private void Save()
    {
        string cleanNim = Nim.Trim(), cleanName = Nama.Trim(), cleanEmail = Email.Trim();
        if (cleanNim.Length < 8 || cleanNim.Length > 12 || !cleanNim.All(char.IsAsciiDigit))
            throw new ArgumentException("NIM harus terdiri dari 8 sampai 12 digit angka.");
        if (cleanName.Length == 0 || cleanName.Length > 100) throw new ArgumentException("Nama wajib diisi, maksimal 100 karakter.");
        if (!Departments.Contains(Jurusan)) throw new ArgumentException("Pilih jurusan.");
        if (!Genders.Contains(Gender)) throw new ArgumentException("Pilih jenis kelamin.");
        if (cleanEmail.Length > 150 || !MailAddress.TryCreate(cleanEmail, out var address) || address.Address != cleanEmail)
            throw new ArgumentException("Email tidak valid.");
        bool updated = editingId != 0;
        repository.Save(new Student(editingId,cleanNim,cleanName,Jurusan,Gender,cleanEmail));
        Reset(); Load();
        Status = updated ? "Data mahasiswa berhasil diperbarui." : "Data mahasiswa berhasil disimpan.";
    }

    public void Reset()
    {
        editingId = 0; SelectedStudent = null; Nim = ""; Nama = ""; Jurusan = ""; Gender = ""; Email = "";
        DeletePending = false; Status = "Form dikosongkan."; OnChanged(nameof(FormTitle));
    }

    private void Load()
    {
        var rows = repository.Search(SearchText.Trim());
        stats = repository.GetStats();
        Reset();
        Students.Clear();
        foreach (var row in rows) Students.Add(row);
        foreach (var property in new[] { nameof(TotalStudents),nameof(TotalInformatika),nameof(TotalSistemInformasi),nameof(TotalLakiLaki),nameof(TotalPerempuan) }) OnChanged(property);
        Status = $"Menampilkan {Students.Count} dari {stats.Total} mahasiswa.";
    }

    private void RequestDelete()
    {
        if (SelectedStudent is null) { Status = "Pilih mahasiswa yang ingin dihapus."; return; }
        DeletePending = true; Status = $"Hapus data {SelectedStudent.Nama}?";
    }

    private void Delete()
    {
        if (!DeletePending || SelectedStudent is null) return;
        repository.Delete(SelectedStudent.Id); Reset(); Load(); Status = "Data mahasiswa berhasil dihapus.";
    }

    private void Guard(Action action)
    {
        try { action(); }
        catch (ArgumentException ex) { Status = ex.Message; }
        catch (InvalidOperationException ex) { Status = ex.Message; }
        catch (SqlException ex) when (ex.Number is 2601 or 2627) { Status = "NIM sudah terdaftar."; }
        catch (SqlException) { Status = "Database tidak dapat diakses. Periksa SQL Server dan PBKK_SQL_CONNECTION."; }
    }

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field,value)) return false;
        field = value; OnChanged(name); return true;
    }
    private void OnChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
    public event PropertyChangedEventHandler? PropertyChanged;
}
