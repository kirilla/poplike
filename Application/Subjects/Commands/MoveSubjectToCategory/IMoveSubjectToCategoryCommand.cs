namespace Poplike.Application.Subjects.Commands.MoveSubjectToCategory;

public interface IMoveSubjectToCategoryCommand
{
    Task Execute(IUserToken userToken, MoveSubjectToCategoryCommandModel model);
}
