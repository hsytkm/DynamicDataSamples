using DynamicData;
using DynamicDataSamples.Wpf.Models;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace DynamicDataSamples.Wpf.Views.Pages;

public partial class SourceCache2Page : MyPageControlBase
{
    public SourceCache2Page()
    {
        DataContext = new SourceCache2PageViewModel();
        InitializeComponent();
    }
}

public sealed class SourceCache2PageViewModel : BindableBase, IDisposable
{
    readonly CompositeDisposable _disposables = new();

    public ReadOnlyObservableCollection<IdRandomValuePair> Items { get; }
    public RelayCommand<IdRandomValuePair?> UpdateValueCommand { get; } = new(x => x?.UpdateValue());

    public SourceCache2PageViewModel()
    {
        var model = App.Current.Model;

        model.RandomValueCache.Connect()
            .ObserveOnUIDispatcher()
            .Bind(out ReadOnlyObservableCollection<IdRandomValuePair>? items)
            .Subscribe().AddTo(_disposables);

        Items = items;
    }

    public void Dispose() => _disposables.Dispose();
}
