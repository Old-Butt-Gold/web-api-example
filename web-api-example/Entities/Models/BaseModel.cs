namespace Entities.Models;

public abstract class BaseModel<T>
{
    public abstract T Id { get; set; }
}