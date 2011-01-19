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
    public partial class TitleThirdPage : Page
    {
        private const float beginTimeMonster1 = 14;
        private const float beginTimeMonster2 = 8.7f;
        private const float beginTimeMonster3 = 12.5f;

        public TitleThirdPage()
        {
            InitializeComponent();
            Monster2.BeginTime = TimeSpan.FromSeconds(beginTimeMonster2);
            Monster3.BeginTime = TimeSpan.FromSeconds(beginTimeMonster3);
            Monster1.BeginTime = TimeSpan.FromSeconds(beginTimeMonster1);

            Monster1.StartPlay();
            Monster2.StartPlay();
            Monster3.StartPlay();

            SB3_Ex3.Begin();
            SB3_Ex3.Completed += new EventHandler(SB3_Ex3_Completed);
        }

        private void SB3_Ex3_Completed(object sender, EventArgs e)
        {
            SB3_Ex3.Stop();

            Monster1.StopPlay();
            Monster2.StopPlay();
            Monster3.StopPlay();

            SB3_Ex3.Begin();
            Monster1.StartPlay();
            Monster2.StartPlay();
            Monster3.StartPlay();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
