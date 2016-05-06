$(document).ready(function ()
{
    $(".Active-Command").click(function ()
    {
        localStorage.setItem("name", "");
        var sys = $('input').each(function ()
        {
            localStorage.setItem("name", localStorage.getItem("name") + $(this).attr("data-sysname") + ",");
        });
        var names = localStorage.getItem("name").substring(0, localStorage.getItem("name").length - 1);

        $.ajax(
        {
            type: "post",
            url: "/System/ActiveSystem",
            data: {
                names: names
            },
            success: function (data)
            {
                window.location.href = "/JobMonitor/Index";
            }
        });
    });
    LoadSystemInfo();
    setInterval("LoadSystemInfo()", 3000);
});

function LoadSystemInfo()
{

    $.ajax({
        url: "/System/SystemInfo",
        type: "post",
        dataType: "html",
        success: function (data)
        {
            SaveStatus();
            $(".table-bordered").remove();
            $(".context").append(data);
            Resore();
        }
    });
}
var status = [];
function SaveStatus()
{
    $('.sys_id').each(function ()
    {
        localStorage.setItem($(this).attr("data-sysname"), $(this).prop("checked"));
    });
}

function Resore()
{
    $('.sys_id').each(function ()
    {
        $(this).prop("checked", localStorage.getItem($(this).attr("data-sysname")));
    });
}