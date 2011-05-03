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
    public partial class TitleSecondPage : Page
    {
        public static event EventHandler NextPage;
        private const float beginTimeVoodoo3 = 12.1f;
        private const float beginTimeVoodoo2 = 17.4f;
        private const float beginTimeVoodoo1 = 15.8f;
        public TitleSecondPage()
        {
            InitializeComponent();
            Voodoo3_1.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo3);
            Voodoo3_2.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo3);
            Voodoo2_1.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo2);
            Voodoo2_2.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo2);
            Voodoo1_1.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo1);
            Voodoo1_2.BeginTime = TimeSpan.FromSeconds(beginTimeVoodoo1);

            Voodoo3_2.StartPlay();
            Voodoo3_1.StartPlay();
            Voodoo2_2.StartPlay();
            Voodoo2_1.StartPlay();
            Voodoo1_2.StartPlay();
            Voodoo1_1.StartPlay();

            SB2_Ex2.Begin();
            SB2_Ex2.Completed += new EventHandler(SB2_Ex2_Completed);
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

        private void SB2_Ex2_Completed(object sender, EventArgs e)
        {
            SB2_Ex2.Stop();
            Voodoo3_2.StopPlay();
            Voodoo3_1.StopPlay();
            Voodoo2_2.StopPlay();
            Voodoo2_1.StopPlay();
            Voodoo1_2.StopPlay();
            Voodoo1_1.StopPlay();
            Voodoo3_2.StartPlay();
            Voodoo3_1.StartPlay();
            Voodoo2_2.StartPlay();
            Voodoo2_1.StartPlay();
            Voodoo1_2.StartPlay();
            Voodoo1_1.StartPlay();
            SB2_Ex2.Begin();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
