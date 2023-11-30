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
                                                             new PropertyMetadata((obj, args) => ((PlaceHolderTextBox)(obj)).UpdatePlace()));

        public string PlaceHolderText
        {
            get => (string)GetValue(PlaceHolderTextProperty);
            set => SetValue(PlaceHolderTextProperty, value);
        }
        public static readonly DependencyProperty PlaceHolderTextProperty = DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(PlaceHolderTextBox));


        public PlaceHolderTextBox()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void UpdatePlace()
        {
            placeHolder.Visibility = string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
