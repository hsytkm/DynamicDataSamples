using System.Windows.Input;

namespace DynamicDataSamples.Wpf.Infrastructures;

public sealed class RelayCommand : ICommand
{
    private readonly Action _action;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
        => (_action, _canExecute) = (execute, canExecute);

    public void Execute(object? parameter) => _action();

    public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

    public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public sealed class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _action;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T?> execute, Func<bool>? canExecute = null)
        => (_action, _canExecute) = (execute, canExecute);

    public void Execute(object? parameter) => _action(parameter is not null ? (T)parameter : default);

    public bool CanExecute(object? parameter) => (_canExecute is null) || _canExecute();

    public void ChangeCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
