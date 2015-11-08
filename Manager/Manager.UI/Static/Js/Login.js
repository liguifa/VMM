$(document).ready(function ()
{
    //登录事件
    $("button[type=submit]").click(function ()
    {
        //前台响应登录事件
        //TODO

        //获取登录参数
        var username = $(this).parents(".form-horizontal").find(".username").val();
        var password = $(this).parents(".form-horizontal").find(".password").val();
        var userType = $(this).parents(".form-horizontal").attr("data-type");
        //var remember = $(this).parents(".form-horizontal").find(".metro-checkbox");

        //数据验证
        var regexUsername = "/^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/";    //验证用户名是email格式的
        var regexPassword = "/^[0-9a-zA-Z]{6,18}$/";  //验证密码是6-18位数字或字母
        //var regexUserType = 

        //验证username格式是否正确
        if (CheckStringUsingRegex(regexUsername, username))
        {
            //TODO
            return false;
        }

        if (CheckStringUsingRegex(regexPassword, password))
        {
            //TODO
            return false;
        }

        //将数据发送到后端执行登录
        $.ajax(
        {
            type: "post",
            url: "/Home/Login",
            data:
            {
                username: username,
                password: password,
                userType:userType
            },
            usccess: function (data)
            {
                //data.Status = true登录成功.
                if (data.Status)
                {
                    //重定向到首页
                    //window.location.href = "/D";
                }
                else
                {
                    //登录失败提示
                }
            }
        });

        return false;
    });
});

//通过传入的正则表达式验证传入的数据格式是否正确
function CheckStringUsingRegex(regexString,value)
{
    var regexObject = new RegExp(regexString);
    if (regexObject.exec(value) == value)
    {
        return true;
    }
    return false;
}