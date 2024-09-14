using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewDigitalPlatform.Models
{
    public class MenuModel:ObservableObject
    {
        public bool IsSelected { get; set; }
        public int Key { get; set; }
        public string MenuHeader { get; set; }
        public string TargetView { get; set; }
        public string MenuIcon { get; set; }
        
    }
}
