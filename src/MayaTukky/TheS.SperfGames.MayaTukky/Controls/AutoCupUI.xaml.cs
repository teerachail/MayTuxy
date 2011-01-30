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
    public partial class AutoCupUI : UserControl
    {
        public AutoCupUI()
        {
            InitializeComponent();

            Cup_story1.Completed += new EventHandler(Cup_story1_Completed);
            Cup_story2.Completed += new EventHandler(Cup_story2_Completed);
            Cup_story3.Completed += new EventHandler(Cup_story3_Completed);
            Cup_story4.Completed += new EventHandler(Cup_story4_Completed);
            Cup_story5.Completed += new EventHandler(Cup_story5_Completed);
            Cup_story6.Completed += new EventHandler(Cup_story6_Completed);
            Cup_story7.Completed += new EventHandler(Cup_story7_Completed);
            Cup_story8.Completed += new EventHandler(Cup_story8_Completed);
        }
        public void Play()
        {
            Cup_story1.Begin();
        }

        public void Stop()
        {
            Cup_story1.Stop();
            Cup_story2.Stop();
            Cup_story3.Stop();
            Cup_story4.Stop();
            Cup_story5.Stop();
            Cup_story6.Stop();
            Cup_story7.Stop();
            Cup_story8.Stop();
        }
        private void Cup_story8_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story1.Begin();
        }

        private void Cup_story7_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story8.Begin();
        }

        private void Cup_story6_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story7.Begin();
        }

        private void Cup_story5_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story6.Begin();
        }

        private void Cup_story4_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story5.Begin();
        }

        private void Cup_story3_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story4.Begin();
        }

        private void Cup_story2_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story3.Begin();
        }

        private void Cup_story1_Completed(object sender, EventArgs e)
        {
            Stop();
            Cup_story2.Begin();
        }
    }
}
