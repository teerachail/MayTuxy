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
using System.Windows.Navigation;
using PerfEx.Infrastructure;

namespace TheS.SperfGames.MayaTukky.Views
{
    public partial class LoadPage : Page,IAnime
    {
        double progress = 0;
        public LoadPage()
        {
            InitializeComponent(); 
            Loaded += new RoutedEventHandler(Scene1_Loaded);
            Storyboard1.Completed += new EventHandler(Storyboard1_Completed);
            SB_Timer.Completed += new EventHandler(SB_Timer_Completed);
        }

        private void SB_Timer_Completed(object sender, EventArgs e)
        {
            int gate;
            progress = progress + 1.8;
            gate = Convert.ToInt32(progress);
            txt_Number.Text = Convert.ToString(gate) + "%";
            if (progress >= 100)
            {
                progress = 0;
                txt_Number.Text = "100%";
            }
            else
            {
                SB_Timer.Begin();
            }
        }

        private void Scene1_Loaded(object sender, RoutedEventArgs e)
        {
            SB_Timer.Begin();
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        private void Storyboard1_Completed(object sender, EventArgs e)
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
                return Storyboard1.BeginTime;
            }
            set
            {
                Storyboard1.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Storyboard1.SpeedRatio;
            }
            set
            {
                Storyboard1.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Storyboard1.Begin();
        }

        public void StopPlay()
        {
            Storyboard1.Stop();
        }

        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
