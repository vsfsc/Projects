using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PowerPA.PersonalizedSettings
{
    [ToolboxItemAttribute(false)]
    public class PersonalizedSettings : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/PowerPA/PersonalizedSettings/PersonalizedSettingsUserControl.ascx";

        protected override void CreateChildControls()
        {
            PersonalizedSettingsUserControl control = Page.LoadControl(_ascxPath) as PersonalizedSettingsUserControl;
            if (control!=null)
            {
                control.webObj = this;
            }
            Controls.Add(control);
        }

        #region 自定义设置
        /// <summary>
        /// 活动列表名称
        /// </summary>
        private string _myAction = "用户-操作";
        [Personalizable, WebDisplayName("活动列表名称"), WebDescription("录入数据的列表名称，如：活动"), WebBrowsable, Category("自定义设置")]
        public string MyActions
        {
            get
            {
                return this._myAction;
            }
            set
            {
                this._myAction = value;
            }
        }


        /// <summary>
        /// 操作列表名称
        /// </summary>
        private string _actionList = "操作";
        [Personalizable, WebDisplayName("操作列表名称"), WebDescription("活动关联的操作列表的名称，如：操作"), WebBrowsable, Category("自定义设置")]
        public string ActionList
        {
            get
            {
                return this._actionList;
            }
            set
            {
                this._actionList = value;
            }
        }


        private string _userDesc = "1. 表格中，除操作外均可自定义填写数值、设置频度、可能使用时段及计量方式;2. 点击已设置并保存后的操作，可以查看操作详细设置;3. 关闭或离开本页前，请确保您已保存所有修改项！";
        [Personalizable, WebDisplayName("使用说明"), WebDescription("使用说明信息，多条请用;隔开"), WebBrowsable, Category("自定义设置")]
        public string UserDesc
        {
            get
            {
                return this._userDesc;
            }
            set
            {
                this._userDesc = value;
            }
        }

        /// <summary>
        /// 使用频度分级
        /// </summary>
        private string _frequencies = "每日必做、几乎每天、大多数天、每周偶尔、每月偶尔、年内偶尔";
        [Personalizable, WebDisplayName("频度分级"), WebDescription("频度的级别，从高到底排列，用、隔开"), WebBrowsable, Category("自定义设置")]
        public string Frequencies
        {
            get
            {
                return this._frequencies;
            }
            set
            {
                this._frequencies = value;
            }
        }


        /// <summary>
        /// 一天中的时刻分段
        /// </summary>
        private string _periods = "不限、早晨、上午、中午、下午、晚上";
        [Personalizable, WebDisplayName("时刻分段"), WebDescription("时刻的分段，从早到晚排列，用、隔开"), WebBrowsable, Category("自定义设置")]
        public string Periods
        {
            get
            {
                return this._periods;
            }
            set
            {
                this._periods = value;
            }
        }

        /// <summary>
        /// 设置值的计量方式
        /// </summary>
        private string _doms = "不限、时长、数量";
        [Personalizable, WebDisplayName("时刻分段"), WebDescription("时刻的分段，从早到晚排列，用、隔开"), WebBrowsable, Category("自定义设置")]
        public string DoMs
        {
            get
            {
                return this._doms;
            }
            set
            {
                this._doms = value;
            }
        }


        #endregion
    }
}
