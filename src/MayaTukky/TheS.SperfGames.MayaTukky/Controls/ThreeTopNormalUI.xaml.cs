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
    public partial class ThreeTopNormalUI : UserControl,IAnime
    {
        public ThreeTopNormalUI()
        {
          InitializeComponent();

            Loaded += new RoutedEventHandler(ThreeTopNormal_Loaded);
            B1_Story_1normol.Completed += new EventHandler(B1_Story_1normol_Completed);
            B2_Story_1normol.Completed += new EventHandler(B2_Story_1normol_Completed);
            B3_Story_1normol.Completed += new EventHandler(B3_Story_1normol_Completed);
        }

        private void ThreeTopNormal_Loaded(object sender, RoutedEventArgs e) 
        {
            if (AutoPlay) 
            {
                StartPlay();
            }
        }
        private void B3_Story_1normol_Completed(object sender, EventArgs e) 
        {
            completeAnime();
        }

        private void B2_Story_1normol_Completed(object sender, EventArgs e) 
        {
            completeAnime();
        }

        private void B1_Story_1normol_Completed(object sender, EventArgs e) 
        {
            completeAnime();
        }
        private void completeAnime() 
        {
            StopPlay();
            EventHandler temp = PlayCompleted;
            if (temp != null) {
                temp(this,EventArgs.Empty);
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

        public TimeSpan? BeginTime {
            get 
            {
                return B1_Story_1normol.BeginTime;
            }
            set 
            {
                B1_Story_1normol.BeginTime = value;
                B2_Story_1normol.BeginTime = value;
                B3_Story_1normol.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio 
        {
            get 
            {
                return B1_Story_1normol.SpeedRatio;
            }
            set 
            {
                B1_Story_1normol.SpeedRatio = value;
                B2_Story_1normol.SpeedRatio = value;
                B3_Story_1normol.SpeedRatio = value;
            }
        }

        public void StartPlay() 
        {
            B1_Story_1normol.Begin();
            B2_Story_1normol.Begin();
            B3_Story_1normol.Begin();
        }

        public void StopPlay() 
        {
            B1_Story_1normol.Stop();
            B2_Story_1normol.Stop();
            B3_Story_1normol.Stop();
        }

        #endregion
    
    }
}
