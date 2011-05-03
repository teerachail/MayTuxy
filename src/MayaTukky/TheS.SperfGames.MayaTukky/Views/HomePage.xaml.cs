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

namespace TheS.SperfGames.MayaTukky.Views
{
    public partial class HomePage : Page
    {
        public static event EventHandler NextPage;

        public HomePage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Scene2_Loaded);
            bT_Single.MouseLeftButtonDown += new MouseButtonEventHandler(bT_Single_MouseLeftButtonDown);
            SB0_Intro.Completed += new EventHandler(SB0_Intro_Completed);
            SB1_Intro.Completed += new EventHandler(SB1_Intro_Completed);
        }

        private void Scene2_Loaded(object sender, RoutedEventArgs e)
        {
            SB2_Smoke_Spin1.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Smoke_Spin2.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Smoke_Spin3.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Intro_mouth.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Intro_eye.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin1.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin2.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin3.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin4.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin5.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin6.RepeatBehavior = RepeatBehavior.Forever;
            SB2_SmokeThink_Spin7.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Single_Move.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Family_Move.RepeatBehavior = RepeatBehavior.Forever;
            SB2_Online_Move.RepeatBehavior = RepeatBehavior.Forever;

            StartPlay();
            if (AutoPlay)
            {
                StartPlay();
            }
        }

        private void bT_Single_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tempt = NextPage;
            if (tempt != null)
            {
                tempt(null, null);
            }

            EventHandler temp = NextStage;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        private void SB0_Intro_Completed(object sender, EventArgs e)
        {
            SB0_Intro.Stop();
            SB1_Intro.Begin();
        }

        private void SB1_Intro_Completed(object sender, EventArgs e)
        {
            SB1_Intro.Stop();
            SB2_Intro.Begin();
            SB2_Smoke_Spin1.Begin();
            SB2_Smoke_Spin2.Begin();
            SB2_Smoke_Spin3.Begin();
            SB2_Intro_mouth.Begin();
            SB2_Intro_eye.Begin();
            SB2_SmokeThink_Spin1.Begin();
            SB2_SmokeThink_Spin2.Begin();
            SB2_SmokeThink_Spin3.Begin();
            SB2_SmokeThink_Spin4.Begin();
            SB2_SmokeThink_Spin5.Begin();
            SB2_SmokeThink_Spin6.Begin();
            SB2_SmokeThink_Spin7.Begin();
            SB2_Single_Move.Begin();
            SB2_Family_Move.Begin();
            SB2_Online_Move.Begin();

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
                return SB0_Intro.BeginTime;
            }
            set
            {
                SB0_Intro.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return SB0_Intro.SpeedRatio;
            }
            set
            {
                SB0_Intro.SpeedRatio = value;
                SB1_Intro.SpeedRatio = value;
                SB2_Intro.SpeedRatio = value;
                SB2_Smoke_Spin1.SpeedRatio = value;
                SB2_Smoke_Spin2.SpeedRatio = value;
                SB2_Smoke_Spin3.SpeedRatio = value;
                SB2_Intro_mouth.SpeedRatio = value;
                SB2_Intro_eye.SpeedRatio = value;
                SB2_SmokeThink_Spin1.SpeedRatio = value;
                SB2_SmokeThink_Spin2.SpeedRatio = value;
                SB2_SmokeThink_Spin3.SpeedRatio = value;
                SB2_SmokeThink_Spin4.SpeedRatio = value;
                SB2_SmokeThink_Spin5.SpeedRatio = value;
                SB2_SmokeThink_Spin6.SpeedRatio = value;
                SB2_SmokeThink_Spin7.SpeedRatio = value;
                SB2_Single_Move.SpeedRatio = value;
                SB2_Family_Move.SpeedRatio = value;
                SB2_Online_Move.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            SB0_Intro.Begin();
        }

        public void StopPlay()
        {
            SB1_Intro.Stop();
            SB2_Intro.Stop();
            SB2_Smoke_Spin1.Stop();
            SB2_Smoke_Spin2.Stop();
            SB2_Smoke_Spin3.Stop();
            SB2_Intro_mouth.Stop();
            SB2_Intro_eye.Stop();
            SB2_SmokeThink_Spin1.Stop();
            SB2_SmokeThink_Spin2.Stop();
            SB2_SmokeThink_Spin3.Stop();
            SB2_SmokeThink_Spin4.Stop();
            SB2_SmokeThink_Spin5.Stop();
            SB2_SmokeThink_Spin6.Stop();
            SB2_SmokeThink_Spin7.Stop();
            SB2_Single_Move.Stop();
            SB2_Family_Move.Stop();
            SB2_Online_Move.Stop();
        }

        #endregion

        public event EventHandler NextStage;

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
