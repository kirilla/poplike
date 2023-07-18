namespace Poplike.Application.Categories.Commands.RemoveCategory;

public class RemoveCategoryCommandModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool Confirmed { get; set; }
}
