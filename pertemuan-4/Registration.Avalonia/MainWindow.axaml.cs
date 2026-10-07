using Avalonia.Controls;
using Avalonia.Interactivity;
using Registration.Core;
namespace Registration.Avalonia;
public partial class MainWindow : Window
{
    private readonly RegistrationStore store = new();
    private Student? pendingDelete;
    public MainWindow()
    {
        InitializeComponent();
        cmbProdi.ItemsSource = RegistrationStore.Programs;
        lstMahasiswa.ItemsSource = store.Students;
        confirmPanel.IsVisible = false;
        store.Students.CollectionChanged += (_, _) => txtJumlah.Text = $"Jumlah mahasiswa: {store.Students.Count}";
    }
    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            string? gender = rbLaki.IsChecked == true ? "Laki-laki" : rbPerempuan.IsChecked == true ? "Perempuan" : null;
            store.Add(txtNim.Text ?? "", txtNama.Text ?? "", cmbProdi.SelectedItem as string, gender);
            ClearForm(); txtStatus.Text = "Data mahasiswa berhasil disimpan.";
        }
        catch (ArgumentException ex) { txtStatus.Text = ex.Message; }
    }
    private void ClearForm()
    {
        txtNim.Text = ""; txtNama.Text = ""; cmbProdi.SelectedIndex = -1;
        rbLaki.IsChecked = false; rbPerempuan.IsChecked = false;
        pendingDelete = null; confirmPanel.IsVisible = false; txtNim.Focus();
    }
    private void Reset_Click(object? sender, RoutedEventArgs e) { ClearForm(); txtStatus.Text = "Form dikosongkan."; }
    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        pendingDelete = lstMahasiswa.SelectedItem as Student;
        if (pendingDelete is null) { txtStatus.Text = "Pilih mahasiswa yang ingin dihapus."; return; }
        confirmPanel.IsVisible = true; txtStatus.Text = $"Konfirmasi penghapusan {pendingDelete.Nama}.";
    }
    private void Confirm_Click(object? sender, RoutedEventArgs e)
    {
        try { store.Remove(pendingDelete); ClearForm(); txtStatus.Text = "Data mahasiswa berhasil dihapus."; }
        catch (ArgumentException ex) { txtStatus.Text = ex.Message; }
    }
    private void Cancel_Click(object? sender, RoutedEventArgs e) { pendingDelete = null; confirmPanel.IsVisible = false; txtStatus.Text = "Penghapusan dibatalkan."; }
}
