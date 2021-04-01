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
using Xamarin.Forms.Shapes;

using parabolic_1_3.ViewModels;

namespace parabolic_1_3
{
    public partial class MainPage : ContentPage
    {
        public float left = 0;
        public float top = 0;
        public float right = 0;
        public float bottom = 0;
        public float left2 = 0;
        public float top2 = 0;
        public float right2 = 0;
        public float bottom2 = 0;

        public float[] rect_point = new float[4 * 4];
        public bool[] rect_pn = new bool[4 * 4];
        SKRect[] rect = new SKRect[4 * 4];

        public float default_Init_x = 0;
        public float default_Init_y = 0;
        public float default_radius = 0;
        public float default_radius_move_allow = 0;
        public float Init_x = 0;
        public float Init_y = 0;
        public float control_rcos = 0;
        public float control_rsin = 0;
        public float control_rcos_abs = 0;
        public float control_rsin_abs = 0;

        public bool drag_onoff = true;

        public int time_interval;
        public bool timer_stop_PN = false;
        public bool parabolic_clear = false;
        public int parabolic_cnt = 0;
        public int parabolic_pre_cnt = 0;

        System.Threading.Timer timer_;

        SKPoint[] points = new SKPoint[50000];
        SKPoint[] Points2 = new SKPoint[4];

        public float[] Init_x_circle = new float[360];
        public float[] Init_y_circle = new float[360];

        /*ADD GITHUB EXAMPLE CODE*/
        /*Dictionary, List.. 센스있게 쓰네*/
        //배열처럼 담아두는 느낌인가? study 필요
        private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
        private readonly Dictionary<long, SKPoint> temporaryCircle = new Dictionary<long, SKPoint>();
        private readonly List<SKPoint> pathsCircle = new List<SKPoint>();
        private readonly List<SKPath> paths = new List<SKPath>();
        
        public struct Square
        {
            public double Width { get; set; }
            public double Height { get; set; }
            public double Area { get; private set; }

            public Square(double width, double height)
            {
                Width = width;
                Height = height;
                Area = width * width;
            }
        }

        bool wall_1_pn = false;
        public MainPage()
        {
            InitializeComponent();
            CanvasView2.InvalidateSurface();
            CanvasView.InvalidateSurface();
        }
        private void BTN1_Clicked(object sender, EventArgs e)
        {
            timer_stop_PN = true;
            parabolic_clear = false;
            parabolic_cnt = 0;
            parabolic_pre_cnt = 0;
            wall_1_pn = false;

            StartTimer(); // make graphics
        }

        private void StartTimer()
        {
            if (timer_stop_PN)
            {
                time_interval = 5;
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
#if false
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
#endif
        private void BTN2_Clicked(object sender, EventArgs e)
        {
            parabolic_cnt = 0;
            time_interval = 0;
            if (timer_stop_PN)
            {
                timer_stop();
                timer_stop_PN = false;
                drag_onoff = true; //Initial x,y 
                Init_x = 0;
                Init_y = 0;
                //parabolic_cnt = 0;
                //parabolic_pre_cnt = 0;
                //parabolic_clear = true;
                //timer_stop_PN = false;
                //wall_1_pn = true;
            }
            CanvasView.InvalidateSurface();
        }
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();

            //End Point Block Location Setting
            for (int i = 0; i < 5; i++)
            {
                rect_point[i] = (float)(e.Info.Width * 0.75 + e.Info.Width * (0.25 / 4) * i);
            }
            rect_point[5] = (float)(e.Info.Height * 0.2);
            rect_point[6] = (float)(e.Info.Height * 0.4);
            rect_point[7] = (float)(e.Info.Height * 0.6);
            rect_point[8] = (float)(e.Info.Height * 0.8);
            rect_point[9] = (float)(e.Info.Height);
            for (int i = 0; i < 16; i++)
            {
                rect[i] = new SKRect(rect_point[i % 4], rect_point[i / 4 + 5],
                    rect_point[(i % 4) + 1], rect_point[i / 4 + 6]);
            }


            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };

            //Init Location Constant
            float InitConstant_X = (float)(e.Info.Width * 0.075);
            float InitConstant_Y = (float)(e.Info.Height * 0.7);
            default_radius_move_allow = (float)(e.Info.Width * 0.07);
            default_Init_x = (float)(e.Info.Width * 0.075);
            default_Init_y = (float)(e.Info.Height * 0.7);

            //Init Circle
            if (drag_onoff)
            {
                Init_x = (float)(e.Info.Width * 0.075);
                Init_y = (float)(e.Info.Height * 0.7);

                //Init rect_pn
                for (int i = 0; i < 16; i++) rect_pn[i] = true;
            }

            //Init Location
            SKCanvas canvas6 = surface.Canvas;
            SKPaint paint6 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 2
            };

            SKCanvas canvas5 = surface.Canvas;
            SKPaint paintInit = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };

            if (timer_stop_PN != true)
            {
                canvas6.DrawCircle(InitConstant_X, InitConstant_Y, (float)(e.Info.Width * 0.07), paint6);
                canvas5.DrawCircle(Init_x, Init_y, (float)(e.Info.Width * 0.01), paintInit);
            }

            float dis = (float)(Math.Pow(Init_x - default_Init_x, 2) + Math.Pow(Init_y - default_Init_y, 2));
            float dis_init_circle = (float)Math.Sqrt(dis);

            control_rcos = (Init_x - default_Init_x) / dis_init_circle;
            control_rsin = (Init_y - default_Init_y) / dis_init_circle;
            control_rcos_abs = Math.Abs((Init_x - default_Init_x) / dis_init_circle); //각도 절대값 괜찮나?
            control_rsin_abs = Math.Abs((Init_y - default_Init_y) / dis_init_circle); //각도 절대값 괜찮나?
            Init_x = control_rcos * default_radius_move_allow + default_Init_x;
            Init_y = control_rsin * default_radius_move_allow + default_Init_y;

            // 공 날리기전에는 무조건 초기 원 설정 범위 내부에 들어오게 한다.
            if (timer_stop_PN != true)
            {
                canvas5.DrawCircle(Init_x, Init_y, (float)(e.Info.Width * 0.01), paintInit);
            }
            //TEST ACTION
            float Vo_test = ((float)Math.Sqrt((Math.Pow(Init_x - default_Init_x, 2) + Math.Pow(Init_y - default_Init_y, 2)))
                / default_radius_move_allow) * 300; //500 -> 600 ->

            //TEST PARABOLIC MOTION
            int i_2 = 0;
            for (int i = parabolic_cnt - 1; i < parabolic_cnt; i++)
            {
                if (i == -1) continue;

                float t = (float)i / 80;
                float g = 5 * t;
 
                float x = (float)(Vo_test * control_rcos_abs) * t + Init_x;
                //float y = ((float)(Vo_test * control_rsin_abs) * t - g * t ); //info.Height ( ex) 640 )
                float y = ((float)(Vo_test * control_rsin_abs) * t - g * t * 4); //info.Height ( ex) 640 )

                //y = 491 - y;
                //y = (e.Info.Height - y);
                y = Init_y - y;
                if (y < 0) //ex) -3 -30 - 100.. -30 -3
                {
                    i_2++;
                    Vo_test = (float)(Vo_test * 0.9);
                    t = (float)i_2 / 50;
                    g = 5 * t;
                    //y = (y * -1) + g * t; //중력가속도 추가, 부딪힐때마다 파워 감소
                    y = (y * -1) + g * t * 4; //중력가속도 추가, 부딪힐때마다 파워 감소
                }

                if (wall_1_pn != true && x >= (float)0.6 * e.Info.Width &&
                   x <= (float)0.62 * e.Info.Width && y >= (float)0.2 * e.Info.Height)
                {
                    wall_1_pn = true;
                }
                if (wall_1_pn)
                {
                    x = (float)0.6 * e.Info.Width + ((float)0.6 * e.Info.Width - x);
                }

                //GAME END
                if (y > e.Info.Height || x < 0 || x > e.Info.Width)
                {
                    parabolic_cnt = 0;
                    time_interval = 0;
                    if (timer_stop_PN)
                    {
                        timer_stop();
                        timer_stop_PN = false;
                        drag_onoff = true; //Initial x,y 
                        Init_x = 0;
                        Init_y = 0;

                        //Init rect_pn
                        for (int j = 0; j < 16; j++) rect_pn[j] = true;
                    }
                    CanvasView.InvalidateSurface();
                    CanvasView2.InvalidateSurface();
                    break;
                }

                // End Point Switch On
                // First 1~16 check
                // 0  1  2  3  4
                // |  |  |  |  |
                // |  |  |  |  |
                // |  |  |  |  |
                // |  |  |  |  |
                if( x > (float)(e.Info.Width * 0.75) )
                {
                    for (int row = 0; row < 4; row++)
                    {
                        if (rect_point[row] < x && rect_point[row+1] > x)
                        {
                            for (int col = 0; col < 4; col++)
                            {
                                //col 0~1, 1~2, 2~3, 3~4
                                if (rect_point[col + 5] < y && rect_point[col + 6] > y) 
                                {
                                    rect_pn[4 * col + row] = false;
                                }
                            }

                        }
                    }
                    CanvasView2.InvalidateSurface();
                }

                //공 위치 갱신 Draw
                canvas.DrawCircle(x, y, (float)0.01 * e.Info.Width, paint);
                
            }
        }

#if false
        private void CanvasView_PaintSurface_temp(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };

            // //TEST WALL 1
            // SKCanvas canvas2 = surface.Canvas;
            // SKPoint[] points2 = new SKPoint[4];
            // points2[0] = new SKPoint((float)0.6 * e.Info.Width, (float)0.2 * e.Info.Height);
            // points2[1] = new SKPoint((float)0.61 * e.Info.Width, (float)0.2 * e.Info.Height);
            // points2[2] = new SKPoint((float)0.6 * e.Info.Width, (float)0.8 * e.Info.Height);
            // points2[3] = new SKPoint((float)0.61 * e.Info.Width, (float)0.8 * e.Info.Height);
            // canvas2.Clear();
            // 
            // SKPaint paint2 = new SKPaint
            // {
            //     Style = SKPaintStyle.Fill,
            //     Color = Color.Blue.ToSKColor(),
            //     StrokeWidth = 30
            // };
            // //canvas2.DrawPath(SKPath)
            // canvas2.DrawLine(points2[0], points2[1], paint2); //1
            // canvas2.DrawLine(points2[0], points2[2], paint2); //2
            // canvas2.DrawLine(points2[1], points2[3], paint2); //1
            // canvas2.DrawLine(points2[2], points2[3], paint2); //2
            // 
            // //TEST RECTANGLE GOAL 1
            // SKCanvas canvas3 = surface.Canvas;
            // float left2 = (float)(e.Info.Width * 0.75);
            // float right2 = (float)(e.Info.Width);
            // float bottom2 = (float)(e.Info.Height);
            // float top2 = (float)(e.Info.Height * 0.2);
            // SKRect rect2 = new SKRect(left2, top2, right2, bottom2);
            // //canvas3.Clear();
            // 
            // SKPaint paint3 = new SKPaint
            // {
            //     Style = SKPaintStyle.Fill,
            //     Color = Color.RosyBrown.ToSKColor()
            // };
            // 
            // canvas3.DrawRect(rect2, paint3);

            //Init Location Constant
            //float InitConstant   = (float)(e.Info.Width * 0.07);
            float InitConstant_X_left = (float)(e.Info.Width * 0.005);
            float InitConstant_X_right = (float)(e.Info.Width * 0.145);
            float InitConstant_Y_up = (float)(e.Info.Height * 0.63);
            float InitConstant_Y_bottom = (float)(e.Info.Height * 0.77);
            float InitConstant_X = (float)(e.Info.Width * 0.075);
            float InitConstant_Y = (float)(e.Info.Height * 0.7);
            default_radius_move_allow = (float)(e.Info.Width * 0.07);
            default_Init_x = (float)(e.Info.Width * 0.075);
            default_Init_y = (float)(e.Info.Height * 0.7);
            //Init Circle
            if (drag_onoff)
            {
                Init_x = (float)(e.Info.Width * 0.075);
                Init_y = (float)(e.Info.Height * 0.7);
            }

            //Init Location
            SKCanvas canvas6 = surface.Canvas;
            //canvas6.Clear();
            SKPaint paint6 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 2
            };
            if (timer_stop_PN != true)
            {
                //var x = touchPath;
                //var y = touchPath;
                //x.GetPoints.X
                //canvas6.DrawCircle((touchPath.GetPoint.X, Init_y, (float)(e.Info.Width * 0.07), paint6);
                canvas6.DrawCircle(InitConstant_X, InitConstant_Y, (float)(e.Info.Width * 0.07), paint6);

            }

            SKCanvas canvas5 = surface.Canvas;
            SKPaint paintInit = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };
            //canvas5.Clear();
            if (timer_stop_PN != true)
            {
                float dis = (float)(Math.Pow(Init_x - default_Init_x, 2) + Math.Pow(Init_y - default_Init_y, 2));
                
                float dis_init_circle = (float)Math.Sqrt(dis);
                if(default_radius_move_allow <= dis_init_circle) //초기 원 범위 벗어날 경우,
                {
                    control_rcos = (Init_x - default_Init_x) / dis_init_circle;
                    control_rsin = (Init_y - default_Init_y) / dis_init_circle;
                    Init_x = control_rcos * default_radius_move_allow + default_Init_x;
                    Init_y = control_rsin * default_radius_move_allow + default_Init_y;
                }

                canvas5.DrawCircle(Init_x, Init_y, (float)(e.Info.Width*0.01), paintInit);
            }

#if false
            //  /*ADD GITHUB EXAMPLE CODE*/
            //  var touchPathStroke = new SKPaint
            //  {
            //      IsAntialias = true,
            //      Style = SKPaintStyle.Fill,
            //      Color = SKColors.Purple,
            //      StrokeWidth = 5
            //  };
            //  if (timer_stop_PN != true)
            //  {
            //       // draw the paths
            //       foreach (var touchPath in temporaryPaths)
            //       {
            //           canvas.DrawPath(touchPath.Value, touchPathStroke);
            //       }
            //       foreach (var touchPath in paths)
            //       {
            //           canvas.DrawPath(touchPath, touchPathStroke);
            //           
            //       }
            // 
            //      // 
            //      // // draw the paths
            //      // foreach (var touchPath in temporaryCircle)
            //      // { 
            //      //     
            //      //     //canvas.DrawPath(touchPath.Value, touchPathStroke);
            //      //     canvas.DrawCircle(touchPath.Value.X, touchPath.Value.Y, (float)(e.Info.Width * 0.01), touchPathStroke);
            //      // }
            //      // foreach (var touchPath in pathsCircle)
            //      // {
            //      //     //canvas.DrawPath(touchPath, touchPathStroke);
            //      //     canvas.DrawCircle(touchPath, (float)(e.Info.Width * 0.01), touchPathStroke);
            //      // }
            //  
            //  }
            // else
            // {
            //     canvas.Clear();
            // }
#endif

            //TEST ACTION
            float Vo_test = ((float)Math.Sqrt((Math.Pow(Init_x - default_Init_x, 2) + Math.Pow(Init_y - default_Init_y, 2))) 
                / default_radius_move_allow) * 500;
            

            //TEST PARABOLIC MOTION           
            float Vo = (float)PowerSlider.Value;
            float Rad = (float)(Math.PI / 180) * (float)RadSlider.Value;
            float g = 10;
            int i_2 = 0;

            //for (int i = parabolic_pre_cnt; i < parabolic_cnt; i++)
            for (int i = parabolic_cnt-1; i < parabolic_cnt; i++)
            //for (int i = 0; i < 5000; i++)
            {
                if (i == -1) continue;

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
                //  if (wall_1_pn != true && y >= (float)0.2 * e.Info.Height && 
                //      y <= (float)0.8 * e.Info.Height && x >= (float)0.6 * e.Info.Width)
                //  {
                //      wall_1_pn = true;
                //  }
                //  if (wall_1_pn)
                //  {
                //      x = (float)0.6 * e.Info.Width + ((float)0.6 * e.Info.Width - x);
                //  }
                //points[i] = new SKPoint(x, y);

                //
                 if(wall_1_pn !=true && x >= (float)0.6 * e.Info.Width && 
                    x <= (float)0.62 * e.Info.Width && y >= (float)0.2 * e.Info.Height)
                 {
                     wall_1_pn = true;
                 }
                 if (wall_1_pn)
                 {
                     x = (float)0.6 * e.Info.Width + ((float)0.6 * e.Info.Width - x);
                                         
                     //canvas3.DrawRect(rect, paint3);
                 }
                canvas.DrawCircle(x, y, (float)0.01*e.Info.Width, paint);
            }



        }
#endif
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //CanvasView.InvalidateSurface();
        }

        private void CanvasView_PaintSurface3(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();

            //test rectangle ////Rectangle
            //test rectangle SKCanvas canvas4 = surface.Canvas;
            //test rectangle float left = (float)(e.Info.Width * 0.25);
            //test rectangle float right = (float)(e.Info.Width * 0.5);
            //test rectangle float bottom = (float)(e.Info.Height * 0.5);
            //test rectangle float top = (float)(e.Info.Height * 0.2);
            //test rectangle SKRect rect = new SKRect(left, top, right, bottom);
            //test rectangle canvas4.Clear();
            //test rectangle SKPaint paint3 = new SKPaint
            //test rectangle {
            //test rectangle     Style = SKPaintStyle.Fill,
            //test rectangle     Color = Color.RosyBrown.ToSKColor()
            //test rectangle };
            //test rectangle canvas4.DrawRect(rect, paint3);

            //x:2094, y 491
            //Test Wall
            SKCanvas canvas2 = surface.Canvas;
            SKPoint[] points2 = new SKPoint[4];
            points2[0] = new SKPoint((float)0.6 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[1] = new SKPoint((float)0.61 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[2] = new SKPoint((float)0.6 * e.Info.Width, (float)0.8 * e.Info.Height);
            points2[3] = new SKPoint((float)0.61 * e.Info.Width, (float)0.8 * e.Info.Height);
            canvas2.Clear();

            SKPaint paint2 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 10
            };
            canvas2.DrawLine(points2[0], points2[1], paint2); //1
            canvas2.DrawLine(points2[0], points2[2], paint2); //2
            canvas2.DrawLine(points2[1], points2[3], paint2); //1
            canvas2.DrawLine(points2[2], points2[3], paint2); //2

            ////Rectangle
            SKCanvas canvas3 = surface.Canvas;
            float left2 = (float)(e.Info.Width * 0.75);
            float right2 = (float)(e.Info.Width);
            float bottom2 = (float)(e.Info.Height);
            float top2 = (float)(e.Info.Height * 0.2);
            SKRect rect2 = new SKRect(left2, top2, right2, bottom2);
            canvas3.Clear();

            SKPaint paint3 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.RosyBrown.ToSKColor()
            };

            canvas3.DrawRect(rect2, paint3);

            //Init Circle
            float Init_x = (float)(e.Info.Width * 0.15);
            float Init_y = (float)(e.Info.Height * 0.7);
            SKCanvas canvas5 = surface.Canvas;
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 5
            };
            canvas5.Clear();
            if (timer_stop_PN != true)
            {
                canvas5.DrawCircle(Init_x, Init_y, (float)(e.Info.Width*0.01), paint);
            }

            //Init Location
            SKCanvas canvas6 = surface.Canvas;
            canvas6.Clear();
            SKPaint paint6 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 2
            };
            if (timer_stop_PN != true)
            {
                canvas6.DrawCircle(Init_x, Init_y, (float)(e.Info.Width * 0.1), paint6);
            }
        }

        private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
        {
            Shape shape = (sender as Element).Parent as Shape;
            e.Data.Properties.Add("Square", new Square(shape.Width, shape.Height));
        }

        private void CanvasView_PaintSurface2(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();
            default_radius = (float)(e.Info.Width * 0.01);
            default_radius_move_allow = (float)(e.Info.Width * 0.07);
            default_Init_x = (float)(e.Info.Width * 0.075);
            default_Init_y = (float)(e.Info.Height * 0.7);
            //test rectangle ////Rectangle
            //test rectangle SKCanvas canvas4 = surface.Canvas;
            //test rectangle float left = (float)(e.Info.Width * 0.25);
            //test rectangle float right = (float)(e.Info.Width * 0.5);
            //test rectangle float bottom = (float)(e.Info.Height * 0.5);
            //test rectangle float top = (float)(e.Info.Height * 0.2);
            //test rectangle SKRect rect = new SKRect(left, top, right, bottom);
            //test rectangle canvas4.Clear();
            //test rectangle SKPaint paint3 = new SKPaint
            //test rectangle {
            //test rectangle     Style = SKPaintStyle.Fill,
            //test rectangle     Color = Color.RosyBrown.ToSKColor()
            //test rectangle };
            //test rectangle canvas4.DrawRect(rect, paint3);

            //x:2094, y 491
            //Test Wall
            SKCanvas canvas2 = surface.Canvas;
            SKPoint[] points2 = new SKPoint[4];
            points2[0] = new SKPoint((float)0.6 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[1] = new SKPoint((float)0.61 * e.Info.Width, (float)0.2 * e.Info.Height);
            points2[2] = new SKPoint((float)0.6 * e.Info.Width, (float)0.8 * e.Info.Height);
            points2[3] = new SKPoint((float)0.61 * e.Info.Width, (float)0.8 * e.Info.Height);
            //canvas2.Clear();

            SKPaint paint2 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = (float)(e.Info.Width * 0.015)
            };
            //canvas.DrawLine(points2[0], points2[1], paint2); //1
            canvas.DrawLine(points2[0], points2[2], paint2); //2
            //canvas.DrawLine(points2[1], points2[3], paint2); //1
            //canvas.DrawLine(points2[2], points2[3], paint2); //2

            // End Point Rectangle
            // SKCanvas canvas3 = surface.Canvas;
            // float left2 = (float)(e.Info.Width * 0.75);
            // float right2 = (float)(e.Info.Width);
            // float bottom2 = (float)(e.Info.Height);
            // float top2 = (float)(e.Info.Height * 0.2);
            // SKRect rect2 = new SKRect(left2, top2, right2, bottom2);
            // //canvas3.Clear();
            // 
            // SKPaint paint3 = new SKPaint
            // {
            //     Style = SKPaintStyle.Fill,
            //     Color = Color.RosyBrown.ToSKColor()
            // };
            // canvas3.DrawRect(rect2, paint3);


            SKCanvas canvas_test = surface.Canvas;
            SKPaint painttest = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor()
            };
            SKPaint painttest_false = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor()
            };
            for (int i = 0; i < 5; i++)
            {
                rect_point[i] = (float)(e.Info.Width * 0.75 + e.Info.Width * (0.25/4) * i);
            }

            rect_point[5] = (float)(e.Info.Height * 0.2);
            rect_point[6] = (float)(e.Info.Height * 0.4);
            rect_point[7] = (float)(e.Info.Height * 0.6);
            rect_point[8] = (float)(e.Info.Height * 0.8);
            rect_point[9] = (float)(e.Info.Height);

            for(int i = 0; i < 16; i++)
            {
                rect[i] = new SKRect(rect_point[i % 4], rect_point[i / 4 + 5],
                    rect_point[(i % 4) + 1], rect_point[i / 4 + 6]);
            }

            //rect[0] = new SKRect(rect_point[0], rect_point[5], rect_point[1], rect_point[6]);
            //rect[1] = new SKRect(rect_point[1], rect_point[5], rect_point[2], rect_point[6]);
            //rect[2] = new SKRect(rect_point[2], rect_point[5], rect_point[3], rect_point[6]);
            //rect[3] = new SKRect(rect_point[3], rect_point[5], rect_point[4], rect_point[6]);
            //
            //rect[4] = new SKRect(rect_point[0], rect_point[6], rect_point[1], rect_point[7]);
            //rect[5] = new SKRect(rect_point[1], rect_point[6], rect_point[2], rect_point[7]);
            //rect[6] = new SKRect(rect_point[2], rect_point[6], rect_point[3], rect_point[7]);
            //rect[7] = new SKRect(rect_point[3], rect_point[6], rect_point[4], rect_point[7]);
            //
            //rect[8] = new SKRect(rect_point[0], rect_point[7], rect_point[1], rect_point[8]);
            //rect[9] = new SKRect(rect_point[1], rect_point[7], rect_point[2], rect_point[8]);
            //rect[10] = new SKRect(rect_point[2], rect_point[7], rect_point[3], rect_point[8]);
            //rect[11] = new SKRect(rect_point[3], rect_point[7], rect_point[4], rect_point[8]);
            //
            //rect[12] = new SKRect(rect_point[0], rect_point[8], rect_point[1], rect_point[9]);
            //rect[13] = new SKRect(rect_point[1], rect_point[8], rect_point[2], rect_point[9]);
            //rect[14] = new SKRect(rect_point[2], rect_point[8], rect_point[3], rect_point[9]);
            //rect[15] = new SKRect(rect_point[3], rect_point[8], rect_point[4], rect_point[9]);


            for (int i = 0; i < 16; i++)
            {
                if(rect_pn[i]) 
                    canvas_test.DrawRect(rect[i], painttest);
                //else 
                //    canvas_test.DrawRect(rect[i], painttest_false);
            }
            // //Init Circle
            // float Init_x = (float)(e.Info.Width * 0.075);
            // float Init_y = (float)(e.Info.Height * 0.7);
            // SKCanvas canvas5 = surface.Canvas;
            // SKPaint paint = new SKPaint
            // {
            //     Style = SKPaintStyle.Fill,
            //     Color = Color.Red.ToSKColor(),
            //     StrokeWidth = 5
            // };
            // //canvas5.Clear();
            // if (timer_stop_PN != true)
            // {
            //     canvas5.DrawCircle(Init_x, Init_y, (float)(e.Info.Width*0.01), paint);
            // }
            // 
            // //Init Location
            // SKCanvas canvas6 = surface.Canvas;
            // //canvas6.Clear();
            // SKPaint paint6 = new SKPaint
            // {
            //     Style = SKPaintStyle.Stroke,
            //     Color = Color.Blue.ToSKColor(),
            //     StrokeWidth = 2
            // };
            // if (timer_stop_PN != true)
            // {
            //     canvas6.DrawCircle(Init_x, Init_y, (float)(e.Info.Width * 0.07), paint6);
            // }
        }

        private void CanvasView_Touch(object sender, SKTouchEventArgs e)
        {
#if false
            /*
            switch (e.ActionType)
            {
                //Init Circle Location
                case SKTouchAction.Pressed:
                    // start of a stroke
                    var p = new SKPath();
                    //p.MoveTo(e.Location);
                    p.AddCircle(e.Location.X, e.Location.Y, default_radius);
                    temporaryPaths[e.Id] = p;
                    
                    var mv = new SKPoint();
                    mv.X = e.Location.X;
                    mv.Y = e.Location.Y;
                    
                    temporaryCircle[e.Id] = mv;

                    break;
                case SKTouchAction.Moved:
                    // the stroke, while pressed
                    if (e.InContact && temporaryPaths.TryGetValue(e.Id, out var moving))
                        moving.AddCircle(e.Location.X, e.Location.Y, default_radius);
                        //moving.LineTo(e.Location);

                    if (e.InContact && temporaryCircle.TryGetValue(e.Id, out var moving2))
                    {
                        //moving.LineTo(e.Location);
                        moving2.X = e.Location.X;
                        moving2.Y = e.Location.Y;
                        
                    }
                    break;
                case SKTouchAction.Released:
                    // end of a stroke
                    if (temporaryPaths.TryGetValue(e.Id, out var releasing))
                        paths.Add(releasing);
                    
                    temporaryPaths.Remove(e.Id);

                    if (temporaryCircle.TryGetValue(e.Id,out var releasing2))
                    {
                        pathsCircle.Add(releasing2);
                    }
                    temporaryCircle.Remove(e.Id);

                    break;
                case SKTouchAction.Cancelled:
                    // we don't want that stroke
                    temporaryPaths.Remove(e.Id);

                    temporaryCircle.Remove(e.Id);
                    break;
            }
            */
#endif
            drag_onoff = false;
            switch (e.ActionType)
            {
                //1? case SKTouchAction.Pressed:
                //1?     break;
                case SKTouchAction.Moved:
                    Init_x = e.Location.X;
                    Init_y = e.Location.Y;
                    if (e.InContact)
                        ((SKCanvasView)sender).InvalidateSurface();
                    break;
                case SKTouchAction.Released:
                    timer_stop_PN = true;
                    parabolic_clear = false;
                    parabolic_cnt = 0;
                    parabolic_pre_cnt = 0;
                    wall_1_pn = false;
                    break;
            }
             // // update the UI
             // if (e.InContact)
             //     ((SKCanvasView)sender).InvalidateSurface();
             // 
            if (timer_stop_PN)
            {
                StartTimer(); // make graphics
            }

            // we have handled these events
            e.Handled = true;
            //drag_onoff = true;
        }
    }
}
