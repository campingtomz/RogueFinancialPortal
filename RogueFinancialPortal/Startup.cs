using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RogueFinancialPortal.Startup))]
namespace RogueFinancialPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
