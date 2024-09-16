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
    /// VerticalPipeline.xaml 的交互逻辑
    /// </summary>
    public partial class VerticalPipeline : ComponentBase
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
            DependencyProperty.Register("LiquidColor", typeof(Brush), typeof(VerticalPipeline),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9916a1ff"))));

        
        public VerticalPipeline()
        {
            InitializeComponent();
            // 初始化一个状态
            VisualStateManager.GoToState(this, "WEFlowState", false);
        }
    }
}
