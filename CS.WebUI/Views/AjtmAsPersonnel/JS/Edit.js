
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
            initTree("AsType", "AS_TYPE");
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
                    console.log(this.value);
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
                    $("#" + treeId).data("obj", obj);
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
                $.get("../AjtmAsDetail/GetAsDetail", {
                    unitId: treeNode.id
                }, function (r) {
                    console.log(r);
                });
            }
        }
    }, znode);
});

function Open() {
    layui.use(['layer'], function () {
        var layer = layui.layer;
        layer.open({
            id: "Aup",
            closeBtn: true,  //关闭按钮
            type: 1,
            title:"上编信息",
            area: ['800px', '350px'],
            btn: ['确定'],
            shade: 0,       //不显示遮罩
            content: $("#ActionUp"),
            success: function (layero) {
                var btn = layero.find('.layui-layer-btn');
                btn.bind("click", function () {
                    alert("text");
                });
            }
        });
    });
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