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
/// AndroidStudioSplashScreen.xaml 的交互逻辑
/// </summary>
public partial class AndroidStudioSplashScreen
{
    public ISplashService SplashService { get; }
    public Plugin Plugin { get; }

    public static readonly DependencyProperty CurrentProgressProperty = DependencyProperty.Register(
        nameof(CurrentProgress), typeof(double), typeof(AndroidStudioSplashScreen), new PropertyMetadata(0.0));

    private double _lastProgress = 0.0;
    private double _lastProgressDelta = 0.0;

    public double CurrentProgress
    {
        get { return (double)GetValue(CurrentProgressProperty); }
        set { SetValue(CurrentProgressProperty, value); }
    }

    private bool _canClose = false;

    public BitmapImage? SplashImage { get; }

    public AndroidStudioSplashScreen(ISplashService splashService, Plugin plugin)
    {
        SplashService = splashService;
        Plugin = plugin;
        SplashImage = GetSplashImage();
        InitializeComponent();

        SplashService.SplashEnded += SplashServiceOnSplashEnded;
        SplashService.ProgressChanged += SplashServiceOnProgressChanged;
    }

    private BitmapImage? GetSplashImage()
    {
        try
        {
            return Plugin.Settings.IsCustomAndroidStudioSplashImageEnabled ? new BitmapImage(new Uri(Plugin.Settings.CustomAndroidStudioSplashImagePath, UriKind.RelativeOrAbsolute)) : GetDefaultSplashImage();
        }
        catch (Exception e)
        {
            return null;
        }
        
    }

    private BitmapImage GetDefaultSplashImage()
    {
        return new BitmapImage(new Uri($"/MoreSplashScreen;component/Assets/AndroidStudio/{AppBase.AppCodeName}.png", UriKind.RelativeOrAbsolute));
    }

    private void SplashServiceOnSplashEnded(object? sender, EventArgs e)
    {;
        Dispatcher.InvokeAsync(() =>
        {
            UpdateProgress(100, 0.1, (_, _) =>
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

    private void AndroidStudioSplashScreen_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void AndroidStudioSplashScreen_OnClosing(object? sender, CancelEventArgs e)
    {
        if (!_canClose)
        {
            e.Cancel = true;
        }
    }
}