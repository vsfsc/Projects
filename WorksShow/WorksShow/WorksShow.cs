using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace WorksShow.WorksShow
{
    [ToolboxItemAttribute(false)]
    public class WorksShow : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WorksShow/WorksShow/WorksShowUserControl.ascx";

        protected override void CreateChildControls()
        {
            WorksShowUserControl control = Page.LoadControl(_ascxPath) as WorksShowUserControl;
            if (control != null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置
        /// <summary>
        /// 活动列表名称
        /// </summary>
        private string _workslist = "作品";
        [Personalizable, WebDisplayName("作品列表名称"), WebDescription("作品存储的列表名称，如：作品"), WebBrowsable, Category("自定义设置")]
        public string WorksList
        {
            get
            {
                return this._workslist;
            }
            set
            {
                this._workslist = value;
            }
        }


        /// <summary>
        /// 操作列表名称
        /// </summary>
        private string _userworks = "用户-作品";
        [Personalizable, WebDisplayName("用户-作品关系列表"), WebDescription("用户-作品关系列表的名称，如：用户-作品"), WebBrowsable, Category("自定义设置")]
        public string UserWorks
        {
            get
            {
                return this._userworks;
            }
            set
            {
                this._userworks = value;
            }
        }


        private string _taskDoc = "任务文档";
        [Personalizable, WebDisplayName("任务文档列表"), WebDescription("任务文档列表名称"), WebBrowsable, Category("自定义设置")]
        public string TaskDoc
        {
            get
            {
                return this._taskDoc;
            }
            set
            {
                this._taskDoc = value;
            }
        }

        private string _activity = "活动";
        [Personalizable, WebDisplayName("活动列表"), WebDescription("活动列表名称"), WebBrowsable, Category("自定义设置")]
        public string Activity
        {
            get
            {
                return this._activity;
            }
            set
            {
                this._activity = value;
            }
        }

        private string _mediaLibrary = "活动媒体库";
        [Personalizable, WebDisplayName("活动媒体库"), WebDescription("活动媒体库名称"), WebBrowsable, Category("自定义设置")]
        public string MediaLibrary
        {
            get
            {
                return this._mediaLibrary;
            }
            set
            {
                this._mediaLibrary = value;
            }
        }

        private string _amedia = "活动媒体";
        [Personalizable, WebDisplayName("活动媒体关系列表"), WebDescription("活动媒体关系列表名称"), WebBrowsable, Category("自定义设置")]
        public string ActivityMedia
        {
            get
            {
                return this._amedia;
            }
            set
            {
                this._amedia = value;
            }
        }


        private string _FileIcon = "文件-图标";
        [Personalizable, WebDisplayName("文件图标列表"), WebDescription("文件图标列表名称"), WebBrowsable, Category("自定义设置")]
        public string FileIcon
        {
            get
            {
                return this._FileIcon;
            }
            set
            {
                this._FileIcon = value;
            }
        }

        /// <summary>
        /// Office Online Server服务器地址
        /// </summary>
        private string _ooServer = "http://ismart.neu.edu.cn/";
        [Personalizable, WebDisplayName("Office Online Server服务器地址"), WebDescription("Office Online Server服务器地址"), WebBrowsable, Category("自定义设置")]
        public string OOServer
        {
            get
            {
                return this._ooServer;
            }
            set
            {
                this._ooServer = value;
            }
        }

        /// <summary>
        /// 列表名称
        /// </summary>
        private int _width = 600;
        [Personalizable, WebDisplayName("显示宽度"), WebDescription("列表名称，如:600"), WebBrowsable, Category("自定义设置")]
        public int DispWidth
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        /// <summary>
        /// 列表名称
        /// </summary>
        private int _height = 450;
        [Personalizable, WebDisplayName("显示宽度"), WebDescription("列表名称，如:450"), WebBrowsable, Category("自定义设置")]
        public int DispHeight
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }
        #endregion
    }
}
