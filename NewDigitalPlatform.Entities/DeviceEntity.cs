using System;
using System.Collections.Generic;

namespace NewDigitalPlatform.Entities
{
    public class DeviceEntity
    {
        public string DeviceNum { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string W { get; set; }
        public string H { get; set; }
        public string DeviceTypeName { get; set; }
        public string Header { get; set; }

        public string FlowDirection { get; set; }
        public string Rotate { get; set; }

        public List<DevicePropItemEntity> Props { get; set; }
        public List<VariableEntity> Vars { get; set; }
    }
}
