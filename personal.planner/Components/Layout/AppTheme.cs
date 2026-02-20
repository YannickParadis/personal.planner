using MudBlazor;

namespace personal.planner.Components.Layout;

public static class AppTheme
{
    public static MudTheme Theme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#1D4ED8",
            Secondary = "#0F766E",
            Info = "#0369A1",
            Success = "#166534",
            Warning = "#9A3412",
            Error = "#B91C1C",
            Background = "#F3F6FB",
            Surface = "#FFFFFF",
            AppbarBackground = "#FFFFFF",
            DrawerBackground = "#EEF3FA",
            DrawerText = "#0F172A",
            TextPrimary = "#0F172A",
            TextSecondary = "#334155",
            ActionDefault = "#4B5563",
            ActionDisabled = "#94A3B8",
            LinesDefault = "#D1DAE8",
            TableLines = "#D1DAE8",
            TableStriped = "#F8FAFC"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#60A5FA",
            Secondary = "#2DD4BF",
            Info = "#38BDF8",
            Success = "#4ADE80",
            Warning = "#FBBF24",
            Error = "#F87171",
            Background = "#0B1220",
            Surface = "#111B2E",
            AppbarBackground = "#111B2E",
            DrawerBackground = "#0F172A",
            DrawerText = "#E2E8F0",
            TextPrimary = "#E2E8F0",
            TextSecondary = "#94A3B8",
            ActionDefault = "#CBD5E1",
            ActionDisabled = "#64748B",
            LinesDefault = "#263246",
            TableLines = "#263246",
            TableStriped = "#0D1627"
        }
    };
}
