using DynamicData;
using DynamicData.Binding;
using DynamicDataSamples.Wpf.Models;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace DynamicDataSamples.Wpf.Views.Pages;

public partial class SourceCache1Page : MyPageControlBase
{
    public SourceCache1Page()
    {
        DataContext = new SourceCache1PageViewModel();
        InitializeComponent();
    }
}

public sealed class SourceCache1PageViewModel : BindableBase, IDisposable
{
    readonly CompositeDisposable _disposables = new();

    public ReadOnlyObservableCollection<IdRandomValuePair> Items1 { get; }
    public ObservableCollectionExtended<IdRandomValuePair> Items2 { get; }

    public SourceCache1PageViewModel()
    {
        var model = App.Current.Model;

        model.RandomValueCache.Connect()
            .ObserveOnUIDispatcher()
            .Bind(out ReadOnlyObservableCollection<IdRandomValuePair>? items1)
            .Subscribe().AddTo(_disposables);

        ObservableCollectionExtended<IdRandomValuePair> items2 = new();
        model.RandomValueCache.Connect()
            .Filter(x => x.Id != 0 && (x.Id % 2) == 0)  // multiple of 2
            .Sort(SortExpressionComparer<IdRandomValuePair>.Ascending(x => x.Value))
            .ObserveOnUIDispatcher()
            .Bind(items2)
            .Subscribe().AddTo(_disposables);

        Items1 = items1;
        Items2 = items2;
    }

    public void Dispose() => _disposables.Dispose();
}
