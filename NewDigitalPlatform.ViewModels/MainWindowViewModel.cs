using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewDigitalPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NewDigitalPlatform.ViewModels
{
    public partial class MainWindowViewModel:ObservableObject
    {
        [ObservableProperty]
        public UserModel _lobalUserInfo;
        [ObservableProperty]
        public List<MenuModel> _menus;
        [ObservableProperty]
        private object _viewContent;

        //命令
        public RelayCommand<object> SwitchPageCommand { get; set; }

        public   MainWindowViewModel()
        {
            _lobalUserInfo = DependencyInjection.GetService<UserModel>();
            Menus = new List<MenuModel>();
            #region 菜单数据
            Menus.Add(new MenuModel
            {
                IsSelected = true,
                MenuHeader = "监控",
                MenuIcon = "\ue639",
                TargetView = "MonitorPage"
            });
            Menus.Add(new MenuModel
            {
                MenuHeader = "趋势",
                MenuIcon = "\ue61a",
                TargetView = "TrendPage"
            });
            Menus.Add(new MenuModel
            {
                MenuHeader = "报警",
                MenuIcon = "\ue60b",
                TargetView = "AlarmPage"
            });
            Menus.Add(new MenuModel
            {
                MenuHeader = "报表",
                MenuIcon = "\ue703",
                TargetView = "ReportPage"
            });
            Menus.Add(new MenuModel
            {
                MenuHeader = "配置",
                MenuIcon = "\ue60f",
                TargetView = "SettingsPage"
            });
            #endregion
            SwitchPageCommand = new RelayCommand<object>(ShowPage);
            ShowPage(Menus[0]);


        }
        
        [RelayCommand]
        public void ShowPage(object obj)
        {
            var model = obj as MenuModel;
            if (model != null)
            {
                if (ViewContent != null && ViewContent.GetType().Name == model.TargetView) return;

                Type type = Assembly.Load("NewDigitalPlatform.Views")
                    .GetType("NewDigitalPlatform.Views.Pages." + model.TargetView)!;
                ViewContent = Activator.CreateInstance(type)!;
            }

        }/*if (model != null)
            {
                if (ViewContent != null && ViewContent.GetType().Name == model.TargetView) return;

                Type type = Assembly.Load("DigitaPlatform.Views")
                    .GetType("DigitaPlatform.Views.Pages." + model.TargetView)!;
                ViewContent = Activator.CreateInstance(type)!;
            }*/

        

    }
}
