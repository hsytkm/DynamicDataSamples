using System.Reactive.Concurrency;

namespace DynamicDataSamples.Wpf.Infrastructures;

// from https://github.com/dotnet/reactive/blob/9f2a8090cea4bf931d4ac3ad071f4df147f4df50/Rx.NET/Source/src/System.Reactive/Platforms/Desktop/Linq/DispatcherObservable.cs

/// <summary>
/// <para>If call Schedule on UIThread then schedule immediately else dispatch BeginInvoke.</para>
/// <para>UIDispatcherScheduler is created when access to UIDispatcher.Default first in the whole application.</para>
/// <para>If you want to explicitly initialize, call UIDispatcherScheduler.Initialize() in App.xaml.cs.</para>
/// </summary>
public static class UIDispatcherScheduler
{
    private static Lazy<SynchronizationContextScheduler> DefaultScheduler { get; } =
        new Lazy<SynchronizationContextScheduler>(() =>
        {
            if (SynchronizationContext.Current == null)
            {
                throw new InvalidOperationException("SynchronizationContext.Current is null");
            }

            return new SynchronizationContextScheduler(SynchronizationContext.Current);
        });

    /// <summary>
    /// <para>If call Schedule on UIThread then schedule immediately else dispatch BeginInvoke.</para>
    /// <para>UIDispatcherScheduler is created when access to UIDispatcher.Default first in the whole application.</para>
    /// <para>If you want to explicitly initialize, call UIDispatcherScheduler.Initialize() in App.xaml.cs.</para>
    /// </summary>
    public static IScheduler Default => DefaultScheduler.Value;

    internal static bool IsSchedulerCreated = DefaultScheduler.IsValueCreated;

    /// <summary>
    /// Create UIDispatcherSchedule on called thread if is not initialized yet.
    /// </summary>
    public static void Initialize()
    {
        _ = DefaultScheduler.Value;
    }
}