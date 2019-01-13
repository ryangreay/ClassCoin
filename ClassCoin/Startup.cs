using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClassCoin.Startup))]
namespace ClassCoin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}