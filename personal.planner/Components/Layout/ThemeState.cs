namespace personal.planner.Components.Layout;

public sealed class ThemeState
{
    private bool _isDarkMode;

    public event Action? Changed;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode == value)
            {
                return;
            }

            _isDarkMode = value;
            Changed?.Invoke();
        }
    }
}
