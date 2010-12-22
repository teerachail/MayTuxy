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
    public partial class TukkyLoseUI : UserControl,IAnime
    {
        public TukkyLoseUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SadTukkyEmotion_Loaded);
            Tukky_SadStory1.Completed += new EventHandler(Tukky_SadStory1_Completed);
        }

        private void SadTukkyEmotion_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        private void Tukky_SadStory1_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
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
                return Tukky_SadStory1.BeginTime;
            }
            set
            {
                Tukky_SadStory1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Tukky_SadStory1.SpeedRatio;
            }
            set
            {
                Tukky_SadStory1.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Tukky_SadStory1.Begin();
        }

        public void StopPlay()
        {
            Tukky_SadStory1.Stop();
        }

        #endregion
    }
}
