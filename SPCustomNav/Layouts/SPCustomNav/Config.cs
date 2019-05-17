using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCustomNav.Layouts.SPCustomNav
{
    class Config
    {
        public static string Load(SPWeb web, NavType navType)
        {
            string str = null;
            try
            {
                StreamReader reader;
                //SPList settingList = SPMIPUtility.CheckSettingList("SPMIPSetting");
                SPList settingList = SPContext.Current.Web.Lists[""];
                switch (navType)
                {
                    case NavType.Top:
                        {
                            SPFile topNavSettingFile = settingList.RootFolder.Files["TopNav.xml"];
                            if (topNavSettingFile.Exists)
                            {
                                using (reader = new StreamReader(topNavSettingFile.OpenBinaryStream()))
                                {
                                    str = reader.ReadToEnd();
                                }
                            }
                            break;
                        }
                    case NavType.Left:
                        {
                            SPFile leftNavSettingFile = settingList.RootFolder.Files["LeftNav.xml"];
                            if (leftNavSettingFile.Exists)
                            {
                                using (reader = new StreamReader(leftNavSettingFile.OpenBinaryStream()))
                                {
                                    str = reader.ReadToEnd();
                                }
                            }
                            break;
                        }

                    default:
                        throw new Exception("Unexpected Case");
                }
                return str;
            }
            catch (Exception exception)
            {
                throw;// SPMIPTrace.WriteError("SPMIPNavigation", exception);
                return str;
            }
        }

        public static bool Save(SPWeb web, string fileContent, NavType navType)
        {
            bool flag = false;
            try
            {
                SPList settingList = SPContext.Current.Web.Lists[""];//SPMIPUtility.CheckSettingList("SPMIPSetting");
                byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
                web.AllowUnsafeUpdates = true;
                switch (navType)
                {
                    case NavType.Top:
                        {
                            settingList.RootFolder.Files.Add("TopNav.xml", bytes, true);
                            break;
                        }
                    case NavType.Left:
                        {
                            settingList.RootFolder.Files.Add("LeftNav.xml", bytes, true);
                            break;
                        }

                    default:
                        settingList.RootFolder.Files.Add("TopNav.xml", bytes, true);
                        break;
                }
                flag = true;
            }
            catch (Exception exception)
            {
                throw;
            }
            return flag;
        }

        public enum NavType
        {
            Top,
            Left
        }
    }

}
