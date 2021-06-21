﻿$(function () {
    initzTree();
    //
});
layui.use(['element', 'form', 'layer', 'jquery', 'laydate'], function () {
    var element = layui.element, form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;

    $('.layui-tab-title').on('click', function (t) {
        var unitid = $("#UnitId").val();
        var e = t.target;
        switch (e.getAttribute("lay-id")) {
            case "tabAsUnit":
                $("#iframeAsUnit").attr("src", "");
                break;
            case "AsDetail":
                $("#iframeAsDetail").attr("src", "");
                break;
            case "AsLeader":
                $("#iframeAsLeader").attr("src", "");
                break;
            default: break;
        }
    });
});

function initzTree() {
    var ztree = $("#unitId");
    var znode = JSON.parse($("#Unit").val());
    var ztreeObj = $.fn.zTree.init(ztree, {
        view: {
            dblClickExpand: false,
            showLine: false
        },
        data: {
            simpleData: {
                enable: true
            }
        },
        callback: {
            onClick: function (event, treeId, treeNode) {
                $("#UnitId").val(treeNode.id);
            }
        }
    }, znode);
}