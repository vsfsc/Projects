<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorksShowUserControl.ascx.cs" Inherits="WorksShow.WorksShow.WorksShowUserControl" %>
<%-- 引用脚本区 --%>
        <style type="text/css">
                                .pcontant{
                                    width: 900px;
				margin: 10px auto;
                                }
			.container {
				width: 800px;
				margin: 10px auto;
			}

			.box {
				width:180px;
				height: 180px;
				float: left;
				margin-left: 10px;
				margin-bottom: 20px;
			}

			.box img {
				width:180px;
				max-width: 180px;
			}

			.box h4 {
				width: 100%;
				margin-bttom:10px;
			}
		</style>
        <script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/jquery-1.11.0.min.js"></script>
		<script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/EZView.js"></script>
		<script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/draggable.js"></script>
		<script type="text/javascript">
			$(function() {
				// Init pluging in all the images
				$('img.ez').EZView();
			})
		</script>


        <script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/jquery-1.11.0.min.js"></script>
		<script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/EZView.js"></script>
		<script type="text/javascript" src="../../../../_layouts/15/WorksShow/js/draggable.js"></script>
		<script type="text/javascript">
			$(function() {
				// Init pluging in all the images
				$('img.ez').EZView();
			})
		</script>
<script type="text/javascript">
	function txtdiva() {
		document.getElementById("txtDiv").style.display = "";
		document.getElementById('btnSubmit').style.display = '';
		document.getElementById('inputbutton').style.display = 'none';

	}

	function YiComments() {
		document.getElementById('inputbutton').style.display = 'none';
	}

	function ValidComments() {
		var txt = document.getElementById('<%=TxtComments.ClientID%>');
			if (txt.value.length == 0) {
				alert("评语不能为空！");
				txt.select();
				return false;
			}
			return true
		}

		function rate(obj, oEvent) {

			// 图片地址设置
			var imgSrc = 'images/star_gray.gif'; //没有填色的星星
			var imgSrc_2 = 'images/star_red.gif'; //打分后有颜色的星星

			if (obj.rateFlag) return;
			var e = oEvent || window.event;
			var target = e.target || e.srcElement;
			var imgArray = obj.getElementsByTagName("img");
			for (var i = 0; i < imgArray.length; i++) {
				imgArray[i]._num = i;
				imgArray[i].onclick = function () {
					if (obj.rateFlag) return;
					obj.rateFlag = true;
					document.getElementById('HiddenField1').value = this._num + 1;

					//    alert(this._num+1);  //this._num+1这个数字写入到数据库中,作为评分的依据
				};
			}
			if (target.tagName == "IMG") {
				for (var j = 0; j < imgArray.length; j++) {
					if (j <= target._num) {
						imgArray[j].src = imgSrc_2;
					} else {
						imgArray[j].src = imgSrc;
					}
				}
			} else {
				for (var k = 0; k < imgArray.length; k++) {
					imgArray[k].src = imgSrc;
				}
			}
		}
</script>

<link rel="stylesheet" href="../../../../_layouts/15/WorksShow/css/default.css" />


<%-- *****相关作品脚本引用结束***** --%>
<%-- 部件显示区 --%>
<div id="divWorks" runat="server">

	<div id="floatposition">
		<div class="f" id="divrightnav">
			<ul>
				<li><a href="#a01">作品简介</a></li>
				<li><a href="#a02">设计思路</a></li>
				<li><a href="#a03">创意特色</a></li>
				<li><a href="#a04">作品描述</a></li>
				<li><a href="#a05">作品附件</a></li>
				<li><a href="#a06">作品点评</a></li>
				<li><a href="#a00" style="color: #32cd32; text-decoration: none;">回到顶部</a></li>
				<li>
					<div style="line-height:25px;">
						<asp:ImageButton ID="ibtnView" runat="server" ImageUrl="../../../../_layouts/15/WorksShow/images/view.png" ToolTip="浏览次数" Height="18px" Width="18px" Enabled="false"/>
						&nbsp;<asp:Label ID="lbReadCount" runat="server" Text="0"></asp:Label>
						<br />
						<asp:ImageButton ID="ibtnFav" runat="server" ImageUrl="../../../../_layouts/15/WorksShow/images/unfav.png" ToolTip="收藏" Height="18px" Width="18px" OnClick="ibtnFav_Click" CausesValidation="False" />
						&nbsp;<asp:Label ID="lbFavCount" runat="server" Text="0"></asp:Label>
						<br/>
						<asp:ImageButton ID="ibtnLike" runat="server" ImageUrl="../../../../_layouts/15/WorksShow/images/unlike.png" ToolTip="点赞" Height="18px" Width="18px" OnClick="ibtnLike_Click" CausesValidation="False" />
						 &nbsp;<asp:Label ID="lbLike" runat="server" Text="0"></asp:Label>
					</div>
				</li>
			</ul>
		</div>

	</div>
	<div class="content" id="divWorksContent">
		<asp:Label ID="lbErr" runat="server" Text="" ForeColor="red"></asp:Label>
        <table style="width: 100%;">
            <tr>
                <td style="width:80%;vertical-align:top;min-width:500px;">
                    <table cellpadding="2" cellspacing="0" style="font-size: 14px; width: 800px; text-align: left">
                        <tr>
                            <td class="worktitle" valign="bottom" id="a00" style="text-align: center;" align="center">
                                <div class="h3css">
                                    <asp:Label ID="LbWorksName" runat="server" Text="作品标题"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="text-align: right;">
                                    <span style="font-weight: bold;">作者</span>：<asp:Label ID="lbAuthor" runat="server" Text="***"></asp:Label>
                                    &nbsp;&nbsp;
						 <span style="font-weight: bold;">发布日期</span>：<asp:Label ID="lbPub" runat="server" Text="0000/00/00"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td class="titlea"><span class="titleaspan">作品状态：</span>&nbsp;
							<asp:Label ID="LbWorksStatus" runat="server" Text="A120622"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="titlea">
                                <span class="titleaspan">作品类别：</span>&nbsp;
					<asp:Label ID="LbWorksType" runat="server" Text="计算机动画、游戏"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px"></td>
                        </tr>

                        <tr>
                            <td class="titlea titlebga" id="a01">
                                <div class="titlebg">作品简介</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divIntro" runat="server" class="lblcss">正文内容</div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td class="titlebga" id="a02">
                                <div class="titlebg">设计思路</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divIdeas" runat="server" class="lblcss">正文内容</div>
                            </td>
                        </tr>

                        <tr>
                            <td style="height: 10px">&nbsp;</td>
                        </tr>

                        <tr>
                            <td class="titlebga" id="a03">
                                <div class="titlebg">创意特色</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divFeatures" class="lblcss" runat="server">
                                    正文内容
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td style="height: 10px"></td>
                        </tr>

                        <tr>
                            <td class="titlebga" id="a04">
                                <div class="titlebg" style="font-size: 13px">作品描述</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divDesc" runat="server" style="padding-left: 10px;">
                                    正文内容
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td style="height: 10px"></td>
                        </tr>

                        <tr>
                            <td class="titlebga" id="a05">
                                <div class="titlebg">作品附件</div>

                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px; color: #555; background-color: #EAEAEA">
                                <%-- 文档附件 --%>
                                <fieldset id="fsdocs" runat="server">
                                    <legend>照片与文档</legend>
                                    <div id="DocsDiv">
                                        <div id="DocsList" runat="server" class="container">
                                            <%--<div class="box">
                                                <img src="../../../../_layouts/15/WorksShow/Icons/256_ICDOCX.PNG" class="ez" alt="测试">
                                                <h4 style="text-align: center;">测试</h4>
                                            </div>--%>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset id="fsmedias" runat="server">
                                    <legend>音频与视频</legend>

                                    <%-- 音视频附件 --%>

                                    <div class="video_list clearfix" id="MediaList" runat="server">
                                    </div>
                                    <!--OPEN WIN-->
                                    <div id="showdiv">
                                        <div class="showdivbg"></div>
                                        <div class="showlayer">
                                            <a href="javascript:;" class="close"></a>
                                            <div id="CuPlayer">loading...</div>
                                            <script type="text/javascript">
                                                function cuplayerPlay(flvlink, imagelink) {
                                                    var flvwidth = 900;
                                                    var flvheight = 488 + 20;
                                                    var so = new SWFObject("CuPlayerMiniV4.swf", "CuPlayerV4", flvwidth, flvheight, "9", "#000000");
                                                    so.addParam("allowfullscreen", "true");
                                                    so.addParam("allowscriptaccess", "always");
                                                    so.addParam("wmode", "opaque");
                                                    so.addParam("quality", "high");
                                                    so.addParam("salign", "lt");
                                                    so.addVariable("CuPlayerSetFile", "CuPlayerSetFile.xml"); //播放器配置文件地址,例SetFile.xml、SetFile.asp、SetFile.php、SetFile.aspx
                                                    so.addVariable("CuPlayerFile", flvlink); //视频文件地址
                                                    so.addVariable("CuPlayerImage", "images/start.jpg"); //视频略缩图,本图片文件必须正确
                                                    so.addVariable("CuPlayerWidth", flvwidth); //视频宽度
                                                    so.addVariable("CuPlayerHeight", flvheight); //视频高度
                                                    so.addVariable("CuPlayerAutoPlay", "yes"); //是否自动播放
                                                    so.addVariable("CuPlayerLogo", "logo.png"); //Logo文件地址
                                                    so.addVariable("CuPlayerPosition", "bottom-right"); //Logo显示的位置
                                                    so.write("CuPlayer");

                                                }
                                            </script>
                                            <div id="videowen"></div>
                                        </div>
                                    </div>
                                    <p>
                                        <!--OPEN WIN-->
                                    </p>
                                </fieldset>
                                <%-- 其他附件 --%>
                                <fieldset>
                                    <legend>其它</legend>
                                    <div id="OthersDiv">
                                        <div id="OthersList" runat="server">

                                        </div>
                                    </div>
                                </fieldset>


                            </td>
                        </tr>


                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td class="titlebga" style="background-color: #6e5328" id="a06">
                                <div class="titlebgaa">作品点评</div>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px"></td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px; height: 20px; border-bottom: 1px dotted #555">
                                <div>
                                    <span style="color: #777">当前评分：</span>
                                    <span runat="server" id="DivScore">
                                        <img src="../../../../_layouts/15/WorksShow/images/star.png" /><img src="../../../../_layouts/15/WorksShow/images/star.png" /><img src="../../../../_layouts/15/WorksShow/images/star.png" /><img src="../../../../_layouts/15/WorksShow/images/star.png" /><img src="../../../../_layouts/15/WorksShow/images/star.png" /></span>&nbsp;
										<span style="color: #1a66b3">已有<asp:Label ID="LbPersons" runat="server" Text="1"></asp:Label>人评论</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px"></td>
                        </tr>

                        <tr>
                            <td style="padding-left: 20px">
                                <asp:GridView ID="GvComments" runat="server" AutoGenerateColumns="False" AllowPaging="True" BorderStyle="None" CssClass="grouptd" GridLines="None" AlternatingRowStyle-HorizontalAlign="Left" HorizontalAlign="Left">
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="点评者">
                                            <ItemStyle Width="100px" Wrap="False" CssClass="liuyan" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Comments" HeaderText="评论内容">
                                            <ItemStyle Width="500px" CssClass="neirong" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Score" HeaderText="评分">
                                            <ItemStyle Width="60px" Wrap="False" CssClass="fenshu" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Created" DataFormatString="{0:yyyy-MM-dd}" HeaderText="点评时间">
                                            <ItemStyle Width="100px" Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#494B4C" ForeColor="#E6E6E6" HorizontalAlign="Left" />

                                    <AlternatingRowStyle HorizontalAlign="Left" BackColor="#CCFFFF"></AlternatingRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px"></td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20; color: #555">
                                <div id="txtDiv" style="display: none">
                                    <div style="margin-bottom: 5px">
                                        <span style="color: #777">评分：</span>
                                        <span onmouseover="rate(this,event)">
                                            <img src="../../../../_layouts/15/WorksShow/images/star_gray.gif" /><img src="../../../../_layouts/15/WorksShow/images/star_gray.gif" /><img src="../../../../_layouts/15/WorksShow/images/star_gray.gif" /><img src="../../../../_layouts/15/WorksShow/images/star_gray.gif" /><img src="../../../../_layouts/15/WorksShow/images/star_gray.gif" /></span>
                                    </div>
                                    <div>
                                        评语：<br />
                                        <asp:TextBox ID="TxtComments" runat="server" Height="150px" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px"></td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20;">
                                <input type="button" value="我要点评" class="buttoncss" onclick="txtdiva()" id="inputbutton" />
                                <asp:Button ID="BtnSubmit" OnClientClick="return ValidComments()" runat="server" Text="发表" CssClass="buttoncssa" Style="display: none" BorderStyle="None" />

                                <asp:HiddenField ID="HiddenField1" Value="0" runat="server" />

                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20%;vertical-align:top;padding-left:10px;min-width:200px;">
                    <table style="width: 100%;display:none">
                        <tr>
                            <td style="border:1px solid #efefef;padding:10px;">
                                <h4>热门作品</h4><hr />
                                <dl>
                                    <dt><a href="#">作品1</a></dt>
                                    <dt><a href="#">作品2</a></dt>
                                </dl>
                            </td>
                        </tr>
                        <tr>
                            <td style="border:1px solid #efefef;padding:10px;">
                                <h4>最新作品</h4><hr />
                                <dl>
                                    <dt><a href="#">作品1</a></dt>
                                    <dt><a href="#">作品2</a></dt>
                                </dl>
                            </td>

                        </tr>
                        <tr>
                            <td style="border:1px solid #efefef;padding:10px;">
                                <h4>我的收藏</h4><hr />
                                <dl>
                                    <dt><a href="#">作品1</a></dt>
                                    <dt><a href="#">作品2</a></dt>
                                </dl>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

	</div>
	<div id="bottomNav" style="display:none">
		<div id="WorksNavDiv" runat="server">
			<div style="float: left; font-size: 14px; margin: 5px;">
				<asp:HyperLink ID="lnkPre" runat="server" CssClass="ms-core-suiteLink-a">上一作品：</asp:HyperLink>
			</div>
			<div style="float: right; font-size: 14px; margin: 5px;">
				<asp:HyperLink ID="lnkNext" runat="server" CssClass="ms-core-suiteLink-a">下一作品：</asp:HyperLink>
			</div>
		</div>
	</div>
	<div id="LeftLayer" runat="server" class="LeftLayer" visible="False">
	</div>
	<div id="RightLayer" runat="server" class="RightLayer" visible="False">
	</div>
	<div id="last"></div>

	<script type="text/javascript">
		$(function () {
			$(window).scroll(function () {

				var distanceTop = $('#last').offset().top - $(window).height();

				if ($(window).scrollTop() > distanceTop) { //判断是否滚动到页面底部
					$('#slidebox').animate({
						'marginLeft': '0px'
					}, 300);
				} else {
					$('#slidebox').stop(true).animate({
						'marginLeft': '-430px'
					}, 100);
				}

			});

			$('#slidebox .close').bind('click', function () { //关闭按钮
				$(this).parent("#slidebox").stop(true).animate({
					'marginLeft': '-430px'
				}, 100);
			});
		});
	</script>

	<div id="slidebox">
	</div>
</div>

<div id="divErr" runat="server"></div>
