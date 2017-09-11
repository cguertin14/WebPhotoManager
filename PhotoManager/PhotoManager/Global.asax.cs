using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhotoManager
{
    public sealed class DB
    {
        private static readonly DB instance = new DB();
        private DB() { }
        public static Models.Users Users { get; set; }
        public static Models.Photos Photos { get; set; }
        public static DAL.DataBase DataBase { get; set; }
        public static void Initialize(string DB_Path, string SQL_Journal_Path = "")
        {
            String MainDB_Connection_String = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" +
                                              DB_Path +
                                              "'; Integrated Security=true;Max Pool Size=1024;Pooling=true;";
            DataBase = new DAL.DataBase(MainDB_Connection_String, SQL_Journal_Path);
            DataBase.TrackSQL =  false;
            Users = new Models.Users(DataBase);
            Photos = new Models.Photos(DataBase);
        }
    }
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DB.Initialize(Server.MapPath(@"~\App_Data\DB.mdf"),
                          Server.MapPath(@"~\App_Data\SQL_Journal.txt"));
        }
    }
}
