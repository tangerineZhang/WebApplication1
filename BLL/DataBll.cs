using DAL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class DataBll
    {

        //超级管理员显示
        public static List<DataTables> SelectData(int pageindex, int pagesize)
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            pairs.Add("@pageindex", pageindex);
            pairs.Add("@pagesize", pagesize);
             DataTable dt= DBHelper.ExecSqlGetDataTable("LCXN_Select", pairs);
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return list; 
        }


        //按事业部显示
        public static List<DataTables> Select(int pageindex, int pagesize,string Name)
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            pairs.Add("@pageindex", pageindex);
            pairs.Add("@pagesize", pagesize);
            pairs.Add("@Name", Name);
            DataTable dt = DBHelper.ExecSqlGetDataTable("LCXN_OrgName", pairs);
           List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
           
            return list;
        }


        //显示用户
        public static List<UserTable> SelectUser()
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            DataTable dt= DBHelpertwo.ExecSqlGetDataTable("LCXN_selectuser", pairs);

            List<UserTable> list = JsonConvert.DeserializeObject<List<UserTable>>(JsonConvert.SerializeObject(dt));
            return list;
          ;
        }


        //按ID查询角色
        public static List<UserTable> User(string ids)
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            pairs.Add("@ids", ids);
          DataTable dt=  DBHelpertwo.ExecSqlGetDataTable("LCXN_User", pairs);
            List<UserTable> list = JsonConvert.DeserializeObject<List<UserTable>>(JsonConvert.SerializeObject(dt));
            return list;
        }

        //计算未完成流程数
        public static List<DataTables> SumCount()
        {
            Dictionary<string, object> pairs = new Dictionary<string, object>();
           
            DataTable dt = DBHelper.ExecSqlGetDataTable("LCXN_SumCount", pairs);
            
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
         
            return list;
        }

    }
}
