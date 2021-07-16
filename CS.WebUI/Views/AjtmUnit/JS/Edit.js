//开局渲染
layui.use(['form', 'layer', 'jquery'], function () {
    var form = layui.form, layer = layui.layer, $ = layui.jquery;
    form.verify({
        name: function (value) {
            if (value.length <= 0) {
                return '单位名称不能为空！';
            }
        }
    });
});
$(function () {
    setTimeout(function () {
        var zsNodes = {};
        if ($("#Unit").val() !== "") {
            zsNodes = JSON.parse($("#Unit").val());
            $.comboztree("PARENT_ID", { ztreenode: zsNodes });
        } else {
            $.comboztree("PARENT_ID", {});
        }
        if ($("#AsUnitJson").val()) {
            var AsUnit = JSON.parse($("#AsUnitJson").val());
            for (var i = 0; i < AsUnit.length; i++) {
                add(AsUnit[i]);
            }
        }
        var trArr = $("#AsUnit").find("tr");
        if (trArr.length === 0) add();
    }, 0);

    InitInputNumberEvent();
});




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
                  <td><input type="text" autocomplete="off" value="` + r.VERIFICATION_NUM + `" class="layui-input" onafterpaste="InputNumber(this)" onkeyup="InputNumber(this)" oninput="InputNumber(this)"></td>
                  <td><input type="text" autocomplete="off" value="` + r.BEGIN_NUM + `" class="layui-input" onafterpaste="InputNumber(this)" onkeyup="InputNumber(this)" oninput="InputNumber(this)"></td>
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