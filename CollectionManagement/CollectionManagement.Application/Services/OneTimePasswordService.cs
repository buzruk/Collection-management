using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Application.Services;

public class OneTimePasswordService(UserManager<User> userManager,
                                    IUnitOfWorkAsync unitOfWorkAsync)
  : IOneTimePasswordService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUnitOfWorkAsync _unitOfWorkAsync = unitOfWorkAsync;

    /// <summary>
    /// Sends an OTP code to the user using Messager
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    /// <exception cref="FurnitureException"></exception>
    public void SendEmail(SendOtpDto dto)
    {
        //var emailMessage = new MimeMessage();

        //emailMessage.From.Add(new MailboxAddress("Site administration", "buzurgmexrs@gmail.com"));
        //emailMessage.To.Add(new MailboxAddress("", email));
        //emailMessage.Subject = subject;
        //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //{
        //  Text = message
        //};

        //using (var client = new SmtpClient())
        //{
        //  await client.ConnectAsync("smtp.metanit.com", 465, true);
        //  await client.AuthenticateAsync("admin@metanit.com", "password");
        //  await client.SendAsync(emailMessage);

        //  await client.DisconnectAsync(true);
        //}

        string from = "buzurgmexrs@gmail.com";
        string to = dto.Email;
        string pass = "BNQAG2AyH7i7HceY";
        MailMessage mailMessage = new(from, to);
        mailMessage.Subject = dto.Subject;
        mailMessage.Body = dto.Message;
        SmtpClient smtp = new("smtp.mail.ru", 587);
        smtp.Credentials = new NetworkCredential(from, pass);
        smtp.EnableSsl = true;
        smtp.Send(mailMessage);
    }

    /// <summary>
    /// Confirm user phone number using user manager
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="CollectionException"></exception>
    public async Task ConfirmEmailAsync(ConfirmEmailDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new ArgumentNullException("User not found");
        }

        var otpRepository = await _unitOfWorkAsync.GetRepositoryAsync<OneTimePassword>();
        var otp = await otpRepository.GetAsync(o => o.Email == dto.Email);

        if (otp is null)
        {
            throw new ArgumentNullException("OTP not found");
        }

        var date = DateTime.UtcNow;
        if (date > otp.ExpirationDate)
        {
            await otpRepository.RemoveAsync(otp);
            await _unitOfWorkAsync.SaveChangesAsync();
            throw new CollectionException("OTP expired");
        }

        if (otp.Code != dto.Code)
        {
            throw new CollectionException("Invalid OTP");
        }

        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);

        await otpRepository.RemoveAsync(otp);
        await _unitOfWorkAsync.SaveChangesAsync();
    }
}
