/**
 * Created by gaozijian on 2017/3/22.
 */
//这里声明公用函数 使用System命名空间
var System = {};
System.UseActive=function (sele) {
    //给所有有激活功能的按钮添加事件
    $(sele).click(function () {
        var sec = $(this).attr("data-active");
        var eles = sec=="this"? $(this):$(sec);
        if (eles.hasClass("active")) {
            eles.removeClass("active");
        } else {
            eles.addClass("active");
        }
    });
};
$(function () {
    System.UseActive(".canactive");
    $(".menuitem").click(function () {
        var pagename = $(this).attr("data-pageid");
        Debug.LoadPage(pagename);
    });
    Debug.LoadPage("Index");

});

System.LoadPage = function (name, query) {
    //此函数根据name加载一个页面到maincontent中
    //query为查询参数
    var href = "/Home/" + name+"?"+query;
    window.location.href=href;
}

var Debug = {};
Debug.LoadPage = function (name) {
    $("#maincontent").load(name + ".html");


};
