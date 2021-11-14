<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Board.aspx.cs" Inherits="WebFormExam01.Board" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src ="https://code.jquery.com/jquery-latest.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if ($("#<%=hidden_checkStatus.ClientID%>").attr("value") == 'Y') {
                $("#<%=btn_checkList.ClientID%>").css({ "color": "#ffffff", "background-color": "#000000" });
                $("#<%=btn_unCheckedList.ClientID%>").css({ "color": "#000000", "background-color": "ffffff" });
            } else {
                
                $("#<%=btn_unCheckedList.ClientID%>").css({ "color": "#ffffff", "background-color": "#000000" });
                $("#<%=btn_checkList.ClientID%>").css({ "color": "#000000", "background-color": "ffffff" });

            }


            $("#check_All").click(function () {
                $(".cb_check").prop("checked", $(this).is(':checked'));
            });



            //선택된 포스트의 검토 상태를 일괄 변경
            $('.btn_update_checkStatusAll').click(function(){

            var idxs = "";
            var cnt = 0;

            if($(".cb_check:checked").length <= 0){
                alert("랭킹에 반영할 작품을 선택해주세요.");
                return;
            }

            $(".cb_check:checked").each(function () {
                
                    idxs += $(this).val() + ",";
                    cnt++;
               
            });

            var curStatus = $(this).attr("name");
            var strQuestion;
            var strSuccess;
            var strFail;

            if (curStatus == "N") {
                strQuestion = "모두 검토 반영하시겠습니까?" + idxs
                strSuccess = "총 " + cnt + " 개의 게시글이 검토 반영되었습니다";
                strFail = "검토 반영에 실패했습니다";
            } else {
                strQuestion = "모두 검토 반영을 해제하시겠습니까?"
                strSuccess = "총 " + cnt + " 개의 게시글이 검토 반영 해제되었습니다";
                strFail = "검토 반영 해제에 실패했습니다";
            }


            if (confirm(strQuestion)) {
                $.ajax({
                    type: "POST",
                    url: location.pathname + "/update_checkStatusAll",
                    contentType: "application/json; charset=utf-8",
                    data: "{ 'idxs':" + "'" + idxs + "'" + ", 'curStatus':" + "'" + curStatus + "'" + "}",
                    dataType: "json",
                    success: (data) => alert(data.d),
                    error: () => alert(strFail),
                    complete: () => location.reload() 
                })
            }
            return;


            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" id="btn_checkList" text="확인리스트" OnClick="btn_checkList_Click"/>
            <asp:Button runat="server" id="btn_unCheckedList" text="미확인리스트" OnClick="btn_unCheckedList_Click"/>
        </div>

        <div>
            <asp:TextBox ID="txb_name" runat="server"></asp:TextBox>
            <asp:Button ID="btn_search" runat="server" Text="검색" OnClick="btn_search_Click" />
        </div>

        <div>
            <input type="button" class="btn_update_checkStatusAll" value="검토 반영하기" name="N" />
            <input type="button" class="btn_update_checkStatusAll" value="검토 해제하기" name="Y" />
        </div>


        <div>
            <asp:DropDownList ID="ddl_pagesize" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddl_pagesize_SelectedIndexChanged">
                <asp:ListItem Value="3" Text="3" Selected="True"></asp:ListItem>
                <asp:ListItem Value="5" Text="5"></asp:ListItem>
            </asp:DropDownList>
        </div>

    <table border="1px solid black">
        <thead>
            <tr>
                <th><input type="checkbox" id="check_All" /></th>
                <th>아이디</th>
                <th>이름</th>
                <th>확인여부</th>
                <th>차단여부</th>
            </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="repList" runat="server" EnableViewState="true" >
           <ItemTemplate>
            
                   <tr>
                       <td><input type="checkbox" class="cb_check" value="<%# Eval("Id") %>" /></td>
                       <td><%# Eval("Id") %></td>
                       <td><%# Eval("Name") %></td>
                       <td><%# Eval("CheckYN") %></td>
                       <td><%# Eval("BlockYN") %></td>
                   </tr>
             
           </ItemTemplate>
         </asp:Repeater>
        </tbody>
    </table>

        <asp:Label ID="lbl_test" runat="server" ></asp:Label>
        <asp:HiddenField ID="hidden_checkStatus" runat="server" Value="N" />

    </form>
</body>
</html>
