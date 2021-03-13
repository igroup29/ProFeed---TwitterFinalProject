using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using TwitterAPI.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }


    //**************************************************************************
    // User Management
    //**************************************************************************

    //Get user id From DataBase if exist
    public int isExist(string email)
    {
        int returnUserId = 0;
        SqlConnection con = null;

        try
        {
            con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

            //fix here
            String selectSTR = "SELECT UserID FROM TUsers_cs Where Email=" + email;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row

                returnUserId = Convert.ToInt32(dr["UserId"]);
            }
            return returnUserId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
    }

    //unresolved  -  
    public int editUser(TUser user)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("DBConnectionString"); // create the connection
            String cStr = "UPDATE TUsers_CS set UserName = '" + user.UserName + "' where UserID = " + user.UserID; // helper method to build the insert string
            cmd = CreateCommand(cStr, con);             // create the command
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //Get All Lists from DB by User
    public List<TList> getUserLists(TUser user)
    {
        SqlConnection con = null;
        //  SqlConnection con2 = null;
        try
        {
            con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT ListID,ListAVG,Product";
            selectSTR += "FROM TList_CS";
            selectSTR += "WHERE UserID =" + user.UserID;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            //  SqlCommand cmd2;

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                                                                                   // SqlDataReader dr2;

            List<TList> Tlist = new List<TList>();

            while (dr.Read())
            {   // Read till the end of the data into a row
                TList tl = new TList();

                tl.ListID = Convert.ToInt32(dr["ListID"]);
                tl.ListAVGRank = Convert.ToDouble(dr["ListAVGRank"]);
                tl.Product = (string)dr["ListName"];
       
                Tlist.Add(tl);
            }

            //return RecipeList;
            con.Close();
            cmd.Connection.Close();

            return Tlist;


        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //Get All TUses from DB
    public List<TUser> getUsers()
    {
        List<TUser> userList = new List<TUser>();
        SqlConnection con = null;

        try
        {
            con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

            //fix here
            String selectSTR = "SELECT * FROM TUser_CS";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                TUser user = new TUser();
                user.UserID = Convert.ToInt32(dr["UserID"]);
                user.Email = (string)dr["Email"];
                user.UserName = (string)dr["UserName"];
                //user.Image = 
                userList.Add(user);
            }

            return userList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
    }

    //Insert TUser to DB
    public int insertUser(TUser user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("DBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(user);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Inside catch block. Exception: {0}", ex.Message);
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------
    //Unresolved -  Build the Insert command String
    private String BuildInsertCommand(TUser user)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        sb.AppendFormat("Values('{0}','{1}','{2}')", user.Email, user.UserName, user.Image);
        String prefix = "INSERT INTO Item_CS " + "(Email,FirstName,LastName)";
        command = prefix + sb.ToString();

        return command;
    }


    //**************************************************************************
    // List Management
    //**************************************************************************

    //Get All Lists from DB
    public List<TList> getList()
    {
        SqlConnection con = null;
        //  SqlConnection con2 = null;
        try
        {
            con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM TList_CS";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            //  SqlCommand cmd2;

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                                                                                   // SqlDataReader dr2;

            List<TList> tLists = new List<TList>();

            while (dr.Read())
            {   // Read till the end of the data into a row
                TList tl = new TList();

                tl.ListID = Convert.ToInt32(dr["ListID"]);
                tl.ListAVGRank = Convert.ToDouble(dr["ListAVGRank"]);
                tl.Product = (string)dr["ListName"];


                tLists.Add(tl);
            }

            //return RecipeList;
            con.Close();
            cmd.Connection.Close();

            return tLists;


   
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //Delete list from DB by id
    public void deleteList(int tListId)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("DBConnectionString"); // create the connection
            String cStr = "DELETE FROM TList_CS where ListID = " + tListId;      // helper method to build the insert string
            cmd = CreateCommand(cStr, con);             // create the command
            cmd.ExecuteScalar(); // execute the command
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //Insert HubList to database
    public int insertList(TList tList)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("DBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(tList);      // helper method to build the insert string
        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Inside catch block. Exception: {0}", ex.Message);
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------
    //Unresolved -  Build the Insert command String
    private String BuildInsertCommand(TList tlist)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}','{1}','{2}')", tlist.ListID, tlist.ListAVGRank, tlist.Product);
        String prefix = "INSERT INTO HubList_CS " + "(ListName,ImageURL,ListDescription)";
        command = prefix + sb.ToString();
        return command;
    }

    //**************************************************************************
    // Profile Management
    //**************************************************************************


}




