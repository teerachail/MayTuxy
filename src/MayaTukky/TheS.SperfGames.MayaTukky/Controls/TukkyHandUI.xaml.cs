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
    public partial class TukkyHandUI : UserControl, IAnime
    {
        public TukkyHandUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(TukkyHand_Loaded);
            Hand_playStory1.Completed += new EventHandler(Hand_playStory1_Completed);
            Hand_playStory1_2.Completed += new EventHandler(Hand_playStory1_2_Completed);
            Hand_playStory2.Completed += new EventHandler(Hand_playStory2_Completed);
        }

        private void TukkyHand_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        private void Hand_playStory2_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
            StartPlay();
            
        }

        private void Hand_playStory1_2_Completed(object sender, EventArgs e)
        {
            StopPlay();
            Hand_playStory2.Begin();
        }

        private void Hand_playStory1_Completed(object sender, EventArgs e)
        {
            StopPlay();
            Hand_playStory1_2.Begin();
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
                return Hand_playStory1.BeginTime;
            }
            set
            {
                Hand_playStory1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Hand_playStory1.SpeedRatio;
            }
            set
            {
                Hand_playStory1.SpeedRatio = value;
                Hand_playStory1_2.SpeedRatio = value;
                Hand_playStory2.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Hand_playStory1.Begin();
        }

        public void StopPlay()
        {
            Hand_playStory1.Stop();
            Hand_playStory1_2.Stop();
            Hand_playStory2.Stop();
        }

        #endregion
    }
}
