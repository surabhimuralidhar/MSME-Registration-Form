using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MsmeReviewApp
{
    public partial class PendingReviews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid(string keyword = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT * FROM MSME_VendorDetails 
                         WHERE Status IN ('Pending', 'Rejected')";

                if (!string.IsNullOrEmpty(keyword))
                {
                    query += @" AND (
                        MSMERegNo LIKE @Keyword OR
                        ClassificationYear LIKE @Keyword OR
                        ClassificationDate LIKE @Keyword OR
                        EnterpriseType LIKE @Keyword OR
                        VendorRemarks LIKE @Keyword
                      )";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                if (!string.IsNullOrEmpty(keyword))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPending.DataSource = dt;
                gvPending.DataBind();
            }
        }


        protected void gvPending_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPending.EditIndex = e.NewEditIndex;
            BindGrid(txtSearchReg.Text.Trim());
        }

        protected void gvPending_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvPending.DataKeys[e.RowIndex].Value);

            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "DELETE FROM MSME_VendorDetails WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            BindGrid(); 
        }

        protected void gvPending_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPending.EditIndex = -1;
            BindGrid(txtSearchReg.Text.Trim());
        }

        protected void gvPending_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvPending.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvPending.Rows[e.RowIndex];
            DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
            TextBox txtRemarks = (TextBox)row.FindControl("txtUpdateRemarks");

            string status = ddlStatus.SelectedValue;
            string remarks = txtRemarks.Text;

            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "UPDATE MSME_VendorDetails SET Status = @Status, VendorRemarks = @Remarks WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Remarks", remarks);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvPending.EditIndex = -1;
            BindGrid(txtSearchReg.Text.Trim());
        }

        protected void gvPending_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "EditReview")
    {
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        string msmeregno = gvPending.DataKeys[rowIndex]["MSMERegNo"].ToString();
        string id = gvPending.DataKeys[rowIndex]["ID"].ToString();

        Response.Redirect($"MainForm.aspx?edit=1&regno={msmeregno}&id={id}");
    }
}

        protected void txtSearchReg_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchReg.Text.Trim();
            BindGrid(keyword);
        }


        protected void gvPending_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvPending.EditIndex != e.Row.RowIndex)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();

                if (status == "Rejected")
                {
                    LinkButton editBtn = (LinkButton)e.Row.FindControl("btnEdit");
                    HyperLink hlEdit = (HyperLink)e.Row.FindControl("hlEdit");

                    if (editBtn != null)
                    {
                        editBtn.Enabled = false;
                        editBtn.Text = "No Actions";
                        editBtn.ForeColor = System.Drawing.Color.Gray;
                    }

                    if (hlEdit != null)
                    {
                        hlEdit.Enabled = false;
                        hlEdit.Text = "Not Editable";
                        hlEdit.ForeColor = System.Drawing.Color.Gray;
                        hlEdit.NavigateUrl = ""; 
                    }
                }
            }
        }

    }
}
