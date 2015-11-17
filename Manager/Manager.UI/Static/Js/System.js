$(document).ready(function ()
{
    $("#Active-Command").click(function ()
    {
        var names = "{'count':'3','rows':{{'name':'1'},{'name':'2'},{'name':'3'}}}";
        $.ajax(
        {
            type:"post",
            url: "/System",
            data: {
                    names: names
            },
            success:function(data)
            {
                
            }
        });
    });
});