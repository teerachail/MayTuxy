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
    public partial class GhostDoorUI : UserControl
    {
        public GhostDoorUI()
        {
            InitializeComponent();
            Door_story1.Completed += new EventHandler(Door_story1_Completed);
            Door_story2.Completed += new EventHandler(Door_story2_Completed);
            Door_story3.Completed += new EventHandler(Door_story3_Completed);
        }
        public void Play()
        {
            Door_story1.Begin();
        }

        public void Stop()
        {
            Door_story1.Stop();
            Door_story2.Stop();
            Door_story3.Stop();
        }

        private void Door_story3_Completed(object sender, EventArgs e)
        {
            Stop();
            Door_story1.Begin();
        }

        private void Door_story2_Completed(object sender, EventArgs e)
        {
            Stop();
            Door_story3.Begin();
        }

        private void Door_story1_Completed(object sender, EventArgs e)
        {
            Stop();
            Door_story2.Begin();
        }
    }
}
