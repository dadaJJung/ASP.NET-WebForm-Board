using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;
namespace WebFormExam01
{

    public partial class Board : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                mf_List();
            }

        }

        [WebMethod]
        public static void update_checkStatusAll(string idxs, string curStatus)
        {

            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand("updateCheckStatusAll", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idxs", idxs);
            cmd.Parameters.AddWithValue("@curStatus", curStatus);
            cmd.ExecuteNonQuery();
            con.Close();

        }


        protected void mf_List()
        {


            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand("writePost", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", txb_name.Text);
            cmd.Parameters.AddWithValue("@CheckYN", hidden_checkStatus.Value);
            cmd.Parameters.AddWithValue("@BlockYN", "");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "Post");

            repList.DataSource = ds.Tables[0];
            repList.DataBind();

            con.Close();

        }

        //체크리스트 클릭시
        protected void btn_checkList_Click(object sender, EventArgs e)
        {
            hidden_checkStatus.Value = "Y";
            mf_List();
        }

        //미체크리스트 클릭시
        protected void btn_unCheckedList_Click(object sender, EventArgs e)
        {

            hidden_checkStatus.Value = "N";
            mf_List();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            mf_List();
        }
    }
}