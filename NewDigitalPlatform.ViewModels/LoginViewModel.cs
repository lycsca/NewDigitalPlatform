
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewDigitalPlatform.DataAssets.Imp;
using NewDigitalPlatform.Models;
using NewDigitalPlatform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitaPlatform.ViewModels
{
    public partial class LoginViewModel: ObservableObject
    {
        public UserModel User { get; set; }
        public RelayCommand<object> LoginCommand { get; set; }
        [ObservableProperty]
        public string _failedMsg;

        ILocalDataAccess _localDataAccess;


        //利用依赖注入得到localDataAccess实例
        public LoginViewModel(ILocalDataAccess localDataAccess)
        {
            _localDataAccess = localDataAccess;

                User = new UserModel();
                LoginCommand = new RelayCommand<object>(DoLogin);
        }



        private void DoLogin(object obj)
        {
            // 对接数据库
            try
            {
                var data = _localDataAccess.Login(User.UserName, User.Password);
                if (data == null) throw new Exception("登录失败，没有用户信息");

                // 记录一下主窗口所需要的用户信息，对于SimpleIOC   同一个实例，默认是单例
                var main = DependencyInjection.GetService<UserModel>();
                
                if (main != null)
                {
                    main.UserName = User.UserName;
                    main.Password = User.Password;
                    main.RealName = data.Rows[0]["real_name"].ToString()!;
                    main.UserType = int.Parse(data.Rows[0]["user_type"].ToString()!);
                    main.Gender = int.Parse(data.Rows[0]["gender"].ToString()!);
                    main.Department = data.Rows[0]["department"].ToString()!;
                    main.PhoneNumber = data.Rows[0]["phone_num"].ToString()!;
                }
                
                (obj as Window).DialogResult = true;
            }
            catch (Exception ex)
            {
                FailedMsg = ex.Message;
            }
        }
    }
}
