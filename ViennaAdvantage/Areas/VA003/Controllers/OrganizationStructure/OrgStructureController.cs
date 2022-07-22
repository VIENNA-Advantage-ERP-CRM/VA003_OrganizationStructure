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
        public ActionResult GetTree(int windowNo,bool showOrgUnits)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GetTree(windowNo, @Url.Content("~/"), "",showOrgUnits)), JsonRequestBehavior.AllowGet);
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

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult AddNewTree(string name)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            name = Server.HtmlDecode(name);

            return Json(JsonConvert.SerializeObject(orgStrct.AddNewTree(name)), JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorizeAttribute] // redirect to login page if request is not Authorized
        [AjaxSessionFilterAttribute] // redirect to Login/Home page if session expire
        public ActionResult RefreshOrgType()
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.RefreshOrgType()), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Logo as Null on Org Info
        /// </summary>
        /// <param name="AD_Org_ID">Organization ID</param>
        /// <returns>Query Result</returns>
        public ActionResult ClearOrgLogo(int AD_Org_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.ClearOrgLogo(AD_Org_ID)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Window ID
        /// </summary>
        /// <param name="WindowName">Window Name</param>
        /// <returns>Window ID</returns>
        public ActionResult GetWindowName(String WindowName)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GetWindowName(WindowName)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Max Sequnece no from Tree node against Tree ID
        /// </summary>
        /// <param name="AD_Client_ID">Client ID</param>
        /// <param name="AD_Tree_ID">Tree ID</param>
        /// <returns>Seq No</returns>
        public ActionResult GetMaxSequenceNo(int AD_Client_ID, int AD_Tree_ID)
        {
            Ctx ctx = Session["ctx"] as Ctx;
            OrgStructure orgStrct = new OrgStructure(ctx);
            return Json(JsonConvert.SerializeObject(orgStrct.GetMaxSequenceNo(AD_Client_ID, AD_Tree_ID)), JsonRequestBehavior.AllowGet);
        }

    }
}