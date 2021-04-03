using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace parabolic_1_3.ViewModels
{
    public class BindingPage : INotifyPropertyChanged
    {
        //Binding Text
        int game_power = 0;
        int game_level = 0;
        int game_score = 0;
        int game_rad = 0;
        public int Game_power { get => game_power; set { game_power = value;  NotifyPropertyChanged("Game_power"); } }
        public int Game_level { get => game_level; set { game_level = value; NotifyPropertyChanged("Game_level"); } }
        public int Game_score { get => game_score; set { game_score = value; NotifyPropertyChanged("Game_score"); } }
        public int Game_rad { get => game_rad; set { game_rad = value; NotifyPropertyChanged("Game_rad"); } }


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
