namespace Checkout.PaymentGateway.BankSimulator.Services;

internal class TestCards
{
    internal static readonly HashSet<string> CardsThatRequire3dsAuthentication = new[]
    {
        "4500622868341387"
    }.ToHashSet();

    internal static readonly HashSet<string> CardsWithActivityAmountExceeded = new[]
    {
        "4556294593757189"
    }.ToHashSet();

    internal static readonly HashSet<string> CardsWithSecurityViolation  = new[]
    {
        "4556253752712245"
    }.ToHashSet();

    internal static readonly HashSet<string> LostCards = new[]
    {
        "4941202060999329"
    }.ToHashSet();
}
