using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Api.Specs.Domain;

public class MerchantTests
{
    [Fact]
    public void WhenCreated_StatusIsActive()
    {
        var merchant = new Merchant(MerchantId.From("m1"), "Test Ltd.");
        merchant.Status.Should().Be(MerchantStatus.Active);
        merchant.IsActive.Should().BeTrue();
    }

    [Fact]
    public void CloseAccount_WhenCalled_UpdatesStatus()
    {
        var merchant = new Merchant(MerchantId.From("m1"), "Test Ltd.");

        merchant.CloseAccount();

        merchant.Status.Should().Be(MerchantStatus.Inactive);
        merchant.IsActive.Should().BeFalse();
    }

    [Fact]
    public void CloseAccount_WhenAccountAlreadyClosed_ThrowsException()
    {
        var merchant = new Merchant(MerchantId.From("m1"), "Test Ltd.");
        merchant.CloseAccount();

        var act = () => merchant.CloseAccount();

        act.Should().Throw<ApplicationException>()
            .WithMessage("An inactive account cannot be closed");
    }
}
