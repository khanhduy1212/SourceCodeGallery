namespace XProject.Domain.Abstract
{
    public interface IAuthorizationService
    {
        bool CheckAccess(int userID, string feature, string permissionType = null);
    }
}
