using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Controls;

namespace ControlHelper
{
    /// <summary>
    /// OpenFileDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OpenFileDialog : UserControl
    {
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(OpenFileDialog),
                                                                 new FrameworkPropertyMetadata((obj, args) => ((OpenFileDialog)obj).UpdatePathValue(obj))
                                                                 {
                                                                     BindsTwoWayByDefault = true,
                                                                     DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                                                 });

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(OpenFileDialog),
                                                                 new FrameworkPropertyMetadata()
                                                                 {
                                                                     BindsTwoWayByDefault = true,
                                                                     DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                                                 });

        public string PlaceHolderText
        {
            get => (string)GetValue(PlaceHolderTextProperty);
            set => SetValue(PlaceHolderTextProperty, value);
        }
        public static readonly DependencyProperty PlaceHolderTextProperty = DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(OpenFileDialog),
                                                                            new FrameworkPropertyMetadata((obj, args) => ((OpenFileDialog)obj).UpdatePlaceHolderTextValue(obj))
                                                                            {
                                                                                BindsTwoWayByDefault = false
                                                                            });

        public string PathFindBtnName
        {
            get => (string)GetValue(PathFindBtnNameProperty);
            set => SetValue(PathFindBtnNameProperty, value);
        }
        public static readonly DependencyProperty PathFindBtnNameProperty = DependencyProperty.Register("PathFindBtnName", typeof(string), typeof(OpenFileDialog),
                                                                            new FrameworkPropertyMetadata((obj, args) => ((OpenFileDialog)obj).UpdatePathFindBtnName(obj))
                                                                            {
                                                                                BindsTwoWayByDefault = false
                                                                            });

        public OpenFileDialog()
        {
            InitializeComponent();
        }

        private void UpdatePathValue(DependencyObject obj)
        {
            OpenFileDialog ctrl = obj as OpenFileDialog;
            if (ctrl == null)
                return;

            ctrl.pathBox.Text = ctrl.Path;
        }

        private void UpdatePlaceHolderTextValue(DependencyObject obj)
        {
            OpenFileDialog ctrl = obj as OpenFileDialog;
            if (ctrl == null)
                return;

            ctrl.pathBox.PlaceHolderText = ctrl.PlaceHolderText;
        }

        private void UpdatePathFindBtnName(DependencyObject obj)
        {
            OpenFileDialog ctrl = obj as OpenFileDialog;
            if (ctrl == null)
                return;

            ctrl.pathFIndBtn.Content = ctrl.PathFindBtnName;
        }

        private void OpenFileDialogBtnClick(object sender, RoutedEventArgs args)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            try
            {
                dialog.Filter = Filter;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return;
            }

            bool? result = dialog.ShowDialog();
            if (result == true)
                SetValue(PathProperty, dialog.FileName);
        }
    }
}
