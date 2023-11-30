using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;


using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlHelper
{
    /// <summary>
    /// ColorPicker.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        /// <summary>
        /// 선택된 색상 정보 반환
        /// </summary>
        public System.Windows.Media.Color SelectedColor
        {
            get
            {
                if (m_colorData == null)
                    return Colors.Black;

                return m_colorData.ColorValue;
            }
        }

        /// <summary>
        /// 색상 정보
        /// </summary>
        private ColorData m_colorData;

        /// <summary>
        /// 마지막으로 선택한 색상 위치 정보
        /// </summary>
        private Point m_lastSelectedColorPoint;

        private float m_lastSelectedHueValue;

        public ColorPicker()
        {
            InitializeComponent();

            m_colorData = new ColorData();
            this.DataContext = m_colorData;

            rectColorMonitor.MouseDown += RectColorMonitorMouseDown;
            rectColorMonitor.MouseMove += RectColorMonitorMouseMove;
            rectColorMonitor.MouseUp += RectColorMonitorMouseUp;

            rectHueMonitor.MouseDown += RectHueMonitorMouseDown;
            rectHueMonitor.MouseMove += RectHueMonitorMouseMove;
            rectHueMonitor.MouseUp += RectHueMonitorMouseUp;

            Loaded += ColorPickerLoaded;
        }

        /// <summary>
        /// 화면 갱신
        /// </summary>
        public void UpdateView(System.Drawing.Color color)
        {
            m_colorData.UpdateView(color);
        }

        /// <summary>
        /// RGB 값을 이용한 메인 View 갱신
        /// </summary>
        private void UpdateMainView(byte r, byte g, byte b)
        {
            double h, s, v;
            ConvertRgbToHsv(r / 255.0, g / 255.0, b / 255.0, out h, out s, out v);

            // update Saturation and Value locator
            double xPos = s * rectSample.ActualWidth;
            double yPos = (1 - v) * rectSample.ActualHeight;

            SampleSelector.Margin = new Thickness(xPos - (SampleSelector.Height / 2), yPos - (SampleSelector.Height / 2), 0, 0);

            m_lastSelectedHueValue = (float)h;
            h /= 360;
            const int gradientStops = 6;
            rectSample.Fill = new SolidColorBrush(GetColorFromPosition(((int)(h * 255)) * gradientStops));

            // Update Hue locator
            HueSelector.Margin = new Thickness(0, (h * rectHueMonitor.ActualHeight) - (HueSelector.ActualHeight / 2), 0, 0);
        }

        /// <summary>
        /// 선택 위치 값을 이용하여 RGB 값 갱신
        /// </summary>
        private void UpdateColorPosition(double dPosX, double dPosY)
        {
            SampleSelector.Margin = new Thickness(dPosX - (SampleSelector.Height / 2), dPosY - (SampleSelector.Height / 2), -10, -10);

            float yComponent = 1 - (float)(dPosY / rectSample.ActualHeight);
            float xComponent = (float)(dPosX / rectSample.ActualWidth);

            System.Windows.Media.Color col = ConvertHsvToRgb(m_lastSelectedHueValue, xComponent, yComponent);
            col.A = m_colorData.A;

            m_colorData.UpdateView(System.Drawing.Color.FromArgb(col.A, col.R, col.G, col.B));
        }

        /// <summary>
        /// 선택 위치 값을 이용하여 HSV 값 갱신
        /// </summary>
        private void UpdateHuePosition(double dPosY)
        {
            int iHuePos = (int)(dPosY / rectHueMonitor.ActualHeight * 255);
            int iGradientStops = 6;

            System.Windows.Media.Color col = GetColorFromPosition(iHuePos * iGradientStops);
            rectSample.Fill = new SolidColorBrush(col);
            HueSelector.Margin = new Thickness(0, dPosY - (HueSelector.ActualHeight / 2), 0, 0);
            m_lastSelectedHueValue = (int)((float)(dPosY / rectHueMonitor.ActualHeight) * 360);
            UpdateColorPosition(m_lastSelectedColorPoint.X, m_lastSelectedColorPoint.Y);
        }

        /// <summary>
        /// HSV 값을 이용하여 RGB 값 변환
        /// </summary>
        private System.Windows.Media.Color ConvertHsvToRgb(float h, float s, float v)
        {
            h = h / 360;
            if (s > 0)
            {
                if (h >= 1)
                    h = 0;
                h = 6 * h;
                int hueFloor = (int)Math.Floor(h);
                byte a = (byte)Math.Round(255 * v * (1.0 - s));
                byte b = (byte)Math.Round(255 * v * (1.0 - (s * (h - hueFloor))));
                byte c = (byte)Math.Round(255 * v * (1.0 - (s * (1.0 - (h - hueFloor)))));
                byte d = (byte)Math.Round(255 * v);

                switch (hueFloor)
                {
                    case 0: return System.Windows.Media.Color.FromArgb(255, d, c, a);
                    case 1: return System.Windows.Media.Color.FromArgb(255, b, d, a);
                    case 2: return System.Windows.Media.Color.FromArgb(255, a, d, c);
                    case 3: return System.Windows.Media.Color.FromArgb(255, a, b, d);
                    case 4: return System.Windows.Media.Color.FromArgb(255, c, a, d);
                    case 5: return System.Windows.Media.Color.FromArgb(255, d, a, b);
                    default: return System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
                }
            }
            else
            {
                byte d = (byte)(v * 255);
                return System.Windows.Media.Color.FromArgb(255, d, d, d);
            }
        }

        /// <summary>
        /// RGB 값을 이용하여 HSV 값 변환 
        /// </summary>
        private void ConvertRgbToHsv(double r, double g, double b, out double h, out double s, out double v)
        {
            double colorMax = Math.Max(Math.Max(r, g), b);

            v = colorMax;
            if (v == 0)
            {
                h = 0;
                s = 0;
                return;
            }

            // normalize
            r /= v;
            g /= v;
            b /= v;

            double colorMin = Math.Min(Math.Min(r, g), b);
            colorMax = Math.Max(Math.Max(r, g), b);

            s = colorMax - colorMin;
            if (s == 0)
            {
                h = 0;
                return;
            }

            // normalize saturation
            r = (r - colorMin) / s;
            g = (g - colorMin) / s;
            b = (b - colorMin) / s;
            colorMax = Math.Max(Math.Max(r, g), b);

            // calculate hue
            if (colorMax == r)
            {
                h = 0.0 + 60.0 * (g - b);
                if (h < 0.0)
                    h += 360.0;
            }
            else if (colorMax == g)
            {
                h = 120.0 + 60.0 * (b - r);
            }
            else // colorMax == b
            {
                h = 240.0 + 60.0 * (r - g);
            }
        }

        /// <summary>
        /// 위치 값을 이용하여 RGB 색상 반환
        /// </summary>
        private System.Windows.Media.Color GetColorFromPosition(int position)
        {
            byte mod = (byte)(position % 255);
            byte diff = (byte)(255 - mod);
            byte alpha = 255;

            switch (position / 255)
            {
                case 0: return System.Windows.Media.Color.FromArgb(alpha, 255, mod, 0);
                case 1: return System.Windows.Media.Color.FromArgb(alpha, diff, 255, 0);
                case 2: return System.Windows.Media.Color.FromArgb(alpha, 0, 255, mod);
                case 3: return System.Windows.Media.Color.FromArgb(alpha, 0, diff, 255);
                case 4: return System.Windows.Media.Color.FromArgb(alpha, mod, 0, 255);
                case 5: return System.Windows.Media.Color.FromArgb(alpha, 255, 0, diff);
                default: return Colors.Black;
            }
        }

        #region Trigger

        /// <summary>
        /// 색상 위치 초기화
        /// </summary>
        private void ColorPickerLoaded(object sender, RoutedEventArgs e)
        {
            if (m_colorData == null)
                return;

            UpdateMainView(m_colorData.R, m_colorData.G, m_colorData.B);
        }

        /// <summary>
        /// HSV 값 중 H에 대한 입력 값 검증
        /// </summary>
        private void HTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            string inputText = tb.Text;
            int integerValue;
            if (int.TryParse(inputText, out integerValue) == false)
                return;

            if (integerValue <= 0)
                tb.Text = "0";
            else if (360 <= integerValue)
                tb.Text = "360";
        }

        /// <summary>
        /// HSV 값 중 SV에 대한 입력 값 검증
        /// </summary>
        private void SVTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            string inputText = tb.Text;
            int integerValue;
            if (int.TryParse(inputText, out integerValue) == false)
                return;

            if (integerValue <= 0)
                tb.Text = "0";
            else if (100 <= integerValue)
                tb.Text = "100";
        }

        /// <summary>
        /// 색상 변경 시 갱신 처리 적용
        /// </summary>
        private void ColorChangeKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            DependencyProperty prop = TextBox.TextProperty;

            BindingExpression binding = BindingOperations.GetBindingExpression(tb, prop);
            if (binding != null)
                binding.UpdateSource();

            ColorPickerLoaded(null, null);
        }

        /// <summary>
        /// RGB 색상 선택 MouseDown
        /// </summary>
        private void RectColorMonitorMouseDown(object sender, MouseButtonEventArgs e)
        {
            rectColorMonitor.CaptureMouse();

            m_lastSelectedColorPoint = e.GetPosition((UIElement)sender);
            UpdateColorPosition(m_lastSelectedColorPoint.X, m_lastSelectedColorPoint.Y);
        }

        /// <summary>
        /// RGB 색상 선택 MouseMove
        /// </summary>
        private void RectColorMonitorMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            m_lastSelectedColorPoint = e.GetPosition((UIElement)sender);
            int posX = (int)m_lastSelectedColorPoint.X;
            int posY = (int)m_lastSelectedColorPoint.Y;

            if (posY < 0)
                posY = 0;

            if (posY > rectColorMonitor.ActualHeight)
                posY = (int)rectColorMonitor.ActualHeight;

            if (posX < 0)
                posX = 0;

            if (posX > rectColorMonitor.ActualWidth)
                posX = (int)rectColorMonitor.ActualWidth;

            UpdateColorPosition(posX, posY);
        }

        /// <summary>
        /// RGB 색상 선택 MouseUp
        /// </summary>
        private void RectColorMonitorMouseUp(object sender, MouseButtonEventArgs e)
        {
            rectColorMonitor.ReleaseMouseCapture();
        }


        /// <summary>
        /// HSV 색상 선택 MouseDown
        /// </summary>
        void RectHueMonitorMouseDown(object sender, MouseEventArgs e)
        {
            rectHueMonitor.CaptureMouse();

            Point pos = e.GetPosition((UIElement)sender);
            UpdateHuePosition(pos.Y);
        }

        /// <summary>
        /// HSV 색상 선택 MouseUp
        /// </summary>
        void RectHueMonitorMouseUp(object sender, MouseEventArgs e)
        {
            rectHueMonitor.ReleaseMouseCapture();
        }

        /// <summary>
        /// HSV 색상 선택 MouseMove
        /// </summary>
        private void RectHueMonitorMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            int yPos = (int)e.GetPosition((UIElement)sender).Y;
            if (yPos < 0) yPos = 0;
            if (yPos >= rectHueMonitor.ActualHeight) yPos = (int)rectHueMonitor.ActualHeight - 1;
            UpdateHuePosition(yPos);
        }
        #endregion
    }

    /// <summary>
    /// 색상 정보 구조체
    /// </summary>
    public class ColorData : INotifyPropertyChanged
    {
        #region Binding

        /// <summary>
        /// 컬러 값
        /// </summary>
        public string ColorText
        {
            get => m_colorText;
            set
            {
                m_colorText = value;
                OnPropertyChanged();
            }
        }
        private string m_colorText = "000000";

        /// <summary>
        /// 16bit 컬러 값
        /// </summary>
        public string Color565Text
        {
            get => m_color565Text;
            set
            {
                m_color565Text = value;
                OnPropertyChanged();
            }
        }

        private string m_color565Text = "0000";


        /// <summary>
        /// Red 속성 값
        /// </summary>
        public byte R
        {
            get => m_r;
            set
            {
                m_r = value;
                OnPropertyChanged();

                UpdateRGBToHSV();
            }
        }
        private byte m_r = 0;

        /// <summary>
        /// Green 속성 값
        /// </summary>
        public byte G
        {
            get => m_g;
            set
            {
                m_g = value;
                OnPropertyChanged();

                UpdateRGBToHSV();
            }
        }
        private byte m_g = 0;

        /// <summary>
        /// Blue 속성 값
        /// </summary>
        public byte B
        {
            get => m_b;
            set
            {
                m_b = value;
                OnPropertyChanged();

                UpdateRGBToHSV();
            }
        }
        private byte m_b = 0;

        /// <summary>
        /// Alpha 속성 값
        /// </summary>
        public byte A
        {
            get => m_a;
            set
            {
                m_a = value;
                OnPropertyChanged();

                UpdateRGBToHSV();
            }
        }
        private byte m_a = 0;

        /// <summary>
        /// Hue 색상 속성 값
        /// </summary>
        public int H
        {
            get => m_h;
            set
            {
                m_h = value;
                OnPropertyChanged();

                UpdateHSVToRGB();
            }
        }
        private int m_h = 0;

        /// <summary>
        /// Saturation 채도 속성 값
        /// </summary>
        public int S
        {
            get => m_s;
            set
            {
                m_s = value;
                OnPropertyChanged();

                UpdateHSVToRGB();
            }
        }
        private int m_s = 0;

        /// <summary>
        /// Brightness 명도 속성 값
        /// </summary>
        public int V
        {
            get => m_v;
            set
            {
                m_v = value;
                OnPropertyChanged();

                UpdateHSVToRGB();
            }
        }
        private int m_v = 0;

        /// <summary>
        /// 색상 정보
        /// </summary>
        public System.Windows.Media.Color ColorValue
        {
            get => m_colorValue;
            set
            {
                m_colorValue = value;
                OnPropertyChanged("ColorBrush");

                UpdateRGBText();
            }
        }
        private System.Windows.Media.Color m_colorValue = Colors.Black;

        /// <summary>
        /// 색상 Brush 정보
        /// </summary>
        public SolidColorBrush ColorBrush => new SolidColorBrush(ColorValue);

        #endregion

        /// <summary>
        /// 화면 갱신
        /// </summary>
        public void UpdateView(System.Drawing.Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;

            ColorValue = System.Windows.Media.Color.FromArgb(A, R, G, B);
        }

        /// <summary>
        /// RGB 값을 이용한 HSV 값 갱신
        /// </summary>
        private void UpdateRGBToHSV()
        {
            System.Drawing.Color color = System.Drawing.Color.FromArgb(A, R, G, B);

            double dH = color.GetHue();
            double dS = color.GetSaturation();
            double dV = color.GetBrightness();

            m_h = (int)Math.Round(dH, 0);
            m_s = (int)Math.Round(dS * 100.0, 0);
            m_v = (int)Math.Round(dV * 100.0, 0);

            OnPropertyChanged("H");
            OnPropertyChanged("S");
            OnPropertyChanged("V");
        }

        /// <summary>
        /// HSV 값을 이용한 RGB 값 갱신
        /// </summary>
        private void UpdateHSVToRGB()
        {
            double dH = (double)m_h;
            double dS = (double)(m_s / 100.0);
            double dV = (double)(m_v / 100.0);

            int iHStep = Convert.ToInt32(Math.Floor(dH / 60)) % 6;

            double f = dH / 60 - Math.Floor(dH / 60);

            dV = dV * 255;

            int iV = Convert.ToInt32(dV);
            int iP = Convert.ToInt32(dV * (1 - dS));
            int iQ = Convert.ToInt32(dV * (1 - f * dS));
            int iT = Convert.ToInt32(dV * (1 - (1 - f) * dS));

            byte v = (byte)iV;
            byte p = (byte)iP;
            byte q = (byte)iQ;
            byte t = (byte)iT;

            System.Windows.Media.Color newColor;
            if (iHStep == 0) newColor = System.Windows.Media.Color.FromArgb(255, v, t, p);
            else if (iHStep == 1) newColor = System.Windows.Media.Color.FromArgb(255, q, v, p);
            else if (iHStep == 2) newColor = System.Windows.Media.Color.FromArgb(255, p, v, t);
            else if (iHStep == 3) newColor = System.Windows.Media.Color.FromArgb(255, p, q, v);
            else if (iHStep == 4) newColor = System.Windows.Media.Color.FromArgb(255, t, p, v);
            else newColor = System.Windows.Media.Color.FromArgb(255, v, p, q);

            m_a = newColor.A;
            m_r = newColor.R;
            m_g = newColor.G;
            m_b = newColor.B;
            ColorValue = System.Windows.Media.Color.FromArgb(A, R, G, B);

            OnPropertyChanged("A");
            OnPropertyChanged("R");
            OnPropertyChanged("G");
            OnPropertyChanged("B");
        }

        /// <summary>
        /// RGB 색상 TEXT 갱신
        /// </summary>
        private void UpdateRGBText()
        {
            ColorText = string.Format("{0:X2}{1:X2}{2:X2}", R, G, B);

            byte bX1 = (byte)((R & 0xF8) | (G >> 5));
            byte bX2 = (byte)(((G & 0x1C) << 3) | (B >> 3));
            Color565Text = string.Format("{0:X2}{1:X2}", bX1, bX2);
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Property changed event for the enabled status
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string szName = "")
        {
            if (string.IsNullOrEmpty(szName) == true)
                return;

            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(szName));
        }

        #endregion
    }
}
