using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;


public partial class index : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {        
    }

    public string getData()
    {
        //define connection  
        using (SqlConnection connection = new SqlConnection("Server=DESKTOP-PC\\SQLEXPRESS;UID=contact_manager;PWD=password;Database=contact manager"))
        {

            //define query  
            string query = "Select * from contacts";

            //define command  
            var cmd = new SqlCommand(query, connection);

            //open connection  
            connection.Open();

            //read data  
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            //populate data table
            da.Fill(dt);

            //create list for results
            List<object> resultMain = new List<object>();
            int index = 0;

            foreach (DataRow dr in dt.Rows)
            {
                //create dictionary to hold each record
                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (DataColumn dc in dt.Columns)
                {
                    //add key and value to dictionary
                    if (!String.IsNullOrEmpty(dr[dc].ToString()))
                    {
                        result.Add(dc.ColumnName, dr[dc].ToString());
                    }
                }

                //add dictionary to list
                resultMain.Add(result);
                index++;
            }

            //serialise
            return new JavaScriptSerializer().Serialize(resultMain);
        }
        
    }
} 