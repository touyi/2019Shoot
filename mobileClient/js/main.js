var isTouch = true;
var changeId = false;
var isLand = false;
var isend = false;
var isuser = false;
var a_x = 0;
var a_y = 0;
var a_z = 0;
//三个方向上当前的速度
var speedX = 0;
var speedY = 0;
var speedZ = 0;
//时间间隔
var timeInterval = 1 / 30;
//三个方向上的位移
var distanceX = 0;
var distanceY = 0;
var distanceZ = 0;
var websocket = null;
var ipstirng;
getIp("192.168.31.183:7000");
var Terminal = navigator.userAgent;
var MobileTerminal;
if (Terminal.indexOf('Android') > -1 || Terminal.indexOf('Adr') > -1) {
	MobileTerminal = 1;
} else {
	MobileTerminal = -1;
}
window.addEventListener("devicemotion", function(e) {
	if(e==null || e.accelerationIncludingGravity ==null){
		return;
	}
	if(IsPC() != true){
		a_x = e.accelerationIncludingGravity.x;
		a_y = e.accelerationIncludingGravity.y;
		a_z = e.accelerationIncludingGravity.z + 10;
	}
	
	//websocket.send("O"+distanceX+" "+distanceY+" "+"0#");

	speedX += a_x * timeInterval;
	speedY += a_y * timeInterval;
	speedZ += a_z * timeInterval;
}, true);
////////////////////////////////////
function IsPC() {
   var userAgentInfo = navigator.userAgent;
   // TODO 调试判断
   if(userAgentInfo.indexOf("Chrome") > 0){
   	return true;
   }
   var Agents = ["Android", "iPhone",
      "SymbianOS", "Windows Phone",
      "iPad", "iPod"];
   var flag = true;
   for (var v = 0; v < Agents.length; v++) {
      if (userAgentInfo.indexOf(Agents[v]) > 0) {
         flag = false;
         break;
      }
   }
   return flag;
}
function getIp(ipString) {
	ipstring = ipString;
	if ('WebSocket' in window) {
		websocket = new WebSocket('ws://' + ipstring);
	}
	//me.websocket=new WebSocket("ws://"+me.ipstring);
	websocket.onclose = function() {
		if (!isend) {
			if (isuser)
				alert("已有用户连接");
			else {
				alert("游戏断开连接，请重新进入");
			}
		} else
			alert("游戏结束，请重新进入");

	}
	websocket.onerr = function() {
		alert("连接异常");
	}
	websocket.onmessage = function(event) {
		if (event.data == "more") {
			isuser = true;
		}
	}
}
//三个方向上当前速度
window.ontouchstart  =   function(e)  { 
	e.preventDefault(); 
	if (isTouch) {
		foo();
		isTouch = false;
	}

	function foo() {

		distanceX = 0;
		distanceY = 0;
		distanceZ = 0;
		//	distanceX=(speedX*timeInterval).toFixed(1);
		//	distanceY=(speedY*timeInterval).toFixed(1);
		//	distanceZ=(speedZ*timeInterval).toFixed(1);
		//	document.write(distanceZ);

		distanceX = a_x.toFixed(2) * MobileTerminal;
		distanceY = a_y.toFixed(2) * MobileTerminal; //两个轴相互交换
		distanceZ = a_z.toFixed(2) * MobileTerminal;
		if (!isLand) {
			websocket.send("O" + distanceX + " " + distanceY + " " + "0#");
		} else {
			websocket.send("O" + (-1) * distanceY + " " + distanceX + " " + "0#");
		}
		setTimeout(foo, 60);
		//发送数据
	}
}; 
window.onload = function() {
	init();
	var intervalTime;
	window.addEventListener("onorientationchange" in window ? "orientationchange" : "resize", function() {        
		if (window.orientation === 90 || window.orientation === -90) {
			alert("请关闭旋转功能，重新进人游戏"); 
			window.history.back();        
		}    
	}, false);
	var leaveGame = document.getElementById("leave");
	var method = document.getElementById("method");
	var changeWeapon = document.getElementById("weapon");
	var shootGame = document.getElementById("shoot");
	leaveGame.ontouchstart = function() {
		//leaveGame.style.backgroundColor = "#FFF";
		leaveGame.style.opacity = "0.4";
	};
	leaveGame.ontouchend = function() {
		//leaveGame.style.backgroundColor = "blanchedalmond";
		websocket.send("E#");
		isend = true;
		leaveGame.style.opacity = "1";
		window.location.replace("index.html");
	};
	method.ontouchstart = function() {
		//method.style.backgroundColor="#FFF";
		method.style.opacity = "0.4";
	}
	method.ontouchend = function() {
		//method.style.backgroundColor="darkgray";
		method.style.opacity = "1";
		isLand = !isLand;
		if (!isLand) init();
		else initLand();
	}
	changeWeapon.ontouchstart = function() {
		websocket.send("C1#");
		changeWeapon.style.backgroundColor = "#8E8986";
		changeWeapon.style.opacity = 0.5;
	};
	changeWeapon.ontouchmove = function() {
		changeWeapon.style.backgroundColor = "greenyellow";
		changeWeapon.style.opacity = 0.5;
	};
	changeWeapon.ontouchend = function() {
		websocket.send("C0#");
		changeWeapon.style.backgroundColor = "greenyellow";
		changeWeapon.style.opacity = 0.9;
	};
	shootGame.ontouchstart = function() {
		websocket.send("F1#");
		shootGame.style.backgroundColor = "#8E8986";
		shootGame.style.opacity = 0.5;
		// if(intervalTime!=null){
		// 	clearInterval(intervalTime);
		// }
		// intervalTime = setInterval(function() {
		// 	websocket.send("F#");
		// 	console.log("F#");
		// }, 60)
	};
	shootGame.ontouchmove = function() {
		shootGame.style.backgroundColor = "greenyellow";
		shootGame.style.opacity = 0.5;
	};
	shootGame.ontouchend = function() {
		websocket.send("F0#");
		shootGame.style.backgroundColor = "greenyellow";
		// clearInterval(intervalTime);
		// intervalTime = null;
		shootGame.style.opacity = 1;
	};
}

function init() {

	var screenHeight = document.documentElement ? document.documentElement.clientHeight : document.body.clientHeight;
	var screenWidth = document.documentElement ? document.documentElement.clientWidth : document.body.clientWidth;
	var containner = document.getElementById("containner");
	var leaveGame = document.getElementById("leave");
	var method = document.getElementById("method");
	var changeWeapon = document.getElementById("weapon");
	var shootGame = document.getElementById("shoot");
	containner.style.height = screenHeight + "px";
	containner.style.width = screenWidth + "px";
	document.getElementsByTagName("body")[0].style.backgroundImage = "url(img/mainbg.png)";

	leaveGame.style.width = screenWidth * 0.34 + "px";
	leaveGame.style.height = screenWidth * 0.11 + "px";
	leaveGame.style.top = screenHeight * 0.1 + "px";
	leaveGame.style.left = screenWidth * 0.1 + "px";
	leaveGame.style.margin = "0px";
	leaveGame.style.backgroundImage = "url(img/exitPic.png)";

	method.style.width = screenWidth * 0.34 + "px";
	method.style.height = screenWidth * 0.11 + "px";
	method.style.top = screenHeight * 0.1 + "px";
	method.style.left = screenWidth * 0.56 + "px";
	method.style.margin = "0px";
	method.style.backgroundImage = "url(img/method.png)";

	changeWeapon.style.width = screenWidth * 0.2 + "px";
	changeWeapon.style.height = screenWidth * 0.2 + "px";
	changeWeapon.style.top = screenHeight * 0.45 + "px";
	changeWeapon.style.left = screenWidth * 0.65 + "px";
	changeWeapon.style.margin = "0px";

	shootGame.style.width = screenWidth * 0.4 + "px";
	shootGame.style.height = screenWidth * 0.4 + "px";
	shootGame.style.top = (screenHeight * 0.87 - screenWidth * 0.4) + "px";
	shootGame.style.left = screenWidth * 0.3 + "Px";
	shootGame.style.margin = "0px";
}

function initLand() {
	var screenHeight = document.documentElement ? document.documentElement.clientHeight : document.body.clientHeight;
	var screenWidth = document.documentElement ? document.documentElement.clientWidth : document.body.clientWidth;
	var containner = document.getElementById("containner");
	var leaveGame = document.getElementById("leave");
	var method = document.getElementById("method");
	var changeWeapon = document.getElementById("weapon");
	var shootGame = document.getElementById("shoot");
	document.getElementsByTagName("body")[0].style.backgroundImage = "url(img/mainbgLand.png)";

	leaveGame.style.width = screenWidth * 0.11 + "px";
	leaveGame.style.height = screenWidth * 0.34 + "px";
	leaveGame.style.top = screenHeight * 0.1 + "px";
	leaveGame.style.left = screenWidth * 0.75 + "px";
	leaveGame.style.margin = "0px";
	leaveGame.style.backgroundImage = "url(img/leaveLand.png)";

	method.style.width = screenWidth * 0.11 + "px";
	method.style.height = screenWidth * 0.34 + "px";
	method.style.top = (screenHeight * 0.9 - screenWidth * 0.34) + "px";
	method.style.left = screenWidth * 0.75 + "px";
	method.style.margin = "0px";
	method.style.backgroundImage = "url(img/methodLand.png)";

	changeWeapon.style.width = screenWidth * 0.2 + "px";
	changeWeapon.style.height = screenWidth * 0.2 + "px";
	changeWeapon.style.top = screenHeight * 0.12 + "px";
	changeWeapon.style.left = screenWidth * 0.15 + "px";
	changeWeapon.style.margin = "0px";

	shootGame.style.width = screenWidth * 0.4 + "px";
	shootGame.style.height = screenWidth * 0.4 + "px";
	shootGame.style.top = (screenHeight * 0.9 - screenWidth * 0.4) + "px";
	shootGame.style.left = screenWidth * 0.1 + "Px";
	shootGame.style.margin = "0px";
}