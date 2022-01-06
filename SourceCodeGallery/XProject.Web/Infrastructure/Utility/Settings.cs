using System;
using System.Web.Mvc;
using XProject.Domain.Abstract;
using XProject.Domain.Entities;

namespace XProject.Web.Infrastructure.Utility
{
    public class Settings
    {
        #region Repository

        private static ISettingRepository SettingRepository
        {
            get { return DependencyHelper.GetService<ISettingRepository>(); }
        }

        #endregion

        /// <summary>
        ///     Get value of property in system setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingModule">XProject.Domain.Entities.Setting.ModuleType.Indent or any one</param>
        /// <param name="keyGetValue">Setting.ModuleKey.*.* : the key to get it's value</param>
        /// <param name="typeofData">type of return value</param>
        /// <returns>Return a value of the key in module</returns>
        public static dynamic GetSetting<T>(Setting.ModuleType settingModule, T keyGetValue, Type typeofData)
        {
            string module = Enum.GetName(typeof(Setting.ModuleType), settingModule);
            string key = Enum.GetName(typeof(T), keyGetValue);

            Setting t = SettingRepository.GetSetting(module, key);
            dynamic tempData = null;
            if (typeofData == typeof(int)) tempData = Convert.ToInt32(t.Value);
            else if (typeofData == typeof(decimal)) tempData = Convert.ToDecimal(t.Value);
            else if (typeofData == typeof(bool)) tempData = Convert.ToBoolean(t.Value);
            else tempData = t.Value;

            return tempData;
        }

        #region Nested type: SystemConfig

        public class SystemConfig
        {
            //public static bool LoginByEmail()
            //{
            //    return GetSetting(Setting.ModuleType.SystemConfig, Setting.ModuleKey.SystemConfig.LoginByEmail, typeof(bool));
            //}

            public static decimal GST()
            {
                return Convert.ToDecimal(GetSetting(Setting.ModuleType.SystemConfig, Setting.ModuleKey.SystemConfig.Gst, typeof(decimal)));
            }
        }

        public class CustomerConfig
        {
            public static decimal MaxUploadNumber()
            {
                return Convert.ToDecimal(GetSetting(Setting.ModuleType.CustomerConfig, Setting.ModuleKey.CustomerConfig.MaxUploadNumber, typeof(int)));
            }
            public static decimal MaxFileSizeUpload()
            {
                return Convert.ToDecimal(GetSetting(Setting.ModuleType.CustomerConfig, Setting.ModuleKey.CustomerConfig.MaxFileSizeUpload, typeof(int)));
            }
            public static string GiftUrl()
            {
                return Convert.ToString(GetSetting(Setting.ModuleType.CustomerConfig, Setting.ModuleKey.CustomerConfig.GiftUrl, typeof(string)));
            }
        }

        #endregion

    }
}
