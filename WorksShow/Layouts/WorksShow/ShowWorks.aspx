<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="WorksShowDll.Inherits.ShowWorks" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

	<link rel="stylesheet" href="gallery/css/demo.css">
	<link rel="stylesheet" href="gallery/css/iconfont.css">
	<link rel="stylesheet prefetch" href="gallery/css/photoswipe.css">
	<link rel="stylesheet prefetch" href="gallery/css/default-skin/default-skin.css">


	
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
	<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
	<asp:Button ID="Button1" runat="server" Text="页面传值" OnClick="Button1_Click" />
	<div id="ImageDiv" runat="server">
		<%--<div id="_contain">
			<div class="contain" style="margin: 40px 0 0;">
				<div class="body">
					<div class="text">
						<!--data-pswp-uid 是每个gallery的id不能重复-->
						<div class="my-gallery" data-pswp-uid="2">
						
						</div>
					</div>
				</div>
			</div>
		</div>--%>
	</div>
	<script src="gallery/js/jquery.min.js"></script>
	<script src="gallery/js/photoswipe.js"></script>
	<script src="gallery/js/photoswipe-ui-default.min.js"></script>
	<script type="text/javascript">
		var initPhotoSwipeFromDOM = function(gallerySelector) {

			// 解析来自DOM元素幻灯片数据（URL，标题，大小...）
			// (children of gallerySelector)
			var parseThumbnailElements = function(el) {
				var thumbElements = el.childNodes,
					numNodes = thumbElements.length,
					items = [],
					figureEl,
					linkEl,
					size,
					item,
					divEl;

				for (var i = 0; i < numNodes; i++) {

					figureEl = thumbElements[i]; // <figure> element

					// 仅包括元素节点
					if (figureEl.nodeType !== 1) {
						continue;
					}
					divEl = figureEl.children[0];
					linkEl = divEl.children[0]; // <a> element

					size = linkEl.getAttribute('data-size').split('x');

					// 创建幻灯片对象
					item = {
						src: linkEl.getAttribute('href'),
						w: parseInt(size[0], 10),
						h: parseInt(size[1], 10)
					};



					if (figureEl.children.length > 1) {
						// <figcaption> content
						item.title = figureEl.children[1].innerHTML;
					}

					if (linkEl.children.length > 0) {
						// <img> 缩略图节点, 检索缩略图网址
						item.msrc = linkEl.children[0].getAttribute('src');
					}

					item.el = figureEl; // 保存链接元素 for getThumbBoundsFn
					items.push(item);
				}

				return items;
			};

			// 查找最近的父节点
			var closest = function closest(el, fn) {
				return el && (fn(el) ? el : closest(el.parentNode, fn));
			};

			// 当用户点击缩略图触发
			var onThumbnailsClick = function(e) {
				e = e || window.event;
				e.preventDefault ? e.preventDefault() : e.returnValue = false;

				var eTarget = e.target || e.srcElement;

				// find root element of slide
				var clickedListItem = closest(eTarget, function(el) {
					return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
				});

				if (!clickedListItem) {
					return;
				}

				// find index of clicked item by looping through all child nodes
				// alternatively, you may define index via data- attribute
				var clickedGallery = clickedListItem.parentNode,
					childNodes = clickedListItem.parentNode.childNodes,
					numChildNodes = childNodes.length,
					nodeIndex = 0,
					index;

				for (var i = 0; i < numChildNodes; i++) {
					if (childNodes[i].nodeType !== 1) {
						continue;
					}

					if (childNodes[i] === clickedListItem) {
						index = nodeIndex;
						break;
					}
					nodeIndex++;
				}



				if (index >= 0) {
					// open PhotoSwipe if valid index found
					openPhotoSwipe(index, clickedGallery);
				}
				return false;
			};

			// parse picture index and gallery index from URL (#&pid=1&gid=2)
			var photoswipeParseHash = function() {
				var hash = window.location.hash.substring(1),
					params = {};

				if (hash.length < 5) {
					return params;
				}

				var vars = hash.split('&');
				for (var i = 0; i < vars.length; i++) {
					if (!vars[i]) {
						continue;
					}
					var pair = vars[i].split('=');
					if (pair.length < 2) {
						continue;
					}
					params[pair[0]] = pair[1];
				}

				if (params.gid) {
					params.gid = parseInt(params.gid, 10);
				}

				return params;
			};

			var openPhotoSwipe = function(index, galleryElement, disableAnimation, fromURL) {
				var pswpElement = document.querySelectorAll('.pswp')[0],
					gallery,
					options,
					items;

				items = parseThumbnailElements(galleryElement);

				// 这里可以定义参数
				options = {
					barsSize: {
						top: 100,
						bottom: 100
					},
					fullscreenEl: false,
					shareButtons: [{
						id: 'wechat',
						label: '分享微信',
						url: '#'
					}, {
						id: 'weibo',
						label: '新浪微博',
						url: '#'
					}, {
						id: 'download',
						label: '保存图片',
						url: '{{raw_image_url}}',
						download: true
					}],

					// define gallery index (for URL)
					galleryUID: galleryElement.getAttribute('data-pswp-uid'),

					getThumbBoundsFn: function(index) {
						// See Options -> getThumbBoundsFn section of documentation for more info
						var thumbnail = items[index].el.getElementsByTagName('img')[0], // find thumbnail
							pageYScroll = window.pageYOffset || document.documentElement.scrollTop,
							rect = thumbnail.getBoundingClientRect();

						return {
							x: rect.left,
							y: rect.top + pageYScroll,
							w: rect.width
						};
					}

				};

				// PhotoSwipe opened from URL
				if (fromURL) {
					if (options.galleryPIDs) {
						// parse real index when custom PIDs are used 
						for (var j = 0; j < items.length; j++) {
							if (items[j].pid == index) {
								options.index = j;
								break;
							}
						}
					} else {
						// in URL indexes start from 1
						options.index = parseInt(index, 10) - 1;
					}
				} else {
					options.index = parseInt(index, 10);
				}

				// exit if index not found
				if (isNaN(options.index)) {
					return;
				}

				if (disableAnimation) {
					options.showAnimationDuration = 0;
				}

				// Pass data to PhotoSwipe and initialize it
				gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
				gallery.init();
			};

			// loop through all gallery elements and bind events
			var galleryElements = document.querySelectorAll(gallerySelector);

			for (var i = 0, l = galleryElements.length; i < l; i++) {
				galleryElements[i].setAttribute('data-pswp-uid', i + 1);
				galleryElements[i].onclick = onThumbnailsClick;
			}

			// Parse URL and open gallery if it contains #&pid=3&gid=1
			var hashData = photoswipeParseHash();
			if (hashData.pid && hashData.gid) {
				openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
			}
		};

		// execute above function
		initPhotoSwipeFromDOM('.my-gallery');


		$(".my-gallery>figure>div").each(function() {
			$(this).height($(this).width());
		});

		function more(obj, id) {
			if ($('#txt' + id).is(":hidden")) {
				$('#p' + id).hide();
				$('#txt' + id).show();
				obj.innerHTML = '收起';
			} else {
				$('#p' + id).show();
				$('#txt' + id).hide();
				obj.innerHTML = '全文';
			}
		}
	</script>
	<!-- Root element of PhotoSwipe. Must have class pswp. -->
	<div class="pswp" tabindex="-1" role="dialog" aria-hidden="true">

		<!-- Background of PhotoSwipe. 
		 It's a separate element as animating opacity is faster than rgba(). -->
		<div class="pswp__bg"></div>

		<!-- Slides wrapper with overflow:hidden. -->
		<div class="pswp__scroll-wrap">

			<!-- Container that holds slides. 
			PhotoSwipe keeps only 3 of them in the DOM to save memory.
			Don't modify these 3 pswp__item elements, data is added later on. -->
			<div class="pswp__container">
				<div class="pswp__item"></div>
				<div class="pswp__item"></div>
				<div class="pswp__item"></div>
			</div>

			<!-- Default (PhotoSwipeUI_Default) interface on top of sliding area. Can be changed. -->
			<div class="pswp__ui pswp__ui--hidden">

				<div class="pswp__top-bar">

					<!--  Controls are self-explanatory. Order can be changed. -->

					<div class="pswp__counter"></div>

					<button class="pswp__button pswp__button--close" title="关闭（退出）"></button>

					<button class="pswp__button pswp__button--share" title="分享"></button>

					<button class="pswp__button pswp__button--fs" title="全屏查看"></button>

					<button class="pswp__button pswp__button--zoom" title="放大/还原"></button>

					<!-- element will get class pswp__preloader--active when preloader is running -->
					<div class="pswp__preloader">
						<div class="pswp__preloader__icn">
							<div class="pswp__preloader__cut">
								<div class="pswp__preloader__donut"></div>
							</div>
						</div>
					</div>
				</div>

				<div class="pswp__share-modal pswp__share-modal--hidden pswp__single-tap">
					<div class="pswp__share-tooltip"></div>
				</div>

				<button class="pswp__button pswp__button--arrow--left" title="上一张 ( ← )">
			</button>

				<button class="pswp__button pswp__button--arrow--right" title="下一张 ( → )">
			</button>

				<div class="pswp__caption">
					<div class="pswp__caption__center"></div>
				</div>

			</div>

		</div>

	</div>


</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
作品展示
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
作品展示
</asp:Content>
