using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoInsuranceWeb.Startup))]
namespace AutoInsuranceWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
