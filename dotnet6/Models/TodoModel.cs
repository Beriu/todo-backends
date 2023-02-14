namespace dotnet6.Models;

public class TodoModel {

    public Guid Id { get; set; }
    public String Content { get; set; } = "";
    public Boolean IsCompleted { get; set; }
}