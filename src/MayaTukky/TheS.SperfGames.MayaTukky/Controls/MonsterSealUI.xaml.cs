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
    public partial class MonsterSealUI : UserControl,IAnime
    {
        public MonsterSealUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MonsterSeal_Loaded);
            MonsterAnimate.Completed += new EventHandler(MonsterAnimate_Completed);
        }

        private void MonsterAnimate_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        private void MonsterSeal_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                MonsterAnimate.Begin();
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
                return MonsterAnimate.BeginTime;
            }
            set
            {
                MonsterAnimate.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return MonsterAnimate.SpeedRatio;
            }
            set
            {
                MonsterAnimate.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            MonsterAnimate.Begin();
        }

        public void StopPlay()
        {
            MonsterAnimate.Stop();
        }

        #endregion
    }
}
