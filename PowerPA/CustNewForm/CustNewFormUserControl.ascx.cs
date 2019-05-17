using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace PowerPA.CustNewForm
{
    public partial class CustNewFormUserControl : UserControl
    {
        public CustNewForm webObj;

        protected void Page_Load(object sender, EventArgs e)
        {
            initControl();
        }


        private void initControl()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    string listName = webObj.ListName;
                    string[] listfields = webObj.ListFields.Split(';');
                    string siteUrl = webObj.SiteUrl;
                   using (SPSite spSite = new SPSite(siteUrl)) //找到网站集
                   {
                       using (SPWeb spWeb = spSite.OpenWeb())
                       {
                            SPList splist = spWeb.Lists.TryGetList(listName);
                            for (int i = 0; i < listfields.Length; i++)
                            {
                                TableRow tr = new TableRow();

                                SPField fd = splist.Fields[listfields[i]];
                                string fdInternalName = fd.InternalName;

                                //创建标签列
                                TableCell tc1 = new TableCell();//标签

                                Label lb = new Label();
                                lb.ID = fdInternalName;
                                lb.Text = listfields[i];
                                tc1.Controls.Add(lb);
                                tr.Cells.Add(tc1);

                                //创建表单列
                                SPFieldType fdtype =fd .Type;
                                TableCell tc2 = CreatFormCell(fdtype, fdInternalName, listfields[i]);//表单
                                tr.Cells.Add(tc2);

                                //创建说明列
                                TableCell tc3 = new TableCell();//说明
                                Label lbDesc = new Label();
                                lbDesc.ID = fdInternalName+"Desc";
                                lbDesc.Text = fd.Description;
                                tc1.Controls.Add(lbDesc);
                                tr.Cells.Add(tc3);
                                tbForms.Rows.Add(tr);
                            }
                        }
                   }
                    });
               }
                catch (Exception ex)
                {
                    lberr.Text=ex.ToString();
                }

        }

        private TableCell CreatFormCell(SPFieldType fdtype,string Id,string Name)
        {
            TableCell tc = new TableCell();//标签
            switch (fdtype)
            {
                //case SPFieldType.Invalid:
                //    break;
                case SPFieldType.Integer:
                    TextBox tbInt = new TextBox();
                    tbInt.ID = Id;
                    tbInt.ToolTip = "请输入："+Name;
                    tbInt.Text = "30";
                    tbInt.TextMode = TextBoxMode.SingleLine;
                    tc.Controls.Add(tbInt);
                    break;
                case SPFieldType.Text:
                    TextBox tbText = new TextBox();
                    tbText.ID = Id;
                    tbText.Text = "";
                    tbText.ToolTip = "请输入："+Name;
                    tbText.TextMode = TextBoxMode.SingleLine;
                    tc.Controls.Add(tbText);
                    break;
                case SPFieldType.Note:
                    TextBox tbNote = new TextBox();
                    tbNote.ID = Id;
                    tbNote.ToolTip = "请输入："+Name;
                    tbNote.TextMode =TextBoxMode.MultiLine;
                    tbNote.Text = "";
                    tc.Controls.Add(tbNote);
                    break;
                case SPFieldType.DateTime:
                    DateTimeControl dtC = new DateTimeControl();
                    dtC.ID = Id;
                    dtC.ToolTip="请选择或输入："+Name;
                    tc.Controls.Add(dtC);
                    break;
                case SPFieldType.Counter:
                    TextBox tbCounter= new TextBox();
                    tbCounter.ID = Id;
                    tbCounter.ToolTip = "请输入：" + Name;
                    tbCounter.TextMode = TextBoxMode.MultiLine;
                    tbCounter.Text = "";
                    tc.Controls.Add(tbCounter);
                    break;
                //case SPFieldType.Choice:
                //    break;
                //case SPFieldType.Lookup:
                //    break;
                //case SPFieldType.Boolean:
                //    break;
                case SPFieldType.Number:
                    TextBox tbNumber = new TextBox();
                    tbNumber.ID = Id;
                    tbNumber.ToolTip = "请输入：" + Name;
                    tbNumber.TextMode = TextBoxMode.MultiLine;
                    tbNumber.Text = "";
                    tc.Controls.Add(tbNumber);
                    break;
                //case SPFieldType.Currency:
                //    break;
                //case SPFieldType.URL:
                //    break;
                //case SPFieldType.Computed:
                //    break;
                //case SPFieldType.Threading:
                //    break;
                //case SPFieldType.Guid:
                //    break;
                //case SPFieldType.MultiChoice:
                //    break;
                //case SPFieldType.GridChoice:
                //    break;
                //case SPFieldType.Calculated:
                //    break;
                //case SPFieldType.File:
                //    break;
                //case SPFieldType.Attachments:
                //    break;
                //case SPFieldType.User:
                //    break;
                //case SPFieldType.Recurrence:
                //    break;
                //case SPFieldType.CrossProjectLink:
                //    break;
                //case SPFieldType.ModStat:
                //    break;
                //case SPFieldType.Error:
                //    break;
                //case SPFieldType.ContentTypeId:
                //    break;
                //case SPFieldType.PageSeparator:
                //    break;
                //case SPFieldType.ThreadIndex:
                //    break;
                //case SPFieldType.WorkflowStatus:
                //    break;
                //case SPFieldType.AllDayEvent:
                //    break;
                //case SPFieldType.WorkflowEventType:
                //    break;
                //case SPFieldType.Geolocation:
                //    break;
                //case SPFieldType.OutcomeChoice:
                //    break;
                //case SPFieldType.MaxItems:
                //    break;
                default:
                    TextBox tbDefault = new TextBox();
                    tbDefault.ID = Id;
                    tbDefault.ToolTip = "请输入：" + Name;
                    tbDefault.TextMode = TextBoxMode.MultiLine;
                    tbDefault.Text = "";
                    tc.Controls.Add(tbDefault);
                    break;
            }
            return tc;
        }
    }
}
