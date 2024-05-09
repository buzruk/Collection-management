namespace CollectionManagement.Shared.ViewModels.CustomFieldViewModels;

public class CustomFieldViewModel
{
  public int Id { get; set; }

  public string Name { get; set; } = String.Empty;

  public FieldType Type { get; set; } = FieldType.String;
}

