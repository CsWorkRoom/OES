window.onload = function () {
    rdlcSearchClick();
    if ($.trim($("#topCode").html()) != "") {
        $("#topCode").append("<button class='layui-btn search_btn' style='margin-left: 10px;' onclick='rdlcSearchClick()'><i class='layui-icon layui-icon-search'></i>查询</button>");
    }
}

//子页面处理
function rdlcSearchClick() {
    $("#ifm").contents().find("#rdlcId").val($("#ID").val());//把rdlc的编号ID赋给子页
    $("#ifm").contents().find("#queryParams").val(JSON.stringify(getParams()));//把查询json传给子页
    $("#ifm").contents().find("#SearchBtn").click();//触发子页面的查询按钮
}

//从筛选区获得查询参数信息[{Name:名称, Value:值}]
function getParams() {
    //#region 获得url的集合
    var urlParaArr = [];
    var urlParas = $("#QUERY_STRING").val();
    if (urlParas != null && urlParas.length > 0) {
        urlParaArr = $.parseJSON(urlParas);
    }
    //#endregion

    var paramArr = [];
    var sqlCode = Decrypt($("#SQL_CODE").val());
    var parm = sqlCode.match(/@\([\w]+\)/ig);
    parm = Array.from(new Set(parm));//set去重
    if (parm != null) {
        for (var i = 0; i < parm.length; i++) {
            var item = parm[i].replace("@(", "").replace(")", "");
            if ($("#" + item) != null) {//对像是否存在
                var v = $("#" + item).val();
                if (v == null || $.trim(v) == "")
                    v = $("#" + item).text();
                var kv = { Name: item, Value: v }

                //#region 把url的参数值替换到集合中
                for (var j = 0; j < urlParaArr.length; j++)
                {
                    var up = urlParaArr[j];
                    if(up.Name==kv.Name)
                    {
                        kv.Value = up.Value;
                    }
                }
                //#endregion
                paramArr.push(kv);
            }
        }
    }


    return paramArr;
}

$(function () {
    $("#ifm").height($("#divContent").height() - $("#topCode").height() - $("#botCode").height());
});

$(window).resize(function () {
    $("#ifm").height($("#divContent").height() - $("#topCode").height() - $("#botCode").height());
});