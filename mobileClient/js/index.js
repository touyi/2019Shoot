var isOpen=false;
window.onload = function() {
	init();
	if(/Android|webOS|iPhone|iPod|BlackBerry/i.test(navigator.userAgent)) {
    } else {
        alert("请用手机打开，暂不支持电脑，敬请原谅");
        //window.history.back();
    }
	if(isWeiXin()){
		alert("请点击微信右上角,选择在浏览器器中打开，否则无法进入游戏");
		WeixinJSBridge.call('closeWindow');
	}
	window.addEventListener("onorientationchange" in window ? "orientationchange" : "resize", function() {
        if (window.orientation === 90 || window.orientation === -90) {
	        isOpen=true;
	        alert("请关闭旋转功能，否则无法进行游戏");
	        init();
        } 
    }, false);
	var startGame = document.getElementById("start");
	startGame.ontouchstart = function() {
		//start.style.opacity = 1;
		if(!isOpen)   window.location.replace("main.html");
		else {
			alert("请先关闭旋转功能，再重新进入");
		}
		 
	}
}

function init() {
	var screenHeight = document.documentElement ? document.documentElement.clientHeight : document.body.clientHeight;
	var screenWidth = document.documentElement ? document.documentElement.clientWidth : document.body.clientWidth;
	var containner = document.getElementById("containner");
	containner.style.height = screenHeight + "px";
	containner.style.width=screenWidth+"px";
}
function isWeiXin() {
	var ua = window.navigator.userAgent.toLowerCase();
	if(ua.match(/MicroMessenger/i) == 'micromessenger') {
		return true;
	} else {
		return false;
	}
}
function testTransform(){
	var screenchange = 1;
	if(screenchange==1){
		
	}
}