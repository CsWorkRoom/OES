
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
            //验证
            form.verify({
                name: function (value) {
                    if (value.length <= 0) {
                        return '单位名称不能为空！';
                    }
                }
            });
            //增加事件
            $("#ACTION_NO").bind("change", function () {
                if (this.value) {
                    if (this.value.indexOf('减') > -1) {
                        $("#ACTION").val("下编");
                    }
                    if (this.value.indexOf('增') > -1) {
                        $("#ACTION").val("上编");
                    }
                    form.render();
                }
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
        curr.find("input").attr("checked","checked");
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
           <span>` + `[` + item.AS_TYPE + `]` + (item.AS_NO ? item.AS_NO:"暂无用编编号") + `</span>
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
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.open({
            id: "Aup",
            closeBtn: true,  //关闭按钮
            type: 1,
            title:"上编信息",
            area: ['800px', '350px'],
            shade: 0,       //不显示遮罩
            content: $("#ActionUp"),
        });
    });
}

function Close() {
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.close(layer.index);
    });
}


function onSearch() {
    
}


function add(e) {
    var r = { AS_TYPE_ID: 0, VERIFICATION_NUM: 0, BEGIN_NUM: 0 };
    if (e) {
        r = $.extend(r, e);
    }
    var id = getRandomString();
    var zNodes = JSON.parse($("#AsType").val());
    var temp = $(`
             <tr>
                  <td>
                       <input type="text" id="`+ id + `" value="` + r.AS_TYPE_ID + `" class="AsType" >
                  </td>
                  <td><input type="text" autocomplete="off" value="` + r.VERIFICATION_NUM + `" class="layui-input"></td>
                  <td><input type="text" autocomplete="off" value="` + r.BEGIN_NUM + `" class="layui-input"></td>
                  <td>
                       <button type="button" class="layui-btn add">增加</button>
                       <button type="button" class="layui-btn remove">删除</button>
                  </td>
             </tr>     
        `);
    temp.find(".add").bind("click", add);
    temp.find(".remove").bind("click", remove);
    $("#AsUnit").append(temp);
    $.comboztree(id, { ztreenode: zNodes });
}

function remove() {
    var trArr = $("#AsUnit").find("tr");
    if (trArr.length <= 1) return;
    $(this).closest("tr").remove();
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
        let r = unitAsArr();
        if (r.isErr) {
            layer.alert(r.Msg);
            return;
        }
        var url = "../AjtmUnit/Detail";
        SaveForm('form', url);
        return;
        function unitAsArr() {
            //let name = $("#NAME").val();
            //if (name..length === 0) {
            //    return { isErr: true, Msg: "提交失败,单位名称不能为空" };
            //}
            //
            var trArr = $("#AsUnit").find("tr");
            var rArr = [];
            for (var i = 0; i < trArr.length; i++) {
                var tr = $(trArr[i]);
                tr.find("td").eq(0).find("input").val();
                let AS_TYPE_ID = parseInt(tr.find("td").eq(0).find("input").val());
                if (isNaN(AS_TYPE_ID) || AS_TYPE_ID === 0) {
                    return { isErr: true, Msg: "未选择第" + (i + 1) + "行编制类型" };
                }
                let VERIFICATION_NUM = tr.find("td").eq(1).find("input").val();
                let BEGIN_NUM = tr.find("td").eq(2).find("input").val();
                var r = {
                    AS_TYPE_ID: AS_TYPE_ID,
                    VERIFICATION_NUM: VERIFICATION_NUM,
                    BEGIN_NUM: BEGIN_NUM
                };
                rArr.push(r);
            }
            $("#AsUnitJson").val(JSON.stringify(rArr));
            return { isErr: false };
        }
    });
}