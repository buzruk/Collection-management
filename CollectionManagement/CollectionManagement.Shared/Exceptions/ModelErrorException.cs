namespace CollectionManagement.Shared.Exceptions;

public class ModelErrorException(string property, string message)
  : Exception(message)
{
  public string Property { get; set; } = property;
}
