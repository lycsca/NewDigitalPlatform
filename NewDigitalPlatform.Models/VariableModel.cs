
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewDigitalPlatform.Models
{
    public partial class VariableModel : ObservableObject
    {
        // 唯一编码
        public string VarNum { get; set; }
        // 名称
        public string VarName { get; set; }
        // 地址
        public string VarAddress { get; set; }
        // 读取返回数据
        [ObservableProperty]
        private double _value;
      
        // 偏移量
        public double Offset { get; set; }
        // 系数
        public double Modulus { get; set; } = 1;
    }
}
