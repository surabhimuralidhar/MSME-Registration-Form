using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MsmeReviewApp
{
    public partial class ApprovedReviews : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
            if (ConfigurationManager.ConnectionStrings["MsmeDB"] == null)
            {
                throw new Exception("Connection string 'MsmeDB' not found in web.config.");
            }
        }

        protected void txtSearchApproved_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchApproved.Text.Trim();
            BindGrid(keyword);
        }




        private void BindGrid(string keyword = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT * FROM MSME_VendorDetails WHERE Status = 'Approved'";

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

                gvApproved.DataSource = dt;
                gvApproved.DataBind();
            }
        }
    }
}
