layui.define("jquery",
function (s) {
    var d = layui.$,
    m = layui.device(),
    h = "larryElem",
    g = "layui-this",
    b = "larryms-this",
    c = "layui-show",
    j = function () {
        this.config = {},
        this.LarryLayID = ""
    };
    var r = ".larryms-nav",
    t = "larryms-nav-item",
    o = "larryms-nav-bar",
    k = "larryms-nav-tree",
    e = "larryms-nav-child",
    f = "larryms-nav-more",
    p = "layui-anim lauio-anim-upbit",
    l = "grandson",
    q = {
        clickThis: function () {
            var x = d(this),
            v = x.parents(r),
            w = v.attr("lay-filter"),
            y = x.find("a"),
            u = typeof x.attr("lay-unselect") === "string";
            if (x.find("." + e)[0]) {
                return
            }
            if (!(y.attr("href") !== "javascript:;" && y.attr("target") === "_blank") && !u) {
                v.find("." + b).removeClass(b);
                x.addClass(b)
            }
            layui.event.call(this, h, "nav(" + w + ")", x)
        },
        clickChild: function () {
            var w = d(this),
            u = w.parents(r),
            v = u.attr("lay-filter");
            if (!w.hasClass("grandson")) {
                u.find("." + b).removeClass(b);
                w.addClass(b)
            } else {
                return
            }
            layui.event.call(this, h, "nav(" + v + ")", w)
        },
        clickGrandson: function () {
            var w = d(this),
            u = w.parents(r),
            v = u.attr("lay-filter");
            u.find("." + b).removeClass(b);
            w.addClass(b);
            layui.event.call(this, h, "nav(" + v + ")", w)
        },
        showChild: function () {
            var w = d(this),
            u = w.parents(r),
            v = w.parent(),
            x = w.siblings("." + e);
            if (u.hasClass(k)) {
                x.removeClass(p);
                v[x.css("display") === "none" ? "addClass" : "removeClass"](t + "ed")
            }
        },
        showGrandson: function () {
            var w = d(this),
            u = w.parents(r),
            v = w.parent(),
            x = w.siblings("." + e);
            if (u.hasClass(k)) {
                x.removeClass(p);
                v[x.css("display") === "none" ? "addClass" : "removeClass"](l + "ed")
            }
        },
        showFoldInfo: function () {
            var w = d(this),
            u = w.parents(r),
            v = w.parent();
            if (d("#larry_layout").hasClass("larryms-fold")) {
                if (u.hasClass(k)) {
                    layer.tips(w.children("a").text(), w)
                }
            }
        },
        tabClick: function (A, v, B) {
            var y = B || d(this),
            v = v || y.parent().children("li").index(y),
            u = y.parents(".layui-tab").eq(0),
            x = u.children(".layui-tab-content").children(".layui-tab-item"),
            z = y.find("a"),
            w = u.attr("lay-filter");
            if (!(z.attr("href") !== "javascript:;" && z.attr("target") === "_blank")) {
                y.addClass(g).siblings().removeClass(g);
                x.eq(v).addClass(c).siblings().removeClass(c)
            }
            layui.event.call(this, h, "tab(" + w + ")", {
                elem: u,
                index: v
            })
        },
        tabDelete: function (B, A) {
            var u = A || d(this).parent(),
            w = u.index(),
            v = u.parents(".layui-tab").eq(0),
            z = v.children(".layui-tab-content").children(".layui-tab-item"),
            y = v.attr("lay-filter"),
            x = u.attr("lay-id");
            if (u.hasClass(g)) {
                if (u.next()[0]) {
                    q.tabClick.call(u.next()[0], null, w + 1);
                    x = u.next().attr("lay-id")
                } else {
                    if (u.prev()[0]) {
                        q.tabClick.call(u.prev()[0], null, w - 1);
                        x = u.prev().attr("lay-id")
                    }
                }
            }
            u.remove();
            z.eq(w).remove();
            layui.event.call(this, h, "tabDelete(" + y + ")", {
                elem: v,
                index: w
            });
            return x
        },
        collapse: function () {
            var A = d(this),
            z = A.find(".layui-colla-icon"),
            w = A.siblings(".layui-colla-content"),
            v = A.parents(".layui-collapse").eq(0),
            y = v.attr("lay-filter"),
            x = w.css("display") === "none";
            if (typeof v.attr("lay-accordion") === "string") {
                var u = v.children(".layui-colla-item").children("." + c);
                u.siblings(".layui-colla-title").children(".layui-colla-icon").html("&#xe73c;");
                u.removeClass(c)
            }
            w[x ? "addClass" : "removeClass"](c);
            z.html(x ? "&#xe73b;" : "&#xe73c;");
            layui.event.call(this, h, "collapse(" + y + ")", {
                title: A,
                content: w,
                show: x
            })
        }
    };
    j.prototype.init = function (w, v) {
        var y = this,
        x = function () {
            return v ? ('[lay-filter="' + v + '"]') : ""
        }(),
        u = {
            nav: function () {
                var B = 200,
                D = {},
                C = {},
                A = {},
                z = function (F, H, E) {
                    var G = d(this),
                    I = G.find("." + e);
                    if (H.hasClass(k)) {
                        F.css({
                            top: G.position().top,
                            height: G.children("a").height(),
                            opacity: 1
                        })
                    } else {
                        I.addClass(p);
                        F.css({
                            left: G.position().left + parseFloat(G.css("marginLeft")),
                            top: G.position().top + G.height() - F.height()
                        });
                        D[E] = setTimeout(function () {
                            F.css({
                                width: G.width(),
                                opacity: 1
                            })
                        },
                        m.ie && m.ie < 10 ? 0 : B);
                        clearTimeout(A[E]);
                        if (I.css("display") === "block") {
                            clearTimeout(C[E])
                        }
                        C[E] = setTimeout(function () {
                            I.addClass(c);
                            G.find("." + f).addClass(f + "d")
                        },
                        300)
                    }
                };
                d(r).each(function (E) {
                    var G = d(this),
                    F = d('<span class="' + o + '"></span>'),
                    I = G.find("." + t),
                    H = G.find("." + l);
                    if (!G.find("." + o)[0]) {
                        G.append(F);
                        I.on("mouseenter",
                        function () {
                            z.call(this, F, G, E)
                        }).on("mouseleave",
                        function () {
                            if (!G.hasClass(k)) {
                                clearTimeout(C[E]);
                                C[E] = setTimeout(function () {
                                    G.find("." + e).removeClass(c);
                                    G.find("." + f).removeClass(f + "d")
                                },
                                300)
                            }
                        });
                        G.on("mouseleave",
                        function () {
                            clearTimeout(D[E]);
                            A[E] = setTimeout(function () {
                                if (G.hasClass(k)) {
                                    F.css({
                                        height: 0,
                                        top: F.position().top + F.height() / 2,
                                        opacity: 0
                                    })
                                } else {
                                    F.css({
                                        width: 0,
                                        left: F.position().left + F.width() / 2,
                                        opacity: 0
                                    })
                                }
                            },
                            B)
                        })
                    }
                    I.each(function () {
                        var J = d(this),
                        L = J.find("." + e);
                        if (L.find("." + l).length > 0) {
                            H.each(function () {
                                var M = d(this),
                                O = M.find("." + e);
                                if (O[0] && !M.find("." + f)[0]) {
                                    var N = M.children("a");
                                    N.append('<span class="' + f + '"></span>')
                                }
                                M.children("a").off("click", q.showGrandson).on("click", q.showGrandson);
                                O.children("dd").off("click", q.clickGrandson).on("click", q.clickGrandson)
                            })
                        }
                        if (L[0] && !J.find("." + f)[0]) {
                            var K = J.children("a");
                            K.append('<span class="' + f + '"></span>')
                        }
                        J.off("click", q.clickThis).on("click", q.clickThis);
                        J.children("a").off("click", q.showChild).on("click", q.showChild);
                        L.children("dd").off("click", q.clickChild).on("click", q.clickChild);
                        J.off("mouseenter", q.showFoldInfo).on("mouseenter", q.showFoldInfo);
                        L.children("dd").off("mouseenter", q.showFoldInfo).on("mouseenter", q.showFoldInfo)
                    })
                })
            },
            breadcrumb: function () {
                var z = ".layui-breadcrumb";
                d(z + x).each(function () {
                    var C = d(this),
                    B = "lay-separator",
                    D = C.attr(B) || "/",
                    A = C.find("a");
                    if (A.next("span[" + B + "]")[0]) {
                        return
                    }
                    A.each(function (E) {
                        if (E === A.length - 1) {
                            return
                        }
                        d(this).after("<span " + B + ">" + D + "</span>")
                    });
                    C.css("visibility", "visible")
                })
            },
            progress: function () {
                var z = "layui-progress";
                d("." + z + x).each(function () {
                    var B = d(this),
                    C = B.find(".layui-progress-bar"),
                    A = C.attr("lay-percent");
                    C.css("width",
                    function () {
                        return /^.+\/.+$/.test(A) ? (new Function("return " + A)() * 100) + "%" : A
                    }());
                    if (B.attr("lay-showPercent")) {
                        setTimeout(function () {
                            C.html('<span class="' + z + '-text">' + A + "</span>")
                        },
                        350)
                    }
                })
            },
            collapse: function () {
                var z = "layui-collapse";
                d("." + z + x).each(function () {
                    var A = d(this).find(".layui-colla-item");
                    A.each(function () {
                        var E = d(this),
                        D = E.find(".layui-colla-title"),
                        B = E.find(".layui-colla-content"),
                        C = B.css("display") === "none";
                        D.find(".layui-colla-icon").remove();
                        D.append('<i class="layui-icon layui-colla-icon">' + (C ? "" : "&#xe73b;") + "</i>");
                        D.off("click", q.collapse).on("click", q.collapse)
                    })
                })
            }
        };
        return u[w] ? u[w]() : layui.each(u,
        function (z, A) {
            A()
        })
    };
    j.prototype.set = function (u) {
        var v = this;
        d.extend(true, v.config, u);
        return v
    };
    j.prototype.on = function (u, v) {
        return layui.onevent.call(this, h, u, v)
    };
    j.prototype.progress = function (v, x) {
        var u = "layui-progress",
        w = d("." + u + "[lay-filter=" + v + "]"),
        z = w.find("." + u + "-bar"),
        y = z.find("." + u + "-text");
        z.css("width", x);
        y.text(x);
        return this
    };
    j.prototype.tabAdd = function (x, w) {
        if (w.pages !== "page") {
            var B = d(".layui-tab[lay-filter=" + x + "]")
        } else {
            var B = d(".layui-tab[lay-filter=" + x + "]", window.parent.document)
        }
        var z = ".layui-tab-title",
        v = B.children("#larryms_title").children(z),
        A = v.children(".layui-tab-bar"),
        y = B.children(".layui-tab-content"),
        u = '<li lay-id="' + (w.id || "") + '" data-group="' + w.group + '" data-id="' + w.larryID + '" data-url="' + w.url + '">' + (w.title || "unnaming") + "</li>";
        A[0] ? A.before(u) : v.append(u);
        y.append('<div class="layui-tab-item">' + (w.content || "") + "</div>");
        return this
    };
    j.prototype.tabChange = function (x, w, v) {
        if (v !== "page") {
            var A = d(".layui-tab[lay-filter=" + x + "]")
        } else {
            var A = d(".layui-tab[lay-filter=" + x + "]", window.parent.document)
        }
        var y = ".layui-tab-title",
        u = A.children("#larryms_title").children(y),
        z = u.find('>li[lay-id="' + w + '"]');
        q.tabClick(null, null, z);
        return this
    };
    j.prototype.tabDelete = function (w, v) {
        var x = ".layui-tab-title",
        z = d(".layui-tab[lay-filter=" + w + "]"),
        u = z.children("#larryms_title").children(x),
        y = u.find('>li[lay-id="' + v + '"]');
        this.LarryLayID = q.tabDelete(null, y);
        return this
    };
    j.prototype.render = j.prototype.init;
    var a = new j(),
    n = d(document);
    a.render();
    var i = ".layui-tab-title li";
    n.on("click", i, q.tabClick);
    s(h, a)
});