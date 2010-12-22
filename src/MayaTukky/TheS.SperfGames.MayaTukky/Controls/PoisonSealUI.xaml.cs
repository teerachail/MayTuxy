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
    public partial class PoisonSealUI : UserControl,IAnime
    {
        public PoisonSealUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PoisonSeal_Loaded);
            PoisonAnimate.Completed += new EventHandler(PoisonAnimate_Completed);
        }

       private void PoisonAnimate_Completed(object sender, EventArgs e)
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        private void PoisonSeal_Loaded(object sender, RoutedEventArgs e)
        {
            if (AutoPlay)
            {
                PoisonAnimate.Begin();
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
                return PoisonAnimate.BeginTime;
            }
            set
            {
                PoisonAnimate.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return PoisonAnimate.SpeedRatio;
            }
            set
            {
                PoisonAnimate.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            PoisonAnimate.Begin();
        }

        public void StopPlay()
        {
            PoisonAnimate.Stop();
        }

        #endregion
    }
}
