function cacul1(table) {

        var gv_chkCount = 0;
        var gv_during = 0;
        var gvCheck;
        var gvTxt;
        //var table = document.getElementById('<%=gvActivities.ClientID%>');//.getElementsByTagName("input");
        var tr = table.getElementsByTagName("tr");

        var pattem = /^\d+(\.\d+)?$/;
        for (i = 1; i < tr.length; i++) {

            //gvCheck = tr[i].getElementsByTagName("td")[0].getElementsByTagName("input")[0]
            gvTxt = tr[i].getElementsByTagName("td")[2].getElementsByTagName("input")[0]//页面上显示
            if (true) {
                //gv_chkCount = gv_chkCount + 1;
                if (pattem.test(gvTxt.value))
                    gv_during = gv_during + parseFloat(gvTxt.value);
            }

        }
        return "共输入时长 " + gv_during + " 分";
    //"已经选中 " + gv_chkCount + " 条；
}