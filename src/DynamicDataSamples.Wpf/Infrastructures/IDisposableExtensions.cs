namespace DynamicDataSamples.Wpf.Infrastructures;

// from https://github.com/runceel/ReactiveProperty/blob/8ff5df8f211576d638c92a3326886ee891262e52/Source/ReactiveProperty.Core/Extensions/IDisposableExtensions.cs

/// <summary>
/// IDisposable Extensions
/// </summary>
public static class IDisposableExtensions
{
    /// <summary>
    /// Add disposable(self) to CompositeDisposable(or other ICollection)
    /// </summary>
    public static T AddTo<T>(this T disposable, ICollection<IDisposable> container)
        where T : IDisposable
    {
        container.Add(disposable);
        return disposable;
    }
}