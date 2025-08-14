<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingReviews.aspx.cs" Inherits="MsmeReviewApp.PendingReviews" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pending MSME Reviews</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Pending MSME Reviews</h2>

        <asp:TextBox ID="txtSearchReg" runat="server" AutoPostBack="true" OnTextChanged="txtSearchReg_TextChanged"
            Placeholder="Search" />

        <asp:GridView ID="gvPending" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                      OnRowEditing="gvPending_RowEditing"
                      OnRowCancelingEdit="gvPending_RowCancelingEdit"
                      OnRowDeleting="gvPending_RowDeleting"
                      OnRowUpdating="gvPending_RowUpdating"
                      OnRowDataBound="gvPending_RowDataBound">
            <Columns>
                <asp:BoundField DataField="MSMERegNo" HeaderText="Reg No" ReadOnly="true" />
                <asp:BoundField DataField="ClassificationYear" HeaderText="Year" ReadOnly="true" />
                <asp:BoundField DataField="ClassificationDate" HeaderText="Date" ReadOnly="true" />
                <asp:BoundField DataField="EnterpriseType" HeaderText="Type" ReadOnly="true" />
                <asp:TemplateField HeaderText="Certificate">
                    <ItemTemplate>
    <asp:HyperLink ID="hlPDF" runat="server"
        NavigateUrl='<%# ResolveUrl("~/Uploads/") + Eval("MSMERegistrationCertificate") %>'
        Text="View PDF" Target="_blank" />
</ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VendorRemarks" HeaderText="Remarks" ReadOnly="true" />
                <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true" />
                <asp:TemplateField HeaderText="Update Status">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" Text="Update Status" CommandName="Edit" />
                        <br />
                        <asp:HyperLink ID="hlEdit" runat="server" 
    Text="Edit Info" 
    NavigateUrl='<%# Eval("MSMERegNo", "MainForm.aspx?edit=1&regno={0}&id=") + Eval("ID") %>' />

                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Text="Approved" />
                            <asp:ListItem Text="Rejected" />
                        </asp:DropDownList>
                        <br />
                        <asp:TextBox ID="txtUpdateRemarks" runat="server" Placeholder="Remarks" />
                        <br />
                        <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CommandName="Update" />
                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete">
    <ItemTemplate>
        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" 
                        CommandName="Delete" 
                        OnClientClick="return confirm('Are you sure you want to delete this review?');"
                        Visible='<%# Eval("Status").ToString() == "Pending" %>' />
    </ItemTemplate>
</asp:TemplateField>

            </Columns>
        </asp:GridView>

                <p>
    <a href="MainForm.aspx">Submit New Vendor</a> |
    <a href="ApprovedReviews.aspx">Go to Approved Reviews</a>
</p>

    </form>
</body>
</html>
