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
        private int _count = 0;

        private Views.FirstStatePage _firstStage;
        private Views.SecondStatePage _secondStage;
        private Views.ThirdStatePage _thirdStage;
        private Views.TotalScoreFirstPage _totalFirst;
        private Views.TotalScoreSecondPage _totalSecond;
        private Views.TotalScoreThirdPage _totalThird;

        Button btn = new Button();

        public MainPage()
        {
            InitializeComponent();

            _firstStage = new Views.FirstStatePage();
            _secondStage = new Views.SecondStatePage();
            _thirdStage = new Views.ThirdStatePage();
            _totalFirst = new Views.TotalScoreFirstPage();
            _totalSecond = new Views.TotalScoreSecondPage();
            _totalThird = new Views.TotalScoreThirdPage();

            Canvas.SetLeft(btn, 1200);
            Canvas.SetTop(btn, 1000);
            Canvas.SetZIndex(btn, 1);
            btn.Content = "next";
            btn.Width = 50;
            btn.Height = 25;
            LayoutRoot.Children.Add(btn);

            btn.Click += new RoutedEventHandler(btn_Click);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            _count++;
            if (_count == 1) addPage(_totalThird, _firstStage);
            else if (_count == 2) addPage(_firstStage, _totalFirst) ;
            else if (_count == 3) addPage(_totalFirst, _secondStage);
            else if (_count == 4) addPage(_secondStage, _totalSecond);
            else if (_count == 5) addPage(_totalSecond, _thirdStage);
            else { addPage(_thirdStage, _totalThird); _count = 0; }
            
        }

        private void addPage(Page oldPage,Page newPage)
        {
            LayoutRoot.Children.Remove(oldPage);
            LayoutRoot.Children.Add(newPage);
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