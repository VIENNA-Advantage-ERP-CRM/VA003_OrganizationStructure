/********************************************************
 * Module Name    : VA003
 * Purpose        : Create Organization Structure
 * Class Used     : 
 * Chronological Development
 * Karan         2 July 2015
 ******************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAdvantage.Utility;
using VIS.Filters;
using VIS.Models;

namespace VIS.Controllers
{

    public class OrgStructureController : Controller
    {
        //
        // GET: /VIS/OrgStructure/
        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        //[AjaxValidateAntiForgeryToken] // validate antiforgery token 
        public ActionResult Index()
        {
            return View();
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult GetTree(int windowNo, bool showOrgUnits)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GetTree(windowNo, @Url.Content("~/"), "", showOrgUnits)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult GetInitSettings()
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Content(orgStrct.GetInitSettings());
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult GetOrgInfo(int orgID, bool loadLookups)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GetOrgInfo(orgID, loadLookups)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult InsertOrUpdateOrg(OrgStructureData data)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            data.Name = Server.HtmlDecode(data.Name).Replace("'", "''");

            if (!String.IsNullOrEmpty(data.SearchKey))
            {
                data.SearchKey = Server.HtmlDecode(data.SearchKey).Replace("'", "''");
            }

            if (!String.IsNullOrEmpty(data.TaxID))
            {
                data.TaxID = Server.HtmlDecode(data.TaxID).Replace("'", "''");
            }

            if (!String.IsNullOrEmpty(data.EmailAddess))
            {
                data.EmailAddess = Server.HtmlDecode(data.EmailAddess).Replace("'", "''");
            }

            if (!String.IsNullOrEmpty(data.Phone))
            {
                data.Phone = Server.HtmlDecode(data.Phone).Replace("'", "''");
            }

            if (!String.IsNullOrEmpty(data.Fax))
            {
                data.Fax = Server.HtmlDecode(data.Fax).Replace("'", "''");
            }

            return Json(JsonConvert.SerializeObject(orgStrct.InsertOrUpdateOrg(data)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadPic(HttpPostedFileBase pic, int orgID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            MemoryStream ms = new MemoryStream();
            pic.InputStream.CopyTo(ms);
            return Json(JsonConvert.SerializeObject(orgStrct.UploadPic(ms.ToArray(), orgID)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult LoadLookups()
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.LoadLookups()), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult GenerateHeirarchy(int windowNo)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GenerateHierarchy(@Url.Content("~/"), windowNo)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult CreateTree(int windowNo, int treeID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.CreateTree1(treeID, @Url.Content("~/"), windowNo)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create Tree Structure for selected Tree
        /// </summary>
        /// <param name="windowNo">Window no</param>
        /// <param name="treeID">Tree ID</param>
        /// <param name="LegalEntityIds">LegalEntity IDs</param>
        /// <returns>TreeData</returns>
        /// <writer>VIS_0045</writer>
        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult FRPTCreateTree(int windowNo, int treeID, string LegalEntityIds)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.CreateTree1(treeID, @Url.Content("~/"), windowNo, LegalEntityIds)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult AddOrgNode(int treeID, string name, string description, string value, int windowNo, string parentID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            name = Server.HtmlDecode(name).Replace("'", "''");
            if (!string.IsNullOrEmpty(value))
            {
                value = Server.HtmlDecode(value).Replace("'", "''");
            }


            return Json(JsonConvert.SerializeObject(orgStrct.AddOrgNode(treeID, name, description, value, windowNo, @Url.Content("~/"), parentID)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Add New Tree
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="IsOrgUnit">Tree for Org Unit</param>
        /// <returns></returns>
        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult AddNewTree(string name, bool IsOrgUnit)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            name = Server.HtmlDecode(name);

            return Json(JsonConvert.SerializeObject(orgStrct.AddNewTree(name, IsOrgUnit)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult RefreshOrgType()
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.RefreshOrgType()), JsonRequestBehavior.AllowGet);
        }



    }
}