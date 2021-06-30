
$(function () {

    layui.use(['form', 'layer', 'jquery','laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
        laydate.render({
            elem: '#AS_APPROVAL_TIME' //指定元素
        });

        init();

        function init() {
            if ($("#AsApplyDetailJson").val()) {
                var AsUnit = JSON.parse($("#AsApplyDetailJson").val());
                for (var i = 0; i < AsUnit.length; i++) {
                    add(AsUnit[i]);
                }
            }
            var trArr = $("#AsApplyDetail").find("tr");
            if (trArr.length === 0) add();
        }
    });
});


function add(e) {
    var r = { ID: 0, AS_TYPE_ID: 0, AS_PURPOSE_ID: 0, AS_PURPOSE_REMARK: "", APPLY_NUM: 0, APPROVAL_NUM: 0 };
    if (e) {
        r = $.extend(r, e);
    }
    var id = 'input_' + getRandomString();
    var zNodes = JSON.parse($("#AsType").val());
    var zAsPurposeNode = JSON.parse($("#AsPurpose").val());
    var temp = $(`
             <tr>
                  <td>
                       <input type="text" id="`+ id + `" value="` + r.AS_TYPE_ID + `" class="AsType" >
                  </td>
                  <td>
                       <select  style="width:100%;height:40px;" lay-ignore><option value=''>请选择</option></select>
                 </td>
                  <td><input type="text" autocomplete="off" value="` + r.AS_PURPOSE_REMARK + `" class="layui-input"></td>
                  <td>`+ r.APPLY_NUM+`</td>
                  <td><input type="text" autocomplete="off" value="` + r.APPLY_NUM + `" class="layui-input"></td>
                  <td>
                       <input hidden="hidden" value="`+ r.ID +`"/>
                       <button type="button" class="layui-btn add">增加</button>
                       <button type="button" class="layui-btn remove">删除</button>
                  </td>
             </tr>     
        `);
    for (var i = 0; i < zAsPurposeNode.length; i++) {
        var AsNodeP = zAsPurposeNode[i];
        let option = $('<option></option>', {
            value: AsNodeP.ID,
            text: AsNodeP.NAME
        });
        if (AsNodeP.ID === r.AS_PURPOSE_ID) {
            option.attr("selected", "selected");
        }
        temp.find("select").append(option);
    }
    temp.find(".add").bind("click", add);
    if (r.ID === 0)
        temp.find(".remove").bind("click", remove);
    else
        temp.find(".remove").remove();
        
    
    $("#AsApplyDetail").append(temp);
    $.comboztree(id, { ztreenode: zNodes });
    //重新渲染
    layui.use(['form'], function () {
        var form = layui.form;
        form.render('select');
    })
}

function remove() {
    var trArr = $("#AsApplyDetail").find("tr");
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
        let r = rArr();
        if (r.isErr) {
            layer.alert(r.Msg);
            return;
        }
        var url = "../AjtmAsApply/Approval";
        SaveForm('form', url);
        return;
        function rArr() {
            var AsApprval = parseInt($("#AS_APPROVAL_TIME").val());
            if (!AsApprval) {
                return { isErr: true, Msg: "请输入批准时间" };
            }
            var trArr = $("#AsApplyDetail").find("tr");
            var rArr = [];
            for (var i = 0; i < trArr.length; i++) {
                var tr = $(trArr[i]);
                tr.find("td").eq(0).find("input").val();
                let AS_TYPE_ID = parseInt(tr.find("td").eq(0).find("input").val());
                if (isNaN(AS_TYPE_ID) || AS_TYPE_ID === 0) {
                    return { isErr: true, Msg: "未选择第" + (i + 1) + "行编制类型" };
                }
                let AS_PURPOSE_ID = parseInt(tr.find("td").eq(1).find("select").val());
                if (isNaN(AS_PURPOSE_ID) || AS_PURPOSE_ID === 0) {
                    return { isErr: true, Msg: "未选择第" + (i + 1) + "行编制用途" };
                }
                let AS_PURPOSE_REMARK = tr.find("td").eq(2).find("input").val();
                let APPLY_NUM = parseInt(tr.find("td").eq(3).text());
                var APPROVAL_NUM = parseInt(tr.find("td").eq(4).find("input").val());
                if (isNaN(APPROVAL_NUM) || APPROVAL_NUM === 0) {
                    return { isErr: true, Msg: "第" + (i + 1) + "行,请正确输入批准数" };
                }
                var ID = tr.find("td").eq(5).find("input").val();
                var r = { ID: 0, AS_TYPE_ID: 0, AS_PURPOSE_ID: 0, AS_PURPOSE_REMARK: "", APPLY_NUM: 0, APPROVAL_NUM: 0 };
                var r = {
                    ID: ID,
                    AS_TYPE_ID: AS_TYPE_ID,
                    AS_PURPOSE_ID: AS_PURPOSE_ID,
                    AS_PURPOSE_REMARK: AS_PURPOSE_REMARK,
                    APPLY_NUM: APPLY_NUM,
                    APPROVAL_NUM: APPROVAL_NUM
                };
                rArr.push(r);
            }
            $("#AsApplyDetailJson").val(JSON.stringify(rArr));
            return { isErr: false };
        }
    });
}