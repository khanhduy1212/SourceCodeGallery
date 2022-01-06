using System.Collections.Generic;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;

namespace XProject.Web.Infrastructure.Utility
{
    public class MenuHelper
    {
        #region Repository

        private static IMenuRepository MenuRepository
        {
            get { return DependencyHelper.GetService<IMenuRepository>(); }
        }

        #endregion

        public static IEnumerable<Menu> GetAll(int parentId = 0)
        {
            if (!CurrentUser.IsAuthenticated)
                return new Menu[] { };

            return MenuRepository.GetAllChildrenByUser(CurrentUser.Identity.ID,parentId);
        }
    }
}
