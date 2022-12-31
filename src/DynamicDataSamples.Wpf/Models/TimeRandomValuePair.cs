namespace DynamicDataSamples.Wpf.Models;

public sealed class IdRandomValuePair : BindableBase
{
    public int Id { get; }

    public int Value
    {
        get => _value;
        private set => SetProperty(ref _value, value);
    }
    int _value;

    public IdRandomValuePair(int id)
    {
        Id = id;
        UpdateValue();
    }

    public void UpdateValue() => Value = Random.Shared.Next(0, 100);

    public override string ToString() => $"Timer={Id}, Value={Value}";
}