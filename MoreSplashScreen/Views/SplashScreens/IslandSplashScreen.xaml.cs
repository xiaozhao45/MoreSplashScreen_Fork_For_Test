using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Services;

namespace MoreSplashScreen.Views.SplashScreens;

/// <summary>
/// IslandSplashScreen.xaml 的交互逻辑
/// </summary>
public partial class IslandSplashScreen
{
    public ISplashService SplashService { get; }
    public Plugin Plugin { get; }

    public static readonly DependencyProperty CurrentProgressProperty = DependencyProperty.Register(
        nameof(CurrentProgress), typeof(double), typeof(IslandSplashScreen), new PropertyMetadata(0.0));

    private double _lastProgress = 0.0;
    private double _lastProgressDelta = 0.0;

    public double CurrentProgress
    {
        get { return (double)GetValue(CurrentProgressProperty); }
        set { SetValue(CurrentProgressProperty, value); }
    }

    private bool _canClose = false;

    public BitmapImage? SplashImage { get; }

    public IslandSplashScreen(ISplashService splashService, Plugin plugin)
    {
        SplashService = splashService;
        Plugin = plugin;
        InitializeComponent();

        SplashService.SplashEnded += SplashServiceOnSplashEnded;
        SplashService.ProgressChanged += SplashServiceOnProgressChanged;
    }

    void Cal(object sender, RoutedEventArgs e)
    {
        // 获取屏幕的宽度和高度
        var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
        var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

        // 获取窗口的宽度和高度
        var windowWidth = this.Width;
        var windowHeight = this.Height;

        // 计算窗口的左上角位置，使窗口居中

        this.Left = (screenWidth - windowWidth) / 2;
        //CommonDialog.ShowHint(this.Left.ToString());
        this.Top = 0;
    }


    private void SplashServiceOnSplashEnded(object? sender, EventArgs e)
    {;
        Dispatcher.InvokeAsync(() =>
        {
            UpdateProgress(100, 0.1, (_, _ )=>
            {
                _canClose = true;
                Close();
            });
        });
    }

    private void SplashServiceOnProgressChanged(object? sender, double e)
    {
        Dispatcher.InvokeAsync(() =>
        {
            UpdateProgress(e, Math.Max((e - _lastProgress) / 8, 0.5));
            _lastProgressDelta = e - _lastProgress;
            _lastProgress = e;
        });

    }

    private void UpdateProgress(double e, double seconds, EventHandler? callBack = null)
    {
        var da = new DoubleAnimation()
        {
            From = CurrentProgress,
            To = e,
            Duration = new Duration(TimeSpan.FromSeconds(seconds)),
            EasingFunction = new ExponentialEase()
        };
        var storyboard = new Storyboard()
        {
        };
        Storyboard.SetTarget(da, this);
        Storyboard.SetTargetProperty(da, new PropertyPath(CurrentProgressProperty));
        storyboard.Children.Add(da);
        if (callBack != null)
            storyboard.Completed += callBack;
        //storyboard.RepeatBehavior = ;
        storyboard.Begin();
    }

    private void IslandSplashScreen_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void IslandSplashScreen_OnClosing(object? sender, CancelEventArgs e)
    {
        if (!_canClose)
        {
            e.Cancel = true;
        }
    }
}