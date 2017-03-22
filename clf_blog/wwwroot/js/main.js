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
    })
});