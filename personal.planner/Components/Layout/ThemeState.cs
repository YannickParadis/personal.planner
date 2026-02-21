namespace personal.planner.Components.Layout;

public enum ThemeMode
{
    Light = 0,
    DarkBlue = 1,
    DarkSlate = 2
}

public sealed class ThemeState
{
    private ThemeMode _mode = ThemeMode.Light;

    public event Action? Changed;

    public ThemeMode Mode
    {
        get => _mode;
        set
        {
            if (_mode == value)
            {
                return;
            }

            _mode = value;
            Changed?.Invoke();
        }
    }

    public bool IsDarkMode => Mode != ThemeMode.Light;
}
