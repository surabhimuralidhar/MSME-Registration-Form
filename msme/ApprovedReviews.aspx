<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovedReviews.aspx.cs" Inherits="MsmeReviewApp.ApprovedReviews" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approved MSME Reviews</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Approved MSME Reviews</h2>

        <asp:TextBox ID="txtSearchApproved" runat="server" Placeholder="Search..." AutoPostBack="true" OnTextChanged="txtSearchApproved_TextChanged" />
<br /><br />


        <asp:GridView ID="gvApproved" runat="server" AutoGenerateColumns="False">
    <Columns>

        
        <asp:BoundField DataField="MSMERegNo" HeaderText="Reg No" />
        <asp:BoundField DataField="ClassificationYear" HeaderText="Year" />
        <asp:BoundField DataField="ClassificationDate" HeaderText="Date" />
        <asp:BoundField DataField="EnterpriseType" HeaderText="Type" />

        
        <asp:TemplateField HeaderText="Certificate">
            <ItemTemplate>
                <asp:HyperLink ID="hlPDF" runat="server"
                    NavigateUrl='<%# ResolveUrl("~/Uploads/") + Eval("MSMERegistrationCertificate") %>'
                    Text="View PDF" Target="_blank" />
            </ItemTemplate>
        </asp:TemplateField>

        
        <asp:BoundField DataField="VendorRemarks" HeaderText="Remarks" />

    </Columns>
</asp:GridView>

        <p>
    <a href="MainForm.aspx">Submit New Vendor</a> |
    <a href="PendingReviews.aspx">Go to Pending Reviews</a>
</p>



    </form>
</body>
</html>
