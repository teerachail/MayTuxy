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
    public partial class InviteFriendsPage : Page
    {
        public static event EventHandler Close;

        public InviteFriendsPage()
        {
            InitializeComponent();
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var temp = Close;
            if (temp != null) {
                temp(null,null);
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
