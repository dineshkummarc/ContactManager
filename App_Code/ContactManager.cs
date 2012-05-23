using System;
using System.Data.SqlClient;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Web service to manage contacts in database
/// </summary>

[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ContactManager : System.Web.Services.WebService {

    [WebMethod]
    public string ManageContact()
    {
        //get request data
        string queryID = System.Web.HttpContext.Current.Request.QueryString.ToString();
        string[] idComponents = queryID.Split(new Char[] { '=' });
        string method = (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Headers.Get("X-HTTP-Method-Override"))) ? System.Web.HttpContext.Current.Request.Headers.Get("X-HTTP-Method-Override") : System.Web.HttpContext.Current.Request.HttpMethod;
        string model;
        Newtonsoft.Json.Linq.JObject jObj;
        string photo;
        string name;
        string type;
        string address;
        string tel;
        string email;
        string query;
        string returnMessage;

        switch (method)
        {
            case "DELETE":
                //delete contact  
                query = "Delete from contacts where id = " + idComponents[1];

                //set return message
                returnMessage = "DELETE success";

                //exit switch
                break;
            case "PUT":
                //store model data
                model = System.Web.HttpContext.Current.Request.Form["model"];
                jObj = Newtonsoft.Json.Linq.JObject.Parse(model);
                photo = (string)jObj["photo"];
                name = (string)jObj["name"];
                type = (string)jObj["type"];
                address = (string)jObj["address"];
                tel = (string)jObj["tel"];
                email = (string)jObj["email"];

                //update contact
                query = "Update contacts set name = '" + name + "', type = '" + type + "', photo = '" + photo + "', address = '" + address + "', tel = '" + tel + "', email = '" + email + "' where id = " + idComponents[1];
                returnMessage = "PUT success";
                break;
            case "POST":
                //store model data
                model = System.Web.HttpContext.Current.Request.Form["model"];
                jObj = Newtonsoft.Json.Linq.JObject.Parse(model);
                photo = (string)jObj["photo"];
                name = (string)jObj["name"];
                type = (string)jObj["type"];
                address = (string)jObj["address"];
                tel = (string)jObj["tel"];
                email = (string)jObj["email"];

                //add contact
                query = "Insert into contacts values('" + name + "', '" + address + "', '" + tel + "', '" + email + "', '" + type + "', '" + photo + "')";
                returnMessage = "POST success";
                break;
            default:
                query = null;
                returnMessage = "No valid method received";
                break;
        }

        if (!String.IsNullOrEmpty(query))
        {
            //define connection  
            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-PC\\SQLEXPRESS;UID=contact_manager;PWD=password;Database=contact manager"))
            {
                //define command  
                var cmd = new SqlCommand(query, connection);

                //open connection to database 
                connection.Open();

                //execute command
                cmd.ExecuteNonQuery();
            }
        }

        return returnMessage;
    }
}