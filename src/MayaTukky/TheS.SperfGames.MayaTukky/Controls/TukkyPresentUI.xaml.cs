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
    public partial class TukkyPresentUI : UserControl, IAnime
    {
        public TukkyPresentUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(TukkyPresent_Loaded);
            SB_Talk.Completed += new EventHandler(SB_Talk_Completed);

        }

        private void TukkyPresent_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        private void SB_Talk_Completed(object sender, EventArgs e)
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
                return SB_Talk.BeginTime;
            }
            set
            {
                SB_Talk.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return SB_Talk.SpeedRatio;
            }
            set
            {
                SB_Talk.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            SB_Talk.Begin();
        }

        public void StopPlay()
        {
            SB_Talk.Stop();
        }

        #endregion

        public event EventHandler Exit;
    }
}
