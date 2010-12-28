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
            Sb_Laugh.RepeatBehavior = new RepeatBehavior(3);
            Loaded += new RoutedEventHandler(HappyTukkyEmotion_Loaded);
            Sb_Up.Completed += new EventHandler(Sb_Up_Completed);
            Sb_Down.Completed += new EventHandler(Sb_Down_Completed);
            Sb_Laugh.Completed += new EventHandler(Sb_Laugh_Completed);
        }

        private void Sb_Laugh_Completed(object sender, EventArgs e)
        {
            Sb_Down.Begin();
        }

        private void Sb_Down_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        private void Sb_Up_Completed(object sender, EventArgs e)
        {
            Sb_Laugh.Begin();
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
                return Sb_Up.BeginTime;
            }
            set
            {
                Sb_Up.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Sb_Up.SpeedRatio;
            }
            set
            {
                Sb_Up.SpeedRatio = value;
                Sb_Down.SpeedRatio = value;
                Sb_Laugh.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Sb_Up.Begin();
        }

        public void StopPlay()
        {
            Sb_Up.Stop();
            Sb_Down.Stop();
            Sb_Laugh.Stop();
        }

        #endregion
    }
}
