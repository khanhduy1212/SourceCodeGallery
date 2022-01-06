using System.Collections.Generic;
using XProject.Domain.Entities;

namespace XProject.Domain.Abstract
{
    public interface ISettingRepository
    {
        IEnumerable<Setting> GetSettings();
        Setting GetSetting(int settingId);
        Setting GetSetting(string module, string name);
        Setting CreateSetting(Setting info);
        bool UpdateSetting(Setting info);
        bool DeleteSetting(int settingId);

       
    
    }
}
