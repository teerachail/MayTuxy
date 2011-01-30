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
    public partial class TukkyCoverUI : UserControl
    {
        public TukkyCoverUI()
        {
            InitializeComponent();

            Tukky_Story1.Completed += new EventHandler(Tukky_Story1_Completed);
            Tukky_Story2.Completed += new EventHandler(Tukky_Story2_Completed);
            Tukky_Story3.Completed += new EventHandler(Tukky_Story3_Completed);
            Tukky_Story4.Completed += new EventHandler(Tukky_Story4_Completed);
        }

        public void Play()
        {
            Tukky_Story1.Begin();
        }

        public void Stop()
        {
            Tukky_Story1.Stop();
            Tukky_Story2.Stop();
            Tukky_Story3.Stop();
            Tukky_Story4.Stop();
        }

        private void Tukky_Story4_Completed(object sender, EventArgs e)
        {
            Tukky_Story4.Begin();
        }

        private void Tukky_Story3_Completed(object sender, EventArgs e)
        {
            Tukky_Story3.Begin();
        }

        private void Tukky_Story2_Completed(object sender, EventArgs e)
        {
            Tukky_Story2.Begin();
        }

        private void Tukky_Story1_Completed(object sender, EventArgs e)
        {
            Tukky_Story1.Begin();
        }
    }
}
