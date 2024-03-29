layui.define(["larryms", "larryElem"], function (e) {
    var y = layui.$,
		u = layui.larryElem,
		h = layui.larryms,
		m = layui.layer,
		t = "larryTab",
		c = {},
		s = 0,
		a = new Array,
		n = function () {
		    this.config = {
		        data: undefined,
		        url: undefined,
		        type: "POST",
		        cached: true,
		        top_menu: "#larryms_top_menu",
		        spreadOne: true,
		        topFilter: "TopMenu",
		        left_menu: "#larryms_left_menu",
		        leftFilter: "LarrySide",
		        tab_elem: "#larry_tab",
		        tabFilter: "larryTab",
		        tabSession: true,
		        closed: true,
		        tabMax: 25,
		        autoRefresh: false,
		        contextMenu: true,
		        tabShow: true,
		        pageEffect: 1
		    }, this.fonts = {
		        icon: undefined,
		        url: undefined,
		        online: false
		    }, this.larrymsCache = {
		        navHtml: undefined,
		        tab: undefined
		    }
		},
		r = function () {
		    y("#larryms_refresh").off("click").on("click", function () {
		        y("#larry_tab_content").children(".layui-show").children("iframe").attr("src", y("#larry_tab_content").children(".layui-show").children("iframe").attr("src"))
		    });
		    y("body").on("selectstart", function () {
		        return false
		    });
		    y("#buttonRCtrl").find("dd").each(function () {
		        y(this).on("click", function () {
		            var e = y(this).children("a").attr("data-eName");
		            tab.tabCtrl(e)
		        })
		    })
		},
		d = {
		    larryMenuClick: function () {
		        var e = "#larryms_top_menu",
					t = y(e).find("li.larryms-this").children("a").data("group");
		        var i = t !== undefined ? t : "0";
		        tab.on("click(LarrySide)", i, function (e) {
		            tab.tabAdd(e.field)
		        })
		    }
		};
    n.prototype.set = function (e) {
        var t = this;
        y.extend(true, t.config, e);
        return t
    };
    n.prototype.font = function (e) {
        var t = this;
        y.extend(true, t.fonts, e);
        h.font(t.fonts.icon, t.fonts.url, t.fonts.online)
    };
    n.prototype.menuSet = function (e) {
        var t = this;
        if (!e.hasOwnProperty("url") && !e.hasOwnProperty("data")) {
            h.error("数据源解析出错：请设置data或url参数，否则导航菜单无法正常初始化！", h.tit[1]);
            return
        }
        var i = ["data", "url", "type", "cached", "spreadOne", "top_menu", "topFilter", "left_menu", "leftFilter"];
        var a = o(e, i);
        this.set(a)
    };
    n.prototype.tabSet = function (e) {
        var t = this,
			i = ["tab_elem", "tabFilter", "tabSession", "closed", "tabMax", "autoRefresh", "tabShow"];
        var a = o(e, i);
        y.extend(t.config, a);
        return t
    };
    n.prototype.menu = function () {
        var a = this,
			e = a.config;
        if (e.data === undefined && e.url === undefined) {
            h.error("Error: 请为菜单项配置数据源[data || url]", h.tit[1]);
            return
        }
        if (e.data !== undefined && typeof e.data === "object") {
           // console.log("设置了data")
        } else {
            if (e.url !== undefined) {
                y.ajax({
                    type: e.type,
                    url: e.url,
                    async: false,
                    dataType: "json",
                    success: function (e, t, i) {
                        a.larryCompleteMenu(e.data)
                    },
                    error: function (e, t, i) {
                        h.error("larryMS Error:" + i, h.tit[1])
                    },
                    complete: function () {
                        u.render()
                    }
                })
            }
        }
        return a
    };
    n.prototype.larryCompleteMenu = function (e) {
        var t = this,
			i = t.config,
			a = f(i.top_menu, "top_menu"),
			n = f(i.left_menu, "left_menu");
        if (n !== "error") {
            if (a != "undefined") {
                var r = l(e, "on");
                layui.data("larry_menu", {
                    key: "navHtml",
                    value: r
                });
                a.html(r.top);
                n.html(r.left[0]);
                i.top_menu = a;
                i.left_menu = n
            } else {
                var r = l(e, "off");
                layui.data("larry_menu", {
                    key: "navHtml",
                    value: r
                });
                n.html(r);
                i.left_menu = n
            }
        }
    };
    n.prototype.on = function (e, t, d) {
        var i = this,
			a = i.config,
			n = p(e),
			f = t !== undefined ? t : "0";
        if (n.eventName === "click" && n.filter === a.topFilter) {
            a.left_menu.empty();
            a.left_menu.attr("data-group", f);
            a.left_menu.html(layui.data("larry_menu").navHtml.left[f]);
            u.render("nav");
            return "success"
        }
        if (n.eventName === "click" && n.filter === a.leftFilter) {
            var r = a.left_menu.find("li");
            r.each(function () {
                var o = y(this),
					e = o.find("dl"),
					t = o.find(".grandson");
                if (a.spreadOne) {
                    o.on("click", function () {
                        if (o.hasClass("larryms-nav-itemed")) {
                            o.siblings().removeClass("larryms-nav-itemed")
                        }
                    })
                }
                if (e.length > 0) {
                    e.children("dd").each(function () {
                        var o = y(this);
                        y(this).on("click", function () {
                            if (!o.hasClass("grandson")) {
                                var e = y(this).children("a"),
									t = e.data("id"),
									i = e.data("url"),
									a = e.children("i:first").data("font"),
									n = e.children("i:first").data("icon"),
									r = e.children("cite").text(),
									l = {
									    elem: e,
									    field: {
									        id: t,
									        href: i,
									        font: a,
									        icon: n,
									        title: r,
									        group: f
									    }
									};
                                d(l)
                            }
                        })
                    })
                } else {
                    o.on("click", function () {
                        var e = o.children("a"),
							t = e.data("id"),
							i = e.data("url"),
							a = e.children("i:first").data("font"),
							n = e.children("i:first").data("icon"),
							r = e.children("cite").text(),
							l = {
							    elem: e,
							    field: {
							        id: t,
							        href: i,
							        font: a,
							        icon: n,
							        title: r,
							        group: f
							    }
							};
                        d(l)
                    })
                }
            })
        }
    };
    n.prototype.tabInit = function () {
        var e = this,
			t = e.config;
        $container = f(t.tab_elem, "tab_elem");
        t.tab_elem = $container;
        c.titleBox = $container.children("#larryms_title").children("ul.larryms-tab-title");
        c.contentBox = $container.children(".larryms-tab-content");
        c.tabFilter = $container.attr("lay-filter");
        c.tabCtrBox = $container.find("#buttonRCtrl");
        return e
    };
    n.prototype.exists = function (o, d, f) {
        var s = {
            tabIndex: -1,
            pageIndex: undefined,
            pflag: 0,
            id: 0,
            pages: "nav"
        },
			e = c.titleBox === undefined ? this.tabInit() : this;
        c.titleBox.find("li").each(function (e, t) {
            var i = y(this).children("cite"),
				a = y(this).data("id"),
				n = y(this).attr("id"),
				r = y(this).data("url"),
				l = y(this).attr("lay-id");
            if (d !== undefined) {
                s.pages = "nav";
                if (i.text() === o && a === d) {
                    s.tabIndex = e
                } else if (a === undefined || a !== d) {
                    if (i.text() === o && n === "larryms_home") {
                        s.tabIndex = 0
                    }
                }
            } else {
                s.pages = "page";
                if (i.text() === o) {
                    if (r === f) {
                        s.tabIndex = e;
                        s.pflag = e;
                        s.id = l
                    } else {
                        s.pageIndex = e
                    }
                    return s
                } else {
                    //s.pageIndex = -1
                }
            }
        });
        return s
    };
    n.prototype.getTabId = function (a) {
        var n = -1,
			e = c.titleBox === undefined ? this.tabInit() : this;
        c.titleBox.find("li").each(function (e, t) {
            var i = y(this).children("cite");
            if (i.text() === a) {
                n = y(this).attr("lay-id")
            }
        });
        return n
    };
    n.prototype.getCurrentTabId = function () {
        var e = this,
			t = e.config;
        return y(t.tab_elem).find("ul.larryms-tab-title").children("li.layui-this").attr("lay-id")
    };
    n.prototype.tabAdd = function (a) {
        var e = this,
			n = e.config,
			t = "",
			r = "",
			l = e.exists(a.title, a.id, a.href);
        if (l.tabIndex == -1) {
            if (n.tabMax !== "undefined") {
                var o = c.titleBox.children("li").length,
					d = n.tabMax.tipMsg || "为了保障系统流畅运行，只允许同时打开" + n.tabMax + "个选项卡，或请设置允许新增选项卡个数";
                if (typeof n.tabMax === "number") {
                    if (o === n.tabMax) {
                        h.error(d, h.tit[1]);
                        return
                    }
                }
                if (typeof n.tabMax === "object" || typeof n.tabMax === "string") {
                    if (o === n.tabMx.max) {
                        h.error(d, h.tit[1]);
                        return
                    }
                }
            }
            if (!n.tabSession) {
               // console.log(n.tabSession);
                s++
            } else {
                e.session(function (e) {
                    var t = JSON.parse(e.getItem("tabMenu"));
                    var a = new Array;
                    for (i = 0; i < t.length; i++) {
                        a[i] = t[i]["id"]
                    }
                    s = Math.max.apply(null, a);
                    s++
                })
            }
            if (l.pageIndex == undefined) {
                if (a.font !== undefined) {
                    if (a.icon !== undefined) {
                        r += '<i class="' + a.font + " " + a.icon + '" data-icon="' + a.icon + '"></i>'
                    }
                } else {
                    r += '<i class="larry-icon ' + a.icon + '" data-icon="' + a.icon + '"></i>'
                }
                r += "<cite>" + a.title + "</cite>";
                if (n.closed) {
                    r += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + s + '">&#xe62e;</i>'
                }
                t = '<iframe src="' + a.href + '" id="ifr' + s + '" data-group="' + a.group + '" data-id="' + a.id + '" lay-id="' + s + '" name="ifr_' + s + '" class="larryms-iframe"></iframe>';
                u.tabAdd(c.tabFilter, {
                    title: r,
                    content: t,
                    id: s,
                    larryID: a.id,
                    url: a.href,
                    group: a.group,
                    pages: l.pages
                });
                e.tabChange(s);
                e.tabAuto(0);
                e.pageEffect(5, true)
            } else { }
            e.pageEffect(s, Math.ceil(Math.random() * 5));
            if (n.closed) {
                c.titleBox.find("li").children("i.layui-tab-close[data-id=" + s + "]").on("click", function () {
                    e.tabDelete(y(this).parent("li").attr("lay-id"));
                    e.tabAuto(1)
                })
            }
            if (n.tabSession) {
                e.session(function (e) {
                    var t = JSON.parse(e.getItem("tabMenu")) || [];
                    var i = {
                        title: a.title,
                        href: a.href,
                        font: a.font,
                        icon: a.icon,
                        closed: n.closed,
                        group: a.group,
                        id: s,
                        larryID: a.id
                    };
                    t.push(i);
                    e.setItem("tabMenu", JSON.stringify(t));
                    e.setItem("currentTabMenu", JSON.stringify(i))
                })
            }
        } else {
            if (!l.pflag) {
                var f = e.getTabId(a.title);
                e.tabChange(f);
                e.autoRefresh(f);
                e.tabAuto(0)
            } else {
                c.titleBox.find("li[lay-id=" + l.id + "]").click();
                e.autoRefresh(l.id);
                c.tabCtrBox.find("#tabCtrD").click();
                e.tabAuto(0)
            }
        }
    };
    n.prototype.pageEffect = function (e, t) {
        switch (t) {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break
        }
    };
    n.prototype.recoveryTab = function (e) {
        var t = this,
			i = t.config,
			a = t.exists(e.title, e.id, e.href);
        if (a.tabIndex == -1) {
            var n = "";
            if (e.font !== undefined) {
                if (e.icon !== undefined) {
                    n += '<i class="' + e.font + " " + e.icon + '" data-icon="' + e.icon + '"></i>'
                }
            } else {
                n += '<i class="larry-icon ' + e.icon + '" data-icon="' + e.icon + '"></i>'
            }
            n += "<cite>" + e.title + "</cite>";
            if (i.closed) {
                n += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + e.id + '">&#xe62e;</i>'
            }
            var r = '<iframe src="' + e.href + '" id="ifr' + e.id + '" data-group="' + e.group + '" data-id="' + e.LarryID + '" lay-id="' + e.id + '" name="ifr_' + e.id + '" class="larryms-iframe"></iframe>';
            u.tabAdd(c.tabFilter, {
                title: n,
                content: r,
                id: e.id,
                larryID: e.larryID,
                url: e.href,
                font: e.font,
                icon: e.icon,
                group: e.group,
                closed: e.closed,
                pages: a.pages
            })
        } else {
            t.session(function (e) {
                var t = JSON.parse(e.getItem("currentTabMenu"));
                if (t.id !== "0") {
                    y("#larryms_home").removeClass("layui-this")
                }
            })
        }
    };
    n.prototype.tabChange = function (n, e, t) {
        var i = this,
			e = e || "off",
			t = t || "nav";
        if (i.config.tabSession) {
            i.session(function (e) {
                var t = JSON.parse(e.getItem("currentTabMenu"));
                if (!t) return false;
                if (t.id != n) {
                    var i = JSON.parse(e.getItem("tabMenu"));
                    for (var a = 0; a < i.length; a++) {
                        if (i[a].id == n) {
                            e.setItem("currentTabMenu", JSON.stringify(i[a]));
                            break
                        }
                    }
                }
            })
        }
        if (e === "on") {
            var r = y(i.config.top_menu),
				a = y(i.config.left_menu),
				l = y('#larry_tab_title li[lay-id="' + n + '"]'),
				o = l.data("id"),
				d = l.data("group");
            if (o === "larryms-home") {
                var f = l.children("cite").text()
            }
            var s = a.find("a");
            if (s.length == 0) {
                r.children("li").eq(d).click();
                l.click()
            }
            y.each(s, function (e, t) {
                var i = y(t).data("group"),
					a = y(t).data("id"),
					n = y(".larryms-nav-tree");
                if (i !== undefined && i === d) {
                    c(t, a, n, o, f)
                } else if (i !== d && i !== undefined) {
                    r.children("li").eq(d).click();
                    l.click();
                    c(t, a, n, o, f)
                }
            });

            function c(e, t, i, a, n) {
                if (t !== undefined && t === a) {
                    i.find(".larryms-this").removeClass("larryms-this");
                    i.find(".larryms-nav-itemed").removeClass("larryms-nav-itemed");
                    i.find(".grandsoned").removeClass("grandsoned");
                    if (y(e).parents("dd").hasClass("grandson")) {
                        y(e).parents("li").addClass("larryms-nav-itemed");
                        y(e).parents("dd.grandson").addClass("grandsoned");
                        y(e).parent("dd").addClass("larryms-this")
                    } else if (!y(e).parents("dd").hasClass("grandson") && y(e).parent("dd").length) {
                        y(e).parents("li").addClass("larryms-nav-itemed");
                        y(e).parent("dd").addClass("larryms-this")
                    } else {
                        y(e).parent("li").addClass("larryms-this")
                    }
                    return false
                } else if (a === "larryms-home" && t !== undefined) {
                    if (y(e).children("cite").text() === n) {
                        i.find(".larryms-this").removeClass("larryms-this");
                        y(e).parent("li").addClass("larryms-this");
                        return false
                    }
                }
            }
        }
        u.tabChange(i.config.tabFilter, n, t).render()
    };
    n.prototype.tabDelete = function (n) {
        var e = this;
        if (e.config.tabSession) {
            e.session(function (e) {
                var t = JSON.parse(e.getItem("tabMenu"));
                for (var i = 0; i < t.length; i++) {
                    if (t[i].id == n) {
                        t.splice(i, 1)
                    }
                }
                e.setItem("tabMenu", JSON.stringify(t));
                var a = JSON.parse(e.getItem("currentTabMenu"));
                if (a.id == n) {
                    e.setItem("currentTabMenu", JSON.stringify(t.pop()))
                }
            })
        }
        var t = u.tabDelete(e.config.tabFilter, n).render();
        e.tabChange(t.larryElem.LarryLayID, "on");
        e.tabAuto(1)
    };
    n.prototype.tabAuto = function (u) {
        var e = this;
        y("#larryms_title").each(function () {
            var o = y(this),
				d = o.children(".larryms-tab-title"),
				f = d.find(".layui-this"),
				e = d.children("#larryms_home"),
				s = o.find(".larryms-btn-default"),
				c = 0;
            d.find("li").each(function (e, t) {
                c += parseInt(y(t).outerWidth(true))
            });
            if (!d.find("li")[0]) return;
            y(window).off("resize").on("resize", function () {
                var t = parseInt(o.outerWidth(true) - 263),
					i = parseInt(f.outerWidth(true)),
					e = parseInt(f.position().left + 1),
					a = parseInt(d.css("marginLeft")),
					n = e + a,
					r = t - c;
                if (c > t) {
                    s.removeClass("hide");
                    o.addClass("larryms-tab-auto");
                    if (a + e <= 0) {
                        r = 0 - e
                    } else {
                        var l = t + Math.abs(a) - e - i;
                        if (l <= 0) {
                            r = t - e - i
                        } else {
                            r = t - e - i;
                            if (u == 0) {
                                if (r > 0) {
                                    r = 0
                                }
                            } else {
                                if (r > 0) {
                                    r = 0
                                }
                            }
                        }
                    }
                    d.css({
                        marginLeft: r
                    })
                } else {
                    s.addClass("hide");
                    o.removeClass("larryms-tab-auto");
                    d.css({
                        marginLeft: 0
                    })
                }
                y(".larryms-btn-default").off("click").on("click", function () {
                    if (c > t) {
                        var e = parseInt(d.css("marginLeft"));
                        if (y(this).attr("id") === "goLeft") {
                            if (Math.abs(e) !== 0) {
                                if (e + t < 0) {
                                    d.css({
                                        marginLeft: e + t
                                    })
                                } else {
                                    d.css({
                                        marginLeft: 0
                                    })
                                }
                            } else {
                                m.tips("已滚动到最左侧了", y(this), {
                                    tips: [1, "#FF5722"]
                                })
                            }
                        }
                        if (y(this).attr("id") === "goRight") {
                            if (Math.abs(e) !== c - t) {
                                if (Math.abs(e) + t >= c - i) {
                                    d.css({
                                        marginLeft: t - c
                                    })
                                } else {
                                    d.css({
                                        marginLeft: e - t / 2
                                    })
                                }
                            } else {
                                m.tips("已滚动到最右侧了", y(this), {
                                    tips: [1, "#FF5722"]
                                })
                            }
                        }
                    }
                })
            }).resize()
        })
    };
    n.prototype.tabCtrl = function (e) {
        var i = this,
			t = i.config,
			a = i.getCurrentTabId();
        switch (e) {
            case "positionCurrent":
                var n = y(t.tab_elem).find("ul.layui-tab-title").children("li.layui-this"),
                    r = y(t.tab_elem).find('iframe[lay-id="' + a + '"]').attr("src"),
                    l = n.children("i:first").data("font"),
                    o = n.children("i:first").data("icon"),
                    d = n.data("group"),
                    f = n.data("id"),
                    s = {
                        title: n.children("cite").text(),
                        href: r,
                        font: l,
                        icon: o,
                        group: d,
                        id: f
                    };
                i.tabAdd(s);
                i.tabAuto(0);
                break;
            case "closeCurrent":
                if (a > 0) {
                    i.tabDelete(a)
                } else {
                    h.error("默认首页不能关闭的哦！", h.tit[0])
                }
                break;
            case "closeOther":
                var c = y(t.tab_elem).find("ul.layui-tab-title").children("li"),
                    u = c.length;
                if (u > 2) {
                    c.each(function () {
                        var e = y(this),
                            t = e.attr("lay-id");
                        if (t !== a && t !== undefined && t !== "0") {
                            i.tabDelete(t)
                        }
                    })
                } else if (u == 2) {
                    h.error("【默认首页】不能关闭，当前暂无其他可关闭选项卡！", h.tit[0])
                } else {
                    h.error("当前暂无其他可关闭选项卡！", h.tit[0])
                }
                break;
            case "closeAll":
                var c = y(t.tab_elem).find("ul.layui-tab-title").children("li"),
                    u = c.length;
                if (u > 1) {
                    c.each(function () {
                        var e = y(this),
                            t = e.attr("lay-id");
                        if (t > 0) {
                            i.tabDelete(t)
                        }
                    })
                } else {
                    h.error("当前暂无其他可关闭选项卡！", h.tit[0])
                }
                break;
            case "refreshAdmin":
                h.confirm("您确定要重新加载系统吗！", function () {
                    location.reload()
                }, function () {
                    return
                });
                break
        }
    };
    n.prototype.render = function () {
        var l = this,
			o = l.config,
			e = o.top_menu !== undefined ? o.top_menu : "#larryms_top_menu",
			i = o.left_menu !== undefined ? o.left_menu : "#larryms_left_menu";
        if (o.top_menu !== undefined) {
            y(e).on("click", "li", function () {
                var e = y(this),
					t = e.children("a").data("group");
                tab.on("click(" + o.topFilter + ")", t);
                y(i).off("mouseenter", d.larryMenuClick).one("mouseenter", d.larryMenuClick)
            })
        }
        y(i).one("mouseenter", d.larryMenuClick);
        var t = layui.data("larryms").systemSet == undefined ? true : layui.data("larryms").systemSet.tabCache;
        if (!t) {
            o.tabSession = t;
            sessionStorage.removeItem("tabMenu");
            sessionStorage.removeItem("currentTabMenu")
        }
        l.session(function (e) {
            if (o.tabSession) {
                if (e.getItem("tabMenu")) {
                    var t = JSON.parse(e.getItem("tabMenu"));
                    y.each(t, function (e, t) {
                        l.recoveryTab(t)
                    });
                    var i = JSON.parse(e.getItem("currentTabMenu"));
                    if (i) {
                        l.tabChange(i.id, "on");
                        l.tabAuto(1)
                    } else {
                        l.tabChange(t[0].id, "on");
                        l.tabAuto(1)
                    }
                    s = t.length
                } else {
                    var a = y("#larry_tab_title li").eq(0);
                    if (a.length) {
                        var n = JSON.parse(e.getItem("tabMenu")) || [];
                        var r = {
                            font: a.children("i").data("font"),
                            icon: a.children("i").data("icon"),
                            title: a.find("cite").text() == undefined ? a.find("cite").text() : "首页",
                            href: a.data("url"),
                            id: a.attr("lay-id"),
                            LarryID: a.data("id"),
                            closed: false
                        };
                        n.push(r);
                        e.setItem("tabMenu", JSON.stringify(n));
                        e.setItem("currentTabMenu", JSON.stringify(r))
                    }
                }
            }
        });
        y("#larry_tab").on("click", "#larry_tab_title li i.layui-tab-close", function () {
            if (o.closed) {
                l.tabDelete(y(this).parent("li").attr("lay-id"));
                l.tabAuto(1)
            }
        });
        y("#larry_tab").on("click", "#larry_tab_title li", function () {
            var e = y(this).attr("lay-id");
            l.tabChange(e, "on");
            l.autoRefresh(e)
        })
    };

    function l(e, t) {
        if (t == "on") {
            var i = {
                top: "",
                left: []
            };
            for (var a = 0; a < e.length; a++) {
                if (a == 0) {
                    i.top += '<li class="larryms-nav-item larryms-this">'
                } else {
                    i.top += '<li class="larryms-nav-item">'
                }
                i.top += '<a data-group="' + a + '" data-id="larry-' + e[a].id + '" title="' + e[a].title + '">';
                i.top += '<i class="' + e[a].font + " " + e[a].icon + '" data-icon="' + e[a].icon + '" data-font="' + e[a].font + '"></i>';
                i.top += "<cite>" + e[a].title + "</cite>";
                i.top += "</a>";
                i.top += "</li>";
                if (e[a].children !== undefined && e[a].children !== null && e[a].children.length > 0) {
                    i.left[a] = "";
                    var n = "";
                    for (var r = 0; r < e[a].children.length; r++) {
                        n = e[a].children[r];
                        if (a == 0 && r == 0) {
                            if (n.children !== undefined && n.children !== null && n.children.length > 0) {
                                i.left[a] += '<li class="larryms-nav-item larryms-nav-itemed">'
                            } else {
                                i.left[a] += '<li class="larryms-nav-item larryms-this">'
                            }
                        } else if (n.spread && r != 0) {
                            i.left[a] += '<li class="larryms-nav-item larryms-nav-itemed">'
                        } else {
                            i.left[a] += '<li class="larryms-nav-item">'
                        }
                        if (n.children !== undefined && n.children !== null && n.children.length > 0) {
                            i.left[a] += '<a data-group="' + a + '" data-id="larry-' + n.id + '" title="' + n.title + '">';
                            if (n.icon !== undefined && n.icon !== "") {
                                if (n.font !== undefined && n.font !== "") {
                                    i.left[a] += '<i class="' + n.font + " " + n.icon + '" data-icon="' + n.icon + '" data-font="' + n.font + '"></i>'
                                } else {
                                    i.left[a] += '<i class="larry-icon" data-icon="' + n.icon + '" data-font="larry-icon"></i>'
                                }
                            }
                            i.left[a] += "<cite>" + n.title + "</cite>";
                            i.left[a] += '<span class="larryms-nav-more"></span>';
                            i.left[a] += "</a>";
                            i.left[a] += '<dl class="larryms-nav-child">';
                            var l = "";
                            for (f = 0; f < n.children.length; f++) {
                                l = n.children[f];
                                if (l.children !== undefined && l.children !== null && l.children.length > 0) {
                                    i.left[a] += '<dd class="grandson">';
                                    i.left[a] += '<a data-group="' + a + '" data-id="larry-' + l.id + '" title="' + l.title + '">';
                                    if (l.icon !== undefined && l.icon !== "") {
                                        if (l.font !== undefined && l.font !== "") {
                                            i.left[a] += '<i class="' + l.font + " " + l.icon + '" data-icon="' + l.icon + '" data-font="' + l.font + '"></i>'
                                        } else {
                                            i.left[a] += '<i class="larry-icon" data-icon="' + l.icon + '" data-font="larry-icon"></i>'
                                        }
                                    }
                                    i.left[a] += "<cite>" + l.title + "</cite>";
                                    i.left[a] += '<span class="larryms-nav-more"></span>';
                                    i.left[a] += "</a>";
                                    i.left[a] += '<dl class="larryms-nav-child">';
                                    var o = "";
                                    for (var d = 0; d < l.children.length; d++) {
                                        o = l.children[d];
                                        i.left[a] += "<dd>";
                                        i.left[a] += o.url !== undefined && o.url !== "" ? '<a data-group="' + a + '" data-url="' + o.url + '" data-id="larry-' + o.id + '" title="' + o.title + '">' : '<a data-group="' + a + '" data-id="larry-' + o.id + '" title="' + o.title + '">';
                                        if (o.icon !== undefined && o.icon !== "") {
                                            if (o.font !== undefined && o.font !== "") {
                                                i.left[a] += '<i class="' + o.font + " " + o.icon + '" data-icon="' + o.icon + '" data-font="' + o.font + '"></i>'
                                            } else {
                                                i.left[a] += '<i class="larry-icon" data-icon="' + o.icon + '" data-font="larry-icon"></i>'
                                            }
                                        }
                                        i.left[a] += "<cite>" + o.title + "</cite>";
                                        i.left[a] += "</a>"
                                    }
                                    i.left[a] += "</dl>"
                                } else {
                                    i.left[a] += "<dd>";
                                    i.left[a] += l.url !== undefined && l.url !== "" ? '<a data-group="' + a + '" data-url="' + l.url + '" data-id="larry-' + l.id + '" title="' + l.title + '">' : '<a data-group="' + a + '" data-id="larry-' + l.id + '" title="' + l.title + '">';
                                    if (l.icon !== undefined && l.icon !== "") {
                                        if (l.font !== undefined && l.font !== "") {
                                            i.left[a] += '<i class="' + l.font + " " + l.icon + '" data-icon="' + l.icon + '" data-font="' + l.font + '"></i>'
                                        } else {
                                            i.left[a] += '<i class="larry-icon" data-icon="' + l.icon + '" data-font="larry-icon"></i>'
                                        }
                                    }
                                    i.left[a] += "<cite>" + l.title + "</cite>";
                                    i.left[a] += "</a>"
                                }
                                i.left[a] += "</dd>"
                            }
                            i.left[a] += "</dl>"
                        } else {
                            i.left[a] += n.url !== undefined && n.url !== "" ? '<a data-group="' + a + '" data-url="' + n.url + '" data-id="larry-' + n.id + '" title="' + n.title + '">' : '<a data-group="' + a + '" data-id=larry-' + n.id + '" title="' + n.title + '">';
                            if (n.icon !== undefined && n.icon !== "") {
                                if (n.font !== undefined && n.font !== "") {
                                    i.left[a] += '<i class="' + n.font + " " + n.icon + '" data-icon="' + n.icon + '" data-font="' + n.font + '"></i>'
                                } else {
                                    i.left[a] += '<i class="larry-icon" data-icon="' + n.icon + '" data-font="larry-icon"></i>'
                                }
                            }
                            i.left[a] += "<cite>" + n.title + "</cite>";
                            i.left[a] += "</a>"
                        }
                        i.left[a] += "</li>"
                    }
                }
            }
        } else {
            var i = "";
            for (var a = 0; a < e.length; a++) {
                if (a == 0) {
                    i += '<li class="larryms-nav-item">'
                } else {
                    i += '<li class="larryms-nav-item">'
                }
                if (e[a].children !== undefined && e[a].children !== null && e[a].children.length > 0) {
                    i += '<a data-id="larry-' + e[a].id + '">';
                    if (e[a].icon !== undefined && e[a].icon !== null) {
                        if (e[a].font !== undefined && e[a].font !== null) {
                            i += '<i class="' + e[a].font + " " + e[a].icon + '" data-icon="' + e[a].icon + '" data-font="' + e[a].font + '"></i>'
                        } else {
                            i += '<i class="larry-icon" data-icon="' + e[a].icon + '" data-font="larry-icon"></i>'
                        }
                    }
                    i += "<cite>" + e[a].title + "</cite>";
                    i += '<span class="larryms-nav-more"></span>';
                    i += "</a>";
                    i += '<dl class="larryms-nav-child">';
                    var n = "";
                    for (var r = 0; r < e[a].children.length; r++) {
                        n = e[a].children[r];
                        if (n.children !== undefined && n.children !== null && n.children.length > 0) {
                            i += '<dd class="grandson">';
                            i += '<a data-id="larry-' + n.id + '">';
                            if (n.icon !== undefined && n.icon !== "") {
                                if (n.font !== undefined && n.font !== "") {
                                    i += '<i class="' + n.font + " " + n.icon + '" data-icon="' + n.icon + '" data-font="' + n.font + '"></i>'
                                } else {
                                    i += '<i class="larry-icon" data-icon="' + n.icon + '" data-font="larry-icon"></i>'
                                }
                            }
                            i += "<cite>" + n.title + "</cite>";
                            i += '<span class="larryms-nav-more"></span>';
                            i += "</a>";
                            i += '<dl class="larryms-nav-child">';
                            var l = "";
                            for (var f = 0; f < n.children.length; f++) {
                                l = n.children[f];
                                i += "<dd>";
                                i += l.url !== undefined && l.url !== "" ? '<a data-url="' + l.url + '" data-id="larry-' + l.id + '">' : '<a data-id="larry-' + l.id + '">';
                                if (l.icon !== undefined && l.icon !== "") {
                                    if (l.font !== undefined && l.font !== "") {
                                        i += '<i class="' + l.font + " " + l.icon + '" data-icon="' + l.icon + '" data-font="' + l.font + '"></i>'
                                    } else {
                                        i += '<i class="larry-icon" data-icon="' + l.icon + '" data-font="larry-icon"></i>'
                                    }
                                }
                                i += "<cite>" + l.title + "</cite>";
                                i += "</a>";
                                i += "</dd>"
                            }
                            i += "</dl>"
                        } else {
                            i += "<dd>";
                            i += n.url !== undefined && n.url !== "" ? '<a data-url="' + n.url + '" data-id="larry-' + n.id + '">' : '<a data-id="larry-' + n.id + '">';
                            if (n.icon !== undefined && n.icon !== "") {
                                if (n.font !== undefined && n.font !== "") {
                                    i += '<i class="' + n.font + " " + n.icon + '" data-icon="' + n.icon + '" data-font="' + n.font + '"></i>'
                                } else {
                                    i += '<i class="larry-icon" data-icon="' + n.icon + '" data-font="larry-icon"></i>'
                                }
                            }
                            i += "<cite>" + n.title + "</cite>";
                            i += "</a>"
                        }
                        i += "</dd>"
                    }
                    i += "</dl>"
                } else {
                    i += e[a].url !== undefined && e[a].url !== "" ? '<a data-url="' + e[a].url + '" data-id="larry-' + e[a].id + '">' : '<a data-id="larry-' + e[a].id + '">';
                    if (e[a].icon !== undefined && e[a].icon !== "") {
                        if (e[a].font !== undefined && e[a].font !== "") {
                            i += '<i class="' + e[a].font + " " + e[a].icon + '" data-icon="' + e[a].icon + '" data-font="' + e[a].font + '"></i>'
                        } else {
                            i += '<i class="larry-icon" data-icon="' + e[a].icon + '" data-font="larry-icon"></i>'
                        }
                    }
                    i += "<cite>" + e[a].title + "</cite>";
                    i += "</a>"
                }
                i += "</li>"
            }
        }
        return i
    }
    n.prototype.autoRefresh = function (e) {
        var t = this;
        var i = t.config;
        if (i.autoRefresh) {
            var a = c.contentBox.find(".layui-tab-item").children("iframe[data-id=" + e + "]");
            a.attr("src", a.attr("src"))
        }
    };
    n.prototype.session = function (e) {
        if (!window.sessionStorage) {
            return false
        }
        e(window.sessionStorage)
    };

    function o(e, t) {
        var i = {};
        for (var a in e) {
            if (y.inArray(a, t)) {
                i[a] = e[a]
            }
        }
        return i
    }
    function f(e, t) {
        var i;
        if (t == "top_menu") {
            if (e === undefined) {
                i = "undefined";
                return i
            } else {
                if (typeof e !== "string" && typeof e !== "object") {
                    h.error(t + "参数未定义或设置出错", h.tit[1]);
                    i = "error";
                    return i
                }
            }
        } else {
            if (e !== undefined) {
                if (typeof e !== "string" && typeof e !== "object") {
                    h.error(t + "参数未定义或设置出错", h.tit[1]);
                    i = "error";
                    return i
                }
            } else {
                h.error("未设置【" + t + "】参数，请检查参数配置项", h.tit[1]);
                i = "error";
                return i
            }
        }
        if (typeof e === "string") {
            i = y("" + e + "")
        }
        if (typeof e === "object") {
            i = e
        }
        if (i.length === 0) {
            h.error("您虽然设置了" + t + "参数，但DOM中却查找不到定义的【" + e + "】元素,请仔细检查", h.tit[1]);
            i = "error";
            return i
        }
        var a = i.attr("lay-filter");
        if (a === undefined || a === "") {
            h.error("请为【" + e + "】容器设置lay-filter属性", h.error[0])
        }
        return i
    }
    function p(e) {
        var t = {
            eventName: "",
            filter: ""
        };
        if (typeof e !== "string") {
            h.error("事件名设置错误，请参考API文档.", h.tit[2]);
            return
        }
        var i = e.indexOf("(");
        t.eventName = e.substr(0, i);
        t.filter = e.substring(i + 1, e.indexOf(")"));
        return t
    }
    n.prototype.test = function () {
       // console.log("LarryTab-test")
    };
    window.tab = new n;
    if (window.top == window.self) {
        tab.render();
        r()
    }
    e(t, function (e) {
        return tab.set(e)
    })
});