$(function () {
    initTree("Unit", "UNIT_ID", function (event, treeId, treeNode) {
        $("#UNIT_NAME").val(treeNode.name);
        let parentNode = treeNode.getParentNode();
        if (parentNode) {
            $("#UNIT_PARENT").val(parentNode.name);
            $("#UNIT_PARENT_ID").val(parentNode.id);
        } else {
            $("#UNIT_PARENT").val("");
            $("#UNIT_PARENT_ID").val(0);
        }
    });
    layui.use(['form', 'layer', 'jquery', 'laydate'], function () { });
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
        var url = "../AjtmLeader/Edit";
        SaveForm('form', url);
        return;
    });
}