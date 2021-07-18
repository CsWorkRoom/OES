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


function Open(type, id, title) {
    $("#PAction", window.parent.document).val(type);
    switch (type) {
        case "Edit":
            return OpenTopWindow("单位信息编辑", 1050, 600, "/AjtmUnit/Detail?id=" + id + "");
        case "Delete":
            localAjaxPost("/AjtmUnit/SetUnable?id=" + id);
            return;
        case "Leader":
            console.log(title);
            if (!title) title = "领导信息编辑";
            return OpenTopWindow(title, 1500, 800, "/AjtmLeaderUnit/Index?unitId=" + id + "");
        default:
            return;
    }
}


//AJAX异步POST请求
function localAjaxPost(url) {
    layui.use('jquery', function () {
        var $ = layui.$;
        $.post(TransURL(url), function (result) {
            //console.log(result);
            //alert(result.Message.length);
            //debugger;wlf-6-28：新增提示框和刷新
            if (result.IsSuccess == true) {
                var msg = result.Message + " <br/>要刷新列表数据吗？";
                layer.confirm(msg, { area: ['300px', '200px'] }, function (index) {
                    //layer.closeAll();
                    //parent.layer.closeAll();
                    //parent.RefreshData();
                    //刷新当前页面
                    parent.location.reload();

                });

            } else {
                layer.alert(result.Message, { area: ['300px', '200px'] });
            }
        });
    });
}
