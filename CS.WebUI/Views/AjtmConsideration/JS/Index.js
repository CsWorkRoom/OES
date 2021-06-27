$(function () {
    layui.use(['form', 'layer', 'jquery','laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.jquery, laydate = layui.laydate;
        form.verify({
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
});

function save() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var url = "../AjtmConsideration/Index";
        SaveForm('form', url);
        return;
    });
}