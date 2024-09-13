
using System.Windows.Controls;
using System.Windows;

namespace NewDigitalPlatform.Assets
{
    public class PasswordBoxHelper
    {
        #region ����ֵ�ĸ�������
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper),
              new PropertyMetadata(new PropertyChangedCallback(OnPropertyChanged)));

        public static string GetPassword(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }
        #endregion

        #region ��������仯�¼��ĸ�������
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(string), typeof(PasswordBoxHelper),
            new PropertyMetadata(new PropertyChangedCallback(OnAttachChanged)));

        public static string GetAttach(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }
        public static void SetAttach(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }
        #endregion

        // ����������仯ʱ������߼�
        static bool _isUpdating = false;
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pb = (d as PasswordBox);
            pb.PasswordChanged -= Pb_PasswordChanged;
            if (!_isUpdating)
                (d as PasswordBox).Password = e.NewValue.ToString();
            pb.PasswordChanged += Pb_PasswordChanged;
        }
        // �����¼��仯�߼�
        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pb = (d as PasswordBox);
            pb.PasswordChanged += Pb_PasswordChanged;
        }

        private static void Pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (sender as PasswordBox);
            _isUpdating = true;
            SetPassword(pb, pb.Password);
            _isUpdating = false;
        }
    }

}
