using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace MsmeReviewApp
{
    public partial class MainForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["edit"] == "1" &&
                    !string.IsNullOrEmpty(Request.QueryString["regno"]) &&
                    !string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string regNo = Request.QueryString["regno"];
                    string id = Request.QueryString["id"];
                    LoadExistingData(regNo, id);
                }
            }
        }

        private void LoadExistingData(string regNo, string id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM MSME_VendorDetails WHERE MSMERegNo = @RegNo AND ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RegNo", regNo);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtRegNo.Text = reader["MSMERegNo"].ToString();
                    ddlClassYear.SelectedValue = reader["ClassificationYear"].ToString();
                    txtClassDate.Text = Convert.ToDateTime(reader["ClassificationDate"]).ToString("yyyy-MM-dd");
                    ddlType.SelectedValue = reader["EnterpriseType"].ToString();
                    txtRemarks.Text = reader["VendorRemarks"].ToString();

                   
                    ViewState["OriginalID"] = reader["ID"].ToString();
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblUploadError.Text = "";

            string connStr = ConfigurationManager.ConnectionStrings["MsmeDB"].ConnectionString;
            string fileName = "";

            string regNo = txtRegNo.Text.Trim();
            int classYear = Convert.ToInt32(ddlClassYear.SelectedValue);
            DateTime classDate = Convert.ToDateTime(txtClassDate.Text);
            string type = ddlType.SelectedValue;
            string remarks = txtRemarks.Text.Trim();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

               
                if (ViewState["OriginalID"] != null)
                {
                    int originalId = Convert.ToInt32(ViewState["OriginalID"]);

                    
                    if (!fuCertificate.HasFile)
                    {
                        SqlCommand getCmd = new SqlCommand("SELECT MSMERegistrationCertificate FROM MSME_VendorDetails WHERE ID = @ID", con);
                        getCmd.Parameters.AddWithValue("@ID", originalId);
                        object result = getCmd.ExecuteScalar();
                        if (result != null)
                        {
                            fileName = result.ToString();
                        }
                    }
                    else
                    {
                       
                        string ext = Path.GetExtension(fuCertificate.FileName).ToLower();
                        if (ext != ".pdf")
                        {
                            lblUploadError.Text = "Only PDF files are allowed.";
                            return;
                        }

                        fileName = Guid.NewGuid().ToString() + ext;
                        string savePath = Server.MapPath("~/Uploads/") + fileName;
                        fuCertificate.SaveAs(savePath);
                    }

                    
                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM MSME_VendorDetails WHERE ID = @ID", con);
                    deleteCmd.Parameters.AddWithValue("@ID", originalId);
                    deleteCmd.ExecuteNonQuery();
                }
                else
                {
                    
                    if (!fuCertificate.HasFile)
                    {
                        lblUploadError.Text = "Certificate file is required.";
                        return;
                    }

                    string ext = Path.GetExtension(fuCertificate.FileName).ToLower();
                    if (ext != ".pdf")
                    {
                        lblUploadError.Text = "Only PDF files are allowed.";
                        return;
                    }

                    fileName = Guid.NewGuid().ToString() + ext;
                    string savePath = Server.MapPath("~/Uploads/") + fileName;
                    fuCertificate.SaveAs(savePath);
                }

                
                string insertQuery = @"INSERT INTO MSME_VendorDetails
            (MSMERegNo, ClassificationYear, ClassificationDate, EnterpriseType, MSMERegistrationCertificate, VendorRemarks, Status)
            VALUES (@RegNo, @Year, @Date, @Type, @Certificate, @Remarks, 'Pending')";

                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@RegNo", regNo);
                insertCmd.Parameters.AddWithValue("@Year", classYear);
                insertCmd.Parameters.AddWithValue("@Date", classDate);
                insertCmd.Parameters.AddWithValue("@Type", type);
                insertCmd.Parameters.AddWithValue("@Certificate", fileName);
                insertCmd.Parameters.AddWithValue("@Remarks", remarks);
                insertCmd.ExecuteNonQuery();
            }

            Response.Redirect("PendingReviews.aspx");
        }


    }
}
