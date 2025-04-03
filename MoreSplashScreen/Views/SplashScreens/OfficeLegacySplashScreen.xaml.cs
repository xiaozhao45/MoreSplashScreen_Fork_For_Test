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
using System.Windows.Shapes;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Services;

namespace MoreSplashScreen.Views.SplashScreens
{
    /// <summary>
    /// Microsoft365SplashScreen.xaml 的交互逻辑
    /// </summary>
    public partial class OfficeLegacySplashScreen
    {
        public ISplashService SplashService { get; }

        private bool _closed;

        public OfficeLegacySplashScreen(ISplashService splashService)
        {
            SplashService = splashService;
            SplashService.SplashEnded += SplashServiceOnSplashEnded;
            InitializeComponent();
        }

        private void SplashServiceOnSplashEnded(object? sender, EventArgs e)
        {
            SplashService.SplashEnded -= SplashServiceOnSplashEnded;
            if (!_closed)
            {
                Dispatcher.InvokeAsync(Close);
            }
        }

        private void Microsoft365SplashScreen_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Microsoft365SplashScreen_OnClosed(object? sender, EventArgs e)
        {
            _closed = true;
        }
    }
}
