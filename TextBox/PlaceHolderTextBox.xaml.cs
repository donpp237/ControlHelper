using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace ControlHelper
{
    /// <summary>
    /// PlaceHolderTextBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlaceHolderTextBox : UserControl
    {
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PlaceHolderTextBox),
                                                                 new FrameworkPropertyMetadata((obj, args) => ((PlaceHolderTextBox)obj).UpdateTextValue(obj))
                                                                 {
                                                                     BindsTwoWayByDefault = true,
                                                                     DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                                                 });

        public string PlaceHolderText
        {
            get => (string)GetValue(PlaceHolderTextProperty);
            set => SetValue(PlaceHolderTextProperty, value);
        }
        public static readonly DependencyProperty PlaceHolderTextProperty = DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(PlaceHolderTextBox),
                                                                            new FrameworkPropertyMetadata((obj, args) => ((PlaceHolderTextBox)obj).UpdatePlaceHolderTextValue(obj))
                                                                            {
                                                                                BindsTwoWayByDefault = false
                                                                            });

        public PlaceHolderTextBox()
        {
            InitializeComponent();
        }

        private void UpdateTextValue(DependencyObject obj)
        {
            PlaceHolderTextBox ctrl = obj as PlaceHolderTextBox;
            if (ctrl == null)
                return;

            ctrl.textBox.Text = ctrl.Text;
        }

        private void UpdatePlaceHolderTextValue(DependencyObject obj)
        {
            PlaceHolderTextBox ctrl = obj as PlaceHolderTextBox;
            if (ctrl == null)
                return;

            ctrl.placeHolderTextBox.Text = ctrl.PlaceHolderText;
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var ctrl = sender as TextBox;
            if (ctrl == null)
                return;

            SetValue(TextProperty, ctrl.Text);
            placeHolderTextBox.Visibility = string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
