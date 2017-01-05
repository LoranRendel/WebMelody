using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using WaveGenerator;

namespace WebMelody
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CleanHeaders.AddHeader("Server");
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MelodyGeneration.Config(16000, BitDepth.Bit16, 1, 1000*60*2);
        }
    }
}