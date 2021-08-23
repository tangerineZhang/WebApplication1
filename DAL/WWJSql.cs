using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class WWJSql
    {
        //获取提交人的名字和未审批条数
        public DataTable Selectone()
        {

            string str = string.Format("SELECT a.OriginatorName,REPLACE(a.Originator,'lvdu-dc\\','') as Originator,count(a.ID) as lcCount FROM BPM_FlowInst AS a WITH(NOLOCK)" +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID" +
                "  LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID" +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID" +
                "   LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                "  OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK)" +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d" +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0 AND a.State = 2 AND a.StartDate < GETDATE() AND b.ProcessName not in ('合理化建议流程') AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%' " +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712) AND a.OrgName <> '' AND d.JobStepName NOT LIKE '%出纳%' AND d.JobStepName <> '员工销假' AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%'" +
                " group by a.OriginatorName,Originator");
            return DBHelper.ExecSqlDateTable(str);
        }


        //提交人个人的详情
        public DataTable Select(string names)
        {
            string str = string.Format("SELECT a.ID,a.OrgName,a.OriginatorName,REPLACE(a.Originator,'lvdu-dc\\','') as Originator,a.Topic,a.StartDate,d.JobStepName,d.JobUserName" +
                " FROM BPM_FlowInst AS a WITH(NOLOCK)" +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID" +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID" +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID" +
                " LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS" +
                " OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK)" +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d" +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0" +
                " AND a.State=2" +
                " AND a.StartDate < GETDATE()" +
                " AND b.ProcessName not in ('合理化建议流程')" +
                " AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%'" +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712)" +
                " AND a.OrgName <> ''" +
                " AND d.JobStepName NOT LIKE '%出纳%'" +
                " AND d.JobStepName <> '员工销假'" +
                " AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%' " +
                " AND REPLACE(a.Originator,'lvdu-dc\\','')='{0}'", names);
            return DBHelper.ExecSqlDateTable(str);
        }

        //审批人的名字和个数
        public DataTable SelectNameCount()
        {
            string str = string.Format("SELECT d.JobUserName,count(d.JobUserName) as lcCount  FROM BPM_FlowInst AS a WITH(NOLOCK)" +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID" +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID" +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID" +
                "   LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                "  OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK)" +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d" +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0 AND a.State = 2 AND a.StartDate < GETDATE() AND b.ProcessName not in ('合理化建议流程') AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%' " +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712) AND a.OrgName <> '' AND d.JobStepName NOT LIKE '%出纳%' AND d.JobStepName <> '员工销假' AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%' " +
                " group by a.OriginatorName");
            return DBHelper.ExecSqlDateTable(str);
        }

        //审批人的个人详情
        public DataTable SelectName(string names,string Mz)
        {
            string str = string.Format("SELECT (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID,''),'lvdu-dc\',''),',') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1 And ActivityK2Name <> '草稿'  AND[Status] = 0 AND ProcInstID = a.ID) as ProcInstID, d.JobUserName,a.ID,a.Topic,a.StartDate,d.JobStepName,a.OriginatorName,a.OrgName " +
                " FROM BPM_FlowInst AS a WITH(NOLOCK) " +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID " +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID " +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID " +
                " LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                " OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK) " +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName " +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d " +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0" +
                " AND a.State=2" +
                " AND a.StartDate < GETDATE()" +
                " AND b.ProcessName not in ('合理化建议流程')" +
                " AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%'" +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712)" +
                " AND a.OrgName <> ''" +
                " AND d.JobStepName NOT LIKE '%出纳%'" +
                " AND d.JobStepName <> '员工销假'" +
                " AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%' " +
                " AND (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID,''),'lvdu-dc\\',''),',') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1 And ActivityK2Name <> '草稿'  AND[Status] = 0 AND ProcInstID = a.ID) like '%{0}%' and JobUserName like '%{1}%'", names,Mz);
            return DBHelper.ExecSqlDateTable(str);

        }

        //审批人超过七天的流程数量
        public DataTable SelectNameCount(string names, string Mz)
        {
            string str = string.Format("SELECT (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID,''),'lvdu-dc\',''),',') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1 And ActivityK2Name <> '草稿'  AND[Status] = 0 AND ProcInstID = a.ID) as ProcInstID, d.JobUserName,a.ID,a.Topic,a.StartDate,d.JobStepName,a.OriginatorName,a.OrgName " +
                " FROM BPM_FlowInst AS a WITH(NOLOCK) " +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID " +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID " +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID " +
                " LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                " OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK) " +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName " +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d " +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0" +
                " AND a.State=2" +
                " AND a.StartDate < GETDATE()" +
                " AND b.ProcessName not in ('合理化建议流程')" +
                " AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%'" +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712)" +
                " AND a.OrgName <> ''" +
                " AND d.JobStepName NOT LIKE '%出纳%'" +
                " AND d.JobStepName <> '员工销假'" +
                " AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%'" +
                "  AND  datediff(day,a.StartDate, getdate()) > 7 " +
                " AND (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID,''),'lvdu-dc\\',''),',') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1 And ActivityK2Name <> '草稿'  AND[Status] = 0 AND ProcInstID = a.ID) like '%{0}%' and JobUserName like '%{1}%'", names, Mz);
            return DBHelper.ExecSqlDateTable(str);

        }

        //带督办超过七天未完结的流程
        public DataTable DaidubanCount(string names)
        {
            string str = string.Format("SELECT (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID,''),'lvdu-dc\',''),',') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1 And ActivityK2Name <> '草稿'  AND[Status] = 0 AND ProcInstID = a.ID) as ProcInstID, d.JobUserName,a.ID,a.Topic,a.StartDate,d.JobStepName,a.OriginatorName,a.OrgName " +
                " FROM BPM_FlowInst AS a WITH(NOLOCK) " +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID " +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID " +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID " +
                " LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                " OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK) " +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName " +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d " +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0" +
                " AND a.State=2" +
                " AND a.StartDate < GETDATE()" +
                " AND b.ProcessName not in ('合理化建议流程')" +
                " AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%'" +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712)" +
                " AND a.OrgName <> ''" +
                " AND d.JobStepName NOT LIKE '%出纳%'" +
                " AND d.JobStepName <> '员工销假'" +
                " AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%'" +
                "  AND  datediff(day,a.StartDate, getdate()) > 7 " +
                " AND a.OrgName= '%{0}%'", names);
            return DBHelper.ExecSqlDateTable(str);

        }



        //获取用户表数据（编号和名字）
        //public DataTable UserSelect()
        //{
        //    // string str = string.Format("SELECT [EmployeeNo],[Name] FROM[lvdu_bpm].[dbo].[MDS_User] where Disabled = 0 "   ,'lh','gaoyuana','liujfd');
        //    string str = string.Format("SELECT [EmployeeNo],[Name] FROM[lvdu_bpm].[dbo].[MDS_User] where Disabled = 0 and [EmployeeNo] in ('zhangwdc')");
        //    return DBHelper.ExecSqlDateTable(str);
        //}



        public DataTable UserSelect()
        {
            string str = "";
            str += " select CASE WHEN ISNULL(u.F3,'')<>'' THEN u.F3 ELSE u.UserCode END as UserNo,o.F3 as Organization,";
            str += " q.OrgUnitName as DepartmentX,u.UserName as Name,u.UserLoginID as EmployeeNo,u.UserPostName,p.PositionCode,p.PositionName,u.UserLevelName";
            str += " from dbo.MDM_User u inner join dbo.MDM_User_Position_Link up on up.UserGUID = u.UserID and up.IsMainPosition = 1 and up.Status = 1";
            str += " inner join dbo.MDM_PostOrganization_Link po on po.PositionGuid=up.PositionGUID and po.Status=1 inner join dbo.MDM_OrganizationUnit o on o.OrgUnitGUID = po.OrgUnitGUID and o.Status = 1";
            str += " left join dbo.MDM_OrganizationUnit q on q.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 and q.CompanyType=102 left join dbo.MDM_OrganizationUnit i on i.OrgUnitGUID = po.OrgUnitGUID and o.Status = 1 and i.CompanyType = 103 left join dbo.MDM_OrganizationUnit s on s.OrgUnitGUID = po.OrgUnitGUID and o.Status = 1 and s.CompanyType = 101";
            str += " left join dbo.MDM_OrganizationUnit z on z.OrgUnitGUID = po.OrgUnitGUID and o.Status=1 and z.CompanyType=2 left join dbo.MDM_Position as p on p.PositionGUID = po.PositionGuid";
            str += " left join dbo.MDM_User uou on uou.UserID = o.LeaderCode left join dbo.MDM_User uo on uo.UserID = q.LeaderCode left join dbo.MDM_User uo1 on uo1.UserID = i.LeaderCode ";
            str += " left join dbo.MDM_User uo2 on uo2.UserID = s.LeaderCode left join dbo.MDM_User uo3 on uo3.UserID = z.LeaderCode where u.Status = 1";
         //  str += " and u.UserLoginID in ('zhangwdc')";

            return DBHelperstree.ExecSqlDateTable(str);

        }

        //获取提交人的数据（编号和名字）
        public DataTable TUserName()
        {

            string str = string.Format("SELECT a.OriginatorName,count(a.ID) as lcCount, REPLACE(a.Originator,'lvdu-dc\\','') as Originator FROM BPM_FlowInst AS a WITH(NOLOCK)" +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID" +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID" +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID" +
                "   LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS " +
                "  OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK)" +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d" +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0 AND a.State = 2 AND a.StartDate < GETDATE() AND b.ProcessName not in ('合理化建议流程') AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%' " +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712) AND a.OrgName <> '' AND d.JobStepName NOT LIKE '%出纳%' AND d.JobStepName <> '员工销假' AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%' " +
                " group by a.OriginatorName,a.Originator");
            return DBHelper.ExecSqlDateTable(str);
        }


        //获取审批人的数据（编号和名字）
        public DataTable SUserName()
        {
            string str = string.Format("Select (SELECT dbo.StrJoin(REPLACE(ISNULL(bjl.UserID, ''), 'lvdu-dc\\',''),', ') FROM dbo.BPM_JobList AS bjl WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON REPLACE(bjl.UserID, 'lvdu-dc\','')=mhp.AccountName WHERE 1 = 1" +
                " And ActivityK2Name<>'草稿'  AND [Status]=0 AND ProcInstID=a.ID) as ProcInstID,d.JobUserName AS JobUserName" +
                " FROM BPM_FlowInst AS a WITH(NOLOCK)" +
                " LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID=b.ProcessID" +
                " LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID=c.ProcessCategoryID" +
                " LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID=pc.ProcessCategoryID" +
                " LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS" +
                " OUTER APPLY (SELECT dbo.StrJoin(ISNULL(mhp.Name,''),',') AS JobUserName,MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK)" +
                " LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS=mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d" +
                " WHERE 1=1 AND a.State<>6 AND a.State<>5 " +
                " AND a.isDelete=0 AND a.State=2 AND a.StartDate < GETDATE() AND b.ProcessName not in ('合理化建议流程')" +
                " AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%' " +
                " AND a.ID NOT IN (1904030205,1901280093,1907008712) AND a.OrgName <> '' AND d.JobStepName NOT LIKE '%出纳%'" +
                " AND d.JobStepName <> '员工销假' AND d.JobStepName NOT LIKE '%会计%' AND b.ProcessName NOT LIKE '%创新提案申报流程%' ");
            return DBHelper.ExecSqlDateTable(str);
        }


        //获取管理员的数据（编号和名字）
        public DataTable GUserName()
        {
            string str = string.Format("select a.UserName,a.ID,a.UserAccount,a.JoinNmae from JoinUser a where Start=1");
            return DBHelpertwo.ExecSqlDateTable(str);
        }



        //查询所有的数据（dataTable）
        public DataTable Alldate()
        {
            string str = string.Format("Select (CASE when a.OrgName = '总部' THEN'汇通' when a.OrgName = '汇通总部' THEN'汇通' ELSE a.OrgName end)AS OrgName, a.DeptName AS DeptName, a.ID, b.ProcessName AS ProcessName, " +
                "  a.Topic  AS Topic,a.StartDate AS StartDate, " +
                "  REPLACE(a.Originator, 'lvdu-dc\\','') AS Originator,a.OriginatorName AS OriginatorName, " +
                " a.OriginatorName AS OriginatorName,d.JobUserName AS JobUserName,d.JobStepName AS JobStepName" +
                " FROM BPM_FlowInst AS a WITH(NOLOCK) LEFT JOIN ProcessPublish AS b WITH(NOLOCK) ON a.ProcessID = b.ProcessID LEFT JOIN ProcessCategory AS c WITH(NOLOCK) ON b.ProcessCategoryID = c.ProcessCategoryID LEFT JOIN dbo.ProcessCategory AS pc WITH(NOLOCK) ON c.ParentCategoryID = pc.ProcessCategoryID" +
                "  LEFT JOIN dbo.WF_DataAuthority AS wda WITH(NOLOCK) ON a.BranchTemplateID=wda.MainID COLLATE Chinese_PRC_CI_AS  OUTER APPLY(SELECT dbo.StrJoin(ISNULL(mhp.Name, ''), ',') AS JobUserName, MIN(btpjc.JobStepName) AS JobStepName FROM dbo.BPM_ThirdPartyJobCenter AS btpjc  WITH(NOLOCK) LEFT JOIN dbo.MDS_Hr_Person AS mhp WITH(NOLOCK) ON btpjc.JobUserID COLLATE Chinese_PRC_CI_AS = mhp.AccountName" +
                " WHERE btpjc.AppId='BPM' and  btpjc.JobStatus=0 AND  btpjc.ProcInstID=CONVERT(VARCHAR(64),a.ID)) d WHERE 1 = 1 AND a.State <> 6 AND a.State <> 5 AND a.isDelete = 0 " +
                " AND a.State=2/*查询未完结流程*/ AND a.StartDate < getdate() AND b.ProcessName not in ('合理化建议流程') AND b.ProcessName not like '%测试%' AND b.ProcessName not like '%test%' and a.Topic not like '%测试%' AND a.Topic not like '%test%' AND a.ID NOT IN(1904030205, 1901280093, 1907008712)" +
                "  AND a.OrgName <> '' AND d.JobStepName NOT LIKE '%出纳%' AND d.JobStepName <> '员工销假' AND d.JobStepName NOT LIKE '%会计%' AND d.JobStepName NOT LIKE '%总部信息流程部信息经理%' AND d.JobStepName NOT LIKE '%总部信息流程部电子流程制作%' AND b.ProcessName NOT LIKE '%创新提案申报流程%'");
            return DBHelper.ExecSqlDateTable(str);
        }


        //日志添加
        public int AddLoge(string mag, string name)
        {
            string str = string.Format("insert into FlowSendLog (Massgae,Name)  values( '{0}','{1}')", mag, name);
            int i = DBHelpertwo.ExecSqlResult(str);
            return i;
        }


    }


}
