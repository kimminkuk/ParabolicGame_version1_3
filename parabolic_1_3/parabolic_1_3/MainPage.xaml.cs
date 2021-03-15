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
        bool timer_stop_PN = true;
        bool parabolic_clear = false;
        int parabolic_cnt = 0;
        int parabolic_pre_cnt = 0;
        System.Threading.Timer timer_;
        ParabolicCal parabolicCal;
        SKPoint[] points = new SKPoint[50000];
        SKPoint[] Points2 = new SKPoint[4];

        bool wall_1_pn = false;

        public MainPage()
        {
            InitializeComponent();
        }
        private void BTN1_Clicked(object sender, EventArgs e)
        {
            timer_stop_PN = true;
            parabolic_clear = false;
            parabolic_cnt = 0;
            //wall_1_pn = true;
            //parabolicCal.CalPalabolic(); //Cal to Pre Parabolic

            //Thread t1 = new Thread(Thread_CalPalabolic);
            //t1.Start();
            //t1.Join();

            parabolic_pre_cnt = 0;
            wall_1_pn = false;

            StartTimer(); // make graphics

        }

        private void Thread_CalPalabolic(object obj) //x:2094, y 491
        {
            float Vo = (float)PowerSlider.Value;
            float Rad = (float)(Math.PI / 180) * (float)RadSlider.Value;
            float g = 10;
            int i_2 = 0;

            bool wall_1_pn = false;

            //for (int i = parabolic_pre_cnt; i < parabolic_cnt; i++)
            for (int i = 0; i < 5000; i++)
            {
                float t = (float)i / 50;
                g = 5 * t;
                //float x = (float)(Vo * Math.Cos(Rad)) * i;
                //float y =(float)(Vo * Math.Sin(Rad)) * i - g * i; //info.Height ( ex) 640 )

                float x = (float)(Vo * Math.Cos(Rad)) * t;
                float y = (float)(Vo * Math.Sin(Rad)) * t - g * t; //info.Height ( ex) 640 )

                y = 491 - y;
                if ( y < 0 ) //ex) -3 -30 - 100.. -30 -3
                {
                    i_2++;
                    Vo = (float)(PowerSlider.Value * 0.98);
                    ////y = (float)(0.8*Vo * Math.Sin(Rad)) * t - (float)(g * t *1.3);
                    ////y = y - 491;
                    //y = y * -1;
                    t = (float)i_2 / 50;
                    g = 5 * t;
                    //Rad = (float)(Math.PI / 180) * (float)(90 - RadSlider.Value);
                    //y += (float)(Vo * Math.Sin(Rad)) * t - g * t;
                    //y += g * t;
                    y = (y * -1) + g*t; //중력가속도 추가, 부딪힐때마다 파워 감소
                    //if( y > 491)
                    //{
                    //    y -= g * t;
                    //}
                }
                if(wall_1_pn != true && y >= 98.2 && y <= 392.8 && x >= 1256.4 && x <= 1266.4 )
                {
                    wall_1_pn = true;
                    //Vo = (float)(PowerSlider.Value * 0.98);
                    //x = 2094 - x;
                }
                if(wall_1_pn)
                {
                    x = 2094 - x;
                }
                //if (y < 0) //SKPaint 벗어 날 경우, 벽위에 부딪히는 개념 , M1V1 = M2V2 => 난 질량이 없다는 가정이니깐 그냥 위치만 바꿔주면될듯?
                //{
                //    //y = (y * -1) + g*t*2;
                //    //y += info.Height;
                //    //y = y * -1;
                //    //Rad = (float)(Math.PI / 180) * (90-(float)RadSlider.Value);
                //    //y = (float)(Vo * Math.Sin(Rad)) * t - g * t; //info.Height ( ex) 640 )
                //    y = y * -1;
                //}
                //
                //else if ( y > 491 ) // SKPaint 벗어 날 경우, 벽 아래에 부딪히는 개념
                //{
                //    Vo = (float)(PowerSlider.Value * 0.95);
                //    y = y + 491;
                //}
                points[i] = new SKPoint(x, y);
            }
            //x:2094, y 491
            //1456.8, 1256.4, 392.8, 98.2
            //points2[0] = new SKPoint((float)0.6 * 2094, (float)0.2 * 491);
            //points2[1] = new SKPoint((float)0.7 * 2094, (float)0.2 * 491);
            //points2[2] = new SKPoint((float)0.6 * 2094, (float)0.8 * 491);
            //points2[3] = new SKPoint((float)0.7 * 2094, (float)0.8 * 491);
        }

        private void BTN2_Clicked(object sender, EventArgs e)
        {
            if (timer_stop_PN)
            {
                timer_stop();
                //parabolic_cnt = 0;
                //parabolic_pre_cnt = 0;
                //parabolic_clear = true;
                //timer_stop_PN = false;
                //wall_1_pn = true;
            }
        }

        private void StartTimer()
        {
            if (timer_stop_PN)
            {
                time_interval = (int)SpeedSlider.Value; //1초?
                //timer_start(MY_TIMER_TICK_OBJECT2, 0, time_interval);
                timer_start(MY_TIMER_TICK_OBJECT, 0, time_interval);
            }
        }

        private async void MY_TIMER_TICK_OBJECT2(object state)
        {
            await Task.Run(() =>
            {
                parabolic_cnt++;
            });
        }

        private async void MY_TIMER_TICK_OBJECT(object state)
        {
            await Task.Run(() =>
            {
                parabolic_cnt++;
            });
            await Task.Run(() =>
            {
                CanvasView.InvalidateSurface();
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
            //float y = info.Height;
            //float x = info.Width;

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };

            //x:2094, y 491
            SKCanvas canvas2 = surface.Canvas;
            SKPoint[] points2 = new SKPoint[4];
            points2[0] = new SKPoint((float)0.6 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[1] = new SKPoint((float)0.7 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[2] = new SKPoint((float)0.6 * e.Info.Width, (float)0.8 * e.Info.Height);
            points2[3] = new SKPoint((float)0.7 * e.Info.Width, (float)0.8 * e.Info.Height);
            canvas2.Clear();
            SKPaint paint2 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 10
            };
            canvas2.DrawLine(points2[0],points2[1], paint2); //1
            canvas2.DrawLine(points2[0], points2[2], paint2); //2
            canvas2.DrawLine(points2[1], points2[3], paint2); //1
            canvas2.DrawLine(points2[2], points2[3], paint2); //2



            float Vo = (float)PowerSlider.Value;
            float Rad = (float)(Math.PI / 180) * (float)RadSlider.Value;
            float g = 10;
            int i_2 = 0;

            

            //for (int i = parabolic_pre_cnt; i < parabolic_cnt; i++)
            for (int i = parabolic_cnt-1; i < parabolic_cnt; i++)
            //for (int i = 0; i < 5000; i++)
            {
                float t = (float)i / 100;
                //float t = i;
                g = 5 * t;
                //float x = (float)(Vo * Math.Cos(Rad)) * i;
                //float y =(float)(Vo * Math.Sin(Rad)) * i - g * i; //info.Height ( ex) 640 )

                float x = (float)(Vo * Math.Cos(Rad)) * t;
                float y = (float)(Vo * Math.Sin(Rad)) * t - g * t; //info.Height ( ex) 640 )

                y = 491 - y;
                if (y < 0) //ex) -3 -30 - 100.. -30 -3
                {
                    i_2++;
                    Vo = (float)(PowerSlider.Value * 0.98);
                    t = (float)i_2 / 50;
                    g = 5 * t;
                    y = (y * -1) + g * t; //중력가속도 추가, 부딪힐때마다 파워 감소
                }
                if (wall_1_pn != true && y >= (float)0.2 * e.Info.Height && 
                    y <= (float)0.8 * e.Info.Height && x >= (float)0.6 * e.Info.Width)
                {
                    wall_1_pn = true;
                }
                if (wall_1_pn)
                {
                    x = (float)0.6 * e.Info.Width + ((float)0.6 * e.Info.Width - x);
                }
                //points[i] = new SKPoint(x, y);

                canvas.DrawCircle(x, y, 20, paint);
            }

            //for (int i = parabolic_pre_cnt; i < parabolic_cnt - 1; i++)
            //{
            //    canvas.DrawLine(points[i], points[i + 1], paint);
            //}
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //CanvasView.InvalidateSurface();
        }
    }
}
