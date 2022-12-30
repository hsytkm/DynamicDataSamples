using DynamicDataSamples.Wpf.Models;
using System.Windows;

namespace DynamicDataSamples.Wpf;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;
    public MyModel Model { get; } = new();
}
