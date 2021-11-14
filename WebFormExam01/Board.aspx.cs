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

        protected string rowCnt = "3";
        protected string userName = string.Empty;
        protected string checkStatus = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            //queystring으로 들어올때 처리
            if (Request["RowCnt"] != null) 
            {
                rowCnt = Request["RowCnt"];
                ddl_pagesize.SelectedValue = rowCnt;
            }
            if (Request["UserName"] != null)
            {
                userName = Request["UserName"];
                txb_name.Text = userName;
            }
            if (Request["CheckYN"] != null)
            {
                checkStatus = Request["CheckYN"];
                hidden_checkStatus.Value = checkStatus;
            }


            if (!IsPostBack)
            {
                mf_List();
            }

        }

        [WebMethod]
        public static string update_checkStatusAll(string idxs, string curStatus)
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

            return "SUCCESS";

        }


        protected void mf_List()
        {


            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con.Open();

            SqlCommand cmd = new SqlCommand("readPost", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", txb_name.Text);
            cmd.Parameters.AddWithValue("@CheckYN", hidden_checkStatus.Value);
            cmd.Parameters.AddWithValue("@BlockYN", "");
            cmd.Parameters.AddWithValue("@RowCnt", ddl_pagesize.SelectedValue);

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


        //드랍다운리스트의 항목을 변경했을 때 이 이벤트가 발생한다. 
        protected void ddl_pagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf_List();
        }
    }
}