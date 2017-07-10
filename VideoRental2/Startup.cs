using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VideoRental2.Startup))]
namespace VideoRental2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
