namespace CollectionManagement.Infrastructure.Interfaces.Accounts;

public interface IAccountService
{
  Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto, 
                                CancellationToken cancellationToken = default);

  Task<bool> RegisterAsync(AccountRegisterDto registerDto, 
                           CancellationToken cancellationToken = default);

  Task<string> LoginAsync(AccountLoginDto accountLoginDto, 
                          CancellationToken cancellationToken = default);

  Task<bool> PasswordUpdateAsync(PasswordUpdateDto passwordUpdateDto, 
                                 CancellationToken cancellationToken = default);

  Task<bool> DeleteByPasswordAsync(UserDeleteDto userDeleteDto, 
                                   CancellationToken cancellationToken = default);

  //Task SendCodeAsync(SendToEmailDto sendToEmail, CancellationToken cancellationToken = default);
  //Task<bool> VerifyPasswordAsync(UserResetPasswordDto userResetPassword, CancellationToken cancellationToken = default);
}
