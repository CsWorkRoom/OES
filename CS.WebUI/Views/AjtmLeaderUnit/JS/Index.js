$(function () {
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

        //$.get("../AjtmLeaderUnit/GetLeaderInfoByUnit", { UnitID: treeNode.id }, function (r) {
        //    AjaxGetLeaderType(r.LeaderUnit);
        //    AjaxGetLeader(r.Leader);
        //}, "json");
        AjaxGet(treeNode.id);
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

    $("#btnInit").bind("click", init);

   
    var UId = $("#UnitId").val();
    if (UId) {
        var treeNode = $("#UNIT_ID").data("ztree").setValue(UId);
        if (treeNode) {
            $("#UNIT_NAME").val(treeNode.name);
            let parentNode = treeNode.getParentNode();
            if (parentNode) {
                $("#UNIT_PARENT").val(parentNode.name);
                $("#UNIT_PARENT_ID").val(parentNode.id);
            } else {
                $("#UNIT_PARENT").val("");
                $("#UNIT_PARENT_ID").val(0);
            }
        }
        AjaxGet(UId);
    }
});


function AjaxGet(unitId) {
    $.get("../AjtmLeaderUnit/GetLeaderInfoByUnit", { UnitID: unitId }, function (r) {
        console.log(r);
        AjaxGetLeaderType(r.LeaderUnit);
        AjaxGetLeader(r.Leader);
    }, "json");
}

function init() {
    var temp = TempLeaderInfo();
   
    var temptbody = temp.find("tbody");
    var num = 0;
    var list = $(".inputLeaderType");
    for (var i = 0; i < list.length; i++) {
        var e = $(list[i]);
        if (e.val().length > 0) {
            var n = parseFloat(e.val());
            if (isNaN(n)) continue;
            num += n;
            var leaderTypeID = e.attr("data-index");
            var leaderType = e.attr("data-text");
            for (var j = 0; j < n; j++) {
                var tr = TempLeaderByRow(initData({ LEADER_TYPE: leaderType, LEADER_TYPE_ID: leaderTypeID }));
                temptbody.append(tr);
            }
        }
    }
    if (num > 0) {
        $("#LeaderInfo").html(temp);
    }
    else {
        layer.alert("请输入初始化的数量");
    }
}
function AjaxGetLeaderType(r) {
    if (!r) return;
    for (var i = 0; i < r.length; i++) {
        var reslut = r[i];
        console.log(reslut)
        $(".inputLeaderType[data-index='" + reslut.LEADER_TYPE_ID + "']").val(reslut.NUM);
    }
}
function AjaxGetLeader(r) {
    var temp = TempLeaderInfo();
    var temptbody = temp.find("tbody");
    for (var i = 0; i < r.length; i++) {
        var result = r[i];
        var tr = TempLeaderByRow(initData(result));
        temptbody.append(tr);
    }
    $("#LeaderInfo").html(temp);
}

function initData(obj) {
    return $.extend({
        ID: 0,
        LEADER_TYPE: "",
        LEADER_TYPE_ID: 0,
        UNIT_ID: 0,
        UNIT_NAME: "",
        UNIT_PARENT_ID: 0,
        UNIT_PARENT: "",
        LAEDER_LEVEL_ID: 0,
        LEADER_LEVEL: "",
        LEADER_JOB: "",
        LEADER_NAME: "",
        IS_AS: 1,
        IS_USE: 1
    }, obj);
}

function TempLeaderInfo() {
    var temp = $(`
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>领导配置信息</legend>
        </fieldset>
        <div class="layui-form-item">
            <table class="layui-table" style="margin:0 auto;width:80%">
                <colgroup>
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                    <col width="100">
                </colgroup>
                <thead>
                    <tr>
                        <th>领导类型</th>
                        <th>领导职务</th>
                        <th>领导级别</th>
                        <th>是否占编</th>
                        <th>是否在职</th>
                        <th>领导名称</th>
                    </tr>
                </thead>
                <tbody>
                   
                </tbody>
            </table>
        </div>
    `);
    return temp;
}

function TempLeaderByRow(r) {

    var temp = $(`
     <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>`);
    var tds = temp.find("td");
    tds.eq(0).append("<span>" + r.LEADER_TYPE + "</span>");
    tds.eq(1).append(`<input type="text" value="` + r.LEADER_JOB+`" autocomplete="off" class="layui-input">`);
    tds.eq(2).append(tempSelectSetupLevel(r.LAEDER_LEVEL_ID));
    tds.eq(3).append(tempSelectByIF(r.IS_AS));
    tds.eq(4).append(tempSelectByIF(r.IS_USE));
    tds.eq(5).append(`<input type="text" value="` + r.LEADER_NAME+`" autocomplete="off" class="layui-input">`);
    var tr = temp.data("item", r);
    return temp;
}

function tempSelectSetupLevel(currValue) {
    var temp = $(`
     <select lay-ignore style="width:80%;height:30px">
        <option value="">请选择级别</option>
     </select>
    `);
    console.log(currValue);
    var sl = $("#SetupLevel").val();
    if (sl.length > 0) {
        var objlist = JSON.parse(sl);
        for (var i = 0; i < objlist.length; i++) {
            var obj = objlist[i];
            temp.append(`
               <option value="`+ obj.ID + `">` + obj.NAME + `</option>
            `);
        }
    }
    temp.find("option[value='" + currValue + "']").attr("selected", "selected");
    return temp;
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

function tempSelectByIF(currValue) {
     
    var temp = $(`
      <select lay-ignore style="width:80%;height:30px">
            <option value='1'>是</option>
            <option value='0'>否</option>
      </select>
   `);
    temp.find("option[value='" + currValue + "']").attr("selected", "selected");
    return temp;
}

function save() {
    if (saveBefore()) return;
    if (saveBeforeByLeader()) return;
    //
    layui.use(['form', 'layer', 'jquery'], function () {
        var form = layui.form, layer = layui.layer, $ = layui.$;
        var url = "../AjtmLeaderUnit/Index";
        SaveForm('form', url);
        return;
    });
}

function saveBeforeByLeader() {
    var trs = $("#LeaderInfo").find("tbody").find("tr");
    let arr = [];
    for (var i = 0; i < trs.length; i++) {
        var tr = trs.eq(i);
        var item = tr.data("item");
        console.log(item);
        var tds = tr.find("td");
        var LEADER_JOB = tds.eq(1).find("input").val();
        if (LEADER_JOB.trim().length === 0) {
            layer.alert("请填写第" + (i + 1) + "行的领导职务");
            return true;
        }
        var LAEDER_LEVEL_ID = tds.eq(2).find("select").val();
        if (LAEDER_LEVEL_ID.length === 0) {
             layer.alert("请选择第" + (i + 1) + "行的领导级别");
            return true;
        }
        var LEADER_LEVEL = tds.eq(2).find("option:selected").text();
        var IS_AS = tds.eq(3).find("select").val();
        var IS_USE = tds.eq(4).find("select").val();
        var LEADER_NAME = tds.eq(5).find("input").val();

        item = $.extend(item, { LEADER_JOB, LAEDER_LEVEL_ID, LEADER_LEVEL, IS_AS, IS_USE, LEADER_NAME });
        arr.push(item);
    }
    $("#Leader").val(JSON.stringify(arr));
    return false;
}

function saveBefore() {
    let list = $(".inputLeaderType");
    let arr = [];
    for (var i = 0; i < list.length; i++) {
        var e = $(list[i]);
        if (e.val().length > 0) {
            var n = parseFloat(e.val());
            if (isNaN(n)) continue;
            var leaderTypeID = e.attr("data-index");
            var leaderType = e.attr("data-text");
            arr.push({ LEADER_TYPE: leaderType, LEADER_TYPE_ID: leaderTypeID, NUM: n });
        }
    }
    $("#LeaderTypeUnit").val(JSON.stringify(arr));

    var unitId = $("#UNIT_ID").val();
    if (unitId.length === 0) {
        layer.alert("请选择单位");
        return true;
    }
    return false;
}