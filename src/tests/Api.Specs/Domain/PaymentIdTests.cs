using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Api.Specs.Domain;

public class PaymentIdTests
{
    [Fact]
    public void From_WhenCalledWithAnIdThatIsTooShort_ThrowsException()
    {
        var act = () => PaymentId.From("aaa");
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid payment id: aaa *");
    }

    [Fact]
    public void From_WhenCalledWithAnIdThatIsTooLong_ThrowsException()
    {
        var act = () => PaymentId.From(new String('a', 13));
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void From_WhenCalledWithNullOrEmptyString_ThrowsException(string id)
    {
        var act = () => PaymentId.From(id);
        act.Should().Throw<ArgumentException>();
    }
}
