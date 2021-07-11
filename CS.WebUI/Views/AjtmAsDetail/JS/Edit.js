
$(function () {
    //初始化时间主键
    layui.use(['form', 'layer', 'jquery', 'laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
        laydate.render({
            elem: '#APPROVAL_TIME' //指定元素
        });
    });
    //初始化树
    initTree("AsType", "AS_TYPE_ID");
    //初始化树
    initTree("Unit", "UNIT_ID", function (event, treeId, treeNode) {
        $("#UNIT_NAME").val(treeNode.name);
        let parentNode = treeNode.getParentNode();
        if (parentNode) {
            $("#UNIT_PARENT").val(parentNode.name);
            $("#UNIT_PARENT_ID").data("ztree").setValue(parentNode.id);
        } else {
            $("#UNIT_PARENT").val("");
            $("#UNIT_PARENT_ID").data("ztree").setValue(0);
        }
    });
    //初始化树
    initTree("Unit", "UNIT_PARENT_ID", function (event, treeId, treeNode) {
        $("#UNIT_PARENT").val(treeNode.name);
    });
});


//通用方法
function initTree(inputId, treeId, onClick) {
    if (typeof onClick !== "function") onClick = function (event, treeId, treeNode) { };
    var json = $("#" + inputId).val();
    if (json) {
        var node = JSON.parse(json);
        let obj = $.comboztree(treeId, { ztreenode: node, onClick: onClick });
        $("#" + treeId).data("ztree", obj);
    }
}


function save() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        var url = "../AjtmAsDetail/Edit";
        SaveForm('form', url);
        return;
    });
}