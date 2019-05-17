<%@ Assembly Name="WorksShowDll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9242a88229ae4d9c" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Reference Page="ShowWorks.aspx" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="WorksShowDll.Inherits.WorksShow" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
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

	<link rel="stylesheet" href="gallery/css/iconfont.css" />
	<link rel="stylesheet" href="css/default.css" />
	<link rel="stylesheet prefetch" href="gallery/css/photoswipe.css" />
	<link rel="stylesheet prefetch" href="gallery/css/default-skin/default-skin.css" />


	<script type="text/javascript">
		function showframe(url) {
			var iframe = document.getElementById('myIframe'); //动态创建框架
			iframe.src = url; //框架中加载的页面      //将框架添加到当前窗体
		}
	</script>



	<script src="js/jquery-latest.js" type="text/javascript"></script>
	<script type="text/javascript">
		$.fn.ImgZoomIn = function() {
			bgstr = '<div id="ImgZoomInBG" style=" background:#000000; filter:Alpha(Opacity=70); opacity:0.7; position:fixed; left:0; top:0; z-index:10000; width:100%; height:100%; display:none;"><iframe src="about:blank" frameborder="5px" scrolling="yes" style="width:100%; height:100%;"></iframe></div>';
			//alert($(this).attr('src'));
			imgstr = '<img id="ImgZoomInImage" src="' + $(this).attr('src') + '" onclick=$(\'#ImgZoomInImage\').hide();$(\'#ImgZoomInBG\').hide(); style="cursor:pointer; display:none; position:absolute; z-index:10001;" />';
			if ($('#ImgZoomInBG').length < 1) {
				$('body').append(bgstr);
			}
			if ($('#ImgZoomInImage').length < 1) {
				$('body').append(imgstr);
			} else {
				$('#ImgZoomInImage').attr('src', $(this).attr('src'));
			}
			//alert($(window).scrollLeft());
			//alert( $(window).scrollTop());
			$('#ImgZoomInImage').css('left', $(window).scrollLeft() + ($(window).width() - $('#ImgZoomInImage').width()) / 2);
			$('#ImgZoomInImage').css('top', $(window).scrollTop() + ($(window).height() - $('#ImgZoomInImage').height()) / 2);
			$('#ImgZoomInBG').show();
			$('#ImgZoomInImage').show();
		};
		$(document).ready(function() {
			$("#imgTest").bind("click", function() {
				$(this).ImgZoomIn();
			});
		});
	</script>

	<%-- *****媒体播放引用的样式和脚本****** --%>
		<link type="text/css" rel="stylesheet" media="all" href="images/video.css" />
		<%--<link rel="stylesheet" type="text/css" href="common/common.css">--%>
			<%--<link rel="stylesheet" type="text/css" href="common/style.css">--%>
				<script src="common/jquery172.js" type="text/javascript"></script>
				<script src="common/slides.min.jquery.js" type="text/javascript"></script>
				<script src="common/cuplayerLight.js" type="text/javascript"></script>
				<%-- ******************************* --%>

					<%-- *****相关作品脚本引用开始***** --%>

						<link type="text/css" href="More/css/jquery.ui.theme.css" rel="stylesheet" />
						<link type="text/css" href="More/css/jquery.ui.core.css" rel="stylesheet" />
						<link type="text/css" href="More/css/jquery.ui.slider.css" rel="stylesheet" />
						<link rel="stylesheet" href="More/css/style.css" type="text/css" media="screen" />


						<%-- *****相关作品脚本引用结束***** --%>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<%-- <asp:TextBox ID="txtWorksId" runat="server"></asp:TextBox><asp:Button ID="btnSet" runat="server" Text="查看" />--%>
		<div id="floatposition">
			<div class="f" id="divrightnav">
				<ul>
					<li><a href="#a01">作品简介</a></li>
					<li><a href="#a02">安装说明</a></li>
					<li><a href="#a03">作品附件</a></li>
					<li><a href="#a04">设计思路</a></li>
					<li><a href="#a05">重点与难点</a></li>
					<li><a href="#a06">其他说明</a></li>
					<li><a href="#a07">作品点评</a></li>
					<li><a href="#a00" style="color:#32cd32;text-decoration:none;">回到顶部</a></li>
					<li></li>
				</ul>
			</div>

		</div>
		<div class="content" id="divWorksContent">
				<table cellpadding="2" cellspacing="0" style="font-size: 14px; width:800px; text-align:left">
					<tr>
						<td class="worktitle" valign="bottom" id="a00">
							<div class="h3css">作品：
								<asp:Label ID="LbWorksName" runat="server" Text="《愤怒的小鸟》炒热现场气氛"></asp:Label>
							</div>
							<div style="float: right;">
                                <div id="divCount" runat="server" visible="false">阅读：<asp:Label ID="lbReadCount" runat="server" Text="0"></asp:Label> 次 | 收藏:<asp:Label ID="lbFavCount" runat="server" Text="0"></asp:Label> 次</div>
							    <asp:Label ID="lbCount" runat="server" Text="阅读：10 次 | 收藏：7 次 " ></asp:Label>
								<asp:Button ID="BtnUnFav" runat="server" Text="已收藏 | 取消" ToolTip="取消收藏" CausesValidation="False" style="cursor: pointer; border: none;" Visible="false"/>
								<asp:ImageButton ID="BtnFav" runat="server" ImageUrl="images/favorite.png" ToolTip="收藏" CausesValidation="False" />
                                <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="images/delete.png" ToolTip="删除" CausesValidation="False" OnClientClick="javascript:return confirm('确定删除本作品吗?');"  Visible="false"/>
							</div>
						</td>
					</tr>
					<tr>
						<td height="10px">
						</td>
					</tr>
					<tr>
						<td class="titlea"><span class="titleaspan"><asp:Label ID="Label1" runat="server" Text="作品编号："></asp:Label></span>&nbsp;
							<asp:Label ID="LbWorksCode" runat="server" Text="A120622"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="titlea">
							<span class="titleaspan"><asp:Label ID="Label2" runat="server" Text="作品组别："></asp:Label></span>&nbsp;
							<asp:Label ID="LbWorksType" runat="server" Text="计算机动画、游戏"></asp:Label>
						</td>
					</tr>
					<%-- <tr><td class="titlea">
					<span  class="titleaspan"><asp:Label ID="Label3" runat="server" Text="演示地址："></asp:Label></span>&nbsp;<span style="color:#ff6633"><asp:Label ID="LbDemoUrl" runat="server"  Text="http://www.youku.asjkhdhk.com"></asp:Label></span>
				</td>
			</tr>--%>
						<%-- <tr><td class="titlea"><div runat="server" id="DivViewShow"><embed src="http://www.tudou.com/l/QACM1N6YOAI/&rpid=111905378&resourceId=111905378_05_05_99&iid=140062282/v.swf" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" wmode="opaque" width="480" height="400"></embed>
			</div>
			</td></tr>--%>
							<tr>
								<td height="20px">
								</td>
							</tr>

							<tr>
								<td class="titlea titlebga" id="a01">
									<div class="titlebg">作品简介</div>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="LbSubmitProfile" runat="server" CssClass="lblcss" Text="自去年下半年开始，我国土地价格并没出现明显的上涨，同时许多地方出现了土地流拍现象，地方财政不好过。针对土地价格下降，是否应该购房的问题，任志强表示，土地价格越是下降越需要买房。“因为房价下一轮一定是高涨，我们的规律是，下降趋势就意味着下一轮的高涨。”任志强说：“土地供应高的时候，价格就会下降。”自去年下半年开始，我国土地价格并没出现明显的上涨，同时许多地方出现了土地流拍现象，地方财政不好过。针对土地价格下降，是否应该购房的问题，任志强表示，土地价格越是下降越需要买房。“因为房价下一轮一定是高涨，我们的规律是，下降趋势就意味着下一轮的高涨。”任志强说：“土地供应高的时候，价格就会下降。” "></asp:Label>
								</td>
							</tr>
							<tr>
								<td height="10px">
								</td>
							</tr>
							<tr>
								<td class="titlebga" id="a02">
									<div class="titlebg">安装说明</div>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="LbInstallationGuide" runat="server" CssClass="lblcss" Text="但是，为何沈佳宜唯独愿意把心事与柯景腾分享呢？她对他究竟有没有别样的感觉呢？柯景腾暗恋沈佳宜八年最终能否修得正果呢？让我们走进《那些年，我们一起追的女孩》，一起来寻找那最纯真的感动吧！但是，为何沈佳宜唯独愿意把心事与柯景腾分享呢？她对他究竟有没有别样的感觉呢？柯景腾暗恋沈佳宜八年最终能否修得正果呢？让我们走进《那些年，我们一起追的女孩》，一起来寻找那最纯真的感动吧"></asp:Label>
								</td>
							</tr>
							<tr>
								<td height="10px">
								</td>
							</tr>
							<tr>
								<td class="titlebga" id="a03">
									<div class="titlebg">作品附件</div>

								</td>
							</tr>
							<tr>
								<td style=" padding:5px; color: #555;background-color:#EAEAEA">
									<div id="DocsDiv" runat="server">

									</div>
									<div id="ImagesDiv" runat="server">

									</div>
									<div id="MediaDiv" runat="server">
										<fieldset>
											<legend>媒体文件</legend>
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
									</div>
									<div id="OthersDiv" runat="server">

									</div>
								</td>
							</tr>

							<tr>
								<td height="10px"></td>
							</tr>

							<tr>
								<td class="titlebga" id="a04">
									<div class="titlebg"> 设计思路</div>
								</td>
							</tr>
							<tr>
								<td>
									<div id="DivDesignIdeas" class="lblcss" runat="server">柯景腾读国中时是一个成绩暴烂而且又调皮捣蛋的男生，老师将他“托付”给 图书封面班里最优秀的女生沈佳宜。只要他不认真学习，沈佳宜就会用圆珠笔戳他的衣服。在沈佳宜的监督和鼓励下，柯景腾的成绩就像芝麻开花节节高，渐渐地，他也喜欢上了气质优雅的沈佳宜。但是柯景腾却不敢向心爱的女生表白，因为几乎被所有男生喜欢的沈佳宜对追求她的男生一律有一种反感，她只想好好学习，不希望别人介入自己的生活。 但是，为何沈佳宜唯独愿意把心事与柯景腾分享呢？她对他究竟有没有别样的感觉呢？柯景腾暗恋沈佳宜八年最终能否修得正果呢？让我们走进《那些年，我们一起追的女孩》，一起来寻找那最纯真的感动吧
									</div>
								</td>
							</tr>

							<tr>
								<td height="10px"></td>
							</tr>

							<tr>
								<td class="titlebga" id="a05">
									<div class="titlebg" style="font-size:13px">设计重点\难点</div>
								</td>
							</tr>
							<tr>
								<td>
									<div id="DivKeyPoints" runat="server">柯景腾读国中时是一个成绩暴烂而且又调皮捣蛋的男生，老师将他“托付”给 图书封面班里最优秀的女生沈佳宜。只要他不认真学习，沈佳宜就会用圆珠笔戳他的衣服。在沈佳宜的监督和鼓励下，柯景腾的成绩就像芝麻开花节节高，渐渐地，他也喜欢上了气质优雅的沈佳宜。但是柯景腾却不敢向心爱的女生表白，因为几乎被所有男生喜欢的沈佳宜对追求她的男生一律有一种反感，她只想好好学习，不希望别人介入自己的生活。 但是，为何沈佳宜唯独愿意把心事与柯景腾分享呢？她对他究竟有没有别样的感觉呢？柯景腾暗恋沈佳宜八年最终能否修得正果呢？让我们走进《那些年，我们一起追的女孩》，一起来寻找那最纯真的感动吧
									</div>
								</td>
							</tr>

							<tr>
								<td height="10px"></td>
							</tr>

							<tr>
								<td class="titlebga" id="a06">
									<div class="titlebg">其他说明</div>
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="LbComment" runat="server" CssClass="lblcss" Text="柯景腾读国中时是一个成绩暴烂而且又调皮捣蛋的男生，老师将他“托付”给  图书封面班里最优秀的女生沈佳宜。只要他不认真学习，沈佳宜就会用圆珠笔戳他的衣服。在沈佳宜的监督和鼓励下，柯景腾的成绩就像芝麻开花节节高，渐渐地，他也喜欢上了气质优雅的沈佳宜。但是柯景腾却不敢向心爱的女生表白，因为几乎被所有男生喜欢的沈佳宜对追求她的男生一律有一种反感，她只想好好学习，不希望别人介入自己的生活。"></asp:Label>
								</td>
							</tr>

							<tr>
								<td height="10px">

								</td>
							</tr>
							<tr>
								<td class="titlebga" style="background-color:#6e5328" id="a07">
									<div class="titlebgaa">作品点评</div>
								</td>
							</tr>
							<tr>
								<td height="8px"></td>
							</tr>
							<tr>
								<td style=" padding-left:20; height:20px; border-bottom:1px dotted #555">
									<div>
										<span style="color:#777">当前评分：</span>
										<span runat="server" id="DivScore"><img src="images/star_red.gif" /><img src="images/star_red.gif" /><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /></span>&nbsp;
										<span style="color:#1a66b3">已有<asp:Label  id="LbPersons" runat="server" Text="1" ></asp:Label>人评论</span></div>
								</td>
							</tr>
							<tr>
								<td height="5px"></td>
							</tr>

							<tr>
								<td style="padding-left:20px">
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
								<td style=" padding-left:20;color:#555">
									<div id="txtDiv" style="display:none">
										<div style="margin-bottom:5px"><span style="color:#777">评分：</span>
											<span onmouseover="rate(this,event)"><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /><img src="images/star_gray.gif" /></span>
										</div>
										<div>评语：<br />
											<asp:TextBox ID="TxtComments" runat="server" Height="150px" Width="500px" TextMode="MultiLine"></asp:TextBox>
										</div>
									</div>
								</td>
							</tr>
							<tr>
								<td height="5px"></td>
							</tr>
							<tr>
								<td style=" padding-left:20;">
									<input type="button" value="我要点评" class="buttoncss" onclick="txtdiva()" id="inputbutton" />
									<asp:Button ID="BtnSubmit" OnClientClick="return ValidComments()" runat="server" Text="发表" CssClass="buttoncssa" style="display:none" BorderStyle="None" />

									<asp:HiddenField ID="HiddenField1" Value="0" runat="server" />

								</td>
							</tr>
							<tr>
								<td height="30px"></td>
							</tr>
				</table>
				<div id="SrDiv" runat="server" style="padding-bottom:50px">
				</div>

		</div>
		<div id="bottomNav">
			<div id="WorksNavDiv" runat="server">
				<div style="float:left;font-size:14px;margin:5px;">
					<asp:HyperLink ID="lnkPre" runat="server" CssClass="ms-core-suiteLink-a">上一作品：</asp:HyperLink>
				</div>
				<div style="float:right;font-size:14px;margin:5px;">
					<asp:HyperLink ID="lnkNext" runat="server" CssClass="ms-core-suiteLink-a">下一作品：</asp:HyperLink>
				</div>
			</div>
		</div>
		<div id="LeftLayer" runat="server" class="LeftLayer" Visible="False">

		</div>
		<div id="RightLayer" runat="server" class="RightLayer" Visible="False">

		</div>
		<div id="last"></div>
		<script type="text/javascript">
			$(function() {
				$(window).scroll(function() {

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

				$('#slidebox .close').bind('click', function() { //关闭按钮
					$(this).parent("#slidebox").stop(true).animate({
						'marginLeft': '-430px'
					}, 100);
				});
			});
		</script>
		<script type="text/javascript" src="http://code.jquery.com/jquery.min.js"></script>

		<div id="slidebox">

		</div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
	作品展示
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
	作品展示
</asp:Content>
