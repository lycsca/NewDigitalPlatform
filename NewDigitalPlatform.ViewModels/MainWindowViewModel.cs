using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using NewDigitalPlatform.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NewDigitalPlatform.ViewModels
{
    public partial class MainWindowViewModel: ObservableObject
    {
        [ObservableProperty]
        public UserModel _globalUserInfo;
        [ObservableProperty]
        public List<MenuModel> _menus;
        [ObservableProperty]
        private object _viewContent;

        //monitor
        [ObservableProperty]
        public List<RankingItemModel> _rankingList;
        [ObservableProperty]
        public List<MonitorWarnningModel> _warnningList;





        //命令
        public RelayCommand<object> SwitchPageCommand { get; set; }

        public   MainWindowViewModel()
        {
            _globalUserInfo = DependencyInjection.GetService<UserModel>();
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
            // Monitor
            Random random = new Random();
            #region 用气排行
            string[] quality = new string[] { "车间-1", "车间-2", "车间-3", "车间-4",
                "车间-5" };
            RankingList = new List<RankingItemModel>();
            foreach (var q in quality)
            {
                RankingList.Add(new RankingItemModel()
                {
                    Header = q,
                    PlanValue = random.Next(100, 200),
                    FinishedValue = random.Next(10, 150),
                });
            }
            #endregion

            #region 设备提醒
            WarnningList = new List<MonitorWarnningModel>()
                {
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：故障",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                  new MonitorWarnningModel{Message= "PLT-01：保养到期",
                      DateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                };
            #endregion

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

        }

        

    }
}
