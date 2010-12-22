using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PerfEx.Infrastructure;

namespace TheS.SperfGames.MayaTukky.Controls
{
    public partial class TukkyWinUI : UserControl,IAnime
    {
        public TukkyWinUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(HappyTukkyEmotion_Loaded);
            Tukky_happyStory1.Completed += new EventHandler(Tukky_happyStory1_Completed);
        }

        private void Tukky_happyStory1_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        private void HappyTukkyEmotion_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        #region IAnime Members

        public string AnimationName
        {
            get;
            set;
        }

        public bool AutoPlay
        {
            get;
            set;
        }

        public TimeSpan? BeginTime
        {
            get
            {
                return Tukky_happyStory1.BeginTime;
            }
            set
            {
                Tukky_happyStory1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Tukky_happyStory1.SpeedRatio;
            }
            set
            {
                Tukky_happyStory1.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Tukky_happyStory1.Begin();
        }

        public void StopPlay()
        {
            Tukky_happyStory1.Stop();
        }

        #endregion
    }
}
