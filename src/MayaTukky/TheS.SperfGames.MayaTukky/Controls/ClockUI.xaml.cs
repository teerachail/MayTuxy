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
    public partial class ClockUI : UserControl
    {
        public ClockUI()
        {
            InitializeComponent();
            clockFive.Sb_SandFall.Completed += new EventHandler(Sb_SandFall_Completed);
            clockOne.Sb_Light.Completed += new EventHandler(Sb_Light_Completed);
        }

        private void Sb_Light_Completed(object sender, EventArgs e)
        {
            stopPlay();
            var temp = LastCombo;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        private void Sb_SandFall_Completed(object sender, EventArgs e)
        {
            PlayLight();
        }
        public void PlayClockOne()
        {
            clockOne.Sb_SandFall.Begin();
        }
        public void PlayClockTwo()
        {
            clockTwo.Sb_SandFall.Begin();
        }
        public void PlayClockThree()
        {
            clockThree.Sb_SandFall.Begin();
        }
        public void PlayClockFour()
        {
            clockFour.Sb_SandFall.Begin();
        }
        public void PlayClockFive()
        {
            clockFive.Sb_SandFall.Begin();
        }
        public void PlayLight()
        {
            clockOne.Sb_Light.Begin();
            clockTwo.Sb_Light.Begin();
            clockThree.Sb_Light.Begin();
            clockFour.Sb_Light.Begin();
            clockFive.Sb_Light.Begin();
        }
        private void stopPlay()
        {
            clockOne.Sb_SandFall.Stop();
            clockTwo.Sb_SandFall.Stop();
            clockThree.Sb_SandFall.Stop();
            clockFour.Sb_SandFall.Stop();
            clockFive.Sb_SandFall.Stop();

            clockOne.Sb_Light.Stop();
            clockTwo.Sb_Light.Stop();
            clockThree.Sb_Light.Stop();
            clockFour.Sb_Light.Stop();
            clockFive.Sb_Light.Stop();
        }

        public event EventHandler LastCombo;
    }
}
