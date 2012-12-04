using System.Web.Optimization;
using Paragon.Infrastructure;

[assembly: WebActivator.PostApplicationStartMethod(typeof(paragon.web.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace paragon.web.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/bootstrap") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
            var jsBundle = new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap*");
            jsBundle.Transforms.Add(new JsMinify());

            var lessBundle = new StyleBundle("~/Content/bootstrap")
                .Include("~/Content/less/bootstrap.less", "~/Content/less/responsive.less");
            lessBundle.Transforms.Add(new LessMinify());

			BundleTable.Bundles.Add(jsBundle);
			BundleTable.Bundles.Add(lessBundle);

            BundleTable.EnableOptimizations = true;
		}
	}
}
