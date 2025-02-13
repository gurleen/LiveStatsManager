namespace Shared.Enums;

public enum Sport
{
    MensBasketball,
    WomensBasketball,
    Wrestling
}

public static class SportExtensions
{
    public static int NumPeriods(this Sport sport) => sport switch
    {
        Sport.MensBasketball => 2,
        Sport.WomensBasketball => 4,
        Sport.Wrestling => 3,
        _ => 4
    };
}