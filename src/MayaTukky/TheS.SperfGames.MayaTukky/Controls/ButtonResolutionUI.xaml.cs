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

namespace TheS.SperfGames.MayaTukky.Controls
{
    public partial class ButtonResolutionUI : UserControl
    {
        private bool open = true;
        public ButtonResolutionUI()
        {
            InitializeComponent();
            Option.MouseLeftButtonDown += new MouseButtonEventHandler(Option_MouseLeftButtonDown);
            
        }

        private void Option_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (open)
            {
                VisualStateManager.GoToState(this, "off", true);
                open = false;
            }
            else
            {
                VisualStateManager.GoToState(this, "on", true);
                open = true;
            }
        }
    }
}
