
$(function () {
    setTimeout(function () {
        layui.use(['form', 'layer', 'jquery', 'laydate'], function () {
            var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
            //实例化时间
            laydate.render({
                elem: '#FILE_SEND' //发文时间
            });
            laydate.render({
                elem: '#AGREE_TIME' //同意上下编时间
            });
            laydate.render({
                elem: '#CHECKIN_TIME' //实名制信息入库登记时
            });
            //实例化树
            initTree("AsType", "AS_TYPE_ID");
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
            initTree("Unit", "ADownUnitId");
            //验证
            //form.verify({
            //    name: function (value) {
            //        if (value.length <= 0) {
            //            return '单位名称不能为空！';
            //        }
            //    }
            //});
            //增加事件
            $("#ACTION_NO").bind("change", function () {
                if (this.value) {
                    if (this.value.indexOf('减') > -1) {
                        $("#ACTION").val("下编");
                    }
                    if (this.value.indexOf('增') > -1) {
                        $("#ACTION").val("上编");
                    }
                    form.render("select");
                    ActionChange();
                }
            });
            ActionChange();
            form.on('select(sACTION)', function (data) {
                ActionChange();
            });
            form.on('select(sAccessMode)', function (data) {
                $("#ACCESS_MODE").val(data.elem[data.elem.selectedIndex].text);
            });
            function ActionChange() {
                var ACTION = $("#ACTION").val();
                var ModeAccess = $("#ModeAccess").val();
                var list = [];
                console.log(1);
                if (ModeAccess.length !== 0); {
                    console.log(2);
                    var listObj = JSON.parse(ModeAccess);
                    $("#ACCESS_MODE_ID").children().remove();
                    $("#ACCESS_MODE_ID").append("<option value=''></option>");
                    for (var i = 0; i < listObj.length; i++) {
                        console.log(ACTION, listObj[i].ACTION_TYPE);
                        if (ACTION === listObj[i].ACTION_TYPE) {
                            $("#ACCESS_MODE_ID").append("<option value='" + listObj[i].ID + "'>" + listObj[i].NAME + "</option>");
                        }
                    }
                }
                $(".btnAction").hide();
                switch (ACTION) {
                    case "上编":
                        $(".btnActionUp").show();
                        break;
                    case "下编":
                        $(".btnActionDown").show();
                        break;
                    default:
                        break;
                }
                form.render("select");
            }

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
        });
    }, 0);

    var ztree = $("#AupUnitTree");
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
                $("#AupUnit").data("data", treeNode);
                $("#AupAsApply").html("");
                $("#AupAsDetail").html("");
                $.get("../AjtmAsDetail/GetAsDetail", {
                    unitId: treeNode.id
                }, function (r) {
                    var obj = JSON.parse(r);
                    var AsApply = obj.AsApply;
                    for (var i = 0; i < AsApply.length; i++) {
                        var item = AsApply[i];
                        $("#AupAsApply").append(TempAsApplyRadio(item));
                    }
                    render();
                });
            }
        }
    }, znode);

});

function TempAsApplyRadio(item) {
    var temp = $(`
        <div style="cursor:pointer">
           <span style="padding-right: 10px;"><input lay-ignore type="radio" name="AupAsApplyRadio" style="display:inline" /></span>
           <span>` + item.AS_APPLY_NO + `</span>
        </div>`);
    temp.find("input").data("result", item);
    temp.find("input").data("item", item.AS_DETAIL);
    temp.bind("click", function () {
        var curr = $(this);
        $("input[name='AupAsApplyRadio']").removeAttr("checked");
        curr.find("input").attr("checked", "checked");
        var data = curr.find("input").data("item");
        $("#AupAsDetail").html("");
        for (var i = 0; i < data.length; i++) {
            var AsD = data[i];
            $("#AupAsDetail").append(TempAsDetailRadio(AsD));
        }
    });
    return temp;
}
function TempAsDetailRadio(item) {
    var obj = $(`
        <div style="cursor:pointer">
           <span style="padding-right: 10px;"><input lay-ignore type="radio" name="AsDetailRadio" style="display:inline" /></span>
           <span>` + `[` + item.AS_TYPE + `]` + (item.AS_NO ? item.AS_NO : "暂无用编编号") + `</span>
        </div>`);
    obj.find("input").data("result", item);
    obj.bind("click", function () {
        var curr = $(this);
        $("input[name='AsDetailRadio']").removeAttr("checked");
        curr.find("input").attr("checked", "checked");
        console.log(this);
    });
    return obj;
}

function AupSumbit() {
    var AsUnit = $("#AupUnit").data("data");
    if (!AsUnit) {
        return layer.alert("请选择单位");
    }
    var AsApply = $("input[name='AupAsApplyRadio']:checked");
    if (AsApply.length === 0) {
        return layer.alert("请选择用编通知书");
    }
    var AsDetail = $("input[name='AsDetailRadio']:checked");
    if (AsDetail.length === 0) {
        return layer.alert("请选择用编编号");
    }
    //编制单位,主管部门
    $("#UNIT_ID").data("ztree").setValue(AsUnit.id);
    $("#UNIT_NAME").val(AsUnit.name);
    let parentNode = AsUnit.getParentNode();
    if (parentNode) {
        $("#UNIT_PARENT").val(parentNode.name);
        $("#UNIT_PARENT_ID").val(parentNode.id);
    } else {
        $("#UNIT_PARENT").val("");
        $("#UNIT_PARENT_ID").val(0);
    }
    //编制通知单
    var item1 = AsApply.data("result");
    console.log(item1);
    $("#AS_APPLY_ID").val(item1.AS_APPLY_ID);
    $("#AS_APPLY_NO").val(item1.AS_APPLY_NO);
    //用编号码
    var item2 = AsDetail.data("result");
    $("#AS_NO").val(item2.AS_NO);
    //编制类型
    $("#AS_TYPE_ID").data("ztree").setValue(item2.AS_TYPE_ID);
    $("#AS_TYPE").val(item2.AS_TYPE);
    //关闭弹窗
    Close();
}

function render() {
    layui.use(['form'], function () {
        var form = layui.form;
        form.render();
    });
}

function Open() {
    layer.open({
        id: "Aup",
        closeBtn: true,  //关闭按钮
        type: 1,
        title: "上编信息",
        area: ['1000px', '500px'],
        shade: 0,       //不显示遮罩
        content: $("#ActionUp"),
    });
}

function Close() {
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.close(layer.index);
    });
}

function onSearch() {
    layui.use(['form', 'layer', 'jquery', 'laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
        //正文
        var data = {};
        var UnitId = $("#ADownUnitId").val();
        if (UnitId.length !== 0) {
            data.UnitId = UnitId;
        }
        var AccountName = $("#ADwonAccountName").val();
        if (AccountName.trim().length === 0) {
            return layer.alert("请输入用户名称，也可以只输入姓氏");
        }
        data.AccountName = AccountName;
        $.get("../AjtmAsPersonnel/GetPersonnel", data, function (r) {
            if (!r) return layer.alert("未搜索出对应的信息");
            if (r.length === 0) return layer.alert("未搜索出对应的信息");
            var temp = TempTableForAsDown();
            var temptbody = temp.find("tbody");
            for (var i = 0; i < r.length; i++) {
                var item = r[i];
                temptbody.append(TempTableByRow(item));
            }
            $("#ADownInfo").html(temp);
        }, "json");

        function TempTableForAsDown() {
            return $(`
       <table class="layui-table" lay-even style="margin:0 auto">
                <colgroup>
                    <col width="80">
                    <col width="150">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                </colgroup>
                <thead>
                    <tr>
                        <th>姓名</th>
                        <th>编制单位</th>
                        <th>主管部门</th>
                        <th>编制使用通知单号</th>
                        <th>用编序号</th>
                        <th>学历</th>
                        <th>岗位类别</th>
                        <th>编制类型</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
   `);
        }

        function TempTableByRow(item) {
            var temp = $(`
           <tr>
               <td>`+ item.ACCOUNT_NAME + `</td>
               <td>`+ item.UNIT_NAME + `</td>
               <td>`+ item.UNIT_PARENT + `</td>
               <td>`+ item.AS_APPLY_NO + `</td>
               <td>`+ item.AS_NO + `</td>
               <td>`+ item.ACCOUNT_EDUCATION + `</td>
               <td>`+ item.POST_TYPE + `</td>
               <td>`+ item.AS_TYPE + `</td>
               <td>
                   <button type="button" class="layui-btn" onclick="">确定</button>
               </td>
            </tr>
    `);
            temp.find("button").data("item", item);
            temp.find("button").bind("click", TempButtonClick);
            return temp;
        }

        function TempButtonClick() {
            var t = $(this);
            var d = t.data("item");
            //用户信息
            $("#ACCOUNT_NAME").val(d.ACCOUNT_NAME);
            $("#ACCOUNT_EDUCATION").val(d.ACCOUNT_EDUCATION);
            //岗位类型
            $("#POST_TYPE").val(d.POST_TYPE);
            //编制单位
            $("#UNIT_ID").data("ztree").setValue(d.UNIT_ID);
            $("#UNIT_NAME").val(d.UNIT_NAME);
            $("#UNIT_PARENT").val(d.UNIT_PARENT);
            $("#UNIT_PARENT_ID").val(d.UNIT_PARENT_ID);
            //编制通知单
            $("#AS_APPLY_ID").val(d.AS_APPLY_ID);
            $("#AS_APPLY_NO").val(d.AS_APPLY_NO);
            //用编号码
            $("#AS_NO").val(d.AS_NO);
            //编制类型
            $("#AS_TYPE_ID").data("ztree").setValue(d.AS_TYPE_ID);
            $("#AS_TYPE").val(d.AS_TYPE);
            //
            form.render();
            //关闭窗体
            layer.close(layer.index);
        }
    });
}


function OpenDown() {
    layer.open({
        id: "ADown",
        closeBtn: true,  //关闭按钮
        type: 1,
        title: "上编信息",
        area: ['1000px', '500px'],
        shade: 0,       //不显示遮罩
        content: $("#ActionDown"),
    });
}

function getRandomString(len) {
    len = len | 8;
    var str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var strRan = "";
    for (var i = 0; i < len; i++) { //len为随机字符串长度
        strRan += str.charAt(Math.floor(Math.random() * str.length));
    }
    return strRan;
}

function save() {
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        //let r = unitAsArr();
        //if (r.isErr) {
        //    layer.alert(r.Msg);
        //    return;
        //}
        var url = "../AjtmAsPersonnel/Edit";
        SaveForm('form', url);
        return;
        function unitAsArr() {
            //let name = $("#NAME").val();
            //if (name..length === 0) {
            //    return { isErr: true, Msg: "提交失败,单位名称不能为空" };
            //}
            //
        }
    });
}