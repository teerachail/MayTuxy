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
    public partial class ThreeTopLoseUI : UserControl,IAnime
    {
        public ThreeTopLoseUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ThreeTopSad_Loaded);
            Sad_B1_Story1.Completed += new EventHandler(Sad_B1_Story1_Completed);
            Sad_B2_Story1.Completed += new EventHandler(Sad_B2_Story1_Completed);
            Sad_B3_Story1.Completed += new EventHandler(Sad_B3_Story1_Completed);
            Sad_B1_Story2.Completed += new EventHandler(Sad_B1_Story2_Completed);
            Sad_B2_Story2.Completed += new EventHandler(Sad_B2_Story2_Completed);
            Sad_B3_Story2.Completed += new EventHandler(Sad_B3_Story2_Completed);
        }

        private void ThreeTopSad_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }
        private void Sad_B3_Story2_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void Sad_B2_Story2_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void Sad_B1_Story2_Completed(object sender, EventArgs e)
        {
            completeAnime();
        }

        private void Sad_B3_Story1_Completed(object sender, EventArgs e)
        {
            Sad_B3_Story2.Begin();
        }

        private void Sad_B2_Story1_Completed(object sender, EventArgs e)
        {
            Sad_B2_Story2.Begin();
        }

        private void Sad_B1_Story1_Completed(object sender, EventArgs e)
        {
            Sad_B1_Story2.Begin();
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
                return Sad_B1_Story1.BeginTime;
            }
            set
            {
                Sad_B1_Story1.BeginTime = value;
                Sad_B2_Story1.BeginTime = value;
                Sad_B3_Story1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Sad_B1_Story1.SpeedRatio;
            }
            set
            {
                Sad_B1_Story1.SpeedRatio = value;
                Sad_B2_Story1.SpeedRatio = value;
                Sad_B3_Story1.SpeedRatio = value;
                Sad_B1_Story2.SpeedRatio = value;
                Sad_B2_Story2.SpeedRatio = value;
                Sad_B3_Story2.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Sad_B1_Story1.Begin();
            Sad_B2_Story1.Begin();
            Sad_B3_Story1.Begin();
        }

        public void StopPlay()
        {
            Sad_B1_Story1.Stop();
            Sad_B2_Story1.Stop();
            Sad_B3_Story1.Stop();
            Sad_B1_Story2.Stop();
            Sad_B2_Story2.Stop();
            Sad_B3_Story2.Stop();
        }

        #endregion
    }
}
