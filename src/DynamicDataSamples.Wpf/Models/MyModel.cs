using DynamicData;
using System.Reactive.Linq;

namespace DynamicDataSamples.Wpf.Models;

public sealed class MyModel : BindableBase, IDisposable
{
    public SourceList<int> TimerSourceList { get; }

    public MyModel()
    {
        IObservable<IChangeSet<int>> timerChangeSet = Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
            .Select(x => (int)Math.Min(x, int.MaxValue))
            .ToObservableChangeSet(expireAfter: x => TimeSpan.FromMinutes(1));

        TimerSourceList = new(timerChangeSet);
    }

    public void Dispose() => TimerSourceList.Dispose();
}
