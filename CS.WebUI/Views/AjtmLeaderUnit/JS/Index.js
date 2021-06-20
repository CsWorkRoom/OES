$(function () {
 
});

function InitLeaderType(r) {

}

function TempLeaderType(item1,item2) {
    var temp = $(`
     <div class="layui-form-item" class=>
    </div>
   `);
    temp.append(TempLeaderTypeInput(item1))
    if (item2) {
        temp.appendTo(TempLeaderTypeInput(item2))
    }
    return temp;
}

function TempLeaderTypeInput(item) {
    var temp = $(`
        <div class="layui-inline">
            <label class="layui-form-label">`+ item.NAME + `</label>
            <div class="layui-input-inline">
                <input type="text" data-index="`+ item.ID + `" autocomplete="off" class="layui-input">
            </div>
        </div>
    `);
    return Temp;
}