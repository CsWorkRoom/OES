
$(function () {

    layui.use(['form', 'layer', 'jquery','laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
        laydate.render({
            elem: '#APPROVAL_TIME' //指定元素
        });

        init();

        function init() {
            var zsNodes = {};
            if ($("#Unit").val() !== "") {
                zsNodes = JSON.parse($("#Unit").val());
                $.comboztree("UNIT_ID", {
                    ztreenode: zsNodes, onClick: function (event, treeId, treeNode) {
                        $("#UNIT_NAME").val(treeNode.name);
                        let parentNode = treeNode.getParentNode();
                        if (parentNode) {
                            $("#UNIT_PARENT").val(parentNode.name);
                            $("#UNIT_PARENT_ID").val(parentNode.id);
                        } else {
                            $("#UNIT_PARENT").val("");
                            $("#UNIT_PARENT_ID").val(0);
                        }
                    }
                });
            } else {
                $.comboztree("UNIT_ID", {});
            }
            var AsType = $("#AsType").val();
            if (AsType !== "") {
                var AsTypeNode = JSON.parse(AsType);
                $.comboztree("AS_TYPE_ID", {
                    ztreenode: AsTypeNode,
                    onClick: function (event, treeId, treeNode) {
                        $("#AS_TYPE").val(treeNode.name);
                    }
                });
            }
        }
    });
});


function save() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        var url = "../AjtmAsDetail/Edit";
        SaveForm('form', url);
        return; 
    });
}