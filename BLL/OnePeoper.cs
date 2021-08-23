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
   public class OnePeoper
    {
        WWJSql sql = new WWJSql();
        //提交人的名字和个数
        public List<DataTables> TNameCount()
        {
           
            DataTable dt= sql.Selectone();
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return list;
        }


        //提交人的个人详情
        public List<DataTables> TNameone(string names)
        {
           DataTable dt= sql.Select(names);
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return list;
        }


        //审批人的名字和个数
        public List<DataTables> SNameCount()
        {
           DataTable dt= sql.SelectNameCount();
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return list;
        }


        //审批人的个人详情
        public List<DataTables> SNameone(string names,string Mz)
        {
           DataTable dt=  sql.SelectName(names,Mz);
            List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return list;
        }

        //审批人未完结流程数量大于七日的
        public DataTable SNameoneCount(string names, string Mz)
        {
            DataTable dt = sql.SelectNameCount(names, Mz);
            //List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return dt;
        }
        //带督办未完结流程数大于七天
        public DataTable DaidubanSum(string names)
        {
            DataTable dt = sql.DaidubanCount(names);
            return dt;
        }

        //获取提交人的账号
        public DataTable TAccount()
        {
          DataTable dt= sql.TUserName();
         // List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
          return dt;
        }

        //获取审批人的账号
        public DataTable SAccount()
        {
           DataTable dt= sql.SUserName();
           // List<DataTables> list = JsonConvert.DeserializeObject<List<DataTables>>(JsonConvert.SerializeObject(dt));
            return dt;
        }


        //获取管理员人的账号
        public DataTable GAccount()
        {
           DataTable dt= sql.GUserName();
            //List<UserTable> list = JsonConvert.DeserializeObject<List<UserTable>>(JsonConvert.SerializeObject(dt));
            return dt;
        }

        //获取所有人的账号
        public DataTable AllUser()
        {
           DataTable dt= sql.UserSelect();
           // List<UserAll> list = JsonConvert.DeserializeObject<List<UserAll>>(JsonConvert.SerializeObject(dt));
            return dt;
        }

        //获取所有数据（DataTables）
        public DataTable DataAll()
        {
           DataTable dt= sql.Alldate();
            return dt;
        }
        //添加日志
        public int AddLoges(string mag, string name)
        {
            int i = sql.AddLoge(mag, name);
            return i;
        }
    }
}
