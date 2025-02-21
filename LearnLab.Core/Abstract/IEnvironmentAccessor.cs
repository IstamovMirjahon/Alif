
namespace LearnLab.Core.Abstract;
public interface IEnvironmentAccessor
{
    string GetFullName();
    string GetWebRootPath();
    bool HasRole(string role);
    bool IsAdmin(Guid id);
    bool IsAuthorOrAdmin(Guid id);
    bool IsAuthorOrSupervisor(Guid? id);
    string GetUserId();
    string GetUserName();
}
