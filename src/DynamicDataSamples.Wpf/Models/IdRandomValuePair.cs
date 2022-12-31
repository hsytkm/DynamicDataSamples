namespace DynamicDataSamples.Wpf.Models;

public sealed class IdRandomValuePair : BindableBase
{
    public int Id { get; }

    public int Value
    {
        get => _value;
        private set => SetProperty(ref _value, value);
    }
    int _value = int.MinValue;

    public IdRandomValuePair(int id)
    {
        Id = id;
        UpdateValue();
    }

    public void UpdateValue()
    {
        var newValue = Random.Shared.Next(0, 100);
        //if (Value > 0) Debug.WriteLine($"{nameof(Id)}={Id}, {nameof(Value)}={Value} -> {newValue}");
        Value = newValue;
    }

    public override string ToString() => $"{nameof(Id)}={Id}, {nameof(Value)}={Value}";
}