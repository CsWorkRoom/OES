function showSqlHelp() {
    layui.use(['layer'], function () {
        var $ = layui.jquery, layer = layui.layer;
        var content = "在SQL语句中，可以使用使用占位符的方式，来插入HTML输入框的值或者C#程序中的变量及方法，语法如下：<br/>";
        content += "一、插入HTML输入框的值<br/>";
        content += "    @(INPUT_NAME)<br/>";
        content += "二、插入C#的系统变量<br/>";
        content += "    @{USER_ID}当前登录用户ID <br/>";
        content += "    @{USER_NAME}当前登录用户登录名 <br/>";
        content += "    @{DEPARTMENT_ID}当前登录用户所属组织机构ID <br/>";
        content += "    @{DEPARTMENT_CODE}当前登录用户所属组织机构编码 <br/>";
        content += "    @{DEPARTMENT_NAME}当前登录用户所属组织机构名称 <br/>";
        content += "    @{DEPARTMENT_LEVEL}当前登录用户所属组织机构层级 <br/>";
        content += "    @{ALLROLE}当前登录用户所属权限 <br/>";
        content += "三、插入C#中定义的方法（日期函数）：<br/>";
        content += "    @{DATETIME}返回完整的日期格式（日期时间） <br/>";
        content += "    @{DATE}返回完整的日期格式（日期部份）<br/>";
        content += "    @{yyyy(n)}返回年份，参数n为整数，默认为0 <br/>";
        content += "    @{yyyymm(n)}返回年月，参数n为整数，默认为0 <br/>";
        content += "    @{yyyymmdd(n)}返回年月日，参数n为整数，默认为0 <br/>";

        layer.open({
            title: 'SQL语句中可用参数'
            , content: content
            , btn: ['关闭']
            , moveType: 1
            , area: ['800px;', '400px;']
        });
    });

    //终止表列头事件冒泡
    this.addEventListener('click', function (e) {
        e.stopPropagation();
        e.preventDefault();
    }, false);
}

function rdlcHelp() {
    layui.use(['layer'], function () {
        var $ = layui.jquery, layer = layui.layer;
        var content = "rdlc报表的主体是xml内容，请将配置好的rdlc的xml内容拷贝到文本域中：<br/><br/>";
        content += "配置rdlc模版需要安装visual studio工具<br/>";

        layer.open({
            title: 'RDLC报表说明'
            , content: content
            , btn: ['关闭']
            , moveType: 1
            , area: ['800px;', '400px;']
        });
        return;
    });

    //终止表列头事件冒泡
    this.addEventListener('click', function (e) {
        e.stopPropagation();
        e.preventDefault();
    }, false);
}

//表单提交
function save() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        if (isCheckedDefaultInputValues == false) {
            layer.alert("请在保存前，先分析SQL并通过验证");
            return;
        }
        var url = "../AfRdlcReport/Edit";
        SaveForm('form', url);
        return;
    });
}

layui.use(['form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form, element = layui.element; layer = layui.layer, $ = layui.jquery;
    //导出选项
    form.on('switch(switchExport)', function (data) {
        if (this.checked) {
            $("#IS_SHOW_EXPORT").val("1");
            layer.tips('将在表格上方显示“导出”按钮', data.othis)
        } else {
            $("#IS_SHOW_EXPORT").val("0");
            layer.tips('将不会在表格上方显示“导出”按钮', data.othis)
        }
    });

    //调试选项
    form.on('switch(switchDebug)', function (data) {
        if (this.checked) {
            $("#IS_SHOW_DEBUG").val("1");
            layer.tips('将在表格上方显示“导出”按钮', data.othis)
        } else {
            $("#IS_SHOW_DEBUG").val("0");
            layer.tips('将不会在表格上方显示“导出”按钮', data.othis)
        }
    });

    //自定义验证规则
    form.verify({
        name: function (value) {
            if (value.length < 3) {
                return '报表名称不能少于3个字母';
            }
        }
    });

    //提交
    form.on('submit(submit)', function (data) {
        save();//保存
    });

});

//----------------------------------------图表配置代码-----------------------------------------
window.onload = function () {
    //
}


var isCheckedDefaultInputValues = false;
var fields = "[]";//解析出的字段信息{key:字段名,value:字段类型}
//分析SQL语句
function testSql() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        var sql = $("#SQL_CODE").val();
        if (sql == null || sql.length < 1) {
            layer.alert("请输入SQL语句");
            return;
        }
        var url = "../AfRdlcReport/AnalysisSql";//解析sql语句并得到字段集合信息
        var inputjson = "";
        isCheckedDefaultInputValues = false;
        //正则匹配SQL中的自定义参数
        var regx = /@\(([a-zA-Z0-9_\-]+?)\)/g;
        var matchs = sql.match(regx);
        if (matchs == null) {
            $.post(url, {
                dbid: $("#DB_ID").val(),
                reportName: $("#NAME").val(),
                sql: sql,
                inputjson: inputjson
            }, function (result) {
                if (result.IsSuccess == true) {
                    var msg = "测试成功<br/>" + result.Message;
                    isCheckedDefaultInputValues = true;
                    fields = result.Result;
                    layer.alert(msg, { icon: 1 });
                } else {
                    var msg = "测试失败<br/>" + result.Message;
                    layer.alert(msg, { icon: 2 });
                }
            });
            return;
        }

        var html = "<div class='layui-form'>";
        var inputNames = new Array();
        for (var i = 0; i < matchs.length; i++) {
            var name = matchs[i].substr(2, matchs[i].length - 3);
            if ($.inArray(name, inputNames) < 0) {
                inputNames.push(name);
                html += name + "<input type='text' id='" + name + "'/>";
            }
        }
        html += "</div>"

        layer.open({
            title: '请输入如下参数的值，以供测试'
            , area: ['500px', '300px']
            , content: html
            , yes: function () {
                var inputValues = new Array();
                for (var i = 0; i < inputNames.length; i++) {
                    inputValues[i] = new Object();
                    inputValues[i].Name = inputNames[i];
                    inputValues[i].Value = $("#" + inputNames[i]).val();
                }
                var inputjson = JSON.stringify(inputValues);

                $.post(url, {
                    dbid: $("#DB_ID").val(),
                    reportName: $("#NAME").val(),
                    sql: sql,
                    inputjson: inputjson
                }, function (result) {
                    if (result.IsSuccess == true) {
                        isCheckedDefaultInputValues = true;
                        fields = result.Result;
                        var msg = "测试成功<br/>" + result.Message;
                        layer.alert(msg, { icon: 1 });
                    } else {
                        var msg = "测试失败<br/>" + result.Message;
                        layer.alert(msg, { icon: 2 });
                    }
                });
            }
        });
        return;
    });
}

//#region rdlc的xml模版下载

//下载rdlc模版
function downRdlcModule() {
    if (!isCheckedDefaultInputValues)
    {
        layer.alert("请先分析SQL并通过验证!");
        return;
    }
    construtForm("DownRDLC", { fields: fields, reportName: $("#NAME").val() });
}
//下载已配置xml
function downNowRdlc() {
    construtForm("DownRDLC", { fields: fields, reportName: $("#NAME").val(), rdlcId: $("#ID").val() });
}

/** 
 * 构建form表单，以post方式提交 
 * @param actionUrl  提交路径 
 * @param parms      提交参数 
 * @returns {___form0} 
 */
function construtForm(actionUrl, parms) {
    var form = document.createElement("form");
    form.style.display = 'none';;
    form.action = actionUrl;
    form.method = "post";
    document.body.appendChild(form);


    for (var key in parms) {
        var input = document.createElement("input");
        input.type = "hidden";
        input.name = key;
        input.value = parms[key];
        form.appendChild(input);
        // console.log(key);  
        // console.log(parms[key]);  
    }
    form.submit();

    //return form;

}
//#endregion