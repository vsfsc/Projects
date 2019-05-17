using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace VADocumentOnline.DocDownload
{
    public partial class DocDownloadUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReadDocument();
            if (Page.Request["docUrl"] != null)
            {
                GetDownloadFile(Server.UrlDecode (Page.Request["docUrl"].ToString()));

            }
            //ReadDocument();
        }
        public DocDownload webObj { get; set; }
        private void GetDownloadFile(string docUrl)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(webObj.WebUrl))
                    {
                        SPFile file = web.GetFile(docUrl);
                        DownLoad(file);
                    }
                }
            });
        }
        private void DownLoad(SPFile file)
        {
            Stream readStream = file.OpenBinaryStream();
             byte[] fileContent = file.OpenBinary();
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition",
              "attachment;filename=" + Server.UrlEncode(file.Name));
            //Response.ContentType = "text/plain";
            Response.BinaryWrite(fileContent);
            Response.End();
        }
        public void ReadDocument()
        {
            string taskDocName = webObj.ListName;//文档库参数名称
            Table tbl = new Table();
            TableRow tRow;
            TableCell tCell;
            //SPSecurity.RunWithElevatedPrivileges(delegate ()
            //{
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(webObj.WebUrl))
                    {
                        if (!web.Exists)
                        {
                            lblMsg.Text = webObj.WebUrl + "   不存在!";
                            return;
                        }
                        try
                        {
                            SPDocumentLibrary library = web.Lists.TryGetList(taskDocName) as SPDocumentLibrary;
                            foreach (SPListItem doc1 in library.Items)
                            {
                                tRow = new TableRow();
                                tCell = new TableCell();
                                LinkButton btn = new LinkButton();
                                btn.Text = doc1.Url ;
                                btn.CommandName = doc1.Url;
                                btn.Click += Btn_Click;
                                tCell.Controls.Add(btn);
                                tRow.Cells.Add(tCell);
                                tbl.Rows.Add(tRow);
                            }
                            divUpload.Controls.Clear();
                            divUpload.Controls.Add(tbl );
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = ex.ToString();
                        }
                    }
                }
            //});
        }
       
        private void Btn_Click(object sender, EventArgs e)
        {
            string docUrl = ((LinkButton)sender).CommandName;
            Page.Response.Redirect(Page.Request.RawUrl+"?docUrl="+ Server.UrlEncode(docUrl ));
            //GetDownloadFile(((LinkButton)sender).CommandName );
        }
    }
}
