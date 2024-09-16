using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewDigitalPlatform.Components
{
    /// <summary>
    /// HorizontalPipeline.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalPipeline : ComponentBase
    {
        /// <summary>
        /// 流体的颜色
        /// </summary>
        public Brush LiquidColor
        {
            get { return (Brush)GetValue(LiquidColorProperty); }
            set { SetValue(LiquidColorProperty, value); }
        }
        public static readonly DependencyProperty LiquidColorProperty =
            DependencyProperty.Register("LiquidColor", typeof(Brush), typeof(HorizontalPipeline),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9916a1ff"))));
        



        public HorizontalPipeline()
        {
            InitializeComponent();

            var state = VisualStateManager.GoToState(this, "WEFlowState", false);
        }
    }
}
