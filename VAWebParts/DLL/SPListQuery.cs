using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace VAWebParts.DLL
{
    public class SPListQuery
    {

        /// <summary>
        /// 获取网站目录列表
        /// </summary>
        /// <param name="web">SharePoint网站</param>
        /// <returns></returns>
        public static SPList GetCorporateCatalog(SPWeb web)
        {
            //SPSite site = GetSiteByWebTemplateId(18, current.Site.WebApplication);
            SPSite site = GetSiteByWebTemplate("APPCATALOG", web.Site.WebApplication);
            SPList list = GetListByTemplateFeatureId("0AC11793-9C2F-4CAC-8F22-33F93FAC18F2", site.RootWeb);
            return list;
        }

        /// <summary>
        /// 根据功能模板ID查询列表
        /// </summary>
        /// <param name="templateFeatureId">功能模板ID</param>
        /// <param name="web">网站</param>
        /// <returns></returns>
        public static SPList GetListByTemplateFeatureId(string templateFeatureId, SPWeb web)
        {
            foreach (SPList list in web.Lists)
            {
                if (string.Equals(list.TemplateFeatureId.ToString(), templateFeatureId, StringComparison.OrdinalIgnoreCase))
                {
                    return list;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据网站模板ID查询SPSite
        /// </summary>
        /// <param name="webTemplateId">网站模板ID</param>
        /// <param name="webApplication">网站应用程序</param>
        /// <returns></returns>
        public static SPSite GetSiteByWebTemplateId(int webTemplateId, SPWebApplication webApplication)
        {
            foreach (SPSite site in webApplication.Sites)
            {
                if (site.RootWeb.WebTemplateId == webTemplateId)
                {
                    return site;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webTemplate"></param>
        /// <param name="webApplication"></param>
        /// <returns></returns>
        public static SPSite GetSiteByWebTemplate(string webTemplate, SPWebApplication webApplication)
        {
            foreach (SPSite site in webApplication.Sites)
            {
                if (string.Equals(site.RootWeb.WebTemplate, webTemplate, StringComparison.OrdinalIgnoreCase))
                {
                    return site;
                }
            }
            return null;
        }
    }
}
