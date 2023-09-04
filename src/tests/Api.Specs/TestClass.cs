namespace Checkout.PaymentGateway.Api.Specs;

public class TestClass
{
    [Fact]
    public void WhenExecutingDummyTest_ShouldAlwaysPass()
    {
        bool result = true;
        result.Should().BeTrue();
    }
}
