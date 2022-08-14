using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MAKClaims.Models;

namespace MAKClaims.HelperClasses
{
    public class Connection
    {
        // Get all rolls to compare when logging in
        public DataSet AllRolls()
        {
            SqlConnection sc = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand("GetUserRoleByUserId", sc);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet rollTable = new DataSet();
            da.Fill(rollTable);
            return rollTable;
        }
    }
}