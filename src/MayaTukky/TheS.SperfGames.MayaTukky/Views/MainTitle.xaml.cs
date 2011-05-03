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
    public partial class MainTitle : Page
    {
        public static event EventHandler NextPage;

        public MainTitle()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(TukkyCover_Loaded);
            NextButton.MouseLeftButtonDown += new MouseButtonEventHandler(NextButton_MouseLeftButtonDown);
        }

        private void NextButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventHandler temp = NextPage;
            if (temp != null)
            {
                temp(null,null);
            }
        }

        private void TukkyCover_Loaded(object sender, RoutedEventArgs e)
        {
            StartPlay();
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        //public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void StartPlay()
        {
            tukky.Play();
            cup.Play();
            door.Play();
        }

        public void StopPlay()
        {
            tukky.Stop();
            cup.Stop();
            door.Stop();
        }

        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
