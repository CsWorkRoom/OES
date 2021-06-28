$(function () {
    layui.use(['form', 'layer', 'jquery','laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.jquery, laydate = layui.laydate;
        form.verify({
            ids: function (vvalue) {
                if (value.length <= 0) {
                    return '请选择用编申请！';
                }
            },
            name: function (value) {
                if (value.length <= 0) {
                    return '审议表名不能为空！';
                }
            }
        });
        laydate.render({
            elem: '#xTime' //指定元素
        });
    });

    $("#checkAll").change(function (e) {
        var isCheck = $("#checkAll").prop("checked");
        $("input[name='AsApplyCheck']").prop("checked", isCheck);
    })

    $("#checkAll").click();
});

function save() {
    var e = $("input[name='AsApplyCheck']:checked");
    if (e.length === 0) {
        return layer.alert("请选择用编申请！");
    }
    var idArr = [];
    for (var i = 0; i < e.length; i++) {
        var id = e.eq(i).attr("data-id");
        idArr.push(id);
    }
    $("#IDS").val(idArr.toString());
    layui.use(['form', 'layer', 'jquery'], function () {
        var url = "../AjtmConsideration/Index";
        SaveForm('form', url);
        return;
    });
}