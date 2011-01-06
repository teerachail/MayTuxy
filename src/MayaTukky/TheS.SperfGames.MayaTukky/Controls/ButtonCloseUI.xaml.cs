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
    public partial class ButtonCloseUI : UserControl
    {
        public ButtonCloseUI()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ButtonClose_Loaded);
        }

        private void ButtonClose_Loaded(object sender, RoutedEventArgs e)
        {
            //EventHandler temp = Exit;
            //if (temp != null)
            //{
            //    temp(this,EventArgs.Empty);
            //}
        }
        //public event EventHandler Exit;
    }
}
