namespace Checkout.PaymentGateway.Api.Specs;

public class TestClass
{
    [Fact]
    public void WhenExecutingDummyTest_ShouldAlwaysPass()
    {
        const bool result = true;
        result.Should().BeTrue();
    }
}
