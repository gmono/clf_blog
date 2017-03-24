/**
 * Created by gaozijian on 2017/3/22.
 */
$(function () {
    //给所有有激活功能的按钮添加事件
    $(".canactive").click(function () {
        var sec=$(this).attr("data-active");
        var eles=$(sec);
        if(eles.hasClass("active"))
        {
            eles.removeClass("active");
        }
        else
        {
            eles.addClass("active");
        }
    });
    $(".menuitem").click(function () {
        var pagename=$(this).attr("data-pageid");
        Debug.LoadPage(pagename);
    });
    Debug.LoadPage("Index");

});
//这里声明公用函数 使用System命名空间
var System={};
System.LoadPage=function (name,query) {
    //此函数根据name加载一个页面到maincontent中
    //query为查询参数
    var href="/Home/"+name;
    $("#maincontent").load(href,query);
}
System.LoadSide=function (name,query) {
    //由指定的查询参数从指定位置加载页面到main-sidecont中
    var href="/Sides/"+name;
    $("#main-sidecont").load(name,query);
}
System.ShowArticle=function(id)
{
    //显示文章并且 显示侧边栏
    System.LoadPage('Article',{Id:id});
    System.LoadSide('Article',{Id:id});
}
var Debug={};
Debug.LoadPage=function (name) {
    $("#maincontent").load(name+".html");
};