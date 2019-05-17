 //function AddAttach()
 //     {
 //         var items = GetSelectedItemsID()
 //         if (items.length == 0)
 //             alert("请先选择附件文件！");
 //     }
 //       function GetQueryString(name)
 //       {
 //            var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
 //            var r = window.location.search.substr(1).match(reg);
 //            if(r!=null)return  unescape(r[2]); return null;
 //       }
 //       function getlistitem( resultID)
　//　        {
 //　                  var mycontext=new SP.ClientContext();
 //　                  var mysite=mycontext.get_web();
 //                    var query = new SP.CamlQuery();
 //                    var listRel = "用户媒体";//关系表
 //                    var assID = GetQueryString("ID");
 //                    if (assID == null) assID = 0;//newform.aspx
　//　                 query.set_viewXml("<View><Query>"+
 //                                           "<ViewFields>" +
 //                                               "<FieldRef Name='Title' />" +
 //                                               "<FieldRef Name='AssistantID'/>" +
 //                                               "<FieldRef Name='ResultID'/>" +
 //                                          "</ViewFields>" +
 //                                          "<Where><And>" +
 //                                                "<Eq>" +
 //                                                   "<FieldRef Name='AssistantID'/>" +
 //                                                   "<Value Type='Number'>"+ resultID+"</Value>" +
 //                                                "</Eq>" +
 //                                                "<Eq>" +
 //                                                   "<FieldRef Name='ResultID'/>" +
 //                                                   "<Value Type='Number'>"+ s+"</Value>" +
 //                                                "</Eq>" +
 //                                          "</And></Where></Query></View>");
  
 // 　                 var mylist=mysite.get_lists().getByTitle( listRel );
 //  　                myitem= mylist.getItems(query);
  
 //  　                mycontext.load(myitem);
 //  　                mycontext.executeQueryAsync(Function.createDelegate(this,this.getsuccessed),Function.createDelegate　　　　
  
　//　                (this,this.getfailed));
　//　         }
　//　         function getsuccessed()
　//　         { 
　//                var str="";
　//　             var listsE=myitem.getEnumerator();
　//　             while(listsE.moveNext())
 //　　            {
 //　　             str+=listsE.get_current().get_item("Title")+"<br>";   
 //　　            }
　//　             alert (str);
  
 //　　        }
 //　　        function getfailed(sender,args)
　//　         {
 // 　　            alert("failed~!");
 //　　        }

 //           function createListItem() {
 //               var clientContext =SP.ClientContext.get_current();
 //               var oList = clientContext.get_web().get_lists().getByTitle('操作');
 //               var itemCreateInfo = new SP.ListItemCreationInformation();

 //               this.oListItem = oList.addItem(itemCreateInfo);

 //               oListItem.set_item('Title', 'Item from de Hrnode!');

 //               oListItem.update();

 //               clientContext.load(oListItem);

 //               clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
 //           }

 //           function onQuerySucceeded() {

 //               alert('Item created: ' + oListItem.get_id());
 //           }

 //           function onQueryFailed(sender, args) {

 //               alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
//           }
            //获取选中文件的ID
            function GetItemsID()
            {
                var context = SP.ClientContext.get_current();
                var selectedItems = SP.ListOperation.Selection.getSelectedItems(context);
                var item;
                var ItemIDs = '';
                var i;
                for (i = 0; i < selectedItems.length; i++)
                    ItemIDs += selectedItems[i].id + ',';

                if (ItemIDs.length > 0)
                    ItemIDs = ItemIDs.substr(0, ItemIDs.length - 1);
                return ItemIDs;
            }
 //     //获取用户选的ID
 //           function GetSelectedItemsID() {
 //               var context = SP.ClientContext.get_current();
 //               var selectedItems = SP.ListOperation.Selection.getSelectedItems(context);
 //               var list = context.get_web().get_lists().getByTitle("SelectedItemID");
 //               var item;
 //               var ItemIDs= new Array();　//创建一个数组;

 //               for (item in selectedItems) {
 //                   ItemIDs.push(selectedItems[item].id);
 //                   //context.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
 //               }
                
 //               return ItemIDs;
                    
 //           }
 //  function onQuerySucceeded() {
 //  }
 //  function onQueryFailed(sender, args) {
 //       alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
 //  }
 //  function GetListID() {
 //       listId = SP.ListOperation.Selection.getSelectedList();
 //       alert(listId);
 //  }