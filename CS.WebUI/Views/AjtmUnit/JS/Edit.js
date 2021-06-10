layui.use(['form', 'layer', 'jquery'], function () {
    var form = layui.form, layer = layui.layer, $ = layui.jquery;


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

function unitAsArr() {
    var trArr = $("#AsUnit").find("tr");
    var rArr = [];
    for (var i = 0; i < trArr.length; i++) {
        var tr = $(trArr[i]);
        tr.find("td").eq(0).find("input").val();
        var r = {
            AS_TYPE_ID: tr.find("td").eq(0).find("input").val(),
            VERIFICATION_NUM: tr.find("td").eq(1).find("input").val(),
            BEGIN_NUM: tr.find("td").eq(2).find("input").val()
        };
        rArr.push(r);
    }
    $("#AsUnitJson").val(JSON.stringify(rArr));
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
    unitAsArr();
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        var url = "../AjtmUnit/Detail";
        SaveForm('form', url);
        return;
    });
}