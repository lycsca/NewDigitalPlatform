using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using NewDigitalPlatform.Assets;
using NewDigitalPlatform.Components;

namespace NewDigitalPlatform.Assets.Converter
{


    /// <summary>
    /// 对应一个组件
    /// 删除命令 DeleteCommand
    /// 删除命令的参数  DeleteParameter
    /// 选中状态  IsSelected
    /// </summary>
    public class DeviceItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "HL")
            {
                return new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 2000,
                    Y2 = 0,
                    //Height = 0.5,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection { 3.0, 3.0 },
                    ClipToBounds = true,
                };
            }
            else if (value.ToString() == "VL")
            {
                return new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 0,
                    Y2 = 2000,
                    //Width = 0.5,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection { 3.0, 3.0 },
                    ClipToBounds = true,
                };
            }


            var assembly = Assembly.Load("NewDigitalPlatform.Components");
            Type t = assembly.GetType("NewDigitalPlatform.Components." + value.ToString())!;
            var obj = Activator.CreateInstance(t)!;
            if (new string[] { "WidthRule", "HeightRule" }.Contains(value.ToString()))
                return obj;


            // 组件生成
            var c = (ComponentBase)obj;

            Binding binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("DeleteCommand");
            c.SetBinding(ComponentBase.DeleteCommandProperty, binding);
            binding = new Binding();
            //binding.Path = new System.Windows.PropertyPath(".");
            c.SetBinding(ComponentBase.DeleteParameterProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("IsSelected");
            c.SetBinding(ComponentBase.IsSelectedProperty, binding);


            // 处理组件尺寸缩放命令逻辑绑定
            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("ResizeDownCommand");
            c.SetBinding(ComponentBase.ResizeDownCommandProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("ResizeMoveCommand");
            c.SetBinding(ComponentBase.ResizeMoveCommandProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("ResizeUpCommand");
            c.SetBinding(ComponentBase.ResizeUpCommandProperty, binding);


            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("Rotate");
            c.SetBinding(ComponentBase.RotateAngleProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("FlowDirection");
            c.SetBinding(ComponentBase.FlowDirectionProperty, binding);

            return c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


}

