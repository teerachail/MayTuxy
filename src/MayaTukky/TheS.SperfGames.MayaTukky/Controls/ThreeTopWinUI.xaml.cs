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
    public partial class ThreeTopWinUI : UserControl,IAnime
    {
        public ThreeTopWinUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ThreeTopWin_Loaded);
            WinB3_Story1.Completed += new EventHandler(WinB3_Story1_Completed);
            WinB3_Story2.Completed += new EventHandler(WinB3_Story2_Completed);
            WinB2_Story1.Completed += new EventHandler(WinB2_Story1_Completed);
            WinB2_Story2.Completed += new EventHandler(WinB2_Story2_Completed);
            WinB1_Story1.Completed += new EventHandler(WinB1_Story1_Completed);
            WinB1_Story2.Completed += new EventHandler(WinB1_Story2_Completed);
            WinB1_Story3.Completed += new EventHandler(WinB1_Story3_Completed);
            WinB2_Story3.Completed += new EventHandler(WinB2_Story3_Completed);
            WinB3_Story3.Completed += new EventHandler(WinB3_Story3_Completed);
        }

        private void WinB3_Story3_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void WinB2_Story3_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void WinB1_Story3_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void ThreeTopWin_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }
        private void WinB3_Story2_Completed(object sender, EventArgs e)
        {
            WinB3_Story3.Begin();
        }

        private void WinB3_Story1_Completed(object sender, EventArgs e)
        {
            WinB3_Story2.Begin();
        }

        private void WinB2_Story2_Completed(object sender, EventArgs e)
        {
            WinB2_Story3.Begin();
        }

        private void WinB2_Story1_Completed(object sender, EventArgs e)
        {
            WinB2_Story2.Begin();
        }

        private void WinB1_Story2_Completed(object sender, EventArgs e)
        {
            WinB1_Story3.Begin();
        }

        private void WinB1_Story1_Completed(object sender, EventArgs e)
        {
            WinB1_Story2.Begin();
        }
        private void completeAnime()
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
                return WinB1_Story1.BeginTime;
            }
            set
            {
                WinB1_Story1.BeginTime = value;
                WinB2_Story1.BeginTime = value;
                WinB3_Story1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return WinB1_Story1.SpeedRatio;
            }
            set
            {
                WinB3_Story1.SpeedRatio = value;
                WinB3_Story2.SpeedRatio = value;
                WinB2_Story1.SpeedRatio = value;
                WinB2_Story2.SpeedRatio = value;
                WinB1_Story1.SpeedRatio = value;
                WinB1_Story2.SpeedRatio = value;
                WinB1_Story3.SpeedRatio = value;
                WinB2_Story3.SpeedRatio = value;
                WinB3_Story3.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            WinB1_Story1.Begin();
            WinB2_Story1.Begin();
            WinB3_Story1.Begin();
        }

        public void StopPlay()
        {
            WinB3_Story1.Stop();
            WinB3_Story2.Stop();
            WinB2_Story1.Stop();
            WinB2_Story2.Stop();
            WinB1_Story1.Stop();
            WinB1_Story2.Stop();
            WinB1_Story3.Stop();
            WinB2_Story3.Stop();
            WinB3_Story3.Stop();
        }

        #endregion
    }
}
