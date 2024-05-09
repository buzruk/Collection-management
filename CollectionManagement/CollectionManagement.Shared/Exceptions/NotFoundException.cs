namespace CollectionManagement.Shared.Exceptions;

public class NotFoundException(string point, string message) 
  : Exception(message)
{
  public string Point { get; set; } = point;
}
