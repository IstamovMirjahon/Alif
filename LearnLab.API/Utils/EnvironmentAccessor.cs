using LearnLab.Core.Abstract;
using LearnLab.Core.Constants;
using LearnLab.Core.Exceptions;
using LearnLab.Identity.Constants;

namespace LearnLab.API.Utils;


public class EnvironmentAccessor : IEnvironmentAccessor
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _contextAccessor;

    public EnvironmentAccessor(IWebHostEnvironment environment, IHttpContextAccessor contextAccessor)
    {
        _environment = environment;
        _contextAccessor = contextAccessor;
    }

    public string GetFullName()
    {
        var firstName = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.FirstName.ToString()))?.Value;
        var lastName = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.LastName.ToString()))?.Value;
        return $"{firstName} {lastName}";
    }

    public string GetWebRootPath()
    {
        return _environment.WebRootPath;
    }

    public bool HasRole(string role)
    {
        throw new NotImplementedException();
    }

    public bool IsAdmin(Guid id)
    {
        if (_contextAccessor.HttpContext is null)
            throw new LearnLabException("HttpContext can not be null.");

        if (_contextAccessor.HttpContext.User.IsInRole(RoleNames.SuperAdmin))
            return true;

        return false;
    }

    public bool IsAuthorOrAdmin(Guid id)
    {
        if (_contextAccessor.HttpContext is null)
            throw new LearnLabException("HttpContext can not be null.");

        if (_contextAccessor.HttpContext.User.IsInRole(RoleNames.SuperAdmin) || GetUserId() == id.ToString())
            return true;

        return false;
    }

    public bool IsAuthorOrSupervisor(Guid? id)
    {
        if (_contextAccessor.HttpContext is null)
            throw new LearnLabException("HttpContext can not be null.");

        if (_contextAccessor.HttpContext.User.IsInRole(RoleNames.Supervisor) || GetUserId() == id.ToString())
            return true;

        return false;
    }

    public string GetUserId()
    {
        return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.UserId.ToString()))?.Value;
    }

    public string GetUserName()
    {
        return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.UserName.ToString()))?.Value;
    }
}

