using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class DBHelpertwo
    {
        // 连接数据库的字符串(了一通过工具连接)
        //
        

        static string str = System.Configuration.ConfigurationManager.AppSettings["SQLTwo"];
        /// <summary>
        /// 执行存储过程放回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static int ExecSqlGetQuery(string sql, Dictionary<string, object> pairs)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conn;
                foreach (var item in pairs)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
                int i = command.ExecuteNonQuery();
                conn.Close();
                return i;
            }
        }


        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static DataTable ExecSqlGetDataTable(string sql, Dictionary<string, object> pairs)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = conn;
                foreach (var item in pairs)
                {
                    command.Parameters.AddWithValue(item.Key,item.Value);
                }
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();
                conn.Close();
                return dt;
            }
        }

        /// <summary>
        /// 执行增删改,返回受影响行数
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int ExecSqlResult(string strSql)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(strSql,conn);
                int result = command.ExecuteNonQuery();
                conn.Close();
                return result;
            }
        }

        /// <summary>
        /// 执行查询SQL语句,返回DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable ExecSqlDateTable(string strSql)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter(strSql, conn);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                sqlData.Dispose();
                conn.Close();
                return dt;
            }

        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static object ExecSql(string strSql)
        {
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(strSql, conn);
                object result = command.ExecuteScalar();
                conn.Close();
                return result;
            }
        }

      

    }
}
