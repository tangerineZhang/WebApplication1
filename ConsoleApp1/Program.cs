using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Activities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WXSend;



namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            OnePeoper one = new OnePeoper();
            DataTable dt = one.AllUser();//所有人账号
            DataTable Tacc = one.TAccount();//提交人账号
            DataTable Sacc = one.SAccount();//审批人账号
            DataTable Gacc = one.GAccount();//管理员账号
            DataTable All = one.DataAll();//所有数据
            
           


            DataRow[] Tdate = null;
            DataRow[] Sdate = null;
            DataRow[] Gdate = null;
            DataRow[] Sumall = null;
            string connStr = ConfigurationManager.AppSettings["QWXurl"];
            try
            {
                string kk = "截至" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")+"\n";
              
                string ss = "";
                int sum = 0;
                //循环用户表
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss = "";  //每次把消息清空
                    Tdate = Tacc.Select("Originator = '" + dt.Rows[i]["EmployeeNo"].ToString() + "'");
                    if (Tdate.Count() > 0)
                    {
                       

                        ss += "【我发起】有" + Tdate[0]["lcCount"].ToString() + "条未完结流程，请及时跟进 , <a href=\"" + connStr + "/Data/TSelectName?names=" + dt.Rows[i]["EmployeeNo"].ToString() + "\">点击查看详情</a>\n";
                    }
                    Sdate = Sacc.Select("ProcInstID like '%" + dt.Rows[i]["EmployeeNo"].ToString() + "%' and  JobUserName  like  '%"+dt.Rows[i]["Name"].ToString()+"%'");
                    if (Sdate.Count() > 0)
                    {
                        DataTable Scount = one.SNameoneCount(dt.Rows[i]["EmployeeNo"].ToString(), dt.Rows[i]["Name"].ToString());
                        if (Scount.Rows.Count>0)
                        {
                            ss += "【待审批】有" + Sdate.Count().ToString() + "条未完结流程，其中有" + Scount.Rows.Count.ToString() + "条流程超过7天，请及时处理, <a href=\"" + connStr + "/Data/SSelectName?names=" + dt.Rows[i]["EmployeeNo"].ToString() + "&Mz=" + dt.Rows[i]["Name"] + "\">点击查看详情</a>\n";
                        }
                        else
                        {
                            ss += "【待审批】有" + Sdate.Count().ToString() + "条未完结流程,请及时处理, <a href=\"" + connStr + "/Data/SSelectName?names=" + dt.Rows[i]["EmployeeNo"].ToString() + "&Mz=" + dt.Rows[i]["Name"] + "\">点击查看详情</a>\n";
                        }
                    }
                    Gdate = Gacc.Select("UserAccount = '" + dt.Rows[i]["EmployeeNo"].ToString() + "'");
                    string magtwo = "";
                    if (Gdate.Count() > 0)
                    {
                        Sumall = All.Select("OrgName = '" + Gdate[0]["JoinNmae"].ToString() + "'");
                        sum = Sumall.Count();
                        DataTable countsum = one.DaidubanSum(Gdate[0]["JoinNmae"].ToString());
                        if (countsum.Rows.Count>0)
                        {
                            magtwo += "【待督办】有" + sum + "条未完结流程， 其中有" + countsum.Rows.Count.ToString() + "条流程超过7天，请及时督办，<a href=\"" + connStr + "/Data/Selct?ids=" + dt.Rows[i]["EmployeeNo"].ToString() + "\" >点击查看详情</a>\n";
                        }
                        else
                        {
                            magtwo += "【待督办】有" + sum + "条未完结流程，请及时督办，<a href=\"" + connStr + "/Data/Selct?ids=" + dt.Rows[i]["EmployeeNo"].ToString() + "\" >点击查看详情</a>\n";
                        }
                    }
                    if (ss != "")
                    {
                        string msg = kk + ss;
                     //  string touser = "zhang-h2|";
                      string touser = dt.Rows[i]["EmployeeNo"].ToString();
                         QYWeixinHelper.SendText(touser, msg);
                        one.AddLoges(msg, touser);
                    }
                    if (magtwo != "")
                    {
                        string msg = kk + magtwo;
                     // string touser = "zhang-h2|" /*+ dt.Rows[i]["EmployeeNo"].ToString()*/;
                        string touser =  dt.Rows[i]["EmployeeNo"].ToString();
                        QYWeixinHelper.SendText(touser, msg);
                        one.AddLoges(msg, touser);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }

    }
}
