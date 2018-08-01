<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="BookCheckInAndOut.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .auto-style2 {
            width: 150px;
        }

        .auto-style3 {
            height: 24px;
        }

        .auto-style4 {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 10px;
            font-style: normal;
            text-align: left;
            height: 25px;
        }

        .auto-style5 {
            width: 150px;
            height: 25px;
        }

        .auto-style6 {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 10px;
            font-style: normal;
            text-align: left;
            height: 22px;
            direction: ltr;
        }

        .auto-style7 {
            width: 150px;
            height: 22px;
            direction: ltr;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <table id="contentTable" border="0">

        <tr>
            <td class="text2">Book Title:</td>
            <td class="auto-style2">
                <asp:Label CssClass="text" ID="lblBookTitle" runat="server" />                
            </td>
        </tr>

        <tr>
            <td class="text2">ISBN:</td>
            <td class="auto-style2">
                <asp:Label CssClass="text" ID="lblISBN" runat="server" />                
            </td>
        </tr>

        <tr>
            <td class="text2">Publish Year:</td>
            <td class="auto-style2">
                <asp:Label CssClass="text" ID="lblPublishYear" runat="server" />  
            </td>
        </tr>

        <tr>
            <td class="text2">Cover Price:</td>
            <td class="auto-style5">
                <asp:Label CssClass="text" ID="lblPrice" runat="server" /> 
            </td>
        </tr>

        <tr>
            <td class="text2">Status:</td>
            <td class="auto-style7">
                <asp:Label CssClass="text" ID="lblStatus" runat="server" />
            </td>
        </tr>

        <tr>
            <td class="auto-style3"></td>
            <td class="auto-style3"></td>
        </tr>        

        <tr>
            <td class="text2" colspan="2">Book Check Out History</td>
        </tr>

        <tr>

            <td colspan="2">

                <table id="HistoryTable">
                    <tr>
                        <td class="GridHeading">Name </td>
                        <td class="GridHeading">Check Out Date  </td>
                        <td class="GridHeading">Returned Date </td>
                    </tr>

                    <asp:ListView ID="HistoryList" runat="server" ClientIDMode="Static">
                        <ItemTemplate>
                            <tr>
                                <td class="text"><%# Eval("Name") %></td>
                                <td class="text"><%# Eval("CheckOutDate") %> </td>
                                <td class="text"><%# Eval("ReturnDate") %> </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>

                </table>


            </td>

        </tr>

    </table>
</asp:Content>