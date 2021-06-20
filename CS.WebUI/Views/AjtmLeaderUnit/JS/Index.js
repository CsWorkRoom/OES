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


function UrlForGetLeader(id) {

}


function TempLeaderInfo() {
    var temp = $(`
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>领导配置信息</legend>
        </fieldset>
        <div class="layui-form-item">
            <table class="layui-table" style="margin:0 auto">
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
    tds.eq(0).append(r.LEADER_TYPE);
    tds.eq(1).append(tempSelectSetupLevel());
    tds.eq(2).append(`<input type="text" value="" autocomplete="off" class="layui-input">`);
    tds.eq(3).append(tempSelectByIF());
    tds.eq(4).append(tempSelectByIF());
    tds.eq(5).append(`<input type="text" value="" autocomplete="off" class="layui-input">`);
    var tr = temp.find("tr").data("item", r);
    return tds;
}

function tempSelectSetupLevel() {
    var temp = $(`
     <select>
        <option value=""></option>
     </select>
    `);
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
    return temp;
}

function tempSelectByIF() {
    return $(`
      <select>
            <option value='1'>是</option>
            <option value='0'>否</option>
      </select>
   `);
}