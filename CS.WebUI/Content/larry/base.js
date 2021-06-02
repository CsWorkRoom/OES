layui.extend({//设定模块别名
	larryms: "larry/larryms",//larryms.js
	larryMenu: "larry/larryMenu"//larryMenu.js
}).define(["jquery", "larryms", "larryMenu", "layer"], function (e) {
	"use strict";
	var l = layui.$, r = layui.larryms, i = layui.larryMenu(), t = layui.device();
	var n = function () {
		this.config = {
			icon: "larry",
			url: "//at.alicdn.com/t/font_477590_wtu39l8gjfjyk3xr.css",
			online: true
		}, this.screen = function () {
			var e = l(window).width();
			if (e >= 1200) {
				return 4
			} else if (e >= 992) {
				return 3
			} else if (e >= 768) {
				return 2
			} else {
				return 1
			}
		}
	};
	var c = {
		panel: function () {
			l(".larry-panel .tools").off("click").on("click", function () {
				if (l(this).hasClass("larry-unfold1")) {
					l(this).addClass("larry-fold9").removeClass("larry-unfold1");
					l(this).parent(".larry-panel-header").siblings(".larry-panel-body").slideToggle()
				} else {
					l(this).addClass("larry-unfold1").removeClass("larry-fold9");
					l(this).parent(".larry-panel-header").siblings(".larry-panel-body").slideToggle()
				}
			});
			l(".larry-panel .close").off("click").on("click", function () {
				l(this).parents(".larry-panel").parent().fadeOut()
			})
		}, addTab: function (e) {
			if (window.top !== window.self) {
				//console.log(top.tab);
				top.tab.tabAdd(e)
			} else {
				window.location.href = e.href
			}
		}, RightMenu: function (e) {
			i.ContentMenu(e, {name: "body"}, l("body"));
			if (top == self) {
				l("#larry_tab_content").mouseenter(function () {
					i.remove()
				})
			} else {
				l("#larry_tab_content", parent.document).mouseout(function () {
					i.remove()
				})
			}
		}
	};
	n.prototype.init = function () {
		var e = this, t = e.config;
		r.fontset({icon: t.icon, url: t.url, online: t.online});
		layui.config({base: layui.cache.base + "larry/"});
		if (layui.cache.page) {
			layui.cache.page = layui.cache.page.split(",");
			if (l.inArray("larry", layui.cache.page) === -1) {
				var n = {};
				layui.cache.mods = layui.cache.mods === undefined ? "larryms" : layui.cache.mods;
				layui.cache.path = layui.cache.path === undefined ? layui.cache.mods + "/" : layui.cache.path;
				for (var a = 0; a < layui.cache.page.length; a++) {
					n[layui.cache.page[a]] = layui.cache.path + layui.cache.page[a]
				}
				layui.extend(n);
				layui.use(layui.cache.page)
			}
		}
		if (layui.cache.rightMenu !== false && layui.cache.rightMenu !== "custom") {
			c.RightMenu([[{
				text: "刷新当前页", func: function () {
					if (top == self) {
						document.location.reload()
					} else {
						l(".layui-tab-content .layui-tab-item", parent.document).each(function () {
							if (l(this).hasClass("layui-show")) {
								l(this).children("iframe").attr("src", l(this).children("iframe").attr("src"))
							}
						})
					}
				}
			}, {
				text: "重载主框架", func: function () {
					top.document.location.reload()
				}
			}, {
				text: "设置系统主题", func: function () {
					if (top.document.getElementById("larryTheme") !== null) {
						top.document.getElementById("larryTheme").click()
					} else {
						r.error("当前页面不支持主题设置或请登陆系统后设置系统主题", r.tit[0])
					}
				}
			}, {
				text: "选项卡常用操作", data: [[{
					text: "定位当前选项卡", func: function () {
						if (top.document.getElementById("tabCtrD") !== null) {
							top.document.getElementById("tabCtrD").click()
						} else {
							r.error("请先登陆系统，此处无选项卡操作", r.tit[0])
						}
					}
				}, {
					text: "关闭当前选项卡", func: function () {
						if (top.document.getElementById("tabCtrA") !== null) {
							top.document.getElementById("tabCtrA").click()
						} else {
							r.error("请先登陆系统，此处无选项卡操作", r.tit[0])
						}
					}
				}, {
					text: "关闭其他选项卡", func: function () {
						if (top.document.getElementById("tabCtrB") !== null) {
							top.document.getElementById("tabCtrB").click()
						} else {
							r.error("请先登陆系统，此处无选项卡操作", r.tit[0])
						}
					}
				}, {
					text: "关闭全部选项卡", func: function () {
						if (top.document.getElementById("tabCtrC") !== null) {
							top.document.getElementById("tabCtrC").click()
						} else {
							r.error("请先登陆系统，此处无选项卡操作", r.tit[0])
						}
					}
				}]]
			}, {
				text: "清除缓存", func: function () {
					top.document.getElementById("clearCached").click()
				}
			}]
				// , [{
				// text: "访问售后官网", func: function () {
				// 	top.window.open("https://www.#.com")
				// }}]
			])
		} else if (layui.cache.rightMenu === false) {
			i.remove();
			i = null
		}
	};
	n.prototype.panel = function () {
		c.panel()
	};
	n.prototype.render = n.prototype.init;
	var a = new n;
	a.render();
	e("larry", a)
});