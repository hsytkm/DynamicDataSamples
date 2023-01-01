using System.Windows.Controls;

namespace DynamicDataSamples.Wpf.Views;

public partial class PagesListBoxPage : UserControl
{
    public PagesListBoxPage()
    {
        DataContext = new ViewModels.PagesListBoxViewModel();
        InitializeComponent();

        Loaded += PagesListBoxPage_Loaded;
    }

    private void PagesListBoxPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        filterTextBox.Focus();
        Loaded -= PagesListBoxPage_Loaded;
    }
}
