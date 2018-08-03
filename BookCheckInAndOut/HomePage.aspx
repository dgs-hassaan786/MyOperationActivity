<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="BookCheckInAndOut.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .auto-style1 {
            height: 80px;
        }

        .auto-style2 {
            height: 30px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <table id="ListTable" class="table" border="0">
        <tr>

            <td style="width: 0px"></td>
            <td class="RadioButtonColumn">Select </td>
            <td class="GridHeading">Title </td>
            <td class="GridHeading">ISBN  </td>
            <td class="GridHeading">Publish Year </td>
            <td class="GridHeading">Cover Price </td>
            <td class="GridHeading">Status </td>
        </tr>

        <asp:ListView ID="BooksList" runat="server" ClientIDMode="Static">

            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="hd" ClientIDMode="AutoID" Value='<%# Eval("BookID") %>' />
                    </td>
                    <td>
                        <asp:RadioButton runat="server" ID="rd" ClientIDMode="AutoID" onclick="OnCheckChange(this)" />
                    </td>
                    <td class="text"><%# Eval("Title") %></td>
                    <td class="text"><%# Eval("ISBN") %> </td>
                    <td class="text"><%# Eval("PublishYear") %> </td>
                    <td class="text"><%# String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.##}", Eval("CoverPrice")) %> </td>
                    <td id="rd-<%# Eval("BookID")%>" class="text"><%#Eval("CheckOutStatusDescription")%> </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>

        <tr>
            <td style="height: 50px"></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>            
            <td style="text-align: right">
                <asp:Button ID="btnDetails" CssClass="button" ClientIDMode="Static" runat="server" Text="Details" OnClick="BtnDetails_Click" />
            </td>
            <td style="text-align: right">
                <asp:Button ID="btnCheckOut" CssClass="button" ClientIDMode="Static" runat="server" Text="Check Out" OnClick="BtnCheckOut_Click" />
            </td>
            <td style="text-align: right">
                <asp:Button ID="btnCheckIn" CssClass="button" ClientIDMode="Static" runat="server" Text="Check In" OnClick="BtnCheckIn_Click" />
            </td>

        </tr>
    </table>



    <asp:HiddenField ID="hdnField" runat="server" ClientIDMode="Static" />

    <script type="text/javascript">

        //helper function for fetching the element by id
        function getElementById(elementId) {
            return document.getElementById(elementId);
        }

        //replacement of $(document).ready()
        document.addEventListener("DOMContentLoaded", function (event) {

            //disabling the button on load
            var btnDetails = getElementById('btnDetails');
            var btnCheckOut = getElementById('btnCheckOut');
            var btnCheckIn = getElementById('btnCheckIn');

            btnCheckOut.disabled = true;
            btnCheckIn.disabled = true;
            btnDetails.disabled = true;

        });

        function OnCheckChange(rb) {

            //logic for exclusive setting the radio button that was last clicked.
            var table = getElementById('ListTable');

            var radiobtn = table.getElementsByTagName('input');

            for (k = 0; k < radiobtn.length; k++) {
                if ((radiobtn[k].type == 'radio') && (rb.id == radiobtn[k].id)) {
                    radiobtn[k].checked = true;
                    var checkoutStatus = getElementById('rb-' + rb.id);
                } else {
                    radiobtn[k].checked = false;
                }
            }

            //Book id against the selected radio button.
            var hdfield = rb.parentNode.parentNode.cells[0].getElementsByTagName('input');

            if (hdfield[0].type == 'hidden')
                getElementById("hdnField").value = hdfield[0].value;

            //enabling /disabling buttons on the basis of checkin /checkout

            if (rb.parentNode.parentNode.cells[6].innerHTML.trim().toLowerCase() == 'check in') {
                getElementById('btnCheckIn').disabled = true;
                getElementById('btnCheckOut').disabled = false;
            }
            else if (rb.parentNode.parentNode.cells[6].innerHTML.trim().toLowerCase() == 'check out') {
                getElementById('btnCheckOut').disabled = true;
                getElementById('btnCheckIn').disabled = false;
            }

            getElementById('btnDetails').disabled = false;

        }


    </script>

</asp:Content>