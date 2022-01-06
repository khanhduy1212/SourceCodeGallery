using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;

using XProject.Web.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace XProject.Web.Hubs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotificationHub : Hub
    {
        private static IMembershipService MembershipService
        {
            get { return DependencyHelper.GetService<IMembershipService>(); }
        }

        #region Override

        public override Task OnConnected()
        {
            AddConnectionIntoGroups();
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            RemoveConnectionFromGroups();
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            AddConnectionIntoGroups();
            return base.OnReconnected();
        }

        #endregion

        #region Connection manipulation

        public static string GetRoleGroupName(Role role)
        {
            return "role_" + role.ID;
        }

        public static string GetUserGroupName(UserLogin userLogin)
        {
            return "user_" + userLogin.ID;
        }

        private void AddConnectionIntoGroups()
        {
            UserLogin userLogin = MembershipService.GetUserByName(Context.User.Identity.Name);
            if (userLogin != null)
            {
                // add connection to group by role
                foreach (Role role in userLogin.Roles)
                    Groups.Add(Context.ConnectionId, GetRoleGroupName(role));

                // add connection to group by UserLogin
                Groups.Add(Context.ConnectionId, GetUserGroupName(userLogin));
            }
        }

        private void RemoveConnectionFromGroups()
        {
            UserLogin userLogin = MembershipService.GetUserByName(Context.User.Identity.Name);
            if (userLogin != null)
            {
                foreach (Role role in userLogin.Roles)
                    Groups.Remove(Context.ConnectionId, GetRoleGroupName(role));

                Groups.Remove(Context.ConnectionId, GetUserGroupName(userLogin));
            }
        }

        #endregion

        #region Client interactive

        public static void Notify(string message, string dataType)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.notify(message, dataType);
        }
        #endregion

        #region Helpers

        private static string GetEntityName(object entity)
        {
            string name = entity.GetType().Name;

            int lastUnderscorePos = name.LastIndexOf('_');
            if (lastUnderscorePos >= 0)
                name = name.Substring(0, lastUnderscorePos);

            return name;
        }

        #endregion
    }
}
