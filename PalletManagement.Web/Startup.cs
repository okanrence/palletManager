using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PalletManagement.Web.Startup))]
namespace PalletManagement.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
