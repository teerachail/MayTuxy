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
    public partial class Home : Page
    {
        TitleGamesMayaTukky.Intro.Scene1 _pageLoad = new TitleGamesMayaTukky.Intro.Scene1();
        TitleGamesMayaTukky.Intro.Scene2 _explainGame = new TitleGamesMayaTukky.Intro.Scene2();
        TitleGamesMayaTukky.Intro.TitleGame1 _titleFirst = new TitleGamesMayaTukky.Intro.TitleGame1();
        TitleGamesMayaTukky.Intro.TitleGame2 _titleSecond = new TitleGamesMayaTukky.Intro.TitleGame2();
        TitleGamesMayaTukky.Intro.TitleGame3 _titleThird = new TitleGamesMayaTukky.Intro.TitleGame3();
        public Home()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}