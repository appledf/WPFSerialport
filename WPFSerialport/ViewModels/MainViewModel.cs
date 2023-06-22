using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using WPFSerialport.Common;
using WPFSerialport.Common.Models;
using WPFSerialport.Extensions;

namespace WPFSerialport.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public DelegateCommand LoginOutCommand { get; private set; }

        public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);

            //LoginOutCommand = new DelegateCommand(() =>
            //{
            //    //注销当前用户
            //    App.LoginOut(containerProvider);
            //});
            this.containerProvider = containerProvider;
            this.regionManager = regionManager;
        }
        private void Navigate(MenuBar obj)
        {

            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            //MessageBox.Show(obj.Title);
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }

        }


        void CreateMenuBar()
        {
            menuBars.Add(new MenuBar() { Icon = "SerialPort", Title = "串口通信", NameSpace = "SerialPortPage" });
            menuBars.Add(new MenuBar() { Icon = "FileArrowLeftRight", Title = "转换助手", NameSpace = "TransitionView" });
            menuBars.Add(new MenuBar() { Icon = "CogRefresh", Title = "系统设置", NameSpace = "SettingsView" });
            menuBars.Add(new MenuBar() { Icon = "NoteBook", Title = "软件帮助", NameSpace = "HelpView" });
            menuBars.Add(new MenuBar() { Icon = "Information", Title = "关于软件", NameSpace = "AboutView" });
        }

        public void Configure()
        {
            //UserName = AppSession.UserName;
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("SerialPortPage");
        }
    }
}
