$(function () {
    initzTree();
    //
});


function initzTree() {
    var ztree = $("#unitId");
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
                $("#iframeShow").attr("src", "/AjtmUnit/Show?unitid=" + treeNode.id);
            }
        }
    }, znode);
}