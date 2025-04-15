using BlazeInspect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Windows.Interop;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Web.Http;
using WebPush;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Net;
using System.Web.Services.Description;

namespace BlazeInspect.Controllers
{
    public class HomeController : Controller
    {

        //string fileName = "";
        CrudClass Cc = new CrudClass();

        public ActionResult Index()
        {
            ViewBag.Message = "index page";

            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //webpush notification
        public JsonResult SaveSubscription(VapidKeys model)
        {
            string status = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        var user_id = Session["user_credential_id"];
                        var publicKey = System.Web.HttpContext.Current.Application["VAPID_PublicKey"].ToString();
                        var privateKey = System.Web.HttpContext.Current.Application["VAPID_PrivateKey"].ToString();

                        string field = "endpoint, p256DH, auth , user_credential_id , publicKey, privateKey";
                        string values = "'" + model.endpoint + "','" + model.p256DH + "','" + model.auth + "','" + user_id + "','" + publicKey + "','" + privateKey + "' ";
                        status = Cc.InsertionMethodStatus("setSubscription", field, values);
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public void SendNotificationToAllUsers(string title_, string message, int ID)
        {
            VapidKeys model = new VapidKeys();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (ID > 0)
                        {
                            var subscriptions = Cc.SelectSubscription("GetSubscription", ID.ToString(), "0000-00-00", "0000-00-00", "0");

                            // Retrieve VAPID keys from Application object
                            //var publicKey = System.Web.HttpContext.Current.Application["VAPID_PublicKey"].ToString();
                            //var privateKey = System.Web.HttpContext.Current.Application["VAPID_PrivateKey"].ToString();

                            //// VAPID details
                            //var vapidDetails = new VapidDetails("mailto:amaanarman99@gmail.com", publicKey, privateKey);
                            //var webPushClient = new WebPushClient();

                            foreach (var item in subscriptions)
                            {
                                model.Id = item.Id;
                                string Endpoint = item.endpoint;
                                string p256DH = item.p256DH;
                                string auth = item.auth;
                                var publicKey = item.PublicKey;
                                var privateKey = item.PrivateKey;

                                //Create a PushSubscription object from your model data
                                var subscription = new PushSubscription(
                                    Endpoint,
                                    p256DH,
                                    auth
                               );

                                // Payload for the notification
                                var payload = JsonConvert.SerializeObject(new
                                {
                                    title = title_,
                                    body = message
                                });

                                var vapidDetails = new VapidDetails("mailto:amaanarman99@gmail.com", publicKey, privateKey);
                                var webPushClient = new WebPushClient();

                                try
                                {
                                    // Send the notification
                                    webPushClient.SendNotification(subscription, payload, vapidDetails);
                                    Console.WriteLine($"Notification sent to {model.endpoint}");
                                }
                                catch (WebPushException ex)
                                {
                                    // Handle error
                                    Console.WriteLine($"Error sending notification to {model.endpoint}: {ex.Message}");
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }        }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
            }
}
        //webpush notification


        
        public ActionResult FileManager()
        {
            ViewBag.Message = "File Manager.";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult SaveFile(Filemanager model)
        {
            string status = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string id = Session["user_credential_id"].ToString();
                        string name = Session["user_name"].ToString();

                        foreach (var items in model.attachment)
                        {
                            string DocumentationPath2 = "~/admin_assets/File/"+name+"";
                            bool exists2 = System.IO.Directory.Exists(Server.MapPath(DocumentationPath2));
                            if (!exists2)
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath(DocumentationPath2));
                            }
                            var picture = items;
                            //var extensionitem = Path.GetExtension(items.FileName);
                            //string img = Cc.RandomNumber(i, 90000) + extensionitem;
                            picture.SaveAs(Server.MapPath(DocumentationPath2 + "/" + items.FileName));

                            long  bytes = items.ContentLength;
                            double fileSizeInMB = Cc.ConvertBytesToMB(bytes);

                            string field = "File_Name , File_size, User_id";
                            string values = "'" + items.FileName + "','" + fileSizeInMB + "','" + id + "' ";
                            status = Cc.InsertionMethodStatus("SetFile", field, values);

                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShareuserListView()
        {
            login model = new login();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        List<login> InspectorList = Cc.loginSession("inspectorList", "0", "0");
                        ViewBag.InspectorListVB = new SelectList(InspectorList, "user_credential_id", "user_name");
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Views/PartialView/PartialViewUserList.cshtml", model);
        }
        public JsonResult SetShareFile(Filemanager model)
        {
            string status = "";
            string values = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.checkbox.Length > 0)
                        {
                            if (Session["user_name"] != null && Session["user_credential_id"] != null)
                            {
                                string name = Session["user_name"].ToString();

                                foreach (var items in model.checkbox)
                                {
                                    string joinedIDs = string.Join(",", model.shared_id);

                                    values = "Shared_id='" + joinedIDs + "'";

                                    status = Cc.UpdationMethodReturn("updateDocument", values, items.ToString());
                                }
                                if (status == "Saved")
                                {
                                    foreach (var items in model.shared_id)
                                    {
                                        string message_title = "Alert !";
                                        string message_txt = " " + Session["user_name"] + "Share a file with you ";
                                        string field = "receiver_user_id , message_title,message_text";
                                        string value = " '" + items.ToString() + "' , '" + message_title + "' , '" + message_txt + "' ";
                                        status = Cc.InsertionMethodStatus("setNotification", field, value);

                                        SendNotificationToAllUsers("Alert", message_txt, Cc.ToInt32(items.ToString()) );

                                        // Call SignalR hub to notify clients
                                        var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                        context.Clients.All.broadcastMessage(Cc.ToInt32(items.ToString()), name, message_txt, "notification");
                                    }
                                }

                            }
                            else
                            {
                                status = "SessionDestory";
                            }
                        }
                        else
                        {
                            status = "ExceptionError";
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                        //status = "DbVarification";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                    //status = "NetVarification";
                }
            }
            catch (Exception)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //Cc.writeEventLog(opl + " : " + ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDocumentList()
        {
            List<Filemanager> documentList = new List<Filemanager>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];

                    documentList = Cc.SelectDocument("DocumentList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }

            // Mock logic: Combine user images with same File_name
            var groupedResult = documentList
                .GroupBy(x => x.File_name)
                .Select(g => new
                {
                    FileName = g.Key,
                    Users = g.Select(u => new { u.user_name, u.user_img }).Distinct().ToList(),
                    FileDetails = g.FirstOrDefault() // Get common file details (size, date, etc.)
                }).ToList();

            return Json(groupedResult , JsonRequestBehavior.AllowGet);
        }

        //Chat App
        public ActionResult ChatApp()
        {
            ViewBag.Message = "Chat App.";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetList()
        {
            List<Chat> GetList = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];

                    GetList = Cc.SelectChatNotificaion("UserList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetuserData(string ID)
        {
            List<Chat> GetuserData = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    GetuserData = Cc.SelectChatNotificaion("Userdata", ID.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetuserData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChatbox(string ID)
        {
            List<Chat> GetChatbox = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];
                    var receiverID = ID;
                    GetChatbox = Cc.SelectChatNotificaion("Chatbox", user_id.ToString(), "0000-00-00", "0000-00-00", receiverID.ToString()).ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetChatbox, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Sendmessage(Chat model)
        {
            string status = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string id = Session["user_credential_id"].ToString();
                        string name = Session["user_name"].ToString();

                        if (model.attachment != null)
                        {
                            int i = Cc.RandomNumber(10, 1000);
                            foreach (var items in model.attachment)
                            {
                                string DocumentationPath2 = "~/admin_assets/images/attachment";
                                bool exists2 = System.IO.Directory.Exists(Server.MapPath(DocumentationPath2));
                                if (!exists2)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(DocumentationPath2));
                                }
                                i++;
                                var picture = items;
                                var extensionitem3 = Path.GetExtension(items.FileName);
                                string img = Cc.RandomNumber(i, 90000) + extensionitem3;
                                picture.SaveAs(Server.MapPath(DocumentationPath2 + "/" + img));

                                string field = "receiver_user_id , sender_user_id, message_title, attachment";
                                string values = "'" + model.receiver_user_id + "','" + id + "','" + name + "' , '" + img + "' ";
                                status = Cc.InsertionMethodStatus("setNotification", field, values);
                              
                            }
                        }
                        else
                        {
                            string field = "receiver_user_id , sender_user_id, message_text , message_title";
                            string values = "'" + model.receiver_user_id + "','" + id + "','" + model.messege_text + "','" + name + "' ";
                            status = Cc.InsertionMethodStatus("setNotification", field, values);
                       
                        }
                        if (status == "Saved")
                        {
                             SendNotificationToAllUsers("New message",model.messege_text, model.receiver_user_id);
                            // Call SignalR hub to notify clients
                            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                            context.Clients.All.broadcastMessage(model.receiver_user_id, name, model.messege_text , "message");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        //Chat App

        //dashboard
        public JsonResult GetSubscription()
        {
            List<VapidKeys> GetSubscription = new List<VapidKeys>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];

                    GetSubscription = Cc.SelectSubscription("GetSubscription", user_id.ToString(), "0000-00-00", "0000-00-00", "0");
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetSubscription, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMessage()
        {
            List<Chat> GetMessage = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];
                  
                    GetMessage = Cc.SelectChatNotificaion("MessageList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNotification()
        {
            List<Chat> GetNotification = new List<Chat>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];

                    GetNotification = Cc.SelectChatNotificaion("NotificationList", user_id.ToString() , "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    //switch (logintype)
                    //{
                    //    case "Inspector":
                    //        GetNotification = Cc.SelectProduct("NotificationList", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    //        break;
                    //    default:
                    //        GetNotification = Cc.SelectProduct("totalproduct", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                    //        break;
                    //}
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetNotification, JsonRequestBehavior.AllowGet);
        }
        //MarkasRead
        public JsonResult MarkasRead(string ID,string type)
        {
            string result = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (type == "notification")
                        {
                            string values = "isread='true'";
                            result = Cc.UpdationMethodReturn("updateNotication", values, ID.ToString());

                        }
                        else if (type == "message")
                        {
                            string values = "isread='true'";
                            result = Cc.UpdationMethodReturn("updateMessage", values, ID.ToString());
                        }
                    }
                    else
                    {
                        result = "DataBaseError";
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    result = "NetworkError";
                    TempData["NetVarification"] = "UnConnected";
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Gettotalproduct()
        {
            List<Product> Gettotalproduct = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            Gettotalproduct = Cc.SelectProduct("totalproduct", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                            Gettotalproduct = Cc.SelectProduct("totalproduct", "0" , "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(Gettotalproduct, JsonRequestBehavior.AllowGet);
        }
        //is ko sahi kr na ha
        public JsonResult Gettotalinspector()
        {
            List<Product> Gettotalinspector = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            //Complete inspection
                             Gettotalinspector = Cc.SelectProduct("totalinspector", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                             Gettotalinspector = Cc.SelectProduct("totalinspector", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(Gettotalinspector, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getinspectionanalyse()
        {
            List<Product> Getinspectionanalyse = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            Getinspectionanalyse = Cc.SelectProduct("inspectionanalyse", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                            Getinspectionanalyse = Cc.SelectProduct("inspectionanalyse", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(Getinspectionanalyse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetpendingInspection()
        {
            List<Product> GetpendingInspection = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            GetpendingInspection = Cc.SelectProduct("pendingInspection", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                            GetpendingInspection = Cc.SelectProduct("pendingInspection", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetpendingInspection, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInspectionHistory()
        {
            List<Product> GetInspectionHistory = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            GetInspectionHistory = Cc.SelectProduct("InspectionHistory", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                            GetInspectionHistory = Cc.SelectProduct("InspectionHistory", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetInspectionHistory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInspectionStatus()
        {
            List<Product> GetInspectionStatus = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var logintype = Session["login_type"];
                    var user_id = Session["user_credential_id"];
                    switch (logintype)
                    {
                        case "Inspector":
                            GetInspectionStatus = Cc.SelectProduct("InspectionStatus", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                        default:
                            GetInspectionStatus = Cc.SelectProduct("InspectionStatus", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                            break;
                    }
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetInspectionStatus, JsonRequestBehavior.AllowGet);
        }
        //dashboard

        //Product
        public ActionResult Product()
        {
            ViewBag.Message = "Your application description page.";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult ProductInsertionAndUpdation(Product model)
        {
            string status = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.Extinguisher_ID > 0)
                        {
                            string values = " Location_ID='" + model.Location_ + "' , Type='" + model.Type + "' , Capacity='" + model.Capacity + "', Manufacturer='" + model.Manufacturer + "', Date_of_Manufacture='" + model.Date_of_Manufacture + "' , Remarks='" + model.Remarks + "'  ";
                            status = Cc.UpdationMethodReturn("updateproduct", values, model.Extinguisher_ID.ToString());
                        }
                        else
                        {
                            // Generate QR code
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode("product_info:Serial no:" + model.SerialNumber + ";;", QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);

                            // Convert QR code to bitmap
                            Bitmap qrCodeImage = qrCode.GetGraphic(20);

                            // Save QR code to a folder
                            string folderPath = Server.MapPath("~/QRCodeImages/");
                            //string fileName = $"{Guid.NewGuid()}.png"; // Generate a unique file name
                            string fileName = $"{model.SerialNumber}.png"; // Generate a unique file name
                            string filePath = Path.Combine(folderPath, fileName);
                            qrCodeImage.Save(filePath, ImageFormat.Png);

                            string field = "SerialNumber , Location_ID, Type, Capacity,Manufacturer,Date_of_Manufacture,Remarks";
                            string values = "'" + model.SerialNumber + "','" + model.Location_ + "','" + model.Type + "' ,'" + model.Capacity + "','" + model.Manufacturer + "' ,'" + model.Date_of_Manufacture + "' ,'" + model.Remarks + "'";
                            status = Cc.InsertionMethodStatus("setProduct", field, values);
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                status = ex.ToString();
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductDeletion(Product model)
        {
            string result = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.checkbox.Length > 0)
                        {
                            foreach (var items in model.checkbox)
                            {
                                string values = "isdelete='true'";
                                result = Cc.UpdationMethodReturn("updateproduct", values, items.ToString());
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditProduct(string ID)
        {
            Product model = new Product();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if ((int)Convert.ToDouble(ID) > 0)
                        {
                            foreach (var item in Cc.SelectProduct("ProductEdit", ID.Trim().ToString(), "0000-00-00", "0000-00-00","0" ))
                            {
                                model.Extinguisher_ID = item.Extinguisher_ID;
                                model.Location_ = item.Location_;
                                model.Type = item.Type;
                                model.Capacity = item.Capacity;
                                model.Manufacturer = item.Manufacturer;
                                //model.Status = item.Status;
                                model.Remarks = item.Remarks;
                                model.Date_of_Manufacture = item.Date_of_Manufacture;
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }

            return PartialView("~/Views/PartialView/PartialViewProductEdit.cshtml", model);
        }
        public JsonResult GetProductList()
        {
            List<Product> GetProductList = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    //ViewBag.IndividualInvestorListVB = Cc.InvestorListSelectMethod("GetInvestorListInActiveStatus", "IndividualActive");
                    GetProductList = Cc.SelectProduct("ProductList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetProductList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductDetails(string ID)
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if ((int)Convert.ToDouble(ID) > 0)
                        {
                            var user_id = Session["user_credential_id"];

                            ViewBag.ProductDetailVB = Cc.SelectProduct("ProductDetail", ID.ToString() , user_id.ToString() , "0000-00-00", "0000-00-00");

                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }

            return PartialView("~/Views/PartialView/PartialViewProductDetail.cshtml");//, model
        }
        public JsonResult ProductSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Product> ProductSearchList = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    ProductSearchList = Cc.SelectProduct("ProductSearchList", id.ToString() , start_date_id, end_date_id, "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(ProductSearchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailFuntion()
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        var logintype = Session["login_type"];
                        var user_id = Session["user_credential_id"];
                        switch (logintype)
                        {
                            case "Inspector":
                                //Complete inspection
                                ViewBag.DetailFuntionVB = Cc.SelectProduct("DetailpendingInspection", user_id.ToString(), "0000-00-00", "0000-00-00", "0000-00-00");
                                break;
                            default:
                                ViewBag.DetailFuntionVB = Cc.SelectProduct("DetailpendingInspection", "0", "0000-00-00", "0000-00-00", "0000-00-00");
                                break;
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }

            return PartialView("~/Views/PartialView/PartialViewDetail.cshtml");//, model
        }
        public ActionResult ReportList()
        {
            if (Session["Login"] != null)
            {
                int checker = logindata();
                if (checker == 1)
                {
                    return RedirectToAction("Login", "Home");
                }
                try
                {
                    if (Cc.CheckForInternetConnection() == true)
                    {
                        if (Cc.DatabaseConnectionCheck() == true)
                        {
                            TempData["Title"] = "Report List";
                        }
                        else
                        {
                            TempData["DbVarification"] = "UnConnected";
                        }
                    }
                    else
                    {
                        TempData["NetVarification"] = "UnConnected";
                    }

                }
                catch (Exception ex)
                {
                    Cc.WriteEventLog(ex.ToString());
                    throw ex;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public JsonResult GetReportList()
        {
            List<Product> GetReportList = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    var user_id = Session["user_credential_id"];

                  GetReportList = Cc.SelectProduct("ReportList", "0", "0000-00-00", "0000-00-00", "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetReportList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Report_List(string ID)
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if ((int)Convert.ToDouble(ID) > 0)
                        {
                            ViewBag.ReportListVB = Cc.SelectProduct("ReportList", ID.ToString(), "0000-00-00", "0000-00-00", "0000-00-00");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }

            return PartialView("~/Views/PartialView/PartialViewReportList.cshtml");//, model
        }
        public ActionResult GetReport(string SN, string DT )
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        //if ((int)Convert.ToDouble(ID) > 0)
                        //{
                            ViewBag.ProductDetailVB = Cc.SelectProduct("ProductDetail", SN, "0000-00-00", "0000-00-00", "0000-00-00");
                            ViewBag.GetReportVB = Cc.SelectQuestionandResponse("GetReport", SN, "0000-00-00", "0000-00-00", DT);
                        //}
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }

            return PartialView("~/Views/PartialView/PartialViewGetReport.cshtml");//, model
        }
        public JsonResult ReportSearchList(string id, string start_date_id, string end_date_id)
        {
            List<Product> ReportSearchList = new List<Product>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    ReportSearchList = Cc.SelectProduct("ReportSearchList", id.ToString(), start_date_id, end_date_id, "0000-00-00").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(ReportSearchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult inspectionForwardView()
        {
            login model = new login();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        List<login> InspectorList = Cc.loginSession("inspectorList", "0", "0");
                        ViewBag.InspectorListVB = new SelectList(InspectorList, "user_credential_id", "user_name");
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                //string user_name = Cc.ToString(Session["user_name"].ToString().Trim());
                //int user_credential_id = Cc.ToInt32(Session["user_credential_id"].ToString().Trim());
                //string field_error = "controller,method,error_detail,login_id,user_name";
                //string values_error = "'Setup Controller','AddGroupEditedView','" + ex.ToString() + "'," + user_credential_id + ",'" + user_name.ToString() + "'";
                //Cc.InsertionMethod("SetErrorLog", field_error, values_error);
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return PartialView("~/Views/PartialView/PartialViewInspectorList.cshtml", model);
        }
        public JsonResult setinspectionForward(login model)
        {
            string status = "";
            string values = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.checkbox.Length > 0)
                        {
                            if (Session["user_name"] != null && Session["user_credential_id"] != null)
                            {
                                string name = Session["user_name"].ToString();
                                foreach (var items in model.checkbox)
                                {
                                    values = "inspector_id='" + Cc.FirstCharToUpper(model.inspector_id.ToString().ToLower().Trim()) + "', inspector_name='" + model.inspector_name.Trim().ToString() + "',Status='" + "Pending" + "' , Next_Inspection_Date='" + model.Set_inspection_date.ToString() + "'";
                                    status = Cc.UpdationMethodReturn("UpdateProduct", values, items.ToString());
                                }
                                if (status == "Saved")
                                {
                                    string message_title = "Alert !";
                                    string message_txt = "You received a new Inspection from " + Session["user_name"] + ".";
                                    string field = "receiver_user_id , message_title,message_text";
                                    string value = " '" + model.inspector_id + "' , '" + message_title + "' , '" + message_txt + "' ";
                                    status = Cc.InsertionMethodStatus("setNotification", field, value);

                                    SendNotificationToAllUsers("New Inspection", message_txt, model.inspector_id);

                                    // Call SignalR hub to notify clients
                                    var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                    context.Clients.All.broadcastMessage(model.inspector_id, name, message_txt , "notification");
                                }
                               
                            }
                            else
                            {
                                status = "SessionDestory";
                            }
                        }
                        else
                        {
                            status = "ExceptionError";
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                        //status = "DbVarification";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                    //status = "NetVarification";
                }
            }
            catch (Exception)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                //Cc.writeEventLog(opl + " : " + ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        //Product


        //Category
        public ActionResult Setup()
        {
            ViewBag.Message = "Your Category page.";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Category

        //Question
        public ActionResult Question()
        {
            ViewBag.Message = "Your application Question page.";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetQuestionList()
        {
            List<Question> GetQuestionList = new List<Question>();

            if (Cc.CheckForInternetConnection() == true)
            {
                if (Cc.DatabaseConnectionCheck() == true)
                {
                    //ViewBag.IndividualInvestorListVB = Cc.InvestorListSelectMethod("GetInvestorListInActiveStatus", "IndividualActive");
                    GetQuestionList = Cc.SelectQuestionandResponse("QuestionList", "0", "0000-00-00", "0000-00-00" ,"0").ToList();
                }
                else
                {
                    TempData["DbVarification"] = "UnConnected";
                }
            }
            else
            {
                TempData["NetVarification"] = "UnConnected";
            }
            return Json(GetQuestionList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult QuestionInsertionAndUpdation(Question model)
        {
            string status = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.QuestionID > 0)
                        {
                            string values = " QuestionText='" + model.QuestionText + "',Q_option_1='" + model.first_opt + "',Q_option_2='" + model.second_opt + "' ";
                            status = Cc.UpdationMethodReturn("updateQuestion", values, model.QuestionID.ToString());
                        }
                        else
                        {
                            string field = "QuestionText, Q_option_1, Q_option_2 ";
                            string values = "'" + model.QuestionText + "','" + model.first_opt + "','" + model.second_opt+ "' ";
                            status = Cc.InsertionMethodStatus("setQuestion", field, values);
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult QuestionDeletion(Question model)
        {
            string result = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.checkbox.Length > 0)
                        {
                            foreach (var items in model.checkbox)
                            {
                                string values = "isdelete='true'";
                                result = Cc.UpdationMethodReturn("updateQuestion", values, items.ToString());
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditQuestion(string ID)
        {
            Question model = new Question();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if ((int)Convert.ToDouble(ID) > 0)
                        {
                            foreach (var item in Cc.SelectQuestionandResponse("QuestionEdit", ID.Trim().ToString(), "0000-00-00", "0000-00-00", "0"))
                            {
                                model.QuestionID = item.QuestionID;
                                model.QuestionText = item.QuestionText;
                                model.first_opt = item.first_opt;
                                model.second_opt = item.second_opt;
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }

            return PartialView("~/Views/PartialView/PartialViewQuestionEdit.cshtml", model);
        }
        //Question

        //Response
        public ActionResult QR_code()
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Inspection_Form()
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["Login"] != null)
                        {
                            int a = logindata();
                            if (a == 1)
                            {
                                return RedirectToAction("Login", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult ResponseInsertionAndUpdation(Question model)
        {
            string status = null;
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        string id = Session["user_credential_id"].ToString();
                        string name = Session["user_name"].ToString();
                        if (model.Q_ResponseID.Length > 0)
                        {
                            for (int i = 0; i < model.Q_ResponseID.Length; i++)
                            {
                                int QID = model.Q_ResponseID[i] ;
                                string reponse = model.Response[i];
                                string Comment = model.Comment[i];
                                string field = "QuestionID , ResponseText, ProductID , Comment";
                                string values = "'" + QID + "','" + reponse + "','" + model.Q_SerialNumber + "','" + Comment + "' ";
                                status = Cc.InsertionMethodStatus("setResponse", field, values);
                            }
                            if (status == "Saved") {
                                status = Cc.InsertionMethodStatus("UpdateStatus", id, name);
                            }

                        }
                        else
                        {
                            status = "Error";
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                status = ex.ToString();
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        //Response

        //public JsonResult GetSearchIVendor(string id, string start_date_id, string end_date_id)
        //{
        //    List<Product> GetSearchIVendor = new List<Product>();

        //    if (Cc.CheckForInternetConnection() == true)
        //    {
        //        if (Cc.DatabaseConnectionCheck() == true)
        //        {
        //            //ViewBag.IndividualInvestorListVB = Cc.InvestorListSelectMethod("GetInvestorListInActiveStatus", "IndividualActive");
        //            GetSearchIVendor = Cc.SelectCategoryMethod("VendorSearchList", id.ToString().Trim(), start_date_id, end_date_id).ToList();
        //        }
        //        else
        //        {
        //            TempData["DbVarification"] = "UnConnected";
        //        }
        //    }
        //    else
        //    {
        //        TempData["NetVarification"] = "UnConnected";
        //    }


        //    return Json(GetSearchIVendor, JsonRequestBehavior.AllowGet);


        //}

        //public ActionResult VendorEditedView(string ID)
        //{
        //    Product model = new Product();
        //    try
        //    {

        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                if ((int)Convert.ToDouble(ID) > 0)
        //                {
        //                    foreach (var item in Cc.SelectCategoryMethod("VendorEditedView", ID.ToString().Trim(), "0000-00-00", "0000-00-00"))
        //                    {
        //                        model.vendor_id = item.vendor_id;
        //                        model.vendor_name = item.vendor_name;
        //                        model.vendor_contact = item.vendor_contact;
        //                        model.shop_name = item.shop_name;
        //                        model.shop_address = item.shop_address;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                TempData["DbVarification"] = "UnConnected";
        //            }
        //        }
        //        else
        //        {
        //            TempData["NetVarification"] = "UnConnected";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return PartialView("~/Views/HomeShared/PartialViewCategory/PartialViewCategoryEdited.cshtml", model);
        //}

        //public JsonResult VendorDeletion(string ID)
        //{
        //    string result = "false";
        //    try
        //    {
        //        if (Cc.CheckForInternetConnection() == true)
        //        {
        //            if (Cc.DatabaseConnectionCheck() == true)
        //            {
        //                //result = Cc.DeletionMethod(ID, "tbl_category", "category_id", "isdelete=1");
        //                string values = "isdelete='true'";
        //                Cc.UpdationMethod("updateVendor", values, ID.ToString());
        //                result = "true";
        //            }
        //            else
        //            {
        //                result = "-2";
        //                TempData["DbVarification"] = "UnConnected";
        //            }
        //        }
        //        else
        //        {
        //            result = "-1";
        //            TempData["NetVarification"] = "UnConnected";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Cc.writeEventLog(ex.ToString());
        //        throw ex;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //Product

        public ActionResult User_access()
        {
            if (Session["Login"] != null)
            {
                try
                {
                    if (Cc.CheckForInternetConnection() == true)
                    {
                        if (Cc.DatabaseConnectionCheck() == true)
                        {
                            TempData["Title"] = "User Credential";
                            logindata();

                            List<login> loginType = Cc.loginSession("LoginType", "0", "0");
                            ViewBag.loginTypeVB = new SelectList(loginType, "login_type_id", "login_type");
                        }
                        else
                        {
                            TempData["DbVarification"] = "UnConnected";
                        }
                    }
                    else
                    {
                        TempData["NetVarification"] = "UnConnected";
                    }

                }
                catch (Exception ex)
                {
                    Cc.WriteEventLog(ex.ToString());
                    throw ex;
                }
                return View();
            }
            else
            {
                //return View();
                return RedirectToAction("Login", "Home");
            }
        }
        public JsonResult setEditMemberCredential(login model)
        {
            string status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (Session["user_name"] != null && Session["user_credential_id"] != null)
                        {
                            if (model.user_credential_id > 1 )
                            {

                                string values = " user_name='" + model.user_name + "', login_id='" + model.login_id + "', user_mobileNo='" + model.user_mobileNo + "', login_type='" + model.login_type + "', login_type_id=" + model.login_type_id + ", email= '" + model.email + "'";
                                Cc.UpdationMethod("EditMemberCredential", values, model.user_credential_id.ToString());
                                status = "Saved";
                            }
                            else
                            {
                                //string DocumentationPath = "~/User_image";
                                //bool exists = System.IO.Directory.Exists(Server.MapPath(DocumentationPath));

                                //if (!exists)
                                //{
                                //    System.IO.Directory.CreateDirectory(Server.MapPath(DocumentationPath));
                                //}
                                //var userimage = model.emp_pic;
                                //var extensionuserimage = Path.GetExtension(model.emp_pic.FileName);
                                //string image = Cc.RandomNumber(0, 10000) + extensionuserimage;
                                //userimage.SaveAs(Server.MapPath(DocumentationPath + "/" + image));



                                model.password = Cc.Generatepassword();
                                Cc.CredentialSendEmail(model.email.ToString().Trim(), model.user_name, model.login_id, model.password);
                                string field = "user_name , login_id , password , Phone_num , login_type  , email , login_type_id  ";
                                string values = " '" + model.user_name + "' , '" + model.login_id + "' , '" + model.password + "' , '" + model.user_mobileNo + "'  ,'" + model.login_type + "','" + model.email + "' , '" + model.login_type_id + "' ";
                                status = Cc.InsertionMethodStatus("setCredential", field, values);
                            }
                        }
                        else
                        {
                            status = "SessionDestory";
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        status = "DbVarification";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    status = "NetVarification";
                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberUserAccess()
        {
            List<login> GetMemberUserAccess = new List<login>();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        GetMemberUserAccess = Cc.loginSession("MemberUserAccess", "0000", "0000").ToList();
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";

                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";

                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                throw ex;
            }

            return Json(GetMemberUserAccess, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditMemberCredential(string ID)
        {
            login model = new login();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        List<login> loginType = Cc.loginSession("LoginType", "0", "0");
                        ViewBag.loginTypeVB = new SelectList(loginType, "login_type_id", "login_type");

                        foreach (var item in Cc.loginSession("EditMemberUserAccess", ID.ToString(), "000"))
                        {
                            model.user_credential_id = item.user_credential_id;
                            model.user_name = item.user_name.ToString().Trim();
                            model.login_id = item.login_id.ToString().Trim();
                            model.email = item.email;
                            model.user_mobileNo = item.user_mobileNo;
                            model.login_type_id = item.login_type_id;
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                throw ex;
            }
            return PartialView("~/Views/Admin/UserCredential/PartialViewMemberEditCredential.cshtml", model);
        }
     
        public JsonResult DeleteCredentialMember(login model)
        {
            string result = "false";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (model.checkbox.Length > 0)
                        {
                            foreach (var items in model.checkbox)
                            {
                                string values = "isdelete='true'";
                                result = Cc.UpdationMethodReturn("updateMemberCredential", values, items.ToString());
                            }
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Cc.WriteEventLog(ex.ToString());
                //throw ex;
                TempData["ExceptionError"] = "ExceptionError";
                return Json("ExceptionError", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //user control

        //Setting
        public JsonResult SetChangePassword(login model)
        {
            string status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["user_credential_id"] != null)
                        {
                            if (Session["password"].ToString() == model.password )
                            {
                                int ID = Cc.ToInt32(Session["user_credential_id"].ToString());
                                string values = "Password='" + model.NewPassword.ToString().Trim() + "'";
                                status = Cc.UpdationMethodReturn("ChangePassword", values, ID.ToString());
                                Session["password"] = model.NewPassword.ToString().Trim();
                            }
                            else
                            {
                                TempData["DbVarification"] = "UnConnected";
                                return Json("passwordError", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            status = "Session Destory";
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
                //throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult setChangePicture(login model)
        {
            string status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        if (Session["user_name"] != null && Session["user_credential_id"] != null)
                        {
                            string user_name = Session["user_name"].ToString();

                            HttpFileCollectionBase files = Request.Files;
                            if (files.Count > 0)
                            {
                                bool exists = System.IO.Directory.Exists(Server.MapPath("~/Employee/Picture/" + user_name.ToString().Trim() + "/"));

                                if (!exists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Employee/Picture/" + user_name.ToString().Trim() + "/"));
                                }
                                string path = Server.MapPath("~/Employee/Picture/" + user_name.ToString().Trim() + "/");

                                for (int i = 0; i < files.Count; i++)
                                {
                                    HttpPostedFileBase file = files[i];
                                    var extention = Path.GetExtension(file.FileName);

                                    //fileName = user_name + extention;
                                    //file.SaveAs(path + fileName);
                                }
                            }

                            int ID = Cc.ToInt32(Session["user_credential_id"].ToString());
                            //string field = "user_credential_id";
                            //string values = "emp_pic='" + fileName.ToString().Trim() + "'";
                            // Cc.UpdationnMethod("ChangePassword", values, ID.ToString());
                            //Cc.BackOfficeInsertion("Delete", "tbl_user_access", field, values, ID.ToString());
                            status = "Saved";

                        }
                        else
                        {
                            status = "SessionDestory";
                        }

                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        status = "DbVarification";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    status = "NetVarification";
                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                return Json(ex, JsonRequestBehavior.AllowGet);
                //throw ex;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Settings()
        {
            if (Session["Login"] != null)
            {
                int checker = logindata();
                if (checker == 1)
                {
                    return RedirectToAction("Login", "Home");
                }
                try
                {
                    if (Cc.CheckForInternetConnection() == true)
                    {
                        if (Cc.DatabaseConnectionCheck() == true)
                        {
                            TempData["Title"] = "Custom Rights";

                            List<login> User_Name = Cc.loginSession("UserList", "0", "0");
                            ViewBag.User_NameVB = new SelectList(User_Name, "user_credential_id", "user_name");
                        }
                        else
                        {
                            TempData["DbVarification"] = "UnConnected";
                        }
                    }
                    else
                    {
                        TempData["NetVarification"] = "UnConnected";
                    }

                }
                catch (Exception ex)
                {
                    Cc.WriteEventLog(ex.ToString());
                    throw ex;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public JsonResult Rights(string user, string moduleID, string column_name, string result)
        {
            string status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        string field = "" + column_name + " , user_id ,module_permission_id";
                        string values = "'" + result + "','" + user + "','" + moduleID + "'";
                        string value = column_name + "=" + result;
                        status = Cc.BackOfficeInsertion("Rights", field, values, moduleID, user, column_name, value);

                        //string values = column_name + "=" + result;
                        //Cc.BackOfficeInsertion("Rights", field, values, "'" + user + "'", column_name, result, user);
                        //email funcation add krna hai and sms
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRight(string filter_id)
        {
            List<login> GetRight = new List<login>();
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {

                        GetRight = Cc.loginSession("GetRightList", "empty", filter_id).ToList();
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";

                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";

                }
            }
            catch (Exception ex)
            {
                string opl = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
                Cc.WriteEventLog(opl + " : " + ex.ToString());
                throw ex;
            }
            return Json(GetRight, JsonRequestBehavior.AllowGet);
        }
        //Setting

        public ActionResult login()
        {
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        if (Session["Login"] != null)
                        {
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult LoginAndPassword(login model)
        {
            //List<Login> login = new List<Login>();
            string Status = "";
            try
            {
                if (Cc.CheckForInternetConnection() == true)
                {
                    if (Cc.DatabaseConnectionCheck() == true)
                    {
                        Status = Cc.LoginVerification("AdministratorSide", model.login_id.ToString().Trim(), model.password.ToString().Trim());

                        if (Status.ToString().Trim().Equals("Invalid Login Id"))
                        {
                            return Json(Status, JsonRequestBehavior.AllowGet);
                        }
                        else if (Status.ToString().Trim().Equals("Invalid Password Id"))
                        {
                            return Json(Status, JsonRequestBehavior.AllowGet);
                        }
                        else if (Status.ToString().Equals("AdministratorSideVerified"))
                        {
                            rightSessions(Status, model.login_id.ToString().Trim(), model.password.ToString().Trim());
                            Status = Session["login_type"].ToString().Trim();
                        }
                    }
                    else
                    {
                        TempData["DbVarification"] = "UnConnected";
                        return Json("DataBaseError", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["NetVarification"] = "UnConnected";
                    return Json("NetworkError", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(Status, JsonRequestBehavior.AllowGet);
        }


        public void rightSessions(string status, string LoginID, string Password)
        {
            foreach (var item in Cc.loginSession(status, LoginID, Password))
            {
                Session["user_credential_id"] = item.user_credential_id;
                Session["user_name"] = item.user_name.ToString().Trim();
                Session["password"] = item.password.ToString().Trim();
                Session["login_id"] = item.login_id.ToString().Trim();
                Session["user_mobileNo"] = item.user_mobileNo.ToString().Trim();
                Session["login_type"] = item.login_type.ToString().Trim();
                Session["login_picture"] = item.user_img.ToString().Trim();
                if (item.module_name.ToString().Trim() == "Question") 
                {
                    Session["Question_can_read"] = item.can_read;
                    Session["Question_can_create"] = item.can_create;
                    Session["Question_can_delete"] = item.can_delete;
                    Session["Question_can_update"] = item.can_update;
                    Session["Question_can_print"] = item.can_print;
                    Session["Question_can_report"] = item.can_report;

                }
                else if (item.module_name.ToString().Trim() == "Product")
                {
                    Session["Product_can_read"] = item.can_read;
                    Session["Product_can_create"] = item.can_create;
                    Session["Product_can_delete"] = item.can_delete;
                    Session["Product_can_update"] = item.can_update;
                    Session["Product_can_print"] = item.can_print;
                    Session["Product_can_report"] = item.can_report;
                }
                else if (item.module_name.ToString().Trim() == "Chat")
                {
                    Session["Chat_can_read"] = item.can_read;
                    Session["Chat_can_create"] = item.can_create;
                    Session["Chat_can_delete"] = item.can_delete;
                    Session["Chat_can_update"] = item.can_update;
                    Session["Chat_can_print"] = item.can_print;
                    Session["Chat_can_report"] = item.can_report;
                }
                else if (item.module_name.ToString().Trim() == "Settings")
                {
                    Session["Settings_can_read"] = item.can_read;
                    Session["Settings_can_create"] = item.can_create;
                    Session["Settings_can_delete"] = item.can_delete;
                    Session["Settings_can_update"] = item.can_update;
                    Session["Settings_can_print"] = item.can_print;
                    Session["Settings_can_report"] = item.can_report;
                }
                else if (item.module_name.ToString().Trim() == "User_access")
                {
                    Session["User_access_can_read"] = item.can_read;
                    Session["User_access_can_create"] = item.can_create;
                    Session["User_access_can_delete"] = item.can_delete;
                    Session["User_access_can_update"] = item.can_update;
                    Session["User_access_can_print"] = item.can_print;
                    Session["User_access_can_report"] = item.can_report;
                }
            }

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            Session["Login"] = finalString;

        }
        public int logindata()
        {
            if (Session["Login"] != null)
            {

                TempData["user_credential_id"] = Session["user_credential_id"];
                TempData["user_name"] = Session["user_name"];
                TempData["login_id"] = Session["login_id"];
                TempData["user_mobileNo"] = Session["user_mobileNo"];
                TempData["login_type"] = Session["login_type"];
                TempData["login_picture"] = Session["login_picture"];

                TempData["Question_can_read"] = Session["Question_can_read"];
                TempData["Question_can_create"] = Session["Question_can_create"];
                TempData["Question_can_delete"] = Session["Question_can_delete"];
                TempData["Question_can_update"] = Session["Question_can_update"];
                TempData["Question_can_print"] = Session["Question_can_print"];
                TempData["Question_can_report"] = Session["Question_can_report"];

                TempData["Product_can_read"] = Session["Product_can_read"];
                TempData["Product_can_create"] = Session["Product_can_create"];
                TempData["Product_can_delete"] = Session["Product_can_delete"];
                TempData["Product_can_update"] = Session["Product_can_update"];
                TempData["Product_can_print"] = Session["Product_can_print"];
                TempData["Product_can_report"] = Session["Product_can_report"];

                TempData["Chat_can_read"] =  Session["Chat_can_read"] ;
                TempData["Chat_can_create"] = Session["Chat_can_create"] ;
                TempData["Chat_can_delete"] = Session["Chat_can_delete"];
                TempData["Chat_can_update"] = Session["Chat_can_update"];
                TempData["Chat_can_print"] = Session["Chat_can_print"] ;
                TempData["Chat_can_report"] = Session["Chat_can_report"];

                TempData["Settings_can_read"] = Session["Settings_can_read"];
                TempData["Settings_can_create"] = Session["Settings_can_create"];
                TempData["Settings_can_delete"] = Session["Settings_can_delete"] ;
                TempData["Settings_can_update"] = Session["Settings_can_update"] ;
                TempData["Settings_can_print"] = Session["Settings_can_print"];
                TempData["Settings_can_report"] = Session["Settings_can_report"];

                TempData["User_access_can_read"] = Session["User_access_can_read"];
                TempData["User_access_can_create"] = Session["User_access_can_create"] ;
                TempData["User_access_can_delete"] = Session["User_access_can_delete"] ;
                TempData["User_access_can_update"] = Session["User_access_can_update"] ;
                TempData["User_access_can_print"] = Session["User_access_can_print"] ;
                TempData["User_access_can_report"] = Session["User_access_can_report"];
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public ActionResult Logout()
        {
            Session["Login"] = null;
            Session["user_credential_id"] = null;
            Session["user_name"] = null;
            Session["password"] = null;

            Session["login_id"] = null;
            Session["user_mobileNo"] = null;
            Session["login_type"] = null;
            Session["login_picture"] = null;

            TempData["user_credential_id"] = null;
            TempData["user_name"] = null;
            TempData["login_id"] = null;
            TempData["user_mobileNo"] = null;
            TempData["login_type"] = null;
            TempData["login_picture"] = null;

            Session["Question_can_read"] = null;
            Session["Question_can_create"] = null;
            Session["Question_can_delete"] = null;
            Session["Question_can_update"] = null;
            Session["Question_can_print"] = null;
            Session["Question_can_report"] = null;

            TempData["Question_can_read"] = null;
            TempData["Question_can_create"] = null;
            TempData["Question_can_delete"] = null;
            TempData["Question_can_update"] = null;
            TempData["Question_can_print"] = null;
            TempData["Question_can_report"] = null;

            Session["Product_can_read"] = null;
            Session["Product_can_create"] = null;
            Session["Product_can_delete"] = null;
            Session["Product_can_update"] = null;
            Session["Product_can_print"] = null;
            Session["Product_can_report"] = null;

            TempData["Product_can_read"] = null;
            TempData["Product_can_create"] = null;
            TempData["Product_can_delete"] = null;
            TempData["Product_can_update"] = null;
            TempData["Product_can_print"] = null;
            TempData["Product_can_report"] = null;

            TempData["Chat_can_read"] = null;
            Session["Chat_can_read"] = null; 
            TempData["Chat_can_create"] =null;
            Session["Chat_can_create"] = null ;
            TempData["Chat_can_delete"] = null;
            Session["Chat_can_delete"] = null ;
            TempData["Chat_can_update"] = null;
            Session["Chat_can_update"] = null ;
            TempData["Chat_can_print"] = null;
            Session["Chat_can_print"] = null ;
            TempData["Chat_can_report"] = null; 
            Session["Chat_can_report"] = null ;

            TempData["Settings_can_read"] = null; 
            Session["Settings_can_read"] = null ;
            TempData["Settings_can_create"] = null;
            Session["Settings_can_create"] = null ;
            TempData["Settings_can_delete"] = null;
            Session["Settings_can_delete"] = null ;
            TempData["Settings_can_update"] = null;
            Session["Settings_can_update"] = null ;
            TempData["Settings_can_print"] = null; 
            Session["Settings_can_print"] = null ;
            TempData["Settings_can_report"] = null;
            Session["Settings_can_report"] = null ;

            TempData["User_access_can_read"] = null; 
            Session["User_access_can_read"] = null; 
            TempData["User_access_can_create"] = null; 
            Session["User_access_can_create"] = null; 
            TempData["User_access_can_delete"] = null;
            Session["User_access_can_delete"] = null; 
            TempData["User_access_can_update"] = null;
            Session["User_access_can_update"] = null; 
            TempData["User_access_can_print"] = null;
            Session["User_access_can_print"] = null; 
            TempData["User_access_can_report"] = null; 
            Session["User_access_can_report"] = null; 


            return RedirectToAction("Login", "Home");
        }
    }
}