using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using NewDigitalPlatform.DataAssets.Imp;
using NewDigitalPlatform.Entities;
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
using System.Windows;
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
        [ObservableProperty]
        private DeviceItemModel currentDevice;

        //monitor
        [ObservableProperty]
        public List<RankingItemModel> _rankingList;
        [ObservableProperty]
        public List<MonitorWarnningModel> _warnningList;

        [ObservableProperty]
        private string _saveFailedMessage;
        /*[ObservableProperty]
        public List<DeviceItemModel> _deviceList;*/

        public RelayCommand<DeviceItemModel> DeviceSelectedCommand { get; set; }

        private  ILocalDataAccess _localDataAccess;

        [ObservableProperty]
        public ObservableCollection<DeviceItemModel> _deviceList = new ObservableCollection<DeviceItemModel>();

        //命令
        public RelayCommand<object> SwitchPageCommand { get; set; }

        public   MainWindowViewModel(ILocalDataAccess localDataAccess)
        {
            _localDataAccess = localDataAccess;
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

            #region 设备
            this.ComponentsInit();
            #endregion

            DeviceSelectedCommand = new RelayCommand<DeviceItemModel>(model =>
            {
                // 记录一个当前选中组件
                // Model = DeviceItemModel
                // 对当前组件进行选中
                // 进行属性、点位编辑
                if (CurrentDevice != null)
                    CurrentDevice.IsSelected = false;
                if (model != null)
                {
                    model.IsSelected = true;
                }

                CurrentDevice = model;
            });


        }


        private void ComponentsInit()
        {
            var ds = _localDataAccess.GetDevices().Select(d =>
            {
                var dim = new DeviceItemModel((_localDataAccess))
                {
                    Header = d.Header,
                    X = double.Parse(d.X),
                    Y = double.Parse(d.Y),
                    Z = int.Parse(d.Z),
                    Width = double.Parse(d.W),
                    Height = double.Parse(d.H),
                    DeviceType = d.DeviceTypeName,
                    DeviceNum = d.DeviceNum,
                    DeleteCommand = new RelayCommand<DeviceItemModel>(model => DeviceList.Remove(model)),
                    Devices = () => DeviceList.ToList()
                };
                // 初始化每个组件的右键菜单 
                dim.InitContextMenu();

                return dim;
            });
            DeviceList = new ObservableCollection<DeviceItemModel>(ds);

            // 水平/垂直对齐线
            DeviceList.Add(new DeviceItemModel(_localDataAccess) { X = 0, Y = 0, Width = 2000, Height = 0.5, Z = 9999, DeviceType = "HL", IsVisible = false });
            DeviceList.Add(new DeviceItemModel(_localDataAccess) { X = 0, Y = 0, Width = 0.5, Height = 2000, Z = 9999, DeviceType = "VL", IsVisible = false });
            // 宽度/高度对齐线
            DeviceList.Add(new DeviceItemModel(_localDataAccess) { X = 0, Y = 0, Width = 0, Height = 15, Z = 9999, DeviceType = "WidthRule", IsVisible = false });
            DeviceList.Add(new DeviceItemModel(_localDataAccess) { X = 0, Y = 0, Width = 15, Height = 0, Z = 9999, DeviceType = "HeightRule", IsVisible = false });
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

        private void DoItemDropCommand(object obj)
        {
            DragEventArgs e = obj as DragEventArgs;
            var data = (ThumbItemModel)e.Data.GetData(typeof(ThumbItemModel));

            var point = e.GetPosition((IInputElement)e.Source);
            var dim = new DeviceItemModel(_localDataAccess)
            {
                Header = data.Header,
                DeviceNum = "D" + DateTime.Now.ToString("yyyyMMddHHmmssFFF"),
                DeviceType = data.TargetType,
                Width = data.Width,
                Height = data.Height,
                X = point.X - data.Width / 2,
                Y = point.Y - data.Height / 2,

                DeleteCommand = new RelayCommand<DeviceItemModel>(model => DeviceList.Remove(model)),
                Devices = () => DeviceList.ToList()
            };
            dim.InitContextMenu();
            DeviceList.Add(dim);
        }

        private void DoSaveCommand(object obj)
        {
            VisualStateManager.GoToElementState(obj as Window, "NormalSuccess", true);
            VisualStateManager.GoToElementState(obj as Window, "SaveFailedNormal", true);

            // 注意一个问题：对齐对象
            var ds = DeviceList
                .Where(d => !new string[] { "HL", "VL", "WidthRule", "HeightRule" }.Contains(d.DeviceType))
                .Select(dev => new DeviceEntity
                {
                    DeviceNum = dev.DeviceNum,
                    X = dev.X.ToString(),
                    Y = dev.Y.ToString(),
                    Z = dev.Z.ToString(),
                    W = dev.Width.ToString(),
                    H = dev.Height.ToString(),
                    DeviceTypeName = dev.DeviceType,

                    FlowDirection = dev.FlowDirection.ToString(),
                    Rotate = dev.Rotate.ToString(),

                    // 转换属性集合
                    Props = dev.PropList.Select(dp => new DevicePropItemEntity
                    {
                        PropName = dp.PropName,
                        PropValue = dp.PropValue,
                    }).ToList(),

                    // 转换点位集合
                    Vars = dev.VariableList.Select(dv => new VariableEntity
                    {
                        VarNum = dv.VarNum,
                        Header = dv.VarName,
                        Address = dv.VarAddress,
                        Offset = dv.Offset,
                        Modulus = dv.Modulus
                    }).ToList()
                });
            try
            {
                //throw new Exception("保存异常测试，没有执行实际保存逻辑，只用作查看异常提示效果！");

                _localDataAccess.SaveDevice(ds.ToList());

               // _windowState = true;
                // 提示保存成功
                VisualStateManager.GoToElementState(obj as Window, "SaveSuccess", true);
            }
            catch (Exception ex)
            {
                SaveFailedMessage = ex.Message;
                // 提示保存失败，包括异常信息
                VisualStateManager.GoToElementState(obj as Window, "SaveFailedShow", true);
            }
        }

       


    }
}
