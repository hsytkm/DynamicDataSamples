using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace DynamicDataSamples.Wpf.Models;

public sealed class MyModel : BindableBase, IDisposable
{
    readonly CompositeDisposable _disposables = new();

    public SourceList<int> TimerValueList { get; }
    public SourceCache<IdRandomValuePair, int> RandomValueCache { get; }

    public MyModel()
    {
        IObservable<int> timer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .Select(x => (int)Math.Min(x, int.MaxValue));

        // 指定秒が経過したら要素を削除することでコレクションのサイズを制限します
        IObservable<IChangeSet<int>> timerChangeSet = timer.ToObservableChangeSet(expireAfter: x => TimeSpan.FromSeconds(20));
        TimerValueList = new SourceList<int>(timerChangeSet).AddTo(_disposables);

        RandomValueCache = new SourceCache<IdRandomValuePair, int>(x => x.Id).AddTo(_disposables);
        timer.Subscribe(i => RandomValueCache.AddOrUpdate(new IdRandomValuePair(i))).AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();
}
