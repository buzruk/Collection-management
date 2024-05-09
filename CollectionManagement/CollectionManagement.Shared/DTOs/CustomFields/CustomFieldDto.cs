namespace CollectionManagement.Shared.DTOs.CustomFields;

public class CustomFieldDto
{
  public string Name { get; set; } = string.Empty;

  public FieldType Type { get; set; } = FieldType.String;
}
