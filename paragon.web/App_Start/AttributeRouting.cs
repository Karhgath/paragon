using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using System.Reflection;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Paragon.Web.App_Start.AttributeRouting), "Start")]

namespace Paragon.Web.App_Start {
    public static class AttributeRouting {
		public static void RegisterRoutes(RouteCollection routes) {
            
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            routes.MapAttributeRoutes(config =>
            {
                config.AddRoutesFromAssembly(Assembly.GetExecutingAssembly());
                config.AutoGenerateRouteNames = true;
            });
		}

        public static void Start() {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
