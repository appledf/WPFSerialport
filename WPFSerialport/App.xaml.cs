using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using WPFSerialport.Common;
using WPFSerialport.ViewModels;
using WPFSerialport.Views;

namespace WPFSerialport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();

        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<SerialPortPage, SerialPortPageModel>();
            //containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            //containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            //containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();
            //containerRegistry.RegisterForNavigation<HelpView, HelpViewModel>();
            //containerRegistry.RegisterForNavigation<TransitionView, TransitionViewModel>();
        }
    }
}
