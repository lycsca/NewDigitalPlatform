using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewDigitalPlatform.Models
{
    // 这个对象Model属于DeviceItemModel的PropList
    public class DevicePropModel
    {
        // 属性的名称  通信协议（Header）  Protocol（Name）    -》 对接通信异构平台   名称 进行对比
        public string PropName { get; set; }
        public string PropValue { get; set; }
    }
}
