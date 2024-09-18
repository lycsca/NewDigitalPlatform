
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NewDigitalPlatform.Models
{
    public class ThumbModel
    {
        public string Header { get; set; }
        public List<ThumbItemModel> Children { get; set; } = new List<ThumbItemModel>();
    }

    public class ThumbItemModel:ObservableObject
    {
        public string Icon { get; set; }
        public string TargetType { get; set; }
        public string Header { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public RelayCommand<object> ThumbCommand { get; set; }

        public ThumbItemModel()
        {
            ThumbCommand = new RelayCommand<object>(DoThunmCommand);
        }
        private void DoThunmCommand(object obj)
        {
            DragDrop.DoDragDrop(obj as DependencyObject, this, DragDropEffects.Copy);
        }
    }
}
