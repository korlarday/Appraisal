using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AprraisalApplication.Startup))]
namespace AprraisalApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
