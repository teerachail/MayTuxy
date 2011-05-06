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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheS.SperfGames.MayaTukky
{
    public partial class MainPage : UserControl
    {
        private int _gamePageIndex;
        private string[] _pages;

        public MainPage()
        {
            InitializeComponent();

            _pages = new string[]{
                "/HomePage",
                "/MainTitlePage",
                "/TitleFirstPage",
                "/FirstStatePage",
                "/ResultScorePage",
                "/TitleSecondPage",
                "/SecondStatePage",
                "/ResultScorePage",
                "/TitleThirdPage",
                "/ThirdStatePage",
                "/ResultScorePage",
                "/TotalScorePage",
                "/ResultAppoRewardPage",
                "/BannerPage"
            };

            Views.LoadPage.NextPage += new EventHandler(NavigationPage);

            Views.HomePage.NextPage += new EventHandler(NavigationPage);
            Views.HomePage.InvitePage += new EventHandler(HomePage_InvitePage);
            Views.HomePage.TrophiesPage += new EventHandler(HomePage_TrophiesPage);
            Views.TrophiesPage.Close += new EventHandler(TrophiesPage_Close);
            Views.InviteFriendsPage.Close += new EventHandler(TrophiesPage_Close);

            Views.MainTitlePage.NextPage += new EventHandler(NavigationPage);


            Views.TitleFirstPage.NextPage += new EventHandler(NavigationPage);
            Views.TitleSecondPage.NextPage += new EventHandler(NavigationPage);
            Views.TitleThirdPage.NextPage += new EventHandler(NavigationPage);
            Views.FirstStatePage.GameFinish += new EventHandler(NavigationPage);
            Views.SecondStatePage.GameFinish += new EventHandler(NavigationPage);
            Views.ThirdStatePage.GameFinish += new EventHandler(NavigationPage);
            Views.ResultScorePage.CalculateScoreCompleted +=new EventHandler(NavigationPage);
            Views.TotalScorePage.CalculateScoreCompleted += new EventHandler(NavigationPage);

            Views.ResultAppoRewardPage.Completed += new EventHandler(NavigationPage);
            Views.BannerPage.Completed += new EventHandler(NavigationPage);
        }

        void TrophiesPage_Close(object sender, EventArgs e)
        {
            ContentFrame.Navigate(new Uri("/HomePage", UriKind.Relative));
        }

        void HomePage_TrophiesPage(object sender, EventArgs e)
        {
            ContentFrame.Navigate(new Uri("/TrophiesPage", UriKind.Relative));
        }

        void HomePage_InvitePage(object sender, EventArgs e)
        {
            ContentFrame.Navigate(new Uri("/InviteFriendsPage", UriKind.Relative));
        }

        private void NavigationPage(object sender, EventArgs e)
        {
            if (_gamePageIndex < _pages.Count()) ContentFrame.Navigate(new Uri(_pages[_gamePageIndex++], UriKind.Relative));
            else {
                const int ResetPage = 0;
                _gamePageIndex = ResetPage;
            }
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ChildWindow errorWin = new ErrorWindow(e.Uri);
            errorWin.Show();
        }
    }
}