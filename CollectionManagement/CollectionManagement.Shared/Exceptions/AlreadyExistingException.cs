namespace CollectionManagement.Shared.Exceptions;

public class AlreadyExistingException(string point, string message) 
  : Exception(message)
{
  public string Point { get; set; } = point;
}
