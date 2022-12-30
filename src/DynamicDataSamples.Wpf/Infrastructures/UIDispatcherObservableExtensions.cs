using System.Reactive.Linq;

namespace DynamicDataSamples.Wpf.Infrastructures;

// from https://github.com/runceel/ReactiveProperty/blob/8ff5df8f211576d638c92a3326886ee891262e52/Source/ReactiveProperty.NETStandard/Extensions/UIDispatcherSchedulerObservableExtensions.cs

/// <summary>
/// UI Dispatcher Observable Extensions
/// </summary>
public static class UIDispatcherObservableExtensions
{
    /// <summary>
    /// <para>Observe on UIDispatcherScheduler.</para>
    /// <para>
    /// UIDispatcherScheduler is created when access to UIDispatcher.Default first in the whole application.
    /// </para>
    /// <para>
    /// If you want to explicitly initialize, call UIDispatcherScheduler.Initialize() in App.xaml.cs.
    /// </para>
    /// </summary>
    public static IObservable<T> ObserveOnUIDispatcher<T>(this IObservable<T> source) =>
        source.ObserveOn(UIDispatcherScheduler.Default);

    /// <summary>
    /// <para>Subscribe on UIDispatcherScheduler.</para>
    /// <para>
    /// UIDispatcherScheduler is created when access to UIDispatcher.Default first in the whole application.
    /// </para>
    /// <para>
    /// If you want to explicitly initialize, call UIDispatcherScheduler.Initialize() in App.xaml.cs.
    /// </para>
    /// </summary>
    public static IObservable<T> SubscribeOnUIDispatcher<T>(this IObservable<T> source) =>
        source.SubscribeOn(UIDispatcherScheduler.Default);
}