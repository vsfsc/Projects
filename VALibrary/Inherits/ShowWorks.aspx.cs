using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Diagnostics;

namespace VALibrary.Inherits
{
	public partial class ShowWorks : LayoutsPageBase
	{
		#region 控件定义
		protected TextBox TextBox1;
		protected TextBox TextBox2;
		protected HtmlGenericControl ImageDiv;
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			List<WorksFile> files = ImageFile();
			ArrayList figurelist = ImageView(files);
			ImageDiv.InnerHtml = "<div id='_contain'><div class='contain' style='margin:40px 0 0;'><div class='body'><div class='text'><div class='my-gallery' data-pswp-uid='2'>";

			foreach (var figureItem in figurelist)
			{
				ImageDiv.InnerHtml+=figureItem;
			}
			ImageDiv.InnerHtml += "</div></div></div></div></div>";
		}
		public string Name
		{
			get
			{
				return TextBox1.Text;
			}
		}
		public string Time
		{
			get
			{
				return TextBox2.Text;
			}
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			Server.Transfer("WorksShow.aspx", true);
		}

		private static List<WorksFile> ImageFile()
		{
			List<WorksFile> wfiles = new List<WorksFile>();

            WorksFile wfile = new WorksFile(1, "image1", 1, "286x220", "/WorksShow/gallery/images/pic2.jpg");
			wfiles.Add(wfile);

            wfile = new WorksFile(2, "image2", 1, "640x426", "/WorksShow/gallery/images/pic1.jpg");
			wfiles.Add(wfile);

            wfile = new WorksFile(3, "image3", 1, "768x1024", "/WorksShow/gallery/images/pic3.jpg");
			wfiles.Add(wfile);

            wfile = new WorksFile(4, "image4", 1, "900x596", "/WorksShow/gallery/images/pic4.jpg");
			wfiles.Add(wfile);

			return wfiles;
		}
		public static ArrayList ImageView(List<WorksFile> worksFiles)
		{
			var figureList = new ArrayList();
			
			foreach (var worksfile in worksFiles)
			{
				var figureItem = "";
				var filePath = "/_layouts/15/" + worksfile.FilePath;
			    //string imageSize = GetImageSize(filePath)[0]+ "x" + GetImageSize(filePath)[1];
				figureItem += "<figure>";
				figureItem += "<div><a href = '" + filePath + "' data-size='"+worksfile.FileSize+"'><img style = 'height: 100%;width: 100%;' src='" + filePath+"'></a></div>";
				figureItem += "<figcaption style = 'display: none;'>" + worksfile.FileName + "</figcaption>";
				figureItem += "</figure>";
				figureList.Add(figureItem);
			}

			return figureList;
		}

	    public static string[] GetImageSize(string imagePath)
	    {
	        string[] sizes = new string[2];
	        using (var webClient =new WebClient())
	        {
	            var imageData = webClient.DownloadData(imagePath);
	            using (var stream=new MemoryStream())
	            {
                    var image = System.Drawing.Image.FromStream(stream);
	                sizes[0]=image.Size.Height.ToString();
	                sizes[1] = image.Size.Width.ToString();
	            }
	        }
	        return sizes;
	    }
	}
}
