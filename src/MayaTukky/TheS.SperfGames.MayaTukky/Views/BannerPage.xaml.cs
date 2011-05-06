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
    public partial class BannerPage : Page
    {
        public static event EventHandler Completed;

        public BannerPage()
        {
            InitializeComponent();
            SkipButton.Click += new RoutedEventHandler(SkipButton_Click);
        }

        void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = Completed;
            if (temp != null) temp(null, null);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
