//using System;
//using System.Web.Mvc;
//using System.Web.Optimization;
//using System.Web.Routing;
//using WebPush;

//namespace BlazeInspect
//{
//    public class MvcApplication : System.Web.HttpApplication
//    {
//        protected void Application_Start()
//        {
//            AreaRegistration.RegisterAllAreas();
//            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
//            RouteConfig.RegisterRoutes(RouteTable.Routes);
//            BundleConfig.RegisterBundles(BundleTable.Bundles);

//            // Generate VAPID keys on startup
//            GenerateAndStoreVapidKeys();
//        }

//        private void GenerateAndStoreVapidKeys()
//        {
//            // Generate VAPID keys if not already present
//            VapidDetails vapidKeys = VapidHelper.GenerateVapidKeys();

//            // Store public and private keys
//            string publicKey = vapidKeys.PublicKey;
//            string privateKey = vapidKeys.PrivateKey;

//            // You can log or persist these keys if needed
//            Application["VAPID_PublicKey"] = publicKey;
//            Application["VAPID_PrivateKey"] = privateKey;

//            // For development, log the public key so you can use it in JavaScript
//            System.Diagnostics.Debug.WriteLine("VAPID Public Key: " + publicKey);
//        }
//    }
//}


using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebPush;

namespace BlazeInspect
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Generate VAPID keys on startup
            GenerateAndStoreVapidKeys();
        }

        private void GenerateAndStoreVapidKeys()
        {
            // Generate VAPID keys if not already present
            var vapidKeys = VapidHelper.GenerateVapidKeys();

            // Store public and private keys
            string publicKey = vapidKeys.PublicKey;
            string privateKey = vapidKeys.PrivateKey;

            // You can log or persist these keys if needed
            Application["VAPID_PublicKey"] = publicKey;
            Application["VAPID_PrivateKey"] = privateKey;

            // For development, log the public key so you can use it in JavaScript
            System.Diagnostics.Debug.WriteLine("VAPID Public Key: " + publicKey);
        }
    }
}


//using BlazeInspect;
//using Org.BouncyCastle.Crypto.Parameters;
//using Org.BouncyCastle.Utilities.Encoders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Optimization;
//using System.Web.Routing;
//using WebPush;

//namespace BlazeInspect
//{
//    public class MvcApplication : System.Web.HttpApplication
//    {
//        protected void Application_Start()
//        {
//            AreaRegistration.RegisterAllAreas();
//            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
//            RouteConfig.RegisterRoutes(RouteTable.Routes);
//            BundleConfig.RegisterBundles(BundleTable.Bundles);
//        }
//    }
//}

//public class VapidHelper
//{
//    public static VapidDetails GetVapidDetails()
//    {
//        // You can store these keys safely and reuse them
//        var vapidKeys = VapidHelper.GenerateKeys();

//        string publicKey = vapidKeys.PublicKey;
//        string privateKey = vapidKeys.PrivateKey;

//        return new VapidDetails("mailto:your-email@example.com", publicKey, privateKey);
//    }
//    public static VapidKeys GenerateKeys()
//    {
//        return VapidHelper.GenerateKeys();
//    }
//}

