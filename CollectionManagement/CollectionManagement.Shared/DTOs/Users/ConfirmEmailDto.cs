﻿namespace CollectionManagement.Shared.DTOs.Users;

public class ConfirmEmailDto
{
  public string Email { get; set; } = string.Empty;

  public string Code { get; set; } = string.Empty;
}
