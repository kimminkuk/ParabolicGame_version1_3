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

using parabolic_1_3.ViewModels;

namespace parabolic_1_3
{
    public partial class MainPage : ContentPage
    {
        int time_interval;
        bool timer_stop_PN = false;
        bool parabolic_clear = false;
        int parabolic_cnt = 0;
        int parabolic_pre_cnt = 0;
        System.Threading.Timer timer_;
        ParabolicCal parabolicCal;
        SKPoint[] points = new SKPoint[50000];
        public MainPage()
        {
            InitializeComponent();
        }
        private void BTN1_Clicked(object sender, EventArgs e)
        {
            timer_stop_PN = true;
            parabolic_clear = false;
            //parabolicCal.CalPalabolic(); //Cal to Pre Parabolic
            StartTimer(); // make graphics
        }
        private void BTN2_Clicked(object sender, EventArgs e)
        {
            if (timer_stop_PN)
            {
                timer_stop();
                parabolic_cnt = 0;
                parabolic_pre_cnt = 0;
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
                if(parabolic_cnt > 100)
                {
                    parabolic_pre_cnt++;
                }
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
            
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };

            float Vo = (float)PowerSlider.Value;

            float Rad = (float)(Math.PI / 180) * (float)RadSlider.Value;
            float g = 10;
            for (int i = parabolic_pre_cnt; i < parabolic_cnt; i++)
            {
                float t = (float)i / 50;
                g = 5 * t;
                //float x = (float)(Vo * Math.Cos(Rad)) * i;
                //float y =(float)(Vo * Math.Sin(Rad)) * i - g * i; //info.Height ( ex) 640 )

                float x = (float)(Vo * Math.Cos(Rad)) * t;
                float y =(float)(Vo * Math.Sin(Rad)) * t - g * t; //info.Height ( ex) 640 )

                y = info.Height - y;
                if( y < 0 ) //SKPaint 벗어 날 경우, 벽위에 부딪히는 개념 , M1V1 = M2V2 => 난 질량이 없다는 가정이니깐 그냥 위치만 바꿔주면될듯?
                {
                    y = y * -1;
                }

                //else if ( y > info.Height ) // SKPaint 벗어 날 경우, 벽 아래에 부딪히는 개념
                //{
                //    y = info.Height - y;
                //}
                points[i] = new SKPoint(x, y);
            }

            //for (int i = 0; i < parabolic_cnt - 1; i++)
            //{
            //    canvas.DrawLine(points[i], points[i + 1], paint);
            //}
            for (int i = parabolic_pre_cnt; i < parabolic_cnt - 1; i++)
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
