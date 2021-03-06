using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Threading;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace parabolic_1_3
{
    public partial class MainPage : ContentPage
    {
        int time_interval;
        bool timer_stop_PN = false;
        bool parabolic_clear = false;
        int parabolic_cnt = 0;
        System.Threading.Timer timer_;

        public MainPage()
        {
            InitializeComponent();
        }
        private void BTN1_Clicked(object sender, EventArgs e)
        {
            timer_stop_PN = true;
            parabolic_clear = false;
            StartTimer();
        }
        private void BTN2_Clicked(object sender, EventArgs e)
        {
            if (timer_stop_PN)
            {
                timer_stop();
                parabolic_cnt = 0;
                parabolic_clear = true;
            }
        }

        private void StartTimer()
        {
            time_interval = (int)SpeedSlider.Value; //1초?
            timer_start(MY_TIMER_TICK_OBJECT, 0, time_interval);
        }

        private async void MY_TIMER_TICK_OBJECT(object state)
        {
            await Task.Run(() =>
            {
                CanvasView.InvalidateSurface();
                parabolic_cnt++;
            });
        }

        private void timer_start(TimerCallback callback, int start, int time_interval)
        {
            timer_ = new System.Threading.Timer(callback, null, start, time_interval);
        }

        private void timer_stop()
        {
            timer_.Dispose();
        }
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();
            SKPoint[] points = new SKPoint[1000];

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25
            };

            float Vo = (float)PowerSlider.Value;

            float Rad = (float)(Math.PI / 180) * (float)RadSlider.Value;
            float g = 10;
            for (int i = 0; i < parabolic_cnt; i++)
            {
                g = 5 * i;
                float x = (float)(Vo * Math.Cos(Rad)) * i;
                float y = (float)(Vo * Math.Sin(Rad)) * i - g * i;
                Math.Pow(i, 2);
                points[i] = new SKPoint(x, info.Height - y);
            }

            for (int i = 0; i < parabolic_cnt - 1; i++)
            {
                canvas.DrawLine(points[i], points[i + 1], paint);
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //CanvasView.InvalidateSurface();
        }
    }
}
