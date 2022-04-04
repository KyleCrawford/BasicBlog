using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogMe.Startup))]
namespace BlogMe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
