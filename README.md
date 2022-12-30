# DynamicDataSamples

2022.12.30

[DynamicData](https://github.com/reactivemarbles/DynamicData) のついて調査したまとめです。

きっかけは GitHub の Explore Repositories の表示です。GitHubスターは 1.5k あり（ReactiveProperty で 800スター）一定の人気がありそうですが、日本語の紹介記事は全く見つかりませんでした。

## 調査メモ

以下に [Github/README.md](https://github.com/reactivemarbles/DynamicData) の内容を整理します。

### Concept 

DynamicData のコンセプトは、コレクションを直接管理することなく、操作の連鎖によってデータを宣言的に操作/整形すること らしいです。以下は原文。

> The concept behind using dynamic data is you maintain a data source (either `SourceCache<TObject, TKey>` or `SourceList<TObject>`), then chain together various combinations of operators to declaratively manipulate and shape the data without the need to directly manage any collection.

### Observable list vs Observable cache

一意なIDを持っている場合 `SourceCache<TObject, TKey>` を使いましょう。

理由１：`SourceCache<TObject, TKey>` は Dictionary ベースなので要素が重複しません。 一方 `SourceList<TObject>` は重複を許しており、要素新の概念がありません。

理由２：`SourceCache<TObject, TKey>` には Key をベースにした多くの操作が存在し、相対的にパフォーマンスが高いです。

### The Observable List

作成

```cs
var myInts = new SourceList<int>();
myInts.AddRange(Enumerable.Range(0, 10));
```

読み取り専用として公開

```cs
IObservableList<int> readonlyInts = myInts.AsObservableList();
```

監視可能な変更リストの取得

```cs
IObservable<IChangeSet<int>> myIntsObservable = myInts.Connect();
```

### The Observable Cache

作成

```cs
var myCache = new SourceCache<TObject, TKey>(t => key);
```

読み取り専用として公開

```cs
IObservableCache<TObject, TKey> readonlyCache = myCache.AsObservableCache();
```

監視可能な変更リストの取得

```cs
IObservable<IChangeSet<TObject, TKey>> myCacheObservable = myCache.Connect();
```

### Creating Observable Change Sets

`IObservable<IChangeSet<T>>` を作成する主な方法を紹介します。

1. `ISourceList<T>`  もしくは `ISourceCache<T,K>` から作成する（一般的な作成方法）

   ```cs
   var myObservableChangeSet = myDynamicDataSource.Connect();
   ```

2. `IObservable<T>` から作成する

   ```cs
   IObservable<T> myObservable;
   IObservable<IEnumerable<T>> myObservable;
   
   var myObservableChangeSet = myObservable.ToObservableChangeSet(t => t.key);
   ```

   上記の例の問題は、監視可能な変更セットの内部バッキング キャッシュのサイズが永遠に大きくなることです。 対策方法が 2つ 用意されています。

   ```cs
   // 対策1：キャッシュの有効期間を指定する
   var myConnection = myObservable.ToObservableChangeSet(t => t.key,
       expireAfter: item => TimeSpan.FromHours(1));
   ```

   ```cs
   // 対策2：キャッシュの最大サイズを指定する
   var myConnection = myObservable.ToObservableChangeSet(t => t.key,
   	limitSizeTo:10000);
   ```

   ★2つを指定することも可能らしいです。きっと OR 条件で思われますが未確認です。

3. `System.Collections.ObjectModel.ObservableCollection<T>` から作成する

   ※スレッド セーフではないため、UI スレッドでのみ動作する単純なクエリに対してのみ推奨されます。

   ```cs
   var oc = new ObservableCollection<T>();
   
   IObservable<IChangeSet<T>> list = oc.ToObservableChangeSet();
   IObservable<IChangeSet<TObject, TKey>> cache = oc.ToObservableChangeSet(x => x.Key);
   ```

4. `System.ComponentModel.BindingList<T>` から作成する

   ※スレッド セーフではないため、UI スレッドでのみ動作する単純なクエリに対してのみ推奨されます。

   ```cs
   var bl = new BindingList<T>();
   
   IObservable<IChangeSet<T>> list = bl.ToObservableChangeSet();
   IObservable<IChangeSet<TObject, TKey>> cache = bl.ToObservableChangeSet(x => x.Key);
   ```

5. 静的クラス `ObservableChangeSet` から作成する

   ```cs
   IObservable<IChangeSet<int>> observableList = ObservableChangeSet.Create<int>(
     source =>
     {
       // some code to load data and subscribe
       var loader = myService.LoadMyDataObservable().Subscribe(source.Add);
       var subscriber = myService.GetMySubscriptionsObservable().Subscribe(source.Add);
       return new CompositeDisposable(loader, subscriber);
     });
   ```

### Consuming Observable Change Sets

`IObservable<IChangeSet<T>> ` でできること。

#### 操作

各操作によるシーケンスは常にキャッシュを正確に反映します。つまり、追加/更新/削除は常に伝播されます。

以下に README に載っていた操作を紹介します。全てではありません。

- `Filter`　(Rx の `Where`)
- `Sort`
- `GroupOn`
- `Transform` 　(Rx の `Select`)
- `TransformMany `　(Rx の `SelectMany`)
- Aggregation (`Count`, `Max`, `Min`, `StdDev`, `Avg`)
- Logical Operators (`And`, `Or`, `Xor`, `Except`)
- `DisposeMany`
- `DistinctValues`
- Virtualisation (`Virtualise`, `Page`)   ★理解していません。

以下は実装例です。

```cs
// out修飾子でパラメータを渡すためプロパティは使用不可
ReadOnlyObservableCollection<TradeProxy> list;

var myTradeCache = new SourceCache<Trade, long>(trade => trade.Id);

var disposable = myTradeCache.Connect() 
    .Filter(trade =>trade.Status is TradeStatus.Live) 
    .Transform(trade => new TradeProxy(trade))
    .Sort(SortExpressionComparer<TradeProxy>.Descending(t => t.Timestamp))
    .ObserveOnDispatcher()
    .Bind(out list) 
    .DisposeMany()
    .Subscribe();
```

#### コレクション内のオブジェクトのプロパティを観察する

```cs
// 1. 指定プロパティの変化時に、その値を返します
IObservable<TKey> changed1 = cacheList.Connect().WhenValueChanged(p => p.Key);

// 2. 指定プロパティの変化時に、対象インスタンスとプロパティ値のペアを返します
IObservable<PropertyValue<TObject, TKey>> changed2 = cacheList.Connect().WhenPropertyChanged(p => p.Key);

// 3. いづれかの変更通知が発行された場合にインスタンスを返します
IObservable<TObject?> changed3 = cacheList.Connect().WhenAnyPropertyChanged();
```

#### Observing item changes

★大事そうな内容が書いてるけど英文から内容を理解できませんでした。ソフトを触りながら内容を確認したい。



## Links

[reactivemarbles/DynamicData: Reactive collections based on Rx.Net](https://github.com/reactivemarbles/DynamicData)  公式

[Dynamic Data | dynamic data brings the power of reactive (rx) to collections](https://dynamic-data.org/)　github/README.md によると古くてダメらしいです。(alas it is hopelessly out of date.)

[ReactiveUI - Collections](https://www.reactiveui.net/docs/handbook/collections/)



