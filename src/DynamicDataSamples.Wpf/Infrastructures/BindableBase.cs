using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynamicDataSamples.Wpf.Infrastructures;

public abstract class BindableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        NotifyPropertyChanged(propertyName);
        return true;
    }

    protected virtual void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
