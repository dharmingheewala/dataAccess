using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public class DataAccess
    {
        //string commonConnectionString = "Data Source=source;Initial Catalog=dbname;User ID=username;Password=password";
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            List<string> vs = new List<string>();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = null;
            if (!string.IsNullOrEmpty(spName))
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            if (p.ParameterName != "@RETURN_VALUE")
                            {
                                vs.Add(p.ParameterName);
                            }
                        }
                        cmd.Parameters.Clear();
                        for (int i = 0; i < parameterValues.Length; i++)
                        {
                            if (parameterValues[i] == null)
                            {
                                parameterValues[i] = (Object)DBNull.Value;
                            }
                            cmd.Parameters.AddWithValue(vs[i], parameterValues[i]);
                        }
                        sqlDataAdapter.SelectCommand = cmd;


                        try
                        {
                            using (var da = new SqlDataAdapter(cmd))
                            {
                                ds = new DataSet();
                                da.Fill(ds);
                            }
                        }
                        catch
                        {

                        }
                        cmd.Dispose();
                        cnn.Close();
                    }
                }
            }
            return ds;
        }

        //public static DataSet GetDataSet(string commandText, CommandType commandType, params SqlParameter[] parameters)
        //{
        //    DataSet ds = null;
        //    if (!string.IsNullOrEmpty(commandText))
        //    {
        //        using (var cnn = new SqlConnection(Settings.GetConnectionString()))
        //        {
        //            var cmd = cnn.CreateCommand();
        //            cmd.CommandText = commandText;
        //            cmd.CommandType = commandType;

        //            foreach (var item in parameters)
        //                cmd.Parameters.Add(item);

        //            cnn.Open();

        //            using (var da = new SqlDataAdapter(cmd))
        //            {
        //                ds = new DataSet();
        //                da.Fill(ds);

        //            }

        //            cmd.Dispose();
        //            cnn.Close();
        //        }
        //    }
        //    return ds;
        //}

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            List<string> vs = new List<string>();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            int result = 0;
            if (!string.IsNullOrEmpty(spName))
            {
                using (var cnn = new SqlConnection(connectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(spName, cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            if (p.ParameterName != "@RETURN_VALUE")
                            {
                                vs.Add(p.ParameterName);
                            }
                        }
                        cmd.Parameters.Clear();
                        for (int i = 0; i < parameterValues.Length; i++)
                        {
                            if (parameterValues[i] == null)
                            {
                                parameterValues[i] = (Object)DBNull.Value;
                            }
                            cmd.Parameters.AddWithValue(vs[i], parameterValues[i]);
                        }
                        sqlDataAdapter.SelectCommand = cmd;

                        result = cmd.ExecuteNonQuery();

                        cmd.Dispose();
                        cnn.Close();
                    }
                }
            }
            return result;
        }

        public static DataSet ExecuteDataSet(SqlConnection sqlConnection, string spName)
        {
            DataSet data = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            try
            {
                sqlConnection.Open();
                SqlCommand sql = new SqlCommand(spName, sqlConnection);
                sql.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand = sql;
                sqlDataAdapter.Fill(data);
                sqlConnection.Close();
            }
            catch (Exception)
            {

            }
            return data;
        }
    }

}