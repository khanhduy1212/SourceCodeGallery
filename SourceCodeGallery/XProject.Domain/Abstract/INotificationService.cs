using XProject.Domain.Entities;

namespace XProject.Domain.Abstract
{
    public interface INotificationService<T> where T : class
    {
        void NotifyAll(T entity);
        void NotifyUser(UserLogin userLogin, T entity = null);
        void NotifyRole(Role role, T entity = null);
    }
}
