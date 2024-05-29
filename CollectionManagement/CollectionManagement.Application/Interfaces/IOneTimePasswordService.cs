namespace CollectionManagement.Application.Interfaces;

public interface IOneTimePasswordService
{
    void SendEmail(SendOtpDto dto);

    Task ConfirmEmailAsync(ConfirmEmailDto dto);
}
