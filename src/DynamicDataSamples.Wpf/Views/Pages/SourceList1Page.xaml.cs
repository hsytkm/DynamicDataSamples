using DynamicData;
using DynamicData.Binding;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace DynamicDataSamples.Wpf.Views.Pages;

public partial class SourceList1Page : UserControl
{
    public SourceList1Page()
    {
        DataContext = new SourceList1PageViewModel();
        InitializeComponent();
    }
}

public sealed class SourceList1PageViewModel : BindableBase, IDisposable
{
    readonly CompositeDisposable _disposables = new();

    public ReadOnlyObservableCollection<int> Items1 { get; }
    public ObservableCollectionExtended<int> Items2 { get; }

    public SourceList1PageViewModel()
    {
        var model = App.Current.Model;

        model.TimerSourceList.Connect()
            .ObserveOnUIDispatcher()
            .Bind(out ReadOnlyObservableCollection<int>? items1)
            .Subscribe().AddTo(_disposables);

        ObservableCollectionExtended<int> items2 = new();
        model.TimerSourceList.Connect()
            .Filter(x => x != 0 && (x % 2) == 0)    // multiple of 2
            .ObserveOnUIDispatcher()
            .Bind(items2)
            .Subscribe().AddTo(_disposables);

        Items1 = items1;
        Items2 = items2;
    }

    public void Dispose() => _disposables.Dispose();
}
