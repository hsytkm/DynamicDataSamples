using DynamicDataSamples.Wpf.Views.Pages;

namespace DynamicDataSamples.Wpf.Views;

internal static class PageSourceStore
{
    internal static IReadOnlyList<IPageSourceProvider> All { get; } = new IPageSourceProvider[]
    {
        new PageSourceProvider<WelcomePage>(),

        // Pages
        new PageSourceProvider<SourceList1Page>(),
        new PageSourceProvider<SourceCache1Page>(),
        new PageSourceProvider<SourceCache2Page>(),
    };
}
