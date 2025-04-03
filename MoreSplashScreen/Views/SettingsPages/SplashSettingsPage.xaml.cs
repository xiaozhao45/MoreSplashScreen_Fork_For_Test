using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassIsland.Core.Attributes;
using MaterialDesignThemes.Wpf;

namespace MoreSplashScreen.Views.SettingsPages;

/// <summary>
/// SplashSettingsPage.xaml 的交互逻辑
/// </summary>
[SettingsPageInfo("dev.hellowrc.classisland.noMoreSplash.splashSettings", "更多启动屏幕", PackIconKind.RocketLaunchOutline, PackIconKind.RocketLaunch)]
public partial class SplashSettingsPage
{
    public Plugin Plugin { get; }

    public SplashSettingsPage(Plugin plugin)
    {
        Plugin = plugin;
        InitializeComponent();
    }
}