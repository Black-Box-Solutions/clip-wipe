using Syncfusion.Maui.Toolkit.Charts;

namespace Sample.MauiApp1.Pages.Controls;

public class LegendExt : ChartLegend
{
    protected override double GetMaximumSizeCoefficient()
    {
        return 0.5;
    }
}
