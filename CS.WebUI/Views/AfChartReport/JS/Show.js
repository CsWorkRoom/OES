window.onload = function () {
    analysisSql();
    ChartCodeClick();
    if ($.trim($("#topCode").html()) != "") {
        $("#topCode").append("<button class='layui-btn search_btn' style='margin-left: 10px;' onclick='analysisSql();ChartCodeClick()'><i class='layui-icon layui-icon-search'></i>查询</button>");
    }
}

//子页面处理
function ChartCodeClick() {
    var code = $("#CHART_CODE").val();
    $("#ifm").contents().find("#endCode").val(code);
    $("#ifm").contents().find("#SearchBtn").click();//触发CHART子页面的查询按钮
}

//获取图表的配置代码
function getChartCode() {
    $("#ifm").contents().find("#getEndCode").click();
    var ec = $("#ifm").contents().find("#endCode").val();
    if (ec != null && ec != "" && ec.length > 0) {
        $("#CHART_CODE").val(ec);
    }
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
                for (var j = 0; j < urlParaArr.length; j++) {
                    var up = urlParaArr[j];
                    if (up.Name == kv.Name) {
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

//解析sql
function analysisSql() {

    /////发送请求////
    var data = $.ajax({
        type: "post",
        data: { DB_ID: $("#DB_ID").val(), SQL_CODE: Decrypt($("#SQL_CODE").val()), QUERY_STRING: JSON.stringify(getParams()) },
        url: applicationPath + "/AfChartReport/GetDataBySql",
        async: false
    });
    if (data != null && data.responseText != null && data.responseText.length > 0) {
        $("#ifm").contents().find("#seachData").val(data.responseText);
        //var dataArr = $.parseJSON(data.responseText);
    }
    $("#ifm").contents().find("#childRun").click();//
}

$(function () {
    $("#ifm").height($("#divContent").height() - $("#topCode").height() - $("#botCode").height());
});

$(window).resize(function () {
    $("#ifm").height($("#divContent").height() - $("#topCode").height() - $("#botCode").height());
});