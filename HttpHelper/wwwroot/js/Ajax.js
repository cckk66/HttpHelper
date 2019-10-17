var myAjax = function (option = {
    type: "POST",
    url: "",
    data: {},
    success: function () {

    },
    error: function () {

    }
}) {
    debugger
    $.ajax({
        //请求方式
        type: option.type,
        //请求的媒体类型
        contentType: "application/json;charset=UTF-8",
        //请求地址
        url: option.url,
        //数据，json字符串
        data: option.data,
        //请求成功
        success: function (result) {
            option.success(result)
        },
        //请求失败，包含具体的错误信息
        error: function (e) {
            option.error(e)
        }
    });

}