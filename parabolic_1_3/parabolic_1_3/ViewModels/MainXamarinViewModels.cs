using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parabolic_1_3.ViewModels
{
    public class MainXamarinViewModels : INotifyPropertyChanged
    {
        public float left = 0;
        public float top = 0;
        public float right = 0;
        public float bottom = 0;
        public float left2 = 0;
        public float top2 = 0;
        public float right2 = 0;
        public float bottom2 = 0;

        public int time_interval;
        public bool timer_stop_PN = true;
        public bool parabolic_clear = false;
        public int parabolic_cnt = 0;
        public int parabolic_pre_cnt = 0;
        System.Threading.Timer timer_;

        public MainXamarinViewModels()
        {
            Init_game();
        }


        //공 모양하고, Block Setting
        private void Init_game()
        {

        }

        public void EnterGame()
        {
            StartTimer(); // make graphics
        }

        private void StartTimer()
        {
            if (timer_stop_PN)
            {
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
                //CanvasView.InvalidateSurface();
                if (parabolic_cnt > 100)
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
        

        #region notifyproperty
        public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(string propertyName)
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
