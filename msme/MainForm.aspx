<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="MsmeReviewApp.MainForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head><title>MSME Vendor Review</title></head>
<body>
    <form id="form1" runat="server">
        <h2>MSME Vendor Review Form</h2>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" /><br /><br />

        <asp:Label Text="MSME Reg No:" runat="server" /><br />
        <asp:TextBox ID="txtRegNo" runat="server" /><br />

        <asp:Label Text="Classification Year:" runat="server" /><br />
        <asp:DropDownList ID="ddlClassYear" runat="server">
            <asp:ListItem Text="Select" Value="" />
            <asp:ListItem Text="2023" />
            <asp:ListItem Text="2024" />
            <asp:ListItem Text="2025" />
        </asp:DropDownList><br />

        <asp:Label Text="Classification Date:" runat="server" /><br />
        <asp:TextBox ID="txtClassDate" runat="server" TextMode="Date" /><br />

        <asp:Label Text="Enterprise Type:" runat="server" /><br />
        <asp:DropDownList ID="ddlType" runat="server">
            <asp:ListItem Text="Select" Value="" />
            <asp:ListItem Text="Micro" />
            <asp:ListItem Text="Small" />
            <asp:ListItem Text="Medium" />
        </asp:DropDownList><br />

        <asp:Label Text="Upload MSME Registration Certificate (PDF only):" runat="server" /><br />
        <asp:FileUpload ID="fuCertificate" runat="server" /><br />
        <asp:Label ID="lblUploadError" runat="server" ForeColor="Red" /><br />

        <asp:Label Text="Remarks:" runat="server" /><br />
        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" /><br /><br />

        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br /><br />

        <a href="PendingReviews.aspx">Pending Reviews</a> |
        <a href="ApprovedReviews.aspx">Approved Reviews</a>
    </form>
</body>
</html>
