using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsoleApp1;

namespace WebApplication1.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        #region 显示所有
        public ActionResult Index()
        {
            int page = 1; int limit = 100;
            List<DataTables> list = DataBll.SelectData(page, limit);
            int PageCount = (list == null || list.Count == 0) ? 0 : list[0].rowall;
            ViewData["numsum"] = PageCount;
            return View();
        }
        public ActionResult view(int page, int limit)
        {
            List<DataTables> list = DataBll.SelectData(page, limit);
            int PageCount = (list == null || list.Count == 0) ? 0 : list[0].rowall;
            int c = (int)Math.Ceiling((decimal)PageCount / limit);
            ViewBag.parper = (page <= 1) ? 1 : page - 1;
            ViewBag.pagenext = page >= c ? c : page + 1;
            ViewBag.pahelast = c;
            return Json(new LayUi { code = "0", msg = "", count = PageCount.ToString(), data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  带督办显示界面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">账号</param>
        /// <returns></returns>
        public ActionResult Selct(string ids)
        {
            string Mz = "";
            OnePeoper one = new OnePeoper();
            ViewData["id"] = ids;
            List<UserTable> list = DataBll.User(ids);
            if (list.Count > 0)
            {
                Mz = list[0].UserName.ToString();
                ViewData["User"] = Mz;
            }
            else
            {
                Mz = "";
            }
            
            ViewData["Tname"] = ids;
            ViewData["Sname"] = ids;
            if (list.Count > 0)
            {

                for (int i = 0; i < list.Count(); i++)
                {
                    if (ids == list[i].UserAccount)
                    {
                        Session["name"] = list[i].JoinNmae;
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                Session["name"] = "";
            }
            #region 消息数量


            int page = 1; int limit = 10;
            List<DataTables> lists = DataBll.Select(page, limit, Session["name"].ToString());
            int PageCount = (lists == null || lists.Count == 0) ? 0 : lists[0].rowall;
            if (PageCount > 0)
            {
                ViewData["GSum"] = PageCount.ToString();
            }
            else
            {
                ViewData["GSum"] = "0";
            }


            List<DataTables> listtwo = one.SNameone(ids,Mz);
            int PageCountone = listtwo.Count();
            if (PageCountone > 0)
            {
                ViewData["Ssum"] = PageCountone.ToString();
            }
            else
            {
                ViewData["Ssum"] = "0";
            }


            List<DataTables> listone = one.TNameone(ids);
            int PageCounttwo = listone.Count();

            if (PageCounttwo > 0 && Session["name"].ToString() != "")
            {
                ViewData["TSum"] = PageCounttwo.ToString();
            }
            else
            {
                ViewData["TSum"] = "0";
            }


            #endregion

            return View();
        }
        public ActionResult Look(int page,string Name,int limit=100)
        {
            if (Session["name"] != null)
            {
                Name = Session["name"].ToString();
            }
            else
            {
                Name = "";
            }
            List<DataTables> list = DataBll.Select(page, limit, Name);
            int PageCount = (list == null || list.Count == 0) ? 0 : list[0].rowall;
            if (PageCount > 0)
            {
                ViewData["GSum"] = PageCount.ToString();
            }
            else
            {
                ViewData["GSum"] = "0";
            }
            int c = (int)Math.Ceiling((decimal)PageCount / limit);
            ViewBag.parper = (page <= 1) ? 1 : page - 1;
            ViewBag.pagenext = page >= c ? c : page + 1;
            ViewBag.pahelast = c;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].OrgName == Name)
                {

                }
                else
                {
                    Console.Write("请联系管理员");
                }
            }
            return Json(new LayUi { code = "0", msg = "", count = PageCount.ToString(), data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 我发起页面显示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="names">账号</param>
        /// <returns></returns>
        public ActionResult TSelectName(string names)
        {
            string Mz = "";
            OnePeoper one = new OnePeoper();
            List<DataTables> list = one.TNameone(names);
            if (list.Count > 0)
            {
                Mz = list[0].OriginatorName.ToString();
                ViewData["User"] = Mz;
            }
            else
            {
                Mz = "";
            }

            Session["sss"] = names;
            ViewData["Tname"] = names;
            
            ViewData["Gname"] = names;
            ViewData["Sname"] = names;
            #region 消息数量
            int page = 1; int limit = 10;
            List<DataTables> lists = DataBll.Select(page, limit, names);
            int PageCount = (lists == null || lists.Count == 0) ? 0 : lists[0].rowall;
            if (PageCount > 0)
            {
                ViewData["GSum"] = PageCount.ToString();
            }
            else
            {
                ViewData["GSum"] = "0";
            }
            List<DataTables> listtwo = one.SNameone(names, Mz);
            int PageCountone = listtwo.Count();
            if (PageCountone > 0)
            {
                ViewData["Ssum"] = PageCountone.ToString();
            }
            else
            {
                ViewData["Ssum"] = "0";
            }
            List<DataTables> listone = one.TNameone(names);
            int PageCounttwo = list.Count();
            if (PageCounttwo > 0)
            {
                ViewData["TSum"] = PageCounttwo.ToString();
            }
            else
            {
                ViewData["TSum"] = "0";
            }
            #endregion
            return View();
        }
        public ActionResult TName(int page = 1, int limit = 10, string names = "")
        {
            names = Session["sss"].ToString();
            if (names == null)
            {
                Console.Write("您暂时还没有为提交的申请");
                names = Session["kkk"].ToString();
            }
            OnePeoper one = new OnePeoper();
            List<DataTables> list = one.TNameone(names);
            int PageCount = list.Count();

            if (PageCount > 0)
            {
                ViewData["TSum"] = PageCount.ToString();
            }
            else
            {
                ViewData["TSum"] = "0";
            }


            int c = (int)Math.Ceiling((decimal)PageCount / limit);
            ViewBag.parper = (page <= 1) ? 1 : page - 1;
            ViewBag.pagenext = page >= c ? c : page + 1;
            ViewBag.pahelast = c;
            return Json(new LayUi { code = "0", msg = "", count = PageCount.ToString(), data = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 待审批页面显示
       /// <summary>
       /// 
       /// </summary>
       /// <param name="names">账号</param>
       /// <param name="Mz">名字</param>
       /// <returns></returns>
        public ActionResult SSelectName(string names,string Mz)
        {
            OnePeoper one = new OnePeoper();
            List<DataTables> list = one.SNameone(names, Mz);
            Session["kkk"] = names;
            Session["MZ"] = Mz;
            ViewData["User"] = Mz;
            ViewData["Gname"] = names;
            ViewData["Tname"] = names;
            #region 消息数量
            int page = 1; int limit = 10;
            List<DataTables> lists = DataBll.Select(page, limit, names);
            int PageCount = (lists == null || lists.Count == 0) ? 0 : lists[0].rowall;
            if (PageCount > 0)
            {
                ViewData["GSum"] = PageCount.ToString();
            }
            else
            {
                ViewData["GSum"] = "0";
            }
            List<DataTables> listtwo = one.SNameone(names, Mz);
            int PageCountone = listtwo.Count();
            if (PageCountone > 0)
            {
                ViewData["Ssum"] = PageCountone.ToString();
            }
            else
            {
                ViewData["Ssum"] = "0";
            }
            List<DataTables> listone = one.TNameone(names);
            int PageCounttwo = listone.Count();
            if (PageCounttwo > 0)
            {
                ViewData["TSum"] = PageCounttwo.ToString();
            }
            else
            {
                ViewData["TSum"] = "0";
            }
            #endregion
            return View();
        }
        public ActionResult SName(string names, string Mz, int page = 1, int limit = 100)
        {
            names = Session["kkk"].ToString();
            Mz = Session["MZ"].ToString();
            OnePeoper one = new OnePeoper();
            List<DataTables> list = one.SNameone(names,Mz);
            int PageCount = list.Count();
            if (PageCount > 0)
            {
                ViewData["Ssum"] = PageCount.ToString();
            }
            else
            {
                ViewData["Ssum"] = "0";
            }
            int c = (int)Math.Ceiling((decimal)PageCount / limit);
            ViewBag.parper = (page <= 1) ? 1 : page - 1;
            ViewBag.pagenext = page >= c ? c : page + 1;
            ViewBag.pahelast = c;
            return Json(new LayUi { code = "0", msg = "", count = PageCount.ToString(), data = list }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //Layui帮助类返回格式
        public class LayUi
        {
            public string code { get; set; }
            public string msg { get; set; }
            public string count { get; set; }
            public object data { get; set; }
        }
    }
}