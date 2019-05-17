/*! PhotoSwipe - v4.1.1 - 2015-12-24
* http://photoswipe.com
* Copyright (c) 2015 Dmitry Semenov; */
(function (root, factory) { 
	if (typeof define === 'function' && define.amd) {
		define(factory);
	} else if (typeof exports === 'object') {
		module.exports = factory();
	} else {
		root.PhotoSwipe = factory();
	}
})(this, function () {

	'use strict';
	var photoSwipe = function(template, uiClass, items, options){

/*>>framework-bridge*/
/**
 *
 * Set of generic functions used by gallery.
 * 
 * You're free to modify anything here as long as functionality is kept.
 * 
 */
var framework = {
	features: null,
	bind: function(target, type, listener, unbind) {
		var methodName = (unbind ? 'remove' : 'add') + 'EventListener';
		type = type.split(' ');
		for(var i = 0; i < type.length; i++) {
			if(type[i]) {
				target[methodName]( type[i], listener, false);
			}
		}
	},
	isArray: function(obj) {
		return (obj instanceof Array);
	},
	createEl: function(classes, tag) {
		var el = document.createElement(tag || 'div');
		if(classes) {
			el.className = classes;
		}
		return el;
	},
	getScrollY: function() {
		var yOffset = window.pageYOffset;
		return yOffset !== undefined ? yOffset : document.documentElement.scrollTop;
	},
	unbind: function(target, type, listener) {
		framework.bind(target,type,listener,true);
	},
	removeClass: function(el, className) {
		var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
		el.className = el.className.replace(reg, ' ').replace(/^\s\s*/, '').replace(/\s\s*$/, ''); 
	},
	addClass: function(el, className) {
		if( !framework.hasClass(el,className) ) {
			el.className += (el.className ? ' ' : '') + className;
		}
	},
	hasClass: function(el, className) {
		return el.className && new RegExp('(^|\\s)' + className + '(\\s|$)').test(el.className);
	},
	getChildByClass: function(parentEl, childClassName) {
		var node = parentEl.firstChild;
		while(node) {
			if( framework.hasClass(node, childClassName) ) {
				return node;
			}
			node = node.nextSibling;
		}
	},
	arraySearch: function(array, value, key) {
		var i = array.length;
		while(i--) {
			if(array[i][key] === value) {
				return i;
			} 
		}
		return -1;
	},
	extend: function(o1, o2, preventOverwrite) {
		for (var prop in o2) {
			if (o2.hasOwnProperty(prop)) {
				if(preventOverwrite && o1.hasOwnProperty(prop)) {
					continue;
				}
				o1[prop] = o2[prop];
			}
		}
	},
	easing: {
		sine: {
			out: function(k) {
				return Math.sin(k * (Math.PI / 2));
			},
			inOut: function(k) {
				return - (Math.cos(Math.PI * k) - 1) / 2;
			}
		},
		cubic: {
			out: function(k) {
				return --k * k * k + 1;
			}
		}
		/*
			elastic: {
				out: function ( k ) {

					var s, a = 0.1, p = 0.4;
					if ( k === 0 ) return 0;
					if ( k === 1 ) return 1;
					if ( !a || a < 1 ) { a = 1; s = p / 4; }
					else s = p * Math.asin( 1 / a ) / ( 2 * Math.PI );
					return ( a * Math.pow( 2, - 10 * k) * Math.sin( ( k - s ) * ( 2 * Math.PI ) / p ) + 1 );

				},
			},
			back: {
				out: function ( k ) {
					var s = 1.70158;
					return --k * k * ( ( s + 1 ) * k + s ) + 1;
				}
			}
		*/
	},

	/**
	 * 
	 * @return {object}
	 * 
	 * {
	 *  raf : request animation frame function
	 *  caf : cancel animation frame function
	 *  transfrom : transform property key (with vendor), or null if not supported
	 *  oldIE : IE8 or below
	 * }
	 * 
	 */
	detectFeatures: function() {
		if(framework.features) {
			return framework.features;
		}
		var helperEl = framework.createEl(),
			helperStyle = helperEl.style,
			vendor = '',
			features = {};

		// IE8 and below
		features.oldIE = document.all && !document.addEventListener;

		features.touch = 'ontouchstart' in window;

		if(window.requestAnimationFrame) {
			features.raf = window.requestAnimationFrame;
			features.caf = window.cancelAnimationFrame;
		}

		features.pointerEvent = navigator.pointerEnabled || navigator.msPointerEnabled;

		// fix false-positive detection of old Android in new IE
		// (IE11 ua string contains "Android 4.0")
		
		if(!features.pointerEvent) { 

			var ua = navigator.userAgent;

			// Detect if device is iPhone or iPod and if it's older than iOS 8
			// http://stackoverflow.com/a/14223920
			// 
			// This detection is made because of buggy top/bottom toolbars
			// that don't trigger window.resize event.
			// For more info refer to _isFixedPosition variable in core.js

			if (/iP(hone|od)/.test(navigator.platform)) {
				var v = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
				if(v && v.length > 0) {
					v = parseInt(v[1], 10);
					if(v >= 1 && v < 8 ) {
						features.isOldIOSPhone = true;
					}
				}
			}

			// Detect old Android (before KitKat)
			// due to bugs related to position:fixed
			// http://stackoverflow.com/questions/7184573/pick-up-the-android-version-in-the-browser-by-javascript
			
			var match = ua.match(/Android\s([0-9\.]*)/);
			var androidversion =  match ? match[1] : 0;
			androidversion = parseFloat(androidversion);
			if(androidversion >= 1 ) {
				if(androidversion < 4.4) {
					features.isOldAndroid = true; // for fixed position bug & performance
				}
				features.androidVersion = androidversion; // for touchend bug
			}	
			features.isMobileOpera = /opera mini|opera mobi/i.test(ua);

			// p.s. yes, yes, UA sniffing is bad, propose your solution for above bugs.
		}
		
		var styleChecks = ['transform', 'perspective', 'animationName'],
			vendors = ['', 'webkit','Moz','ms','O'],
			styleCheckItem,
			styleName;

		for(var i = 0; i < 4; i++) {
			vendor = vendors[i];

			for(var a = 0; a < 3; a++) {
				styleCheckItem = styleChecks[a];

				// uppercase first letter of property name, if vendor is present
				styleName = vendor + (vendor ? 
										styleCheckItem.charAt(0).toUpperCase() + styleCheckItem.slice(1) : 
										styleCheckItem);
			
				if(!features[styleCheckItem] && styleName in helperStyle ) {
					features[styleCheckItem] = styleName;
				}
			}

			if(vendor && !features.raf) {
				vendor = vendor.toLowerCase();
				features.raf = window[vendor+'RequestAnimationFrame'];
				if(features.raf) {
					features.caf = window[vendor+'CancelAnimationFrame'] || 
									window[vendor+'CancelRequestAnimationFrame'];
				}
			}
		}
			
		if(!features.raf) {
			var lastTime = 0;
			features.raf = function(fn) {
				var currTime = new Date().getTime();
				var timeToCall = Math.max(0, 16 - (currTime - lastTime));
				var id = window.setTimeout(function() { fn(currTime + timeToCall); }, timeToCall);
				lastTime = currTime + timeToCall;
				return id;
			};
			features.caf = function(id) { clearTimeout(id); };
		}

		// Detect SVG support
		features.svg = !!document.createElementNS && 
						!!document.createElementNS('http://www.w3.org/2000/svg', 'svg').createSVGRect;

		framework.features = features;

		return features;
	}
};

framework.detectFeatures();

// Override addEventListener for old versions of IE
if(framework.features.oldIE) {

	framework.bind = function(target, type, listener, unbind) {
		
		type = type.split(' ');

		var methodName = (unbind ? 'detach' : 'attach') + 'Event',
			evName,
			handleEv = function() {
				listener.handleEvent.call(listener);
			};

		for(var i = 0; i < type.length; i++) {
			evName = type[i];
			if(evName) {

				if(typeof listener === 'object' && listener.handleEvent) {
					if(!unbind) {
						listener['oldIE' + evName] = handleEv;
					} else {
						if(!listener['oldIE' + evName]) {
							return false;
						}
					}

					target[methodName]( 'on' + evName, listener['oldIE' + evName]);
				} else {
					target[methodName]( 'on' + evName, listener);
				}

			}
		}
	};
	
}

/*>>framework-bridge*/

/*>>core*/
//function(template, UiClass, items, options)

var self = this;

/**
 * Static vars, don't change unless you know what you're doing.
 */
var doubleTapRadius = 25, 
	numHolders = 3;

/**
 * Options
 */
var _options = {
	allowPanToNext:true,
	spacing: 0.12,
	bgOpacity: 1,
	mouseUsed: false,
	loop: true,
	pinchToClose: true,
	closeOnScroll: true,
	closeOnVerticalDrag: true,
	verticalDragRange: 0.75,
	hideAnimationDuration: 333,
	showAnimationDuration: 333,
	showHideOpacity: false,
	focus: true,
	escKey: true,
	arrowKeys: true,
	mainScrollEndFriction: 0.35,
	panEndFriction: 0.35,
	isClickableElement: function(el) {
		return el.tagName === 'A';
	},
	getDoubleTapZoom: function(isMouseClick, item) {
		if(isMouseClick) {
			return 1;
		} else {
			return item.initialZoomLevel < 0.7 ? 1 : 1.33;
		}
	},
	maxSpreadZoom: 1.33,
	modal: true,

	// not fully implemented yet
	scaleMode: 'fit' // TODO
};
framework.extend(_options, options);


/**
 * Private helper variables & functions
 */

var getEmptyPoint = function() { 
		return {x:0,y:0}; 
	};

var isOpen,
	isDestroying,
	closedByScroll,
	currentItemIndex,
	containerStyle,
	containerShiftIndex,
	currPanDist = getEmptyPoint(),
	startPanOffset = getEmptyPoint(),
	panOffset = getEmptyPoint(),
	upMoveEvents, // drag move, drag end & drag cancel events array
	downEvents, // drag start events array
	globalEventHandlers,
	_viewportSize = {},
	currZoomLevel,
	startZoomLevel,
	translatePrefix,
	translateSufix,
	updateSizeInterval,
	itemsNeedUpdate,
	currPositionIndex = 0,
	offset = {},
	slideSize = getEmptyPoint(), // size of slide area, including spacing
	itemHolders,
	prevItemIndex,
	indexDiff = 0, // difference of indexes since last content update
	dragStartEvent,
	dragMoveEvent,
	dragEndEvent,
	dragCancelEvent,
	transformKey,
	pointerEventEnabled,
	isFixedPosition = true,
	likelyTouchDevice,
	modules = [],
	requestAf,
	cancelAf,
	initalClassName,
	initalWindowScrollY,
	oldIe,
	currentWindowScrollY,
	_features,
	windowVisibleSize = {},
	renderMaxResolution = false,

	// Registers PhotoSWipe module (History, Controller ...)
	registerModule = function(name, module) {
		framework.extend(self, module.publicMethods);
		modules.push(name);
	},

	getLoopedId = function(index) {
		var numSlides = getNumItems();
		if(index > numSlides - 1) {
			return index - numSlides;
		} else  if(index < 0) {
			return numSlides + index;
		}
		return index;
	},
	
	// Micro bind/trigger
	_listeners = {},
	listen = function(name, fn) {
		if(!_listeners[name]) {
			_listeners[name] = [];
		}
		return _listeners[name].push(fn);
	},
	shout = function(name) {
		var listeners = _listeners[name];

		if(listeners) {
			var args = Array.prototype.slice.call(arguments);
			args.shift();

			for(var i = 0; i < listeners.length; i++) {
				listeners[i].apply(self, args);
			}
		}
	},

	getCurrentTime = function() {
		return new Date().getTime();
	},
	applyBgOpacity = function(opacity) {
		bgOpacity = opacity;
		self.bg.style.opacity = opacity * _options.bgOpacity;
	},

	applyZoomTransform = function(styleObj,x,y,zoom,item) {
		if(!renderMaxResolution || (item && item !== self.currItem) ) {
			zoom = zoom / (item ? item.fitRatio : self.currItem.fitRatio);	
		}
			
		styleObj[transformKey] = translatePrefix + x + 'px, ' + y + 'px' + translateSufix + ' scale(' + zoom + ')';
	},
	applyCurrentZoomPan = function( allowRenderResolution ) {
		if(currZoomElementStyle) {

			if(allowRenderResolution) {
				if(currZoomLevel > self.currItem.fitRatio) {
					if(!renderMaxResolution) {
						setImageSize(self.currItem, false, true);
						renderMaxResolution = true;
					}
				} else {
					if(renderMaxResolution) {
						setImageSize(self.currItem);
						renderMaxResolution = false;
					}
				}
			}
			

			applyZoomTransform(currZoomElementStyle, panOffset.x, panOffset.y, currZoomLevel);
		}
	},
	applyZoomPanToItem = function(item) {
		if(item.container) {

			applyZoomTransform(item.container.style, 
								item.initialPosition.x, 
								item.initialPosition.y, 
								item.initialZoomLevel,
								item);
		}
	},
	setTranslateX = function(x, elStyle) {
		elStyle[transformKey] = translatePrefix + x + 'px, 0px' + translateSufix;
	},
	moveMainScroll = function(x, dragging) {

		if(!_options.loop && dragging) {
			var newSlideIndexOffset = currentItemIndex + (slideSize.x * currPositionIndex - x) / slideSize.x,
				delta = Math.round(x - mainScrollPos.x);

			if( (newSlideIndexOffset < 0 && delta > 0) || 
				(newSlideIndexOffset >= getNumItems() - 1 && delta < 0) ) {
				x = mainScrollPos.x + delta * _options.mainScrollEndFriction;
			} 
		}
		
		mainScrollPos.x = x;
		setTranslateX(x, containerStyle);
	},
	calculatePanOffset = function(axis, zoomLevel) {
		var m = midZoomPoint[axis] - offset[axis];
		return startPanOffset[axis] + currPanDist[axis] + m - m * ( zoomLevel / startZoomLevel );
	},
	
	equalizePoints = function(p1, p2) {
		p1.x = p2.x;
		p1.y = p2.y;
		if(p2.id) {
			p1.id = p2.id;
		}
	},
	roundPoint = function(p) {
		p.x = Math.round(p.x);
		p.y = Math.round(p.y);
	},

	mouseMoveTimeout = null,
	onFirstMouseMove = function() {
		// Wait until mouse move event is fired at least twice during 100ms
		// We do this, because some mobile browsers trigger it on touchstart
		if(mouseMoveTimeout ) { 
			framework.unbind(document, 'mousemove', onFirstMouseMove);
			framework.addClass(template, 'pswp--has_mouse');
			_options.mouseUsed = true;
			shout('mouseUsed');
		}
		mouseMoveTimeout = setTimeout(function() {
			mouseMoveTimeout = null;
		}, 100);
	},

	bindEvents = function() {
		framework.bind(document, 'keydown', self);

		if(_features.transform) {
			// don't bind click event in browsers that don't support transform (mostly IE8)
			framework.bind(self.scrollWrap, 'click', self);
		}
		

		if(!_options.mouseUsed) {
			framework.bind(document, 'mousemove', onFirstMouseMove);
		}

		framework.bind(window, 'resize scroll', self);

		shout('bindEvents');
	},

	unbindEvents = function() {
		framework.unbind(window, 'resize', self);
		framework.unbind(window, 'scroll', globalEventHandlers.scroll);
		framework.unbind(document, 'keydown', self);
		framework.unbind(document, 'mousemove', onFirstMouseMove);

		if(_features.transform) {
			framework.unbind(self.scrollWrap, 'click', self);
		}

		if(isDragging) {
			framework.unbind(window, upMoveEvents, self);
		}

		shout('unbindEvents');
	},
	
	calculatePanBounds = function(zoomLevel, update) {
		var bounds = calculateItemSize( self.currItem, _viewportSize, zoomLevel );
		if(update) {
			currPanBounds = bounds;
		}
		return bounds;
	},
	
	getMinZoomLevel = function(item) {
		if(!item) {
			item = self.currItem;
		}
		return item.initialZoomLevel;
	},
	getMaxZoomLevel = function(item) {
		if(!item) {
			item = self.currItem;
		}
		return item.w > 0 ? _options.maxSpreadZoom : 1;
	},

	// Return true if offset is out of the bounds
	modifyDestPanOffset = function(axis, destPanBounds, destPanOffset, destZoomLevel) {
		if(destZoomLevel === self.currItem.initialZoomLevel) {
			destPanOffset[axis] = self.currItem.initialPosition[axis];
			return true;
		} else {
			destPanOffset[axis] = calculatePanOffset(axis, destZoomLevel); 

			if(destPanOffset[axis] > destPanBounds.min[axis]) {
				destPanOffset[axis] = destPanBounds.min[axis];
				return true;
			} else if(destPanOffset[axis] < destPanBounds.max[axis] ) {
				destPanOffset[axis] = destPanBounds.max[axis];
				return true;
			}
		}
		return false;
	},

	setupTransforms = function() {

		if(transformKey) {
			// setup 3d transforms
			var allow3DTransform = _features.perspective && !likelyTouchDevice;
			translatePrefix = 'translate' + (allow3DTransform ? '3d(' : '(');
			translateSufix = _features.perspective ? ', 0px)' : ')';	
			return;
		}

		// Override zoom/pan/move functions in case old browser is used (most likely IE)
		// (so they use left/top/width/height, instead of CSS transform)
	
		transformKey = 'left';
		framework.addClass(template, 'pswp--ie');

		setTranslateX = function(x, elStyle) {
			elStyle.left = x + 'px';
		};
		applyZoomPanToItem = function(item) {

			var zoomRatio = item.fitRatio > 1 ? 1 : item.fitRatio,
				s = item.container.style,
				w = zoomRatio * item.w,
				h = zoomRatio * item.h;

			s.width = w + 'px';
			s.height = h + 'px';
			s.left = item.initialPosition.x + 'px';
			s.top = item.initialPosition.y + 'px';

		};
		applyCurrentZoomPan = function() {
			if(currZoomElementStyle) {

				var s = currZoomElementStyle,
					item = self.currItem,
					zoomRatio = item.fitRatio > 1 ? 1 : item.fitRatio,
					w = zoomRatio * item.w,
					h = zoomRatio * item.h;

				s.width = w + 'px';
				s.height = h + 'px';


				s.left = panOffset.x + 'px';
				s.top = panOffset.y + 'px';
			}
			
		};
	},

	onKeyDown = function(e) {
		var keydownAction = '';
		if(_options.escKey && e.keyCode === 27) { 
			keydownAction = 'close';
		} else if(_options.arrowKeys) {
			if(e.keyCode === 37) {
				keydownAction = 'prev';
			} else if(e.keyCode === 39) { 
				keydownAction = 'next';
			}
		}

		if(keydownAction) {
			// don't do anything if special key pressed to prevent from overriding default browser actions
			// e.g. in Chrome on Mac cmd+arrow-left returns to previous page
			if( !e.ctrlKey && !e.altKey && !e.shiftKey && !e.metaKey ) {
				if(e.preventDefault) {
					e.preventDefault();
				} else {
					e.returnValue = false;
				} 
				self[keydownAction]();
			}
		}
	},

	onGlobalClick = function(e) {
		if(!e) {
			return;
		}

		// don't allow click event to pass through when triggering after drag or some other gesture
		if(moved || zoomStarted || mainScrollAnimating || verticalDragInitiated) {
			e.preventDefault();
			e.stopPropagation();
		}
	},

	updatePageScrollOffset = function() {
		self.setScrollOffset(0, framework.getScrollY());		
	};
	


	



// Micro animation engine
var animations = {},
	numAnimations = 0,
	stopAnimation = function(name) {
		if(animations[name]) {
			if(animations[name].raf) {
				cancelAf( animations[name].raf );
			}
			numAnimations--;
			delete animations[name];
		}
	},
	registerStartAnimation = function(name) {
		if(animations[name]) {
			stopAnimation(name);
		}
		if(!animations[name]) {
			numAnimations++;
			animations[name] = {};
		}
	},
	stopAllAnimations = function() {
		for (var prop in animations) {

			if( animations.hasOwnProperty( prop ) ) {
				stopAnimation(prop);
			} 
			
		}
	},
	animateProp = function(name, b, endProp, d, easingFn, onUpdate, onComplete) {
		var startAnimTime = getCurrentTime(), t;
		registerStartAnimation(name);

		var animloop = function(){
			if ( animations[name] ) {
				
				t = getCurrentTime() - startAnimTime; // time diff
				//b - beginning (start prop)
				//d - anim duration

				if ( t >= d ) {
					stopAnimation(name);
					onUpdate(endProp);
					if(onComplete) {
						onComplete();
					}
					return;
				}
				onUpdate( (endProp - b) * easingFn(t/d) + b );

				animations[name].raf = requestAf(animloop);
			}
		};
		animloop();
	};
	


var publicMethods = {

	// make a few local variables and functions public
	shout: shout,
	listen: listen,
	viewportSize: _viewportSize,
	options: _options,

	isMainScrollAnimating: function() {
		return mainScrollAnimating;
	},
	getZoomLevel: function() {
		return currZoomLevel;
	},
	getCurrentIndex: function() {
		return currentItemIndex;
	},
	isDragging: function() {
		return isDragging;
	},	
	isZooming: function() {
		return isZooming;
	},
	setScrollOffset: function(x,y) {
		offset.x = x;
		currentWindowScrollY = offset.y = y;
		shout('updateScrollOffset', offset);
	},
	applyZoomPan: function(zoomLevel,panX,panY,allowRenderResolution) {
		panOffset.x = panX;
		panOffset.y = panY;
		currZoomLevel = zoomLevel;
		applyCurrentZoomPan( allowRenderResolution );
	},

	init: function() {

		if(isOpen || isDestroying) {
			return;
		}

		var i;

		self.framework = framework; // basic functionality
		self.template = template; // root DOM element of PhotoSwipe
		self.bg = framework.getChildByClass(template, 'pswp__bg');

		initalClassName = template.className;
		isOpen = true;
				
		_features = framework.detectFeatures();
		requestAf = _features.raf;
		cancelAf = _features.caf;
		transformKey = _features.transform;
		oldIe = _features.oldIE;
		
		self.scrollWrap = framework.getChildByClass(template, 'pswp__scroll-wrap');
		self.container = framework.getChildByClass(self.scrollWrap, 'pswp__container');

		containerStyle = self.container.style; // for fast access

		// Objects that hold slides (there are only 3 in DOM)
		self.itemHolders = itemHolders = [
			{el:self.container.children[0] , wrap:0, index: -1},
			{el:self.container.children[1] , wrap:0, index: -1},
			{el:self.container.children[2] , wrap:0, index: -1}
		];

		// hide nearby item holders until initial zoom animation finishes (to avoid extra Paints)
		itemHolders[0].el.style.display = itemHolders[2].el.style.display = 'none';

		setupTransforms();

		// Setup global events
		globalEventHandlers = {
			resize: self.updateSize,
			scroll: updatePageScrollOffset,
			keydown: onKeyDown,
			click: onGlobalClick
		};

		// disable show/hide effects on old browsers that don't support CSS animations or transforms, 
		// old IOS, Android and Opera mobile. Blackberry seems to work fine, even older models.
		var oldPhone = _features.isOldIOSPhone || _features.isOldAndroid || _features.isMobileOpera;
		if(!_features.animationName || !_features.transform || oldPhone) {
			_options.showAnimationDuration = _options.hideAnimationDuration = 0;
		}

		// init modules
		for(i = 0; i < modules.length; i++) {
			self['init' + modules[i]]();
		}
		
		// init
		if(uiClass) {
			var ui = self.ui = new uiClass(self, framework);
			ui.init();
		}

		shout('firstUpdate');
		currentItemIndex = currentItemIndex || _options.index || 0;
		// validate index
		if( isNaN(currentItemIndex) || currentItemIndex < 0 || currentItemIndex >= getNumItems() ) {
			currentItemIndex = 0;
		}
		self.currItem = getItemAt( currentItemIndex );

		
		if(_features.isOldIOSPhone || _features.isOldAndroid) {
			isFixedPosition = false;
		}
		
		template.setAttribute('aria-hidden', 'false');
		if(_options.modal) {
			if(!isFixedPosition) {
				template.style.position = 'absolute';
				template.style.top = framework.getScrollY() + 'px';
			} else {
				template.style.position = 'fixed';
			}
		}

		if(currentWindowScrollY === undefined) {
			shout('initialLayout');
			currentWindowScrollY = initalWindowScrollY = framework.getScrollY();
		}
		
		// add classes to root element of PhotoSwipe
		var rootClasses = 'pswp--open ';
		if(_options.mainClass) {
			rootClasses += _options.mainClass + ' ';
		}
		if(_options.showHideOpacity) {
			rootClasses += 'pswp--animate_opacity ';
		}
		rootClasses += likelyTouchDevice ? 'pswp--touch' : 'pswp--notouch';
		rootClasses += _features.animationName ? ' pswp--css_animation' : '';
		rootClasses += _features.svg ? ' pswp--svg' : '';
		framework.addClass(template, rootClasses);

		self.updateSize();

		// initial update
		containerShiftIndex = -1;
		indexDiff = null;
		for(i = 0; i < numHolders; i++) {
			setTranslateX( (i+containerShiftIndex) * slideSize.x, itemHolders[i].el.style);
		}

		if(!oldIe) {
			framework.bind(self.scrollWrap, downEvents, self); // no dragging for old IE
		}	

		listen('initialZoomInEnd', function() {
			self.setContent(itemHolders[0], currentItemIndex-1);
			self.setContent(itemHolders[2], currentItemIndex+1);

			itemHolders[0].el.style.display = itemHolders[2].el.style.display = 'block';

			if(_options.focus) {
				// focus causes layout, 
				// which causes lag during the animation, 
				// that's why we delay it untill the initial zoom transition ends
				template.focus();
			}
			 

			bindEvents();
		});

		// set content for center slide (first time)
		self.setContent(itemHolders[1], currentItemIndex);
		
		self.updateCurrItem();

		shout('afterInit');

		if(!isFixedPosition) {

			// On all versions of iOS lower than 8.0, we check size of viewport every second.
			// 
			// This is done to detect when Safari top & bottom bars appear, 
			// as this action doesn't trigger any events (like resize). 
			// 
			// On iOS8 they fixed this.
			// 
			// 10 Nov 2014: iOS 7 usage ~40%. iOS 8 usage 56%.
			
			updateSizeInterval = setInterval(function() {
				if(!numAnimations && !isDragging && !isZooming && (currZoomLevel === self.currItem.initialZoomLevel)  ) {
					self.updateSize();
				}
			}, 1000);
		}

		framework.addClass(template, 'pswp--visible');
	},

	// Close the gallery, then destroy it
	close: function() {
		if(!isOpen) {
			return;
		}

		isOpen = false;
		isDestroying = true;
		shout('close');
		unbindEvents();

		showOrHide(self.currItem, null, true, self.destroy);
	},

	// destroys the gallery (unbinds events, cleans up intervals and timeouts to avoid memory leaks)
	destroy: function() {
		shout('destroy');

		if(showOrHideTimeout) {
			clearTimeout(showOrHideTimeout);
		}
		
		template.setAttribute('aria-hidden', 'true');
		template.className = initalClassName;

		if(updateSizeInterval) {
			clearInterval(updateSizeInterval);
		}

		framework.unbind(self.scrollWrap, downEvents, self);

		// we unbind scroll event at the end, as closing animation may depend on it
		framework.unbind(window, 'scroll', self);

		stopDragUpdateLoop();

		stopAllAnimations();

		_listeners = null;
	},

	/**
	 * Pan image to position
	 * @param {Number} x     
	 * @param {Number} y     
	 * @param {Boolean} force Will ignore bounds if set to true.
	 */
	panTo: function(x,y,force) {
		if(!force) {
			if(x > currPanBounds.min.x) {
				x = currPanBounds.min.x;
			} else if(x < currPanBounds.max.x) {
				x = currPanBounds.max.x;
			}

			if(y > currPanBounds.min.y) {
				y = currPanBounds.min.y;
			} else if(y < currPanBounds.max.y) {
				y = currPanBounds.max.y;
			}
		}
		
		panOffset.x = x;
		panOffset.y = y;
		applyCurrentZoomPan();
	},
	
	handleEvent: function (e) {
		e = e || window.event;
		if(globalEventHandlers[e.type]) {
			globalEventHandlers[e.type](e);
		}
	},


	goTo: function(index) {

		index = getLoopedId(index);

		var diff = index - currentItemIndex;
		indexDiff = diff;

		currentItemIndex = index;
		self.currItem = getItemAt( currentItemIndex );
		currPositionIndex -= diff;
		
		moveMainScroll(slideSize.x * currPositionIndex);
		

		stopAllAnimations();
		mainScrollAnimating = false;

		self.updateCurrItem();
	},
	next: function() {
		self.goTo( currentItemIndex + 1);
	},
	prev: function() {
		self.goTo( currentItemIndex - 1);
	},

	// update current zoom/pan objects
	updateCurrZoomItem: function(emulateSetContent) {
		if(emulateSetContent) {
			shout('beforeChange', 0);
		}

		// itemHolder[1] is middle (current) item
		if(itemHolders[1].el.children.length) {
			var zoomElement = itemHolders[1].el.children[0];
			if( framework.hasClass(zoomElement, 'pswp__zoom-wrap') ) {
				currZoomElementStyle = zoomElement.style;
			} else {
				currZoomElementStyle = null;
			}
		} else {
			currZoomElementStyle = null;
		}
		
		currPanBounds = self.currItem.bounds;	
		startZoomLevel = currZoomLevel = self.currItem.initialZoomLevel;

		panOffset.x = currPanBounds.center.x;
		panOffset.y = currPanBounds.center.y;

		if(emulateSetContent) {
			shout('afterChange');
		}
	},


	invalidateCurrItems: function() {
		itemsNeedUpdate = true;
		for(var i = 0; i < numHolders; i++) {
			if( itemHolders[i].item ) {
				itemHolders[i].item.needsUpdate = true;
			}
		}
	},

	updateCurrItem: function(beforeAnimation) {

		if(indexDiff === 0) {
			return;
		}

		var diffAbs = Math.abs(indexDiff),
			tempHolder;

		if(beforeAnimation && diffAbs < 2) {
			return;
		}


		self.currItem = getItemAt( currentItemIndex );
		renderMaxResolution = false;
		
		shout('beforeChange', indexDiff);

		if(diffAbs >= numHolders) {
			containerShiftIndex += indexDiff + (indexDiff > 0 ? -numHolders : numHolders);
			diffAbs = numHolders;
		}
		for(var i = 0; i < diffAbs; i++) {
			if(indexDiff > 0) {
				tempHolder = itemHolders.shift();
				itemHolders[numHolders-1] = tempHolder; // move first to last

				containerShiftIndex++;
				setTranslateX( (containerShiftIndex+2) * slideSize.x, tempHolder.el.style);
				self.setContent(tempHolder, currentItemIndex - diffAbs + i + 1 + 1);
			} else {
				tempHolder = itemHolders.pop();
				itemHolders.unshift( tempHolder ); // move last to first

				containerShiftIndex--;
				setTranslateX( containerShiftIndex * slideSize.x, tempHolder.el.style);
				self.setContent(tempHolder, currentItemIndex + diffAbs - i - 1 - 1);
			}
			
		}

		// reset zoom/pan on previous item
		if(currZoomElementStyle && Math.abs(indexDiff) === 1) {

			var prevItem = getItemAt(prevItemIndex);
			if(prevItem.initialZoomLevel !== currZoomLevel) {
				calculateItemSize(prevItem , _viewportSize );
				setImageSize(prevItem);
				applyZoomPanToItem( prevItem ); 				
			}

		}

		// reset diff after update
		indexDiff = 0;

		self.updateCurrZoomItem();

		prevItemIndex = currentItemIndex;

		shout('afterChange');
		
	},



	updateSize: function(force) {
		
		if(!isFixedPosition && _options.modal) {
			var windowScrollY = framework.getScrollY();
			if(currentWindowScrollY !== windowScrollY) {
				template.style.top = windowScrollY + 'px';
				currentWindowScrollY = windowScrollY;
			}
			if(!force && windowVisibleSize.x === window.innerWidth && windowVisibleSize.y === window.innerHeight) {
				return;
			}
			windowVisibleSize.x = window.innerWidth;
			windowVisibleSize.y = window.innerHeight;

			//template.style.width = _windowVisibleSize.x + 'px';
			template.style.height = windowVisibleSize.y + 'px';
		}



		_viewportSize.x = self.scrollWrap.clientWidth;
		_viewportSize.y = self.scrollWrap.clientHeight;

		updatePageScrollOffset();

		slideSize.x = _viewportSize.x + Math.round(_viewportSize.x * _options.spacing);
		slideSize.y = _viewportSize.y;

		moveMainScroll(slideSize.x * currPositionIndex);

		shout('beforeResize'); // even may be used for example to switch image sources


		// don't re-calculate size on inital size update
		if(containerShiftIndex !== undefined) {

			var holder,
				item,
				hIndex;

			for(var i = 0; i < numHolders; i++) {
				holder = itemHolders[i];
				setTranslateX( (i+containerShiftIndex) * slideSize.x, holder.el.style);

				hIndex = currentItemIndex+i-1;

				if(_options.loop && getNumItems() > 2) {
					hIndex = getLoopedId(hIndex);
				}

				// update zoom level on items and refresh source (if needsUpdate)
				item = getItemAt( hIndex );

				// re-render gallery item if `needsUpdate`,
				// or doesn't have `bounds` (entirely new slide object)
				if( item && (itemsNeedUpdate || item.needsUpdate || !item.bounds) ) {

					self.cleanSlide( item );
					
					self.setContent( holder, hIndex );

					// if "center" slide
					if(i === 1) {
						self.currItem = item;
						self.updateCurrZoomItem(true);
					}

					item.needsUpdate = false;

				} else if(holder.index === -1 && hIndex >= 0) {
					// add content first time
					self.setContent( holder, hIndex );
				}
				if(item && item.container) {
					calculateItemSize(item, _viewportSize);
					setImageSize(item);
					applyZoomPanToItem( item );
				}
				
			}
			itemsNeedUpdate = false;
		}	

		startZoomLevel = currZoomLevel = self.currItem.initialZoomLevel;
		currPanBounds = self.currItem.bounds;

		if(currPanBounds) {
			panOffset.x = currPanBounds.center.x;
			panOffset.y = currPanBounds.center.y;
			applyCurrentZoomPan( true );
		}
		
		shout('resize');
	},
	
	// Zoom current item to
	zoomTo: function(destZoomLevel, centerPoint, speed, easingFn, updateFn) {
		/*
			if(destZoomLevel === 'fit') {
				destZoomLevel = self.currItem.fitRatio;
			} else if(destZoomLevel === 'fill') {
				destZoomLevel = self.currItem.fillRatio;
			}
		*/

		if(centerPoint) {
			startZoomLevel = currZoomLevel;
			midZoomPoint.x = Math.abs(centerPoint.x) - panOffset.x ;
			midZoomPoint.y = Math.abs(centerPoint.y) - panOffset.y ;
			equalizePoints(startPanOffset, panOffset);
		}

		var destPanBounds = calculatePanBounds(destZoomLevel, false),
			destPanOffset = {};

		modifyDestPanOffset('x', destPanBounds, destPanOffset, destZoomLevel);
		modifyDestPanOffset('y', destPanBounds, destPanOffset, destZoomLevel);

		var initialZoomLevel = currZoomLevel;
		var initialPanOffset = {
			x: panOffset.x,
			y: panOffset.y
		};

		roundPoint(destPanOffset);

		var onUpdate = function(now) {
			if(now === 1) {
				currZoomLevel = destZoomLevel;
				panOffset.x = destPanOffset.x;
				panOffset.y = destPanOffset.y;
			} else {
				currZoomLevel = (destZoomLevel - initialZoomLevel) * now + initialZoomLevel;
				panOffset.x = (destPanOffset.x - initialPanOffset.x) * now + initialPanOffset.x;
				panOffset.y = (destPanOffset.y - initialPanOffset.y) * now + initialPanOffset.y;
			}

			if(updateFn) {
				updateFn(now);
			}

			applyCurrentZoomPan( now === 1 );
		};

		if(speed) {
			animateProp('customZoomTo', 0, 1, speed, easingFn || framework.easing.sine.inOut, onUpdate);
		} else {
			onUpdate(1);
		}
	}


};


/*>>core*/

/*>>gestures*/
/**
 * Mouse/touch/pointer event handlers.
 * 
 * separated from @core.js for readability
 */

var minSwipeDistance = 30,
	directionCheckOffset = 10; // amount of pixels to drag to determine direction of swipe

var gestureStartTime,
	gestureCheckSpeedTime,

	// pool of objects that are used during dragging of zooming
	p = {}, // first point
	p2 = {}, // second point (for zoom gesture)
	delta = {},
	currPoint = {},
	startPoint = {},
	currPointers = [],
	startMainScrollPos = {},
	releaseAnimData,
	posPoints = [], // array of points during dragging, used to determine type of gesture
	tempPoint = {},

	isZoomingIn,
	verticalDragInitiated,
	oldAndroidTouchEndTimeout,
	currZoomedItemIndex = 0,
	_centerPoint = getEmptyPoint(),
	lastReleaseTime = 0,
	isDragging, // at least one pointer is down
	isMultitouch, // at least two _pointers are down
	zoomStarted, // zoom level changed during zoom gesture
	moved,
	dragAnimFrame,
	mainScrollShifted,
	currentPoints, // array of current touch points
	isZooming,
	currPointsDistance,
	startPointsDistance,
	currPanBounds,
	mainScrollPos = getEmptyPoint(),
	currZoomElementStyle,
	mainScrollAnimating, // true, if animation after swipe gesture is running
	midZoomPoint = getEmptyPoint(),
	currCenterPoint = getEmptyPoint(),
	direction,
	isFirstMove,
	opacityChanged,
	bgOpacity,
	wasOverInitialZoom,

	isEqualPoints = function(p1, p2) {
		return p1.x === p2.x && p1.y === p2.y;
	},
	isNearbyPoints = function(touch0, touch1) {
		return Math.abs(touch0.x - touch1.x) < doubleTapRadius && Math.abs(touch0.y - touch1.y) < doubleTapRadius;
	},
	calculatePointsDistance = function(p1, p2) {
		tempPoint.x = Math.abs( p1.x - p2.x );
		tempPoint.y = Math.abs( p1.y - p2.y );
		return Math.sqrt(tempPoint.x * tempPoint.x + tempPoint.y * tempPoint.y);
	},
	stopDragUpdateLoop = function() {
		if(dragAnimFrame) {
			cancelAf(dragAnimFrame);
			dragAnimFrame = null;
		}
	},
	dragUpdateLoop = function() {
		if(isDragging) {
			dragAnimFrame = requestAf(dragUpdateLoop);
			renderMovement();
		}
	},
	canPan = function() {
		return !(_options.scaleMode === 'fit' && currZoomLevel ===  self.currItem.initialZoomLevel);
	},
	
	// find the closest parent DOM element
	closestElement = function(el, fn) {
		if(!el || el === document) {
			return false;
		}

		// don't search elements above pswp__scroll-wrap
		if(el.getAttribute('class') && el.getAttribute('class').indexOf('pswp__scroll-wrap') > -1 ) {
			return false;
		}

		if( fn(el) ) {
			return el;
		}

		return closestElement(el.parentNode, fn);
	},

	preventObj = {},
	preventDefaultEventBehaviour = function(e, isDown) {
		preventObj.prevent = !closestElement(e.target, _options.isClickableElement);

		shout('preventDragEvent', e, isDown, preventObj);
		return preventObj.prevent;

	},
	convertTouchToPoint = function(touch, p) {
		p.x = touch.pageX;
		p.y = touch.pageY;
		p.id = touch.identifier;
		return p;
	},
	findCenterOfPoints = function(p1, p2, pCenter) {
		pCenter.x = (p1.x + p2.x) * 0.5;
		pCenter.y = (p1.y + p2.y) * 0.5;
	},
	pushPosPoint = function(time, x, y) {
		if(time - gestureCheckSpeedTime > 50) {
			var o = posPoints.length > 2 ? posPoints.shift() : {};
			o.x = x;
			o.y = y; 
			posPoints.push(o);
			gestureCheckSpeedTime = time;
		}
	},

	calculateVerticalDragOpacityRatio = function() {
		var yOffset = panOffset.y - self.currItem.initialPosition.y; // difference between initial and current position
		return 1 -  Math.abs( yOffset / (_viewportSize.y / 2)  );
	},

	
	// points pool, reused during touch events
	ePoint1 = {},
	ePoint2 = {},
	tempPointsArr = [],
	tempCounter,
	getTouchPoints = function(e) {
		// clean up previous points, without recreating array
		while(tempPointsArr.length > 0) {
			tempPointsArr.pop();
		}

		if(!pointerEventEnabled) {
			if(e.type.indexOf('touch') > -1) {

				if(e.touches && e.touches.length > 0) {
					tempPointsArr[0] = convertTouchToPoint(e.touches[0], ePoint1);
					if(e.touches.length > 1) {
						tempPointsArr[1] = convertTouchToPoint(e.touches[1], ePoint2);
					}
				}
				
			} else {
				ePoint1.x = e.pageX;
				ePoint1.y = e.pageY;
				ePoint1.id = '';
				tempPointsArr[0] = ePoint1;//_ePoint1;
			}
		} else {
			tempCounter = 0;
			// we can use forEach, as pointer events are supported only in modern browsers
			currPointers.forEach(function(p) {
				if(tempCounter === 0) {
					tempPointsArr[0] = p;
				} else if(tempCounter === 1) {
					tempPointsArr[1] = p;
				}
				tempCounter++;

			});
		}
		return tempPointsArr;
	},

	panOrMoveMainScroll = function(axis, delta) {

		var panFriction,
			overDiff = 0,
			newOffset = panOffset[axis] + delta[axis],
			startOverDiff,
			dir = delta[axis] > 0,
			newMainScrollPosition = mainScrollPos.x + delta.x,
			mainScrollDiff = mainScrollPos.x - startMainScrollPos.x,
			newPanPos,
			newMainScrollPos;

		// calculate fdistance over the bounds and friction
		if(newOffset > currPanBounds.min[axis] || newOffset < currPanBounds.max[axis]) {
			panFriction = _options.panEndFriction;
			// Linear increasing of friction, so at 1/4 of viewport it's at max value. 
			// Looks not as nice as was expected. Left for history.
			// panFriction = (1 - (_panOffset[axis] + delta[axis] + panBounds.min[axis]) / (_viewportSize[axis] / 4) );
		} else {
			panFriction = 1;
		}
		
		newOffset = panOffset[axis] + delta[axis] * panFriction;

		// move main scroll or start panning
		if(_options.allowPanToNext || currZoomLevel === self.currItem.initialZoomLevel) {


			if(!currZoomElementStyle) {
				
				newMainScrollPos = newMainScrollPosition;

			} else if(direction === 'h' && axis === 'x' && !zoomStarted ) {
				
				if(dir) {
					if(newOffset > currPanBounds.min[axis]) {
						panFriction = _options.panEndFriction;
						overDiff = currPanBounds.min[axis] - newOffset;
						startOverDiff = currPanBounds.min[axis] - startPanOffset[axis];
					}
					
					// drag right
					if( (startOverDiff <= 0 || mainScrollDiff < 0) && getNumItems() > 1 ) {
						newMainScrollPos = newMainScrollPosition;
						if(mainScrollDiff < 0 && newMainScrollPosition > startMainScrollPos.x) {
							newMainScrollPos = startMainScrollPos.x;
						}
					} else {
						if(currPanBounds.min.x !== currPanBounds.max.x) {
							newPanPos = newOffset;
						}
						
					}

				} else {

					if(newOffset < currPanBounds.max[axis] ) {
						panFriction =_options.panEndFriction;
						overDiff = newOffset - currPanBounds.max[axis];
						startOverDiff = startPanOffset[axis] - currPanBounds.max[axis];
					}

					if( (startOverDiff <= 0 || mainScrollDiff > 0) && getNumItems() > 1 ) {
						newMainScrollPos = newMainScrollPosition;

						if(mainScrollDiff > 0 && newMainScrollPosition < startMainScrollPos.x) {
							newMainScrollPos = startMainScrollPos.x;
						}

					} else {
						if(currPanBounds.min.x !== currPanBounds.max.x) {
							newPanPos = newOffset;
						}
					}

				}


				//
			}

			if(axis === 'x') {

				if(newMainScrollPos !== undefined) {
					moveMainScroll(newMainScrollPos, true);
					if(newMainScrollPos === startMainScrollPos.x) {
						mainScrollShifted = false;
					} else {
						mainScrollShifted = true;
					}
				}

				if(currPanBounds.min.x !== currPanBounds.max.x) {
					if(newPanPos !== undefined) {
						panOffset.x = newPanPos;
					} else if(!mainScrollShifted) {
						panOffset.x += delta.x * panFriction;
					}
				}

				return newMainScrollPos !== undefined;
			}

		}

		if(!mainScrollAnimating) {
			
			if(!mainScrollShifted) {
				if(currZoomLevel > self.currItem.fitRatio) {
					panOffset[axis] += delta[axis] * panFriction;
				
				}
			}

			
		}
		
	},

	// Pointerdown/touchstart/mousedown handler
	onDragStart = function(e) {

		// Allow dragging only via left mouse button.
		// As this handler is not added in IE8 - we ignore e.which
		// 
		// http://www.quirksmode.org/js/events_properties.html
		// https://developer.mozilla.org/en-US/docs/Web/API/event.button
		if(e.type === 'mousedown' && e.button > 0  ) {
			return;
		}

		if(initialZoomRunning) {
			e.preventDefault();
			return;
		}

		if(oldAndroidTouchEndTimeout && e.type === 'mousedown') {
			return;
		}

		if(preventDefaultEventBehaviour(e, true)) {
			e.preventDefault();
		}



		shout('pointerDown');

		if(pointerEventEnabled) {
			var pointerIndex = framework.arraySearch(currPointers, e.pointerId, 'id');
			if(pointerIndex < 0) {
				pointerIndex = currPointers.length;
			}
			currPointers[pointerIndex] = {x:e.pageX, y:e.pageY, id: e.pointerId};
		}
		


		var startPointsList = getTouchPoints(e),
			numPoints = startPointsList.length;

		currentPoints = null;

		stopAllAnimations();

		// init drag
		if(!isDragging || numPoints === 1) {

			

			isDragging = isFirstMove = true;
			framework.bind(window, upMoveEvents, self);

			isZoomingIn = 
				wasOverInitialZoom = 
				opacityChanged = 
				verticalDragInitiated = 
				mainScrollShifted = 
				moved = 
				isMultitouch = 
				zoomStarted = false;

			direction = null;

			shout('firstTouchStart', startPointsList);

			equalizePoints(startPanOffset, panOffset);

			currPanDist.x = currPanDist.y = 0;
			equalizePoints(currPoint, startPointsList[0]);
			equalizePoints(startPoint, currPoint);

			//_equalizePoints(_startMainScrollPos, _mainScrollPos);
			startMainScrollPos.x = slideSize.x * currPositionIndex;

			posPoints = [{
				x: currPoint.x,
				y: currPoint.y
			}];

			gestureCheckSpeedTime = gestureStartTime = getCurrentTime();

			//_mainScrollAnimationEnd(true);
			calculatePanBounds( currZoomLevel, true );
			
			// Start rendering
			stopDragUpdateLoop();
			dragUpdateLoop();
			
		}

		// init zoom
		if(!isZooming && numPoints > 1 && !mainScrollAnimating && !mainScrollShifted) {
			startZoomLevel = currZoomLevel;
			zoomStarted = false; // true if zoom changed at least once

			isZooming = isMultitouch = true;
			currPanDist.y = currPanDist.x = 0;

			equalizePoints(startPanOffset, panOffset);

			equalizePoints(p, startPointsList[0]);
			equalizePoints(p2, startPointsList[1]);

			findCenterOfPoints(p, p2, currCenterPoint);

			midZoomPoint.x = Math.abs(currCenterPoint.x) - panOffset.x;
			midZoomPoint.y = Math.abs(currCenterPoint.y) - panOffset.y;
			currPointsDistance = startPointsDistance = calculatePointsDistance(p, p2);
		}


	},

	// Pointermove/touchmove/mousemove handler
	onDragMove = function(e) {

		e.preventDefault();

		if(pointerEventEnabled) {
			var pointerIndex = framework.arraySearch(currPointers, e.pointerId, 'id');
			if(pointerIndex > -1) {
				var p = currPointers[pointerIndex];
				p.x = e.pageX;
				p.y = e.pageY; 
			}
		}

		if(isDragging) {
			var touchesList = getTouchPoints(e);
			if(!direction && !moved && !isZooming) {

				if(mainScrollPos.x !== slideSize.x * currPositionIndex) {
					// if main scroll position is shifted – direction is always horizontal
					direction = 'h';
				} else {
					var diff = Math.abs(touchesList[0].x - currPoint.x) - Math.abs(touchesList[0].y - currPoint.y);
					// check the direction of movement
					if(Math.abs(diff) >= directionCheckOffset) {
						direction = diff > 0 ? 'h' : 'v';
						currentPoints = touchesList;
					}
				}
				
			} else {
				currentPoints = touchesList;
			}
		}	
	},
	// 
	renderMovement =  function() {

		if(!currentPoints) {
			return;
		}

		var numPoints = currentPoints.length;

		if(numPoints === 0) {
			return;
		}

		equalizePoints(p, currentPoints[0]);

		delta.x = p.x - currPoint.x;
		delta.y = p.y - currPoint.y;

		if(isZooming && numPoints > 1) {
			// Handle behaviour for more than 1 point

			currPoint.x = p.x;
			currPoint.y = p.y;
		
			// check if one of two points changed
			if( !delta.x && !delta.y && isEqualPoints(currentPoints[1], p2) ) {
				return;
			}

			equalizePoints(p2, currentPoints[1]);


			if(!zoomStarted) {
				zoomStarted = true;
				shout('zoomGestureStarted');
			}
			
			// Distance between two points
			var pointsDistance = calculatePointsDistance(p,p2);

			var zoomLevel = calculateZoomLevel(pointsDistance);

			// slightly over the of initial zoom level
			if(zoomLevel > self.currItem.initialZoomLevel + self.currItem.initialZoomLevel / 15) {
				wasOverInitialZoom = true;
			}

			// Apply the friction if zoom level is out of the bounds
			var zoomFriction = 1,
				minZoomLevel = getMinZoomLevel(),
				maxZoomLevel = getMaxZoomLevel();

			if ( zoomLevel < minZoomLevel ) {
				
				if(_options.pinchToClose && !wasOverInitialZoom && startZoomLevel <= self.currItem.initialZoomLevel) {
					// fade out background if zooming out
					var minusDiff = minZoomLevel - zoomLevel;
					var percent = 1 - minusDiff / (minZoomLevel / 1.2);

					applyBgOpacity(percent);
					shout('onPinchClose', percent);
					opacityChanged = true;
				} else {
					zoomFriction = (minZoomLevel - zoomLevel) / minZoomLevel;
					if(zoomFriction > 1) {
						zoomFriction = 1;
					}
					zoomLevel = minZoomLevel - zoomFriction * (minZoomLevel / 3);
				}
				
			} else if ( zoomLevel > maxZoomLevel ) {
				// 1.5 - extra zoom level above the max. E.g. if max is x6, real max 6 + 1.5 = 7.5
				zoomFriction = (zoomLevel - maxZoomLevel) / ( minZoomLevel * 6 );
				if(zoomFriction > 1) {
					zoomFriction = 1;
				}
				zoomLevel = maxZoomLevel + zoomFriction * minZoomLevel;
			}

			if(zoomFriction < 0) {
				zoomFriction = 0;
			}

			// distance between touch points after friction is applied
			currPointsDistance = pointsDistance;

			// _centerPoint - The point in the middle of two pointers
			findCenterOfPoints(p, p2, _centerPoint);
		
			// paning with two pointers pressed
			currPanDist.x += _centerPoint.x - currCenterPoint.x;
			currPanDist.y += _centerPoint.y - currCenterPoint.y;
			equalizePoints(currCenterPoint, _centerPoint);

			panOffset.x = calculatePanOffset('x', zoomLevel);
			panOffset.y = calculatePanOffset('y', zoomLevel);

			isZoomingIn = zoomLevel > currZoomLevel;
			currZoomLevel = zoomLevel;
			applyCurrentZoomPan();

		} else {

			// handle behaviour for one point (dragging or panning)

			if(!direction) {
				return;
			}

			if(isFirstMove) {
				isFirstMove = false;

				// subtract drag distance that was used during the detection direction  

				if( Math.abs(delta.x) >= directionCheckOffset) {
					delta.x -= currentPoints[0].x - startPoint.x;
				}
				
				if( Math.abs(delta.y) >= directionCheckOffset) {
					delta.y -= currentPoints[0].y - startPoint.y;
				}
			}

			currPoint.x = p.x;
			currPoint.y = p.y;

			// do nothing if pointers position hasn't changed
			if(delta.x === 0 && delta.y === 0) {
				return;
			}

			if(direction === 'v' && _options.closeOnVerticalDrag) {
				if(!canPan()) {
					currPanDist.y += delta.y;
					panOffset.y += delta.y;

					var opacityRatio = calculateVerticalDragOpacityRatio();

					verticalDragInitiated = true;
					shout('onVerticalDrag', opacityRatio);

					applyBgOpacity(opacityRatio);
					applyCurrentZoomPan();
					return ;
				}
			}

			pushPosPoint(getCurrentTime(), p.x, p.y);

			moved = true;
			currPanBounds = self.currItem.bounds;
			
			var mainScrollChanged = panOrMoveMainScroll('x', delta);
			if(!mainScrollChanged) {
				panOrMoveMainScroll('y', delta);

				roundPoint(panOffset);
				applyCurrentZoomPan();
			}

		}

	},
	
	// Pointerup/pointercancel/touchend/touchcancel/mouseup event handler
	onDragRelease = function(e) {

		if(_features.isOldAndroid ) {

			if(oldAndroidTouchEndTimeout && e.type === 'mouseup') {
				return;
			}

			// on Android (v4.1, 4.2, 4.3 & possibly older) 
			// ghost mousedown/up event isn't preventable via e.preventDefault,
			// which causes fake mousedown event
			// so we block mousedown/up for 600ms
			if( e.type.indexOf('touch') > -1 ) {
				clearTimeout(oldAndroidTouchEndTimeout);
				oldAndroidTouchEndTimeout = setTimeout(function() {
					oldAndroidTouchEndTimeout = 0;
				}, 600);
			}
			
		}

		shout('pointerUp');

		if(preventDefaultEventBehaviour(e, false)) {
			e.preventDefault();
		}

		var releasePoint;

		if(pointerEventEnabled) {
			var pointerIndex = framework.arraySearch(currPointers, e.pointerId, 'id');
			
			if(pointerIndex > -1) {
				releasePoint = currPointers.splice(pointerIndex, 1)[0];

				if(navigator.pointerEnabled) {
					releasePoint.type = e.pointerType || 'mouse';
				} else {
					var mspointerTypes = {
						4: 'mouse', // event.MSPOINTER_TYPE_MOUSE
						2: 'touch', // event.MSPOINTER_TYPE_TOUCH 
						3: 'pen' // event.MSPOINTER_TYPE_PEN
					};
					releasePoint.type = mspointerTypes[e.pointerType];

					if(!releasePoint.type) {
						releasePoint.type = e.pointerType || 'mouse';
					}
				}

			}
		}

		var touchList = getTouchPoints(e),
			gestureType,
			numPoints = touchList.length;

		if(e.type === 'mouseup') {
			numPoints = 0;
		}

		// Do nothing if there were 3 touch points or more
		if(numPoints === 2) {
			currentPoints = null;
			return true;
		}

		// if second pointer released
		if(numPoints === 1) {
			equalizePoints(startPoint, touchList[0]);
		}				


		// pointer hasn't moved, send "tap release" point
		if(numPoints === 0 && !direction && !mainScrollAnimating) {
			if(!releasePoint) {
				if(e.type === 'mouseup') {
					releasePoint = {x: e.pageX, y: e.pageY, type:'mouse'};
				} else if(e.changedTouches && e.changedTouches[0]) {
					releasePoint = {x: e.changedTouches[0].pageX, y: e.changedTouches[0].pageY, type:'touch'};
				}		
			}

			shout('touchRelease', e, releasePoint);
		}

		// Difference in time between releasing of two last touch points (zoom gesture)
		var releaseTimeDiff = -1;

		// Gesture completed, no pointers left
		if(numPoints === 0) {
			isDragging = false;
			framework.unbind(window, upMoveEvents, self);

			stopDragUpdateLoop();

			if(isZooming) {
				// Two points released at the same time
				releaseTimeDiff = 0;
			} else if(lastReleaseTime !== -1) {
				releaseTimeDiff = getCurrentTime() - lastReleaseTime;
			}
		}
		lastReleaseTime = numPoints === 1 ? getCurrentTime() : -1;
		
		if(releaseTimeDiff !== -1 && releaseTimeDiff < 150) {
			gestureType = 'zoom';
		} else {
			gestureType = 'swipe';
		}

		if(isZooming && numPoints < 2) {
			isZooming = false;

			// Only second point released
			if(numPoints === 1) {
				gestureType = 'zoomPointerUp';
			}
			shout('zoomGestureEnded');
		}

		currentPoints = null;
		if(!moved && !zoomStarted && !mainScrollAnimating && !verticalDragInitiated) {
			// nothing to animate
			return;
		}
	
		stopAllAnimations();

		
		if(!releaseAnimData) {
			releaseAnimData = initDragReleaseAnimationData();
		}
		
		releaseAnimData.calculateSwipeSpeed('x');


		if(verticalDragInitiated) {

			var opacityRatio = calculateVerticalDragOpacityRatio();

			if(opacityRatio < _options.verticalDragRange) {
				self.close();
			} else {
				var initalPanY = panOffset.y,
					initialBgOpacity = bgOpacity;

				animateProp('verticalDrag', 0, 1, 300, framework.easing.cubic.out, function(now) {
					
					panOffset.y = (self.currItem.initialPosition.y - initalPanY) * now + initalPanY;

					applyBgOpacity(  (1 - initialBgOpacity) * now + initialBgOpacity );
					applyCurrentZoomPan();
				});

				shout('onVerticalDrag', 1);
			}

			return;
		}


		// main scroll 
		if(  (mainScrollShifted || mainScrollAnimating) && numPoints === 0) {
			var itemChanged = finishSwipeMainScrollGesture(gestureType, releaseAnimData);
			if(itemChanged) {
				return;
			}
			gestureType = 'zoomPointerUp';
		}

		// prevent zoom/pan animation when main scroll animation runs
		if(mainScrollAnimating) {
			return;
		}
		
		// Complete simple zoom gesture (reset zoom level if it's out of the bounds)  
		if(gestureType !== 'swipe') {
			completeZoomGesture();
			return;
		}
	
		// Complete pan gesture if main scroll is not shifted, and it's possible to pan current image
		if(!mainScrollShifted && currZoomLevel > self.currItem.fitRatio) {
			completePanGesture(releaseAnimData);
		}
	},


	// Returns object with data about gesture
	// It's created only once and then reused
	initDragReleaseAnimationData  = function() {
		// temp local vars
		var lastFlickDuration,
			tempReleasePos;

		// s = this
		var s = {
			lastFlickOffset: {},
			lastFlickDist: {},
			lastFlickSpeed: {},
			slowDownRatio:  {},
			slowDownRatioReverse:  {},
			speedDecelerationRatio:  {},
			speedDecelerationRatioAbs:  {},
			distanceOffset:  {},
			backAnimDestination: {},
			backAnimStarted: {},
			calculateSwipeSpeed: function(axis) {
				

				if( posPoints.length > 1) {
					lastFlickDuration = getCurrentTime() - gestureCheckSpeedTime + 50;
					tempReleasePos = posPoints[posPoints.length-2][axis];
				} else {
					lastFlickDuration = getCurrentTime() - gestureStartTime; // total gesture duration
					tempReleasePos = startPoint[axis];
				}
				s.lastFlickOffset[axis] = currPoint[axis] - tempReleasePos;
				s.lastFlickDist[axis] = Math.abs(s.lastFlickOffset[axis]);
				if(s.lastFlickDist[axis] > 20) {
					s.lastFlickSpeed[axis] = s.lastFlickOffset[axis] / lastFlickDuration;
				} else {
					s.lastFlickSpeed[axis] = 0;
				}
				if( Math.abs(s.lastFlickSpeed[axis]) < 0.1 ) {
					s.lastFlickSpeed[axis] = 0;
				}
				
				s.slowDownRatio[axis] = 0.95;
				s.slowDownRatioReverse[axis] = 1 - s.slowDownRatio[axis];
				s.speedDecelerationRatio[axis] = 1;
			},

			calculateOverBoundsAnimOffset: function(axis, speed) {
				if(!s.backAnimStarted[axis]) {

					if(panOffset[axis] > currPanBounds.min[axis]) {
						s.backAnimDestination[axis] = currPanBounds.min[axis];
						
					} else if(panOffset[axis] < currPanBounds.max[axis]) {
						s.backAnimDestination[axis] = currPanBounds.max[axis];
					}

					if(s.backAnimDestination[axis] !== undefined) {
						s.slowDownRatio[axis] = 0.7;
						s.slowDownRatioReverse[axis] = 1 - s.slowDownRatio[axis];
						if(s.speedDecelerationRatioAbs[axis] < 0.05) {

							s.lastFlickSpeed[axis] = 0;
							s.backAnimStarted[axis] = true;

							animateProp('bounceZoomPan'+axis,panOffset[axis], 
								s.backAnimDestination[axis], 
								speed || 300, 
								framework.easing.sine.out, 
								function(pos) {
									panOffset[axis] = pos;
									applyCurrentZoomPan();
								}
							);

						}
					}
				}
			},

			// Reduces the speed by slowDownRatio (per 10ms)
			calculateAnimOffset: function(axis) {
				if(!s.backAnimStarted[axis]) {
					s.speedDecelerationRatio[axis] = s.speedDecelerationRatio[axis] * (s.slowDownRatio[axis] + 
												s.slowDownRatioReverse[axis] - 
												s.slowDownRatioReverse[axis] * s.timeDiff / 10);

					s.speedDecelerationRatioAbs[axis] = Math.abs(s.lastFlickSpeed[axis] * s.speedDecelerationRatio[axis]);
					s.distanceOffset[axis] = s.lastFlickSpeed[axis] * s.speedDecelerationRatio[axis] * s.timeDiff;
					panOffset[axis] += s.distanceOffset[axis];

				}
			},

			panAnimLoop: function() {
				if ( animations.zoomPan ) {
					animations.zoomPan.raf = requestAf(s.panAnimLoop);

					s.now = getCurrentTime();
					s.timeDiff = s.now - s.lastNow;
					s.lastNow = s.now;
					
					s.calculateAnimOffset('x');
					s.calculateAnimOffset('y');

					applyCurrentZoomPan();
					
					s.calculateOverBoundsAnimOffset('x');
					s.calculateOverBoundsAnimOffset('y');


					if (s.speedDecelerationRatioAbs.x < 0.05 && s.speedDecelerationRatioAbs.y < 0.05) {

						// round pan position
						panOffset.x = Math.round(panOffset.x);
						panOffset.y = Math.round(panOffset.y);
						applyCurrentZoomPan();
						
						stopAnimation('zoomPan');
						return;
					}
				}

			}
		};
		return s;
	},

	completePanGesture = function(animData) {
		// calculate swipe speed for Y axis (paanning)
		animData.calculateSwipeSpeed('y');

		currPanBounds = self.currItem.bounds;
		
		animData.backAnimDestination = {};
		animData.backAnimStarted = {};

		// Avoid acceleration animation if speed is too low
		if(Math.abs(animData.lastFlickSpeed.x) <= 0.05 && Math.abs(animData.lastFlickSpeed.y) <= 0.05 ) {
			animData.speedDecelerationRatioAbs.x = animData.speedDecelerationRatioAbs.y = 0;

			// Run pan drag release animation. E.g. if you drag image and release finger without momentum.
			animData.calculateOverBoundsAnimOffset('x');
			animData.calculateOverBoundsAnimOffset('y');
			return true;
		}

		// Animation loop that controls the acceleration after pan gesture ends
		registerStartAnimation('zoomPan');
		animData.lastNow = getCurrentTime();
		animData.panAnimLoop();
	},


	finishSwipeMainScrollGesture = function(gestureType, releaseAnimData) {
		var itemChanged;
		if(!mainScrollAnimating) {
			currZoomedItemIndex = currentItemIndex;
		}


		
		var itemsDiff;

		if(gestureType === 'swipe') {
			var totalShiftDist = currPoint.x - startPoint.x,
				isFastLastFlick = releaseAnimData.lastFlickDist.x < 10;

			// if container is shifted for more than MIN_SWIPE_DISTANCE, 
			// and last flick gesture was in right direction
			if(totalShiftDist > minSwipeDistance && 
				(isFastLastFlick || releaseAnimData.lastFlickOffset.x > 20) ) {
				// go to prev item
				itemsDiff = -1;
			} else if(totalShiftDist < -minSwipeDistance && 
				(isFastLastFlick || releaseAnimData.lastFlickOffset.x < -20) ) {
				// go to next item
				itemsDiff = 1;
			}
		}

		var nextCircle;

		if(itemsDiff) {
			
			currentItemIndex += itemsDiff;

			if(currentItemIndex < 0) {
				currentItemIndex = _options.loop ? getNumItems()-1 : 0;
				nextCircle = true;
			} else if(currentItemIndex >= getNumItems()) {
				currentItemIndex = _options.loop ? 0 : getNumItems()-1;
				nextCircle = true;
			}

			if(!nextCircle || _options.loop) {
				indexDiff += itemsDiff;
				currPositionIndex -= itemsDiff;
				itemChanged = true;
			}
			

			
		}

		var animateToX = slideSize.x * currPositionIndex;
		var animateToDist = Math.abs( animateToX - mainScrollPos.x );
		var finishAnimDuration;


		if(!itemChanged && animateToX > mainScrollPos.x !== releaseAnimData.lastFlickSpeed.x > 0) {
			// "return to current" duration, e.g. when dragging from slide 0 to -1
			finishAnimDuration = 333; 
		} else {
			finishAnimDuration = Math.abs(releaseAnimData.lastFlickSpeed.x) > 0 ? 
									animateToDist / Math.abs(releaseAnimData.lastFlickSpeed.x) : 
									333;

			finishAnimDuration = Math.min(finishAnimDuration, 400);
			finishAnimDuration = Math.max(finishAnimDuration, 250);
		}

		if(currZoomedItemIndex === currentItemIndex) {
			itemChanged = false;
		}
		
		mainScrollAnimating = true;
		
		shout('mainScrollAnimStart');

		animateProp('mainScroll', mainScrollPos.x, animateToX, finishAnimDuration, framework.easing.cubic.out, 
			moveMainScroll,
			function() {
				stopAllAnimations();
				mainScrollAnimating = false;
				currZoomedItemIndex = -1;
				
				if(itemChanged || currZoomedItemIndex !== currentItemIndex) {
					self.updateCurrItem();
				}
				
				shout('mainScrollAnimComplete');
			}
		);

		if(itemChanged) {
			self.updateCurrItem(true);
		}

		return itemChanged;
	},

	calculateZoomLevel = function(touchesDistance) {
		return  1 / startPointsDistance * touchesDistance * startZoomLevel;
	},

	// Resets zoom if it's out of bounds
	completeZoomGesture = function() {
		var destZoomLevel = currZoomLevel,
			minZoomLevel = getMinZoomLevel(),
			maxZoomLevel = getMaxZoomLevel();

		if ( currZoomLevel < minZoomLevel ) {
			destZoomLevel = minZoomLevel;
		} else if ( currZoomLevel > maxZoomLevel ) {
			destZoomLevel = maxZoomLevel;
		}

		var destOpacity = 1,
			onUpdate,
			initialOpacity = bgOpacity;

		if(opacityChanged && !isZoomingIn && !wasOverInitialZoom && currZoomLevel < minZoomLevel) {
			//_closedByScroll = true;
			self.close();
			return true;
		}

		if(opacityChanged) {
			onUpdate = function(now) {
				applyBgOpacity(  (destOpacity - initialOpacity) * now + initialOpacity );
			};
		}

		self.zoomTo(destZoomLevel, 0, 200,  framework.easing.cubic.out, onUpdate);
		return true;
	};


registerModule('Gestures', {
	publicMethods: {

		initGestures: function() {

			// helper function that builds touch/pointer/mouse events
			var addEventNames = function(pref, down, move, up, cancel) {
				dragStartEvent = pref + down;
				dragMoveEvent = pref + move;
				dragEndEvent = pref + up;
				if(cancel) {
					dragCancelEvent = pref + cancel;
				} else {
					dragCancelEvent = '';
				}
			};

			pointerEventEnabled = _features.pointerEvent;
			if(pointerEventEnabled && _features.touch) {
				// we don't need touch events, if browser supports pointer events
				_features.touch = false;
			}

			if(pointerEventEnabled) {
				if(navigator.pointerEnabled) {
					addEventNames('pointer', 'down', 'move', 'up', 'cancel');
				} else {
					// IE10 pointer events are case-sensitive
					addEventNames('MSPointer', 'Down', 'Move', 'Up', 'Cancel');
				}
			} else if(_features.touch) {
				addEventNames('touch', 'start', 'move', 'end', 'cancel');
				likelyTouchDevice = true;
			} else {
				addEventNames('mouse', 'down', 'move', 'up');	
			}

			upMoveEvents = dragMoveEvent + ' ' + dragEndEvent  + ' ' +  dragCancelEvent;
			downEvents = dragStartEvent;

			if(pointerEventEnabled && !likelyTouchDevice) {
				likelyTouchDevice = (navigator.maxTouchPoints > 1) || (navigator.msMaxTouchPoints > 1);
			}
			// make variable public
			self.likelyTouchDevice = likelyTouchDevice; 
			
			globalEventHandlers[dragStartEvent] = onDragStart;
			globalEventHandlers[dragMoveEvent] = onDragMove;
			globalEventHandlers[dragEndEvent] = onDragRelease; // the Kraken

			if(dragCancelEvent) {
				globalEventHandlers[dragCancelEvent] = globalEventHandlers[dragEndEvent];
			}

			// Bind mouse events on device with detected hardware touch support, in case it supports multiple types of input.
			if(_features.touch) {
				downEvents += ' mousedown';
				upMoveEvents += ' mousemove mouseup';
				globalEventHandlers.mousedown = globalEventHandlers[dragStartEvent];
				globalEventHandlers.mousemove = globalEventHandlers[dragMoveEvent];
				globalEventHandlers.mouseup = globalEventHandlers[dragEndEvent];
			}

			if(!likelyTouchDevice) {
				// don't allow pan to next slide from zoomed state on Desktop
				_options.allowPanToNext = false;
			}
		}

	}
});


/*>>gestures*/

/*>>show-hide-transition*/
/**
 * show-hide-transition.js:
 *
 * Manages initial opening or closing transition.
 *
 * If you're not planning to use transition for gallery at all,
 * you may set options hideAnimationDuration and showAnimationDuration to 0,
 * and just delete startAnimation function.
 * 
 */


var showOrHideTimeout,
	showOrHide = function(item, img, out, completeFn) {

		if(showOrHideTimeout) {
			clearTimeout(showOrHideTimeout);
		}

		initialZoomRunning = true;
		initialContentSet = true;
		
		// dimensions of small thumbnail {x:,y:,w:}.
		// Height is optional, as calculated based on large image.
		var thumbBounds; 
		if(item.initialLayout) {
			thumbBounds = item.initialLayout;
			item.initialLayout = null;
		} else {
			thumbBounds = _options.getThumbBoundsFn && _options.getThumbBoundsFn(currentItemIndex);
		}

		var duration = out ? _options.hideAnimationDuration : _options.showAnimationDuration;

		var onComplete = function() {
			stopAnimation('initialZoom');
			if(!out) {
				applyBgOpacity(1);
				if(img) {
					img.style.display = 'block';
				}
				framework.addClass(template, 'pswp--animated-in');
				shout('initialZoom' + (out ? 'OutEnd' : 'InEnd'));
			} else {
				self.template.removeAttribute('style');
				self.bg.removeAttribute('style');
			}

			if(completeFn) {
				completeFn();
			}
			initialZoomRunning = false;
		};

		// if bounds aren't provided, just open gallery without animation
		if(!duration || !thumbBounds || thumbBounds.x === undefined) {

			shout('initialZoom' + (out ? 'Out' : 'In') );

			currZoomLevel = item.initialZoomLevel;
			equalizePoints(panOffset,  item.initialPosition );
			applyCurrentZoomPan();

			template.style.opacity = out ? 0 : 1;
			applyBgOpacity(1);

			if(duration) {
				setTimeout(function() {
					onComplete();
				}, duration);
			} else {
				onComplete();
			}

			return;
		}

		var startAnimation = function() {
			var closeWithRaf = closedByScroll,
				fadeEverything = !self.currItem.src || self.currItem.loadError || _options.showHideOpacity;
			
			// apply hw-acceleration to image
			if(item.miniImg) {
				item.miniImg.style.webkitBackfaceVisibility = 'hidden';
			}

			if(!out) {
				currZoomLevel = thumbBounds.w / item.w;
				panOffset.x = thumbBounds.x;
				panOffset.y = thumbBounds.y - initalWindowScrollY;

				self[fadeEverything ? 'template' : 'bg'].style.opacity = 0.001;
				applyCurrentZoomPan();
			}

			registerStartAnimation('initialZoom');
			
			if(out && !closeWithRaf) {
				framework.removeClass(template, 'pswp--animated-in');
			}

			if(fadeEverything) {
				if(out) {
					framework[ (closeWithRaf ? 'remove' : 'add') + 'Class' ](template, 'pswp--animate_opacity');
				} else {
					setTimeout(function() {
						framework.addClass(template, 'pswp--animate_opacity');
					}, 30);
				}
			}

			showOrHideTimeout = setTimeout(function() {

				shout('initialZoom' + (out ? 'Out' : 'In') );
				

				if(!out) {

					// "in" animation always uses CSS transitions (instead of rAF).
					// CSS transition work faster here, 
					// as developer may also want to animate other things, 
					// like ui on top of sliding area, which can be animated just via CSS
					
					currZoomLevel = item.initialZoomLevel;
					equalizePoints(panOffset,  item.initialPosition );
					applyCurrentZoomPan();
					applyBgOpacity(1);

					if(fadeEverything) {
						template.style.opacity = 1;
					} else {
						applyBgOpacity(1);
					}

					showOrHideTimeout = setTimeout(onComplete, duration + 20);
				} else {

					// "out" animation uses rAF only when PhotoSwipe is closed by browser scroll, to recalculate position
					var destZoomLevel = thumbBounds.w / item.w,
						initialPanOffset = {
							x: panOffset.x,
							y: panOffset.y
						},
						initialZoomLevel = currZoomLevel,
						initalBgOpacity = bgOpacity,
						onUpdate = function(now) {
							
							if(now === 1) {
								currZoomLevel = destZoomLevel;
								panOffset.x = thumbBounds.x;
								panOffset.y = thumbBounds.y  - currentWindowScrollY;
							} else {
								currZoomLevel = (destZoomLevel - initialZoomLevel) * now + initialZoomLevel;
								panOffset.x = (thumbBounds.x - initialPanOffset.x) * now + initialPanOffset.x;
								panOffset.y = (thumbBounds.y - currentWindowScrollY - initialPanOffset.y) * now + initialPanOffset.y;
							}
							
							applyCurrentZoomPan();
							if(fadeEverything) {
								template.style.opacity = 1 - now;
							} else {
								applyBgOpacity( initalBgOpacity - now * initalBgOpacity );
							}
						};

					if(closeWithRaf) {
						animateProp('initialZoom', 0, 1, duration, framework.easing.cubic.out, onUpdate, onComplete);
					} else {
						onUpdate(1);
						showOrHideTimeout = setTimeout(onComplete, duration + 20);
					}
				}
			
			}, out ? 25 : 90); // Main purpose of this delay is to give browser time to paint and
					// create composite layers of PhotoSwipe UI parts (background, controls, caption, arrows).
					// Which avoids lag at the beginning of scale transition.
		};
		startAnimation();

		
	};

/*>>show-hide-transition*/

/*>>items-controller*/
/**
*
* Controller manages gallery items, their dimensions, and their content.
* 
*/

var _items,
	tempPanAreaSize = {},
	imagesToAppendPool = [],
	initialContentSet,
	initialZoomRunning,
	controllerDefaultOptions = {
		index: 0,
		errorMsg: '<div class="pswp__error-msg"><a href="%url%" target="_blank">The image</a> could not be loaded.</div>',
		forceProgressiveLoading: false, // TODO
		preload: [1,1],
		getNumItemsFn: function() {
			return _items.length;
		}
	};


var getItemAt,
	getNumItems,
	initialIsLoop,
	getZeroBounds = function() {
		return {
			center:{x:0,y:0}, 
			max:{x:0,y:0}, 
			min:{x:0,y:0}
		};
	},
	calculateSingleItemPanBounds = function(item, realPanElementW, realPanElementH ) {
		var bounds = item.bounds;

		// position of element when it's centered
		bounds.center.x = Math.round((tempPanAreaSize.x - realPanElementW) / 2);
		bounds.center.y = Math.round((tempPanAreaSize.y - realPanElementH) / 2) + item.vGap.top;

		// maximum pan position
		bounds.max.x = (realPanElementW > tempPanAreaSize.x) ? 
							Math.round(tempPanAreaSize.x - realPanElementW) : 
							bounds.center.x;
		
		bounds.max.y = (realPanElementH > tempPanAreaSize.y) ? 
							Math.round(tempPanAreaSize.y - realPanElementH) + item.vGap.top : 
							bounds.center.y;
		
		// minimum pan position
		bounds.min.x = (realPanElementW > tempPanAreaSize.x) ? 0 : bounds.center.x;
		bounds.min.y = (realPanElementH > tempPanAreaSize.y) ? item.vGap.top : bounds.center.y;
	},
	calculateItemSize = function(item, viewportSize, zoomLevel) {

		if (item.src && !item.loadError) {
			var isInitial = !zoomLevel;
			
			if(isInitial) {
				if(!item.vGap) {
					item.vGap = {top:0,bottom:0};
				}
				// allows overriding vertical margin for individual items
				shout('parseVerticalMargin', item);
			}


			tempPanAreaSize.x = viewportSize.x;
			tempPanAreaSize.y = viewportSize.y - item.vGap.top - item.vGap.bottom;

			if (isInitial) {
				var hRatio = tempPanAreaSize.x / item.w;
				var vRatio = tempPanAreaSize.y / item.h;

				item.fitRatio = hRatio < vRatio ? hRatio : vRatio;
				//item.fillRatio = hRatio > vRatio ? hRatio : vRatio;

				var scaleMode = _options.scaleMode;

				if (scaleMode === 'orig') {
					zoomLevel = 1;
				} else if (scaleMode === 'fit') {
					zoomLevel = item.fitRatio;
				}

				if (zoomLevel > 1) {
					zoomLevel = 1;
				}

				item.initialZoomLevel = zoomLevel;
				
				if(!item.bounds) {
					// reuse bounds object
					item.bounds = getZeroBounds(); 
				}
			}

			if(!zoomLevel) {
				return;
			}

			calculateSingleItemPanBounds(item, item.w * zoomLevel, item.h * zoomLevel);

			if (isInitial && zoomLevel === item.initialZoomLevel) {
				item.initialPosition = item.bounds.center;
			}

			return item.bounds;
		} else {
			item.w = item.h = 0;
			item.initialZoomLevel = item.fitRatio = 1;
			item.bounds = getZeroBounds();
			item.initialPosition = item.bounds.center;

			// if it's not image, we return zero bounds (content is not zoomable)
			return item.bounds;
		}
		
	},

	


	appendImage = function(index, item, baseDiv, img, preventAnimation, keepPlaceholder) {
		

		if(item.loadError) {
			return;
		}

		if(img) {

			item.imageAppended = true;
			setImageSize(item, img, (item === self.currItem && renderMaxResolution) );
			
			baseDiv.appendChild(img);

			if(keepPlaceholder) {
				setTimeout(function() {
					if(item && item.loaded && item.placeholder) {
						item.placeholder.style.display = 'none';
						item.placeholder = null;
					}
				}, 500);
			}
		}
	},
	


	preloadImage = function(item) {
		item.loading = true;
		item.loaded = false;
		var img = item.img = framework.createEl('pswp__img', 'img');
		var onComplete = function() {
			item.loading = false;
			item.loaded = true;

			if(item.loadComplete) {
				item.loadComplete(item);
			} else {
				item.img = null; // no need to store image object
			}
			img.onload = img.onerror = null;
			img = null;
		};
		img.onload = onComplete;
		img.onerror = function() {
			item.loadError = true;
			onComplete();
		};		

		img.src = item.src;// + '?a=' + Math.random();

		return img;
	},
	checkForError = function(item, cleanUp) {
		if(item.src && item.loadError && item.container) {

			if(cleanUp) {
				item.container.innerHTML = '';
			}

			item.container.innerHTML = _options.errorMsg.replace('%url%',  item.src );
			return true;
			
		}
	},
	setImageSize = function(item, img, maxRes) {
		if(!item.src) {
			return;
		}

		if(!img) {
			img = item.container.lastChild;
		}

		var w = maxRes ? item.w : Math.round(item.w * item.fitRatio),
			h = maxRes ? item.h : Math.round(item.h * item.fitRatio);
		
		if(item.placeholder && !item.loaded) {
			item.placeholder.style.width = w + 'px';
			item.placeholder.style.height = h + 'px';
		}

		img.style.width = w + 'px';
		img.style.height = h + 'px';
	},
	appendImagesPool = function() {

		if(imagesToAppendPool.length) {
			var poolItem;

			for(var i = 0; i < imagesToAppendPool.length; i++) {
				poolItem = imagesToAppendPool[i];
				if( poolItem.holder.index === poolItem.index ) {
					appendImage(poolItem.index, poolItem.item, poolItem.baseDiv, poolItem.img, false, poolItem.clearPlaceholder);
				}
			}
			imagesToAppendPool = [];
		}
	};
	


registerModule('Controller', {

	publicMethods: {

		lazyLoadItem: function(index) {
			index = getLoopedId(index);
			var item = getItemAt(index);

			if(!item || ((item.loaded || item.loading) && !itemsNeedUpdate)) {
				return;
			}

			shout('gettingData', index, item);

			if (!item.src) {
				return;
			}

			preloadImage(item);
		},
		initController: function() {
			framework.extend(_options, controllerDefaultOptions, true);
			self.items = _items = items;
			getItemAt = self.getItemAt;
			getNumItems = _options.getNumItemsFn; //self.getNumItems;



			initialIsLoop = _options.loop;
			if(getNumItems() < 3) {
				_options.loop = false; // disable loop if less then 3 items
			}

			listen('beforeChange', function(diff) {

				var p = _options.preload,
					isNext = diff === null ? true : (diff >= 0),
					preloadBefore = Math.min(p[0], getNumItems() ),
					preloadAfter = Math.min(p[1], getNumItems() ),
					i;


				for(i = 1; i <= (isNext ? preloadAfter : preloadBefore); i++) {
					self.lazyLoadItem(currentItemIndex+i);
				}
				for(i = 1; i <= (isNext ? preloadBefore : preloadAfter); i++) {
					self.lazyLoadItem(currentItemIndex-i);
				}
			});

			listen('initialLayout', function() {
				self.currItem.initialLayout = _options.getThumbBoundsFn && _options.getThumbBoundsFn(currentItemIndex);
			});

			listen('mainScrollAnimComplete', appendImagesPool);
			listen('initialZoomInEnd', appendImagesPool);



			listen('destroy', function() {
				var item;
				for(var i = 0; i < _items.length; i++) {
					item = _items[i];
					// remove reference to DOM elements, for GC
					if(item.container) {
						item.container = null; 
					}
					if(item.placeholder) {
						item.placeholder = null;
					}
					if(item.img) {
						item.img = null;
					}
					if(item.preloader) {
						item.preloader = null;
					}
					if(item.loadError) {
						item.loaded = item.loadError = false;
					}
				}
				imagesToAppendPool = null;
			});
		},


		getItemAt: function(index) {
			if (index >= 0) {
				return _items[index] !== undefined ? _items[index] : false;
			}
			return false;
		},

		allowProgressiveImg: function() {
			// 1. Progressive image loading isn't working on webkit/blink 
			//    when hw-acceleration (e.g. translateZ) is applied to IMG element.
			//    That's why in PhotoSwipe parent element gets zoom transform, not image itself.
			//    
			// 2. Progressive image loading sometimes blinks in webkit/blink when applying animation to parent element.
			//    That's why it's disabled on touch devices (mainly because of swipe transition)
			//    
			// 3. Progressive image loading sometimes doesn't work in IE (up to 11).

			// Don't allow progressive loading on non-large touch devices
			return _options.forceProgressiveLoading || !likelyTouchDevice || _options.mouseUsed || screen.width > 1200; 
			// 1200 - to eliminate touch devices with large screen (like Chromebook Pixel)
		},

		setContent: function(holder, index) {

			if(_options.loop) {
				index = getLoopedId(index);
			}

			var prevItem = self.getItemAt(holder.index);
			if(prevItem) {
				prevItem.container = null;
			}
	
			var item = self.getItemAt(index),
				img;
			
			if(!item) {
				holder.el.innerHTML = '';
				return;
			}

			// allow to override data
			shout('gettingData', index, item);

			holder.index = index;
			holder.item = item;

			// base container DIV is created only once for each of 3 holders
			var baseDiv = item.container = framework.createEl('pswp__zoom-wrap'); 

			

			if(!item.src && item.html) {
				if(item.html.tagName) {
					baseDiv.appendChild(item.html);
				} else {
					baseDiv.innerHTML = item.html;
				}
			}

			checkForError(item);

			calculateItemSize(item, _viewportSize);
			
			if(item.src && !item.loadError && !item.loaded) {

				item.loadComplete = function(item) {

					// gallery closed before image finished loading
					if(!isOpen) {
						return;
					}

					// check if holder hasn't changed while image was loading
					if(holder && holder.index === index ) {
						if( checkForError(item, true) ) {
							item.loadComplete = item.img = null;
							calculateItemSize(item, _viewportSize);
							applyZoomPanToItem(item);

							if(holder.index === currentItemIndex) {
								// recalculate dimensions
								self.updateCurrZoomItem();
							}
							return;
						}
						if( !item.imageAppended ) {
							if(_features.transform && (mainScrollAnimating || initialZoomRunning) ) {
								imagesToAppendPool.push({
									item:item,
									baseDiv:baseDiv,
									img:item.img,
									index:index,
									holder:holder,
									clearPlaceholder:true
								});
							} else {
								appendImage(index, item, baseDiv, item.img, mainScrollAnimating || initialZoomRunning, true);
							}
						} else {
							// remove preloader & mini-img
							if(!initialZoomRunning && item.placeholder) {
								item.placeholder.style.display = 'none';
								item.placeholder = null;
							}
						}
					}

					item.loadComplete = null;
					item.img = null; // no need to store image element after it's added

					shout('imageLoadComplete', index, item);
				};

				if(framework.features.transform) {
					
					var placeholderClassName = 'pswp__img pswp__img--placeholder'; 
					placeholderClassName += (item.msrc ? '' : ' pswp__img--placeholder--blank');

					var placeholder = framework.createEl(placeholderClassName, item.msrc ? 'img' : '');
					if(item.msrc) {
						placeholder.src = item.msrc;
					}
					
					setImageSize(item, placeholder);

					baseDiv.appendChild(placeholder);
					item.placeholder = placeholder;

				}
				

				

				if(!item.loading) {
					preloadImage(item);
				}


				if( self.allowProgressiveImg() ) {
					// just append image
					if(!initialContentSet && _features.transform) {
						imagesToAppendPool.push({
							item:item, 
							baseDiv:baseDiv, 
							img:item.img, 
							index:index, 
							holder:holder
						});
					} else {
						appendImage(index, item, baseDiv, item.img, true, true);
					}
				}
				
			} else if(item.src && !item.loadError) {
				// image object is created every time, due to bugs of image loading & delay when switching images
				img = framework.createEl('pswp__img', 'img');
				img.style.opacity = 1;
				img.src = item.src;
				setImageSize(item, img);
				appendImage(index, item, baseDiv, img, true);
			}
			

			if(!initialContentSet && index === currentItemIndex) {
				currZoomElementStyle = baseDiv.style;
				showOrHide(item, (img ||item.img) );
			} else {
				applyZoomPanToItem(item);
			}

			holder.el.innerHTML = '';
			holder.el.appendChild(baseDiv);
		},

		cleanSlide: function( item ) {
			if(item.img ) {
				item.img.onload = item.img.onerror = null;
			}
			item.loaded = item.loading = item.img = item.imageAppended = false;
		}

	}
});

/*>>items-controller*/

/*>>tap*/
/**
 * tap.js:
 *
 * Displatches tap and double-tap events.
 * 
 */

var tapTimer,
	tapReleasePoint = {},
	dispatchTapEvent = function(origEvent, releasePoint, pointerType) {		
		var e = document.createEvent( 'CustomEvent' ),
			eDetail = {
				origEvent:origEvent, 
				target:origEvent.target, 
				releasePoint: releasePoint, 
				pointerType:pointerType || 'touch'
			};

		e.initCustomEvent( 'pswpTap', true, true, eDetail );
		origEvent.target.dispatchEvent(e);
	};

registerModule('Tap', {
	publicMethods: {
		initTap: function() {
			listen('firstTouchStart', self.onTapStart);
			listen('touchRelease', self.onTapRelease);
			listen('destroy', function() {
				tapReleasePoint = {};
				tapTimer = null;
			});
		},
		onTapStart: function(touchList) {
			if(touchList.length > 1) {
				clearTimeout(tapTimer);
				tapTimer = null;
			}
		},
		onTapRelease: function(e, releasePoint) {
			if(!releasePoint) {
				return;
			}

			if(!moved && !isMultitouch && !numAnimations) {
				var p0 = releasePoint;
				if(tapTimer) {
					clearTimeout(tapTimer);
					tapTimer = null;

					// Check if taped on the same place
					if ( isNearbyPoints(p0, tapReleasePoint) ) {
						shout('doubleTap', p0);
						return;
					}
				}

				if(releasePoint.type === 'mouse') {
					dispatchTapEvent(e, releasePoint, 'mouse');
					return;
				}

				var clickedTagName = e.target.tagName.toUpperCase();
				// avoid double tap delay on buttons and elements that have class pswp__single-tap
				if(clickedTagName === 'BUTTON' || framework.hasClass(e.target, 'pswp__single-tap') ) {
					dispatchTapEvent(e, releasePoint);
					return;
				}

				equalizePoints(tapReleasePoint, p0);

				tapTimer = setTimeout(function() {
					dispatchTapEvent(e, releasePoint);
					tapTimer = null;
				}, 300);
			}
		}
	}
});

/*>>tap*/

/*>>desktop-zoom*/
/**
 *
 * desktop-zoom.js:
 *
 * - Binds mousewheel event for paning zoomed image.
 * - Manages "dragging", "zoomed-in", "zoom-out" classes.
 *   (which are used for cursors and zoom icon)
 * - Adds toggleDesktopZoom function.
 * 
 */

var wheelDelta;
	
registerModule('DesktopZoom', {

	publicMethods: {

		initDesktopZoom: function() {

			if(oldIe) {
				// no zoom for old IE (<=8)
				return;
			}

			if(likelyTouchDevice) {
				// if detected hardware touch support, we wait until mouse is used,
				// and only then apply desktop-zoom features
				listen('mouseUsed', function() {
					self.setupDesktopZoom();
				});
			} else {
				self.setupDesktopZoom(true);
			}

		},

		setupDesktopZoom: function(onInit) {

			wheelDelta = {};

			var events = 'wheel mousewheel DOMMouseScroll';
			
			listen('bindEvents', function() {
				framework.bind(template, events,  self.handleMouseWheel);
			});

			listen('unbindEvents', function() {
				if(wheelDelta) {
					framework.unbind(template, events, self.handleMouseWheel);
				}
			});

			self.mouseZoomedIn = false;

			var hasDraggingClass,
				updateZoomable = function() {
					if(self.mouseZoomedIn) {
						framework.removeClass(template, 'pswp--zoomed-in');
						self.mouseZoomedIn = false;
					}
					if(currZoomLevel < 1) {
						framework.addClass(template, 'pswp--zoom-allowed');
					} else {
						framework.removeClass(template, 'pswp--zoom-allowed');
					}
					removeDraggingClass();
				},
				removeDraggingClass = function() {
					if(hasDraggingClass) {
						framework.removeClass(template, 'pswp--dragging');
						hasDraggingClass = false;
					}
				};

			listen('resize' , updateZoomable);
			listen('afterChange' , updateZoomable);
			listen('pointerDown', function() {
				if(self.mouseZoomedIn) {
					hasDraggingClass = true;
					framework.addClass(template, 'pswp--dragging');
				}
			});
			listen('pointerUp', removeDraggingClass);

			if(!onInit) {
				updateZoomable();
			}
			
		},

		handleMouseWheel: function(e) {

			if(currZoomLevel <= self.currItem.fitRatio) {
				if( _options.modal ) {

					if (!_options.closeOnScroll || numAnimations || isDragging) {
						e.preventDefault();
					} else if(transformKey && Math.abs(e.deltaY) > 2) {
						// close PhotoSwipe
						// if browser supports transforms & scroll changed enough
						closedByScroll = true;
						self.close();
					}

				}
				return true;
			}

			// allow just one event to fire
			e.stopPropagation();

			// https://developer.mozilla.org/en-US/docs/Web/Events/wheel
			wheelDelta.x = 0;

			if('deltaX' in e) {
				if(e.deltaMode === 1 /* DOM_DELTA_LINE */) {
					// 18 - average line height
					wheelDelta.x = e.deltaX * 18;
					wheelDelta.y = e.deltaY * 18;
				} else {
					wheelDelta.x = e.deltaX;
					wheelDelta.y = e.deltaY;
				}
			} else if('wheelDelta' in e) {
				if(e.wheelDeltaX) {
					wheelDelta.x = -0.16 * e.wheelDeltaX;
				}
				if(e.wheelDeltaY) {
					wheelDelta.y = -0.16 * e.wheelDeltaY;
				} else {
					wheelDelta.y = -0.16 * e.wheelDelta;
				}
			} else if('detail' in e) {
				wheelDelta.y = e.detail;
			} else {
				return;
			}

			calculatePanBounds(currZoomLevel, true);

			var newPanX = panOffset.x - wheelDelta.x,
				newPanY = panOffset.y - wheelDelta.y;

			// only prevent scrolling in nonmodal mode when not at edges
			if (_options.modal ||
				(
				newPanX <= currPanBounds.min.x && newPanX >= currPanBounds.max.x &&
				newPanY <= currPanBounds.min.y && newPanY >= currPanBounds.max.y
				) ) {
				e.preventDefault();
			}

			// TODO: use rAF instead of mousewheel?
			self.panTo(newPanX, newPanY);
		},

		toggleDesktopZoom: function(centerPoint) {
			centerPoint = centerPoint || {x:_viewportSize.x/2 + offset.x, y:_viewportSize.y/2 + offset.y };

			var doubleTapZoomLevel = _options.getDoubleTapZoom(true, self.currItem);
			var zoomOut = currZoomLevel === doubleTapZoomLevel;
			
			self.mouseZoomedIn = !zoomOut;

			self.zoomTo(zoomOut ? self.currItem.initialZoomLevel : doubleTapZoomLevel, centerPoint, 333);
			framework[ (!zoomOut ? 'add' : 'remove') + 'Class'](template, 'pswp--zoomed-in');
		}

	}
});


/*>>desktop-zoom*/

/*>>history*/
/**
 *
 * history.js:
 *
 * - Back button to close gallery.
 * 
 * - Unique URL for each slide: example.com/&pid=1&gid=3
 *   (where PID is picture index, and GID and gallery index)
 *   
 * - Switch URL when slides change.
 * 
 */


var historyDefaultOptions = {
	history: true,
	galleryUID: 1
};

var historyUpdateTimeout,
	hashChangeTimeout,
	hashAnimCheckTimeout,
	hashChangedByScript,
	hashChangedByHistory,
	hashReseted,
	initialHash,
	historyChanged,
	closedFromUrl,
	urlChangedOnce,
	windowLoc,

	supportsPushState,

	getHash = function() {
		return windowLoc.hash.substring(1);
	},
	cleanHistoryTimeouts = function() {

		if(historyUpdateTimeout) {
			clearTimeout(historyUpdateTimeout);
		}

		if(hashAnimCheckTimeout) {
			clearTimeout(hashAnimCheckTimeout);
		}
	},

	// pid - Picture index
	// gid - Gallery index
	parseItemIndexFromUrl = function() {
		var hash = getHash(),
			params = {};

		if(hash.length < 5) { // pid=1
			return params;
		}

		var i, vars = hash.split('&');
		for (i = 0; i < vars.length; i++) {
			if(!vars[i]) {
				continue;
			}
			var pair = vars[i].split('=');	
			if(pair.length < 2) {
				continue;
			}
			params[pair[0]] = pair[1];
		}
		if(_options.galleryPIDs) {
			// detect custom pid in hash and search for it among the items collection
			var searchfor = params.pid;
			params.pid = 0; // if custom pid cannot be found, fallback to the first item
			for(i = 0; i < _items.length; i++) {
				if(_items[i].pid === searchfor) {
					params.pid = i;
					break;
				}
			}
		} else {
			params.pid = parseInt(params.pid,10)-1;
		}
		if( params.pid < 0 ) {
			params.pid = 0;
		}
		return params;
	},
	updateHash = function() {

		if(hashAnimCheckTimeout) {
			clearTimeout(hashAnimCheckTimeout);
		}


		if(numAnimations || isDragging) {
			// changing browser URL forces layout/paint in some browsers, which causes noticable lag during animation
			// that's why we update hash only when no animations running
			hashAnimCheckTimeout = setTimeout(updateHash, 500);
			return;
		}
		
		if(hashChangedByScript) {
			clearTimeout(hashChangeTimeout);
		} else {
			hashChangedByScript = true;
		}


		var pid = (currentItemIndex + 1);
		var item = getItemAt( currentItemIndex );
		if(item.hasOwnProperty('pid')) {
			// carry forward any custom pid assigned to the item
			pid = item.pid;
		}
		var newHash = initialHash + '&'  +  'gid=' + _options.galleryUID + '&' + 'pid=' + pid;

		if(!historyChanged) {
			if(windowLoc.hash.indexOf(newHash) === -1) {
				urlChangedOnce = true;
			}
			// first time - add new hisory record, then just replace
		}

		var newUrl = windowLoc.href.split('#')[0] + '#' +  newHash;

		if( supportsPushState ) {

			if('#' + newHash !== window.location.hash) {
				history[historyChanged ? 'replaceState' : 'pushState']('', document.title, newUrl);
			}

		} else {
			if(historyChanged) {
				windowLoc.replace( newUrl );
			} else {
				windowLoc.hash = newHash;
			}
		}
		
		

		historyChanged = true;
		hashChangeTimeout = setTimeout(function() {
			hashChangedByScript = false;
		}, 60);
	};



	

registerModule('History', {

	

	publicMethods: {
		initHistory: function() {

			framework.extend(_options, historyDefaultOptions, true);

			if( !_options.history ) {
				return;
			}


			windowLoc = window.location;
			urlChangedOnce = false;
			closedFromUrl = false;
			historyChanged = false;
			initialHash = getHash();
			supportsPushState = ('pushState' in history);


			if(initialHash.indexOf('gid=') > -1) {
				initialHash = initialHash.split('&gid=')[0];
				initialHash = initialHash.split('?gid=')[0];
			}
			

			listen('afterChange', self.updateURL);
			listen('unbindEvents', function() {
				framework.unbind(window, 'hashchange', self.onHashChange);
			});


			var returnToOriginal = function() {
				hashReseted = true;
				if(!closedFromUrl) {

					if(urlChangedOnce) {
						history.back();
					} else {

						if(initialHash) {
							windowLoc.hash = initialHash;
						} else {
							if (supportsPushState) {

								// remove hash from url without refreshing it or scrolling to top
								history.pushState('', document.title,  windowLoc.pathname + windowLoc.search );
							} else {
								windowLoc.hash = '';
							}
						}
					}
					
				}

				cleanHistoryTimeouts();
			};


			listen('unbindEvents', function() {
				if(closedByScroll) {
					// if PhotoSwipe is closed by scroll, we go "back" before the closing animation starts
					// this is done to keep the scroll position
					returnToOriginal();
				}
			});
			listen('destroy', function() {
				if(!hashReseted) {
					returnToOriginal();
				}
			});
			listen('firstUpdate', function() {
				currentItemIndex = parseItemIndexFromUrl().pid;
			});

			

			
			var index = initialHash.indexOf('pid=');
			if(index > -1) {
				initialHash = initialHash.substring(0, index);
				if(initialHash.slice(-1) === '&') {
					initialHash = initialHash.slice(0, -1);
				}
			}
			

			setTimeout(function() {
				if(isOpen) { // hasn't destroyed yet
					framework.bind(window, 'hashchange', self.onHashChange);
				}
			}, 40);
			
		},
		onHashChange: function() {

			if(getHash() === initialHash) {

				closedFromUrl = true;
				self.close();
				return;
			}
			if(!hashChangedByScript) {

				hashChangedByHistory = true;
				self.goTo( parseItemIndexFromUrl().pid );
				hashChangedByHistory = false;
			}
			
		},
		updateURL: function() {

			// Delay the update of URL, to avoid lag during transition, 
			// and to not to trigger actions like "refresh page sound" or "blinking favicon" to often
			
			cleanHistoryTimeouts();
			

			if(hashChangedByHistory) {
				return;
			}

			if(!historyChanged) {
				updateHash(); // first time
			} else {
				historyUpdateTimeout = setTimeout(updateHash, 800);
			}
		}
	
	}
});


/*>>history*/
	framework.extend(self, publicMethods); };
	return photoSwipe;
});