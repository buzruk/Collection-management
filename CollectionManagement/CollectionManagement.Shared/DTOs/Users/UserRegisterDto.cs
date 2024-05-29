using CollectionManagement.Domain.Entities;

namespace CollectionManagement.Shared.DTOs.Users;

public class UserRegisterDto : UserLoginDto
{
    public DateTime BirthDate { get; set; }

    public static explicit operator User(UserRegisterDto dto)
    {
        return new()
        {
            UserName = dto.UserName,
            BirthDate = dto.BirthDate,
            Email = dto.Email,
            Role = dto.Role,
        };
    }
}

