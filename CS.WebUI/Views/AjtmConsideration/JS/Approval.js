
$(function () {
    layui.use(['form', 'layer', 'jquery','laydate'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$, laydate = layui.laydate;
        laydate.render({
            elem: '#AS_APPROVAL_TIME' //指定元素
        });

        init();

        function init() {
            if ($("#AsApply").val()) {
                var AsUnit = JSON.parse($("#AsApply").val());
                for (var i = 0; i < AsUnit.length; i++) {
                    tempApprovel(AsUnit[i]);
                }
            }
            //var trArr = $("#AsApplyDetail").find("tr");
            //if (trArr.length === 0) add();
        }
    });
});


function tempApprovel(item) {
    var temp = $(`
         <div class="AsApproval">
            <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                <legend>`+ item.UNIT_NAME +`</legend>
            </fieldset>
            <div class="layui-form-item">
                <div class="layui-inline">
                    <label class="layui-form-label">编制使用通知单</label>
                    <div class="layui-input-inline">
                        <input type="text"  autocomplete="off" class="layui-input InputApplyNo" readonly="readonly" />
                    </div>
                </div>
            </div>
            <div class="layui-form-item">
                <table class="layui-table" lay-even style="margin:0 auto">
                    <colgroup>
                        <col width="200">
                        <col width="100">
                        <col width="100">
                        <col width="100">
                        <col width="100">
                        <col width="100">
                    </colgroup>
                    <thead>
                        <tr>
                            <th>用编类型</th>
                            <th>编制用途</th>
                            <th>编制详细用途</th>
                            <th>申报数量</th>
                            <th>批准数</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    `);
    temp.attr("id", "ApplyNo_" + item.id);
    temp.data("item", item);
    temp.find(".InputApplyNo").val(item.AS_APPLY_NO);
    temp.find("tbody").attr("id", "AsApplyDetail_" + item.id);
    $("#content").append(temp);
    //
    var AsUnit = JSON.parse(item.AsApplyDetailJson);
    for (var i = 0; i < AsUnit.length; i++) {
        add(temp.find("tbody"), AsUnit[i]);
    }
    //return temp;
}


function add(target,e) {
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
                       <select><option value=''></option></select>
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
        
    
    target.append(temp);
    $.comboztree(id, { ztreenode: zNodes });
    //重新渲染
    layui.use(['form'], function () {
        var form = layui.form;
        form.render('select');
    });
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
        var url = "../AjtmConsideration/Approvel";
        SaveForm('form', url);
        return;
        function rArr() {
            var AsApprval = parseInt($("#AS_APPROVAL_TIME").val());
            if (!AsApprval) {
                return { isErr: true, Msg: "请输入批准时间" };
            }
            var resultArr = [];
            var ApprovalArr = $("#content").find(".AsApproval");
            for (var j = 0; j < ApprovalArr.length; j++) {
                var item = ApprovalArr.eq(j).data("item");
                var trArr = ApprovalArr.eq(j).find("tbody").find("tr");
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
                console.log(rArr);
                item.AsApplyDetailJson = JSON.stringify(rArr);
                resultArr.push(item);
            }
            $("#AsApply").val(JSON.stringify(resultArr));
            return { isErr: false };
        }
    });
}