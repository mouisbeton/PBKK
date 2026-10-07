using System.Windows;
using StudentManagement.Core;
namespace StudentManagement.Wpf;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(new StudentRepository(DatabaseSettings.ConnectionString));
    }
}
