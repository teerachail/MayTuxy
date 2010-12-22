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
    public partial class VoodooSealUI : UserControl,IAnime
    {
        public VoodooSealUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(VoodooSeal_Loaded);
            VoodooAnimate.Completed += new EventHandler(VoodooAnimate_Completed);
        }

        private void VoodooAnimate_Completed(object sender, EventArgs e)
        {
            EventHandler temp = PlayCompleted;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        private void VoodooSeal_Loaded(object sender, RoutedEventArgs e)
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
                return VoodooAnimate.BeginTime;
            }
            set
            {
                VoodooAnimate.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return VoodooAnimate.SpeedRatio;
            }
            set
            {
                VoodooAnimate.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            VoodooAnimate.Begin();
        }

        public void StopPlay()
        {
            VoodooAnimate.Stop();
        }

        #endregion
    }
}
