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
        public float _degrees = 0;
        public float _degrees_reverse = 0;

        public bool drag_onoff = true;
        public bool game_over = false;
        public bool ball_drawing = false;

        public int time_interval;
        public bool timer_stop_PN = false;
        public bool timer_stop_PN_BlockRotation = false;
        public bool parabolic_clear = false;
        public int parabolic_cnt = 0;
        public int parabolic_pre_cnt = 0;
        public int BlockRotationCnt = 0;

        System.Threading.Timer timer_;
        System.Threading.Timer timer_BlockRotation;

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

        //Text Binding
        BindingPage BindingText = new BindingPage();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new BindingPage();
            BindingText = (BindingPage)BindingContext;

            BindingText.Game_level = 1;
            BindingText.Game_power = 0;
            BindingText.Game_score = 0;
            BindingText.Game_rad = 0;

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
                //base.OnAppearing();
                //AnimationLoop(); //Test Smooth Animation
                timer_start(MY_TIMER_TICK_OBJECT, 0, time_interval);
            }
        }

        private void StartTimer_BlockRotation()
        {
            if (timer_stop_PN)
            {
                timer_stop_PN_BlockRotation = true;
                //timer_start_BlockRotation(MY_TIMER_TICK_OBJECT_BlockRotation, 0, time_interval);
            }
        }

        private void timer_start_BlockRotation(TimerCallback callback, int start, int time_interval)
        {
            timer_BlockRotation = new System.Threading.Timer(callback, null, start, time_interval);
        }

        async Task AnimationLoop()
        {
            parabolic_cnt++;
            CanvasView.InvalidateSurface();
            TimeSpan.FromSeconds(1.0 / 30);
        }

        private async void MY_TIMER_TICK_OBJECT(object state)
        {
            //await Task.Run(() =>
            //{
            //    parabolic_cnt++;
            //});
            await Task.Run(() =>
            {
                parabolic_cnt++;
                CanvasView.InvalidateSurface(); 
                if(timer_stop_PN_BlockRotation)
                {
                    BlockRotationCnt++;
                    CanvasView2.InvalidateSurface();
                }
                //CanvasView2.InvalidateSurface();
            });
        }

        private async void MY_TIMER_TICK_OBJECT_BlockRotation(object state)
        {
            await Task.Run(() =>
            {
                //CanvasView2.InvalidateSurface();
            });
        }

        private void timer_start(TimerCallback callback, int start, int time_interval)
        {
            timer_ = new System.Threading.Timer(callback, null, start, time_interval);
        }

        private void timer_stop()
        {
            timer_.Dispose();
            //if (timer_stop_PN_BlockRotation)
            //{
            //    timer_BlockRotation.Dispose();
            //}
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

            if (game_over)
            {
                canvas.Clear();
                //Gameover_textdraw(e);
            }
            else
            {
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

                //Notify Power
                BindingText = (BindingPage)BindingContext;
                BindingText.Game_power = (int)Vo_test;
                BindingText.Game_rad = (int)(control_rcos_abs * 100);

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
                        BlockRotationCnt = 0;
                        game_over = true;
                        if (timer_stop_PN)
                        {
                            timer_stop();
                            timer_stop_PN = false;
                            timer_stop_PN_BlockRotation = false;
                            drag_onoff = true; //Initial x,y 
                            Init_x = 0;
                            Init_y = 0;

                            //Init rect_pn
                            for (int j = 0; j < 16; j++)
                            {
                                //rect_pn = false -> block destory
                                if (!rect_pn[j]) game_over = false;
                                rect_pn[j] = true;
                            }
                        }

                        BindingText.Game_score = 0;
                        BindingText.Game_level = 1;
                        BindingText.Game_power = 0;
                        BindingText.Game_rad = 0;

                        if (game_over)
                        {
                            canvas.Clear();
                            Gameover_textdraw(e);
                            CanvasView2.InvalidateSurface();
                        }
                        else
                        {
                            CanvasView.InvalidateSurface();
                            CanvasView2.InvalidateSurface();
                        }
                        break;
                    }

                    // End Point Switch On
                    // First 1~16 check
                    // 0  1  2  3  4
                    // |  |  |  |  |
                    // |  |  |  |  |
                    // |  |  |  |  |
                    // |  |  |  |  |
                    if (x > (float)(e.Info.Width * 0.75))
                    {
                        test_block_destroy(info, x, y);
                    }

                    //공 위치 갱신 Draw
                    canvas.DrawCircle(x, y, (float)0.01 * e.Info.Width, paint);
                }
            }
        }

        private void test_block_destroy(SKImageInfo info, float x, float y)
        {
            //ok?
            for (int row = 0; row < 4; row++)
            {
                if (rect_pn[row] == true && rect_point[row] < x && rect_point[row + 1] > x)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        //col 0~1, 1~2, 2~3, 3~4
                        if (rect_pn[4 * col + row] == true && rect_point[col + 5] < y && rect_point[col + 6] > y)
                        {
                            rect_pn[4 * col + row] = false;
                            BindingText.Game_score++;

                            //if change, redraw
                            CanvasView2.InvalidateSurface();
                            
                        }
                    }

                }
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //CanvasView.InvalidateSurface();
        }

        private void CanvasView_PaintSurface2(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.
            if (game_over)
            {
                canvas.Clear();
                Gameover_textdraw(e);
                return;
            }
            else
            {
                canvas.Clear();
                default_radius = (float)(e.Info.Width * 0.01);
                default_radius_move_allow = (float)(e.Info.Width * 0.07);
                default_Init_x = (float)(e.Info.Width * 0.075);
                default_Init_y = (float)(e.Info.Height * 0.7);

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

                canvas.DrawLine(points2[0], points2[2], paint2);

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

                for (int i = 0; i < 16; i++)
                {
                    if (rect_pn[i])
                        canvas_test.DrawRect(rect[i], painttest);
                    else //Rotation Block or Delete Block
                    {
                         //Thread?
                         StartTimer_BlockRotation();
                         
                         float rotation_x = (rect_point[i % 4] + rect_point[(i % 4) + 1]) / 2;
                         float rotation_y = (rect_point[i / 4 + 5] + rect_point[i / 4 + 6]) / 2;
                         //float rotation_x = (rect_point[0] + rect_point[1]) / 2;
                         //float rotation_y = (rect_point[5] + rect_point[6]) / 2;
                         //float rotation_x = (info.Width) / 2;
                         //float rotation_y = (info.Height) / 2;
                         
                         OnDrawRoation(rotation_x, rotation_y, e, painttest);
                         
                    }
                }
            }
        }

        private void OnDrawRoation(float rotation_x, float rotation_y, SKPaintSurfaceEventArgs e, SKPaint painttest)
        {
            IncrementDegrees();
            IncrementDegrees_reverse();
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            SKCanvas canvas_Reverse = surface.Canvas;
            canvas_Reverse.Save();
            canvas.Save();

            //Block Color Changed
            SKPaint painttest_false = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,

                Color = Color.Red.ToSKColor()
            };
            //Block Color Changed
            SKPaint painttest_false2 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,

                Color = Color.Green.ToSKColor()
            };
            //Block Color Changed
            SKPaint painttest_false3 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,

                Color = Color.Yellow.ToSKColor()
            };
            //Block Color Changed
            SKPaint painttest_false4 = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,

                Color = Color.Purple.ToSKColor()
            };

            float _squareWidth = (float)(info.Width * 0.25) / 5;

            float WidthMove = rotation_x - (float)((info.Width * 0.2) / 200) * BlockRotationCnt;
            float WidthMove_Reverse = rotation_x + (float)((info.Width * 0.2) / 200) * BlockRotationCnt;

            float HeightMove = rotation_y + (float)((info.Height * 0.8) / 200) * BlockRotationCnt;

            float Vo_Rotation = 300;
            float Rad_const = 0.7071f; // sqrt(2)/2;
            float t = BlockRotationCnt / 80.0f;
            float g = 5 * t;
            float x = rotation_x - (Vo_Rotation * Rad_const) * t / 2;
            float x_Reverse = rotation_x + (Vo_Rotation * Rad_const) * t / 2;
            float x_up = rotation_x - (Vo_Rotation * Rad_const) * t / 20;

            float[] x2 = new float[8];
            float[] x2_reverse = new float[8];

            float[] y2 = new float[4];
            for (int i = 0; i < 8; i++)
            {
                x2[i] = rotation_x - (Vo_Rotation * Rad_const) * t / (1.7f + 0.1f * i);
            }
            for(int i = 0; i < 8; i++)
            {
                x2_reverse[i] = rotation_x + (Vo_Rotation * Rad_const) * t / (1.7f + 0.1f * i);
            }
            //x2[0] = rotation_x - (Vo_Rotation * Rad_const) * t / 2.0f;
            //x2[1] = rotation_x - (Vo_Rotation * Rad_const) * t / 2.5f;
            //
            //x2[2] = rotation_x + (Vo_Rotation * Rad_const) * t / 2.0f;
            //x2[3] = rotation_x + (Vo_Rotation * Rad_const) * t / 2.5f;

            float y = (Vo_Rotation * Rad_const) * t - g * t * 14;

            y2[0] = (Vo_Rotation * Rad_const) * t - g * t * 14f;
            y2[1] = (Vo_Rotation * Rad_const) * t - g * t * 12f;

            y2[2] = (Vo_Rotation * Rad_const) * t - g * t * 14.5f;
            y2[3] = (Vo_Rotation * Rad_const) * t - g * t * 13f;



            y = rotation_y - y;
            for(int i = 0; i < 4; i++)
              y2[i] = rotation_y - y2[i];
            //if (x > info.Width || x < 0 || y > info.Height || y < 0)
            //{
            //    return;
            //}

            // //rotation_x -= WidthMove;
            // //rotation_y += HeightMove;
            // 
            // //canvas.RotateDegrees(_degrees, rotation_x, rotation_y );
            // //canvas.RotateDegrees(_degrees, WidthMove, HeightMove);
            // SKPath path = new SKPath();
            // //path.MoveTo(rotation_x, rotation_y);
            // //path.LineTo(rotation_x - WidthMove, rotation_y);
            // //path.LineTo(rotation_x - WidthMove, rotation_y + HeightMove);
            // //path.LineTo(rotation_x , rotation_y + HeightMove);
            // //path.LineTo(rotation_x, rotation_y);
            // 
            // //path.MoveTo(rotation_x, rotation_y);
            // //path.LineTo(rotation_x + _squareWidth, rotation_y);
            // //path.LineTo(rotation_x + _squareWidth, rotation_y - _squareWidth);
            // //path.LineTo(rotation_x, rotation_y - _squareWidth);
            // //path.LineTo(rotation_x, rotation_y);
            // 
            // //path.MoveTo(WidthMove, HeightMove);
            // //path.LineTo(WidthMove + rotation_x, HeightMove);
            // //path.LineTo(WidthMove + rotation_x, HeightMove - rotation_x);
            // //path.LineTo(WidthMove, HeightMove - rotation_x);
            // //path.LineTo(WidthMove, HeightMove);
            // 
            // //path.MoveTo(WidthMove, HeightMove); //A1
            // //path.LineTo(WidthMove + _squareWidth, HeightMove); // A2
            // //path.LineTo(WidthMove + _squareWidth, HeightMove - _squareWidth); //A3
            // //path.LineTo(WidthMove, HeightMove - _squareWidth); //A4
            // //path.LineTo(WidthMove, HeightMove); //A5 == A1
            // 
            // //canvas.DrawPath(path, painttest_false);
            // 
            // //path.MoveTo(WidthMove, HeightMove); //A1
            // //path.LineTo(WidthMove + _squareWidth, HeightMove); // A2
            // //path.LineTo(WidthMove + _squareWidth, HeightMove - _squareWidth); //A3
            // //path.LineTo(WidthMove, HeightMove); //A5 == A1
            // //
            // //canvas.DrawPath(path, painttest_false);
            // //canvas.Restore();
            // //
            // //canvas_Reverse.RotateDegrees(_degrees_reverse, WidthMove, HeightMove);
            // //SKPath path_reverse = new SKPath();
            // //path_reverse.MoveTo(WidthMove, HeightMove); //A1
            // //path_reverse.LineTo(WidthMove + _squareWidth, HeightMove - _squareWidth); //A3
            // //path_reverse.LineTo(WidthMove, HeightMove - _squareWidth); //A4
            // //path_reverse.LineTo(WidthMove, HeightMove); //A5 == A1
            // //canvas_Reverse.DrawPath(path_reverse, painttest_false2);
            // //canvas_Reverse.Restore();
            // 
            // //canvas.RotateDegrees(_degrees, x, y);
            // //canvas.RotateDegrees(_degrees, x, y);
            // path.MoveTo(x, y); //A1
            // path.LineTo(x + _squareWidth, y); // A2
            // path.LineTo(x + _squareWidth, y - _squareWidth); //A3
            // path.LineTo(x, y); //A5 == A1
            // 
            // canvas.DrawPath(path, painttest_false);
            // canvas.Restore();
            // 
            // //canvas_Reverse.RotateDegrees(_degrees_reverse, x_Reverse, y);
            // //canvas_Reverse.RotateDegrees(_degrees_reverse, x_Reverse, y);
            // SKPath path_reverse = new SKPath();
            // path_reverse.MoveTo(x_Reverse, y); //A1
            // path_reverse.LineTo(x_Reverse + _squareWidth, y - _squareWidth); //A3
            // path_reverse.LineTo(x_Reverse, y - _squareWidth); //A4
            // path_reverse.LineTo(x_Reverse, y); //A5 == A1
            // canvas_Reverse.DrawPath(path_reverse, painttest_false2);
            // canvas_Reverse.Restore();


            SKCanvas[] canvas_divide = new SKCanvas[8];
            //SKPath[] path_divide = new SKPath[16];
            for (int i = 0; i < 8; i++)
            {
                canvas_divide[i] = surface.Canvas;
                canvas_divide[i].Save();
                SKPath path_divide = new SKPath();
                //x += _squareWidth / 4 * (i % 4);
                //y -= _squareWidth / 4 * (i / 4 + 1);
                path_divide.MoveTo(x2[i], y2[i/2]);
                path_divide.LineTo(x2[i] + _squareWidth / 4, y2[i / 2]);
                path_divide.LineTo(x2[i] + _squareWidth / 4 , y2[i / 2] - _squareWidth / 4);
                path_divide.LineTo(x2[i], y2[i / 2] - _squareWidth / 4);
                path_divide.LineTo(x2[i], y2[i / 2]);
                
                if(i > 4)
                    canvas_divide[i].DrawPath(path_divide, painttest_false);
                else
                    canvas_divide[i].DrawPath(path_divide, painttest_false2);

                canvas_divide[i].Restore();
            }

            SKCanvas[] canvas_divide_reverse = new SKCanvas[8];
            for (int i = 0; i < 8; i++)
            {
                canvas_divide_reverse[i] = surface.Canvas;
                canvas_divide_reverse[i].Save();
                SKPath path_divide = new SKPath();
                //x += _squareWidth / 4 * (i % 4);
                //y -= _squareWidth / 4 * (i / 4 + 1);
                path_divide.MoveTo(x2_reverse[i], y2[i / 2]);
                path_divide.LineTo(x2_reverse[i] + _squareWidth / 4, y2[i / 2]);
                path_divide.LineTo(x2_reverse[i] + _squareWidth / 4, y2[i / 2] - _squareWidth / 4);
                path_divide.LineTo(x2_reverse[i], y2[i / 2] - _squareWidth / 4);
                path_divide.LineTo(x2_reverse[i], y2[i / 2]);
                if(i > 4)
                    canvas_divide_reverse[i].DrawPath(path_divide, painttest_false3);
                else
                    canvas_divide_reverse[i].DrawPath(path_divide, painttest_false4);
                canvas_divide_reverse[i].Restore();
            }
        }

        private void IncrementDegrees_reverse()
        {
            _degrees_reverse = parabolic_cnt % 360;
            if (_degrees_reverse >= 360)
            {
                _degrees_reverse = 0;
            }
            _degrees_reverse *= -1;
        }

        private void IncrementDegrees()
        {
            //_degrees += 3.5f;
            _degrees = parabolic_cnt % 360;
            if (_degrees >= 360)
            {
                _degrees = 0;
            }
        }

        private void CanvasView_Gameover(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            canvas.Clear();
            
            SKPaint paintend = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor()
            };

            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                FakeBoldText = true,
                StrokeWidth = 1
            };

            string game_over_text = "END";

            //Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(game_over_text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            //Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(game_over_text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;

            //And draw the text
            canvas.DrawText(game_over_text, xText, yText, textPaint);
        }

        private void Gameover_textdraw(SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info; //그리기 화면에 대한 정보 (너비, 높이 픽셀)
            SKSurface surface = e.Surface; // 그리기 화면 자체
            SKCanvas canvas = surface.Canvas; //그래픽그리기 컨텍스트
            //개체는 그래픽 SKCanvan 변환과 클리핑을 포함 하는 그래픽 상태를 캡슐화 합니다.

            //canvas.Clear();
            
            SKPaint paintend = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor()
            };

            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                FakeBoldText = true,
                StrokeWidth = 1
            };

            //canvas.Save();
            string game_over_text = "END";

            //Adjust TextSize property so text is 95% of screen width
            float textWidth = textPaint.MeasureText(game_over_text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;

            //Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(game_over_text, ref textBounds);

            // Calculate offsets to center the text on the screen
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;

            //And draw the text
            canvas.DrawText(game_over_text, xText, yText, textPaint);

            //canvas.Restore();
            gamecontinue_btn.IsVisible = true;
        }

        private void BTN_game_continue(object sender, EventArgs e)
        {
            parabolic_cnt = 0;
            game_over = false;

            timer_stop_PN = false;
            drag_onoff = true; //Initial x,y 
            Init_x = 0;
            Init_y = 0;
           
            //Init rect_pn
            for (int j = 0; j < 16; j++)
            {
                rect_pn[j] = true;
            }

            BindingText.Game_score = 0;
            BindingText.Game_level = 1;
            BindingText.Game_power = 0;
            BindingText.Game_rad = 0;
            gamecontinue_btn.IsVisible = false;
            CanvasView.InvalidateSurface();
            CanvasView2.InvalidateSurface();
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
            if (!game_over)
            {
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
                        ball_drawing = true;
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
}
