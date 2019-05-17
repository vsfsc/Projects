function cacul()
{
    var gv_chkCount = 0;
    var gvCheck;
    var lbAction;
    var table = document.getElementById('<%=gvActions.ClientID%>'); //.getElementsByTagName("input");
    var tr = table.getElementsByTagName("tr");
    var pattem = /^\d+(\.\d+)?$/;
    for (i = 1; i < tr.length; i++) {
        gvCheck = tr[i].getElementsByTagName("td")[7].getElementsByTagName("input")[0];
        lbAction = tr[i].getElementsByTagName("td")[0].getElementsByTagName("input")[0];
        if (gvCheck.checked) {
            gv_chkCount = gv_chkCount + 1;
        }
    }
}
function selectDpList(ddl)
{
    var sIndex = ddl.selectedIndex; //返回选中是第几项 0,1....
    //var sText = ddl.options[sIndex].text; //返回选中的文本--文本1,文本2 ...
    var sValue = ddl.options[sIndex].value; //返回选中的值--v1,v2 ...
    ddl.style.background = getBackCorlor(sValue);
    changeIsMy(ddl);
}
function changeIsMy(thisctl)
{
    var str = thisctl.id;
    var index = str.lastIndexOf("_");
    str = str.substring(0, index) + "_hdfIsMy";
    var isMy = document.getElementById(str);
    if (isMy.value === "0")
    {
        isMy.value = "01";
    }
    else
    {
        isMy.value = "11";
    }
}
function getBackCorlor(flag)
{
    var style = "background-color:#F0F0F0";
    switch (flag)
    {
        case "6":
            style = "background-color:#FFCCCC";
            break;
        case "5":
            style = "background-color: #FFE5CC";
            break;
        case "4":
            style = "background-color:#FFFFCC";
            break;
        case "3":
            style = "background-color:#CCE5FF";
            break;
        case "2":
            style = "background-color:#CCFFCC";
            break;
        case "1":
            style = "background-color:#F0F0F0";
            break;
        default:
            style = "background-color:#F0F0F0";
            break;
    }
    return style;
}
function IsFormChanged()
{
    var isChanged = false;
    var form = document.forms[0];
    for (var i = 0; i < form.elements.length; i++)
    {
        var element = form.elements[i];
        var type = element.type;
        if (type === "text" || type === "hidden" || type === "textarea" || type === "button")
        {
            if (element.value !== element.defaultValue)
            {
                isChanged = true;
                break;
            }
        }
        else if (type === "radio" || type === "checkbox")
        {
            if (element.checked !== element.defaultChecked)
            {
                isChanged = true;
                break;
            }
        }
        else if (type === "select-one" || type === "select-multiple")
        {
            for (var j = 0; j < element.options.length; j++)
            {
                if (element.options[j].selected!==element.options[j].defaultSelected)
                {
                    isChanged = true;
                    break;
                }
            }
        }
    }
    return isChanged;
}
function close_sure() {
    var ischanged = IsFormChanged();
    if (ischanged) {
        var gnl = confirm("您设置的内容尚未保存，确认保存并关闭页面吗?若不保存，请点击取消！");
        if (gnl === true) {
            //return true;
            var a = " <%=SaveGVToDB()%>";
            console.log(a);
            window.location.href = getbackUrl();
        }
        else {
            window.location.href = getbackUrl();
        }
    }
    else {
        window.location.href = getbackUrl();
    }
}


function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r !== null) return unescape(r[2]);
    return null;
}
function getbackUrl() {
    if (getQueryString("Source") !== null) {
        return getQueryString("Source");
    }
    else {
        var clientContext = SP.ClientContext.get_current();
        return clientContext.get_url();
    }
}