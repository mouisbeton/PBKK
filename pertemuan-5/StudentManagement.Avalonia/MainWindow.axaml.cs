using Avalonia.Controls;
using StudentManagement.Core;
namespace StudentManagement.Avalonia;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(new StudentRepository(DatabaseSettings.ConnectionString));
    }
}
