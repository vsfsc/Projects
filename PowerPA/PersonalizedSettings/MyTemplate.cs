using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PowerPA.PersonalizedSettings
{
    public class MyTemplate:ITemplate
    {
        private string strColumnName;
        private DataControlRowType dcrtColumnType;

        public MyTemplate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 动态添加模版列
        /// </summary>
        /// <param name="strColumnName">列名</param>
        /// <param name="dcrtColumnType">列的类型</param>
        public MyTemplate(string strColumnName, DataControlRowType dcrtColumnType)
        {
            this.strColumnName = strColumnName;
            this.dcrtColumnType = dcrtColumnType;
        }

        public void InstantiateIn(Control ctlContainer)
        {
            switch (dcrtColumnType)
            {
                case DataControlRowType.Header: //列标题
                    {
                        Literal ltr = new Literal
                        {
                            Text = strColumnName
                        };
                        ctlContainer.Controls.Add(ltr);
                        break;
                    }
                case DataControlRowType.DataRow: //模版列内容——加载CheckBox
                    {
                        Label lb0 = new Label
                        {
                            ID = "lbID",
                            Text = ""
                        };
                        ctlContainer.Controls.Add(lb0);
                        break;
                    }

                default:
                    {
                        Label lb1 = new Label
                        {
                            ID = "lbID",
                            Text = ""
                        };
                        ctlContainer.Controls.Add(lb1);
                        break;
                    }
            }
        }
    }
}
