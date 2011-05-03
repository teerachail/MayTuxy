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
    public partial class TitleFirstPage : Page
    {
        public static event EventHandler NextPage;
        private const float beginTime = 9.3f;
        public TitleFirstPage()
        {
            InitializeComponent();
            Poison.BeginTime = TimeSpan.FromSeconds(beginTime);
            Poison.StartPlay();
            
            SB1_Ex1.Begin();
            SB1_Ex1.Completed += new EventHandler(SB1_Ex1_Completed);
            btn_NextStage.MouseLeftButtonDown += new MouseButtonEventHandler(btn_NextStage_MouseLeftButtonDown);
        }

        void btn_NextStage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var temp = NextPage;
            if (temp != null)
            {
                temp(null,null);
            }
        }

        private void SB1_Ex1_Completed(object sender, EventArgs e)
        {
            Poison.StopPlay();
            SB1_Ex1.Stop();
            SB1_Ex1.Begin();
            Poison.StartPlay();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
