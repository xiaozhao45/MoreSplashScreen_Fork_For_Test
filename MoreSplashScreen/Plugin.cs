
using ClassIsland.Core;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Abstractions.Views;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Controls.CommonDialog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoreSplashScreen.Views.SplashScreens;

namespace MoreSplashScreen
{
    [PluginEntrance]
    public class Plugin : PluginBase
    {
        public override void Initialize(HostBuilderContext context, IServiceCollection services)
        {
            //AppBase.Current.AppStarted += (sender, args) => CommonDialog.ShowInfo("Hello world!");
            services.AddTransient<SplashWindowBase, AndroidStudioSplashScreen>();
        }
    }
}
