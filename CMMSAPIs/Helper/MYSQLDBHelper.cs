using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CMMSAPIs.Helper
{
    public class MYSQLDBHelper
    {
        public MYSQLDBHelper(string constr)
        {

            ConnStr = constr;
        }

        private string ConnStr { get; set; }

        internal MySqlConnection TheConnection => new MySqlConnection(ConnStr);

        internal async Task<List<T>> GetData<T>(string qry) where T : class, new()
        {
            try
            {
                using (MySqlConnection conn = TheConnection)
                {
                    using (MySqlCommand cmd = getQryCommand(qry, conn))
                    {
                        await conn.OpenAsync();

                        //MySqlCommand cmd1 = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", conn); // Setting tiimeout on mysqlServer
                        //cmd1.ExecuteNonQuery();
                        cmd.CommandTimeout = 99999;
                        cmd.CommandType = CommandType.Text;
                       
 

                        using (DbDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dataReader);
                            cmd.Parameters.Clear();
                            return dt.MapTo<T>();
                        }
                    }
                }
            }
            catch (MySqlException sqlex)
            {
                throw sqlex;
                throw new Exception("GetData =" +Environment.NewLine, sqlex);
            }
            finally
            {


            }
        }

        internal async Task<int> CheckGetData(string qry)
        {
            try
            {
                using (MySqlConnection conn = TheConnection)
                {
                    using (MySqlCommand cmd = getQryCommand(qry, conn))
                    {
                        await conn.OpenAsync();

                        //MySqlCommand cmd1 = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", conn); // Setting tiimeout on mysqlServer
                        //cmd1.ExecuteNonQuery();
                        cmd.CommandTimeout = 99999;
                        cmd.CommandType = CommandType.Text;



                        using (DbDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dataReader);
                            cmd.Parameters.Clear();
                            return dt.Rows.Count;
                        }
                    }
                }
            }
            catch (MySqlException sqlex)
            {
                throw sqlex;
                throw new Exception("GetData =" + Environment.NewLine, sqlex);
            }
            finally
            {


            }
        }

        internal async Task<int> ExecuteNonQuery(string SpName, MySqlParameter[] sqlpara = null, bool returnvalue = false)
        {
           // MySqlParameter retpara = null;

            try
            {
                using (MySqlConnection conn = TheConnection)
                {
                    using (MySqlCommand cmd = getCommand(SpName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        if (sqlpara != null)
                        {
                            foreach (MySqlParameter para in sqlpara)
                            {
                                if (para.Value == null)
                                {
                                    para.Value = DBNull.Value;
                                }

                                cmd.Parameters.Add(para);
                            }
                        }
                      
                        int i = await cmd.ExecuteNonQueryAsync();
                       
                        return i;
                         
                    }
                }
            }
            catch (MySqlException sqlex)
            {
                throw new Exception("ExecuteNonQuery SPname=" + SpName + Environment.NewLine + sqlex.Message, sqlex);
            }
            finally
            {

               // retpara = null;

            }
        }
        internal async Task<int> ExecuteNonQry<T>(string qry)
        {
            // MySqlParameter retpara = null;

            try
            {
                using (MySqlConnection conn = TheConnection)
                {
                    using (MySqlCommand cmd = getQryCommand(qry, conn))
                    {
                        cmd.CommandTimeout = 99999;
                        cmd.CommandType = CommandType.Text;
                        await conn.OpenAsync();
                       

                        int i = await cmd.ExecuteNonQueryAsync();

                        return i;

                    }
                }
            }
            catch (MySqlException sqlex)
            {
                throw new Exception("ExecuteNonQuery SPname=" + qry + Environment.NewLine + sqlex.Message, sqlex);
            }
            finally
            {

                // retpara = null;

            }
        }
        internal async Task<List<T>> ExecuteToDataTable<T>(string SpName, MySqlParameter[] sqlpara = null) where T : class, new()
        {
            try
            {
                using (MySqlConnection conn = TheConnection)
                {
                    using (MySqlCommand cmd = getCommand(SpName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();

                        if (sqlpara != null)
                        {
                            foreach (MySqlParameter para in sqlpara)
                            {
                                if (para.Value == null)
                                {
                                    para.Value = DBNull.Value;
                                }

                                cmd.Parameters.Add(para);
                            }
                        }
                         
                        using (DbDataReader dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dataReader);
                            cmd.Parameters.Clear();
                            return dt.MapTo<T>();
                        }
                    }
                }
            }
            catch (MySqlException sqlex)
            {
                throw sqlex;
                throw new Exception("ExecuteToDataTable SpName=" + SpName + Environment.NewLine, sqlex);
            }
            finally
            {


            }
        }

        internal MySqlCommand getCommand(string command, MySqlConnection conn)
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandTimeout = conn.ConnectionTimeout;
            cmd.CommandText = command;
            return cmd;
        }
        internal MySqlCommand getQryCommand(string qry, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand(qry);   //check if this line is required? see next line
            cmd=conn.CreateCommand();
            cmd.CommandTimeout = conn.ConnectionTimeout;
            cmd.CommandText = qry;
            return cmd;
        }
        internal void Dispose()
        {
            if (TheConnection != null)
            {
                TheConnection.Dispose();
            }
        }
        private int GetConnectionTimeOut()
        {
            return 600;
        }
    }
}
