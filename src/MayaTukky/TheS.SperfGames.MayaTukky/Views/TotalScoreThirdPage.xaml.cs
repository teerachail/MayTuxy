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
    /// <summary>
    /// หน้าคำนวณเวลาของเกม state 3
    /// </summary>
    public partial class TotalScoreThirdPage : Page
    {
        #region Events

        /// <summary>
        /// การคำนวณคะแนนเสร็จสิ้น
        /// </summary>
        public static event EventHandler CalculateScoreCompleted;

        #endregion Events

        public TotalScoreThirdPage()
        {
            InitializeComponent();

            SB_Calculate.Begin();

            poisonSeal.StartPlay();
            voodooSeal.StartPlay();
            monsterSeal.StartPlay();

            poisonSeal.PlayCompleted += (s, e) =>
            {
                poisonSeal.StartPlay();
            };
            voodooSeal.PlayCompleted += (s, e) =>
            {
                voodooSeal.StartPlay();
            };
            monsterSeal.PlayCompleted += (s, e) =>
            {
                monsterSeal.StartPlay();
            };

            SB_Calculate.Completed += new EventHandler(SB_Calculate_Completed);
            SB_Fusion.Completed += new EventHandler(SB_Fusion_Completed);
            SB_CloudOut.Completed += new EventHandler(SB_CloudOut_Completed);
        }

        private void SB_CloudOut_Completed(object sender, EventArgs e)
        {
            var temp = CalculateScoreCompleted;
            if (temp != null)
            {
                temp(this,null);
            }
        }

        private void SB_Fusion_Completed(object sender, EventArgs e)
        {
            SB_CloudOut.Begin();
        }

        private void SB_Calculate_Completed(object sender, EventArgs e)
        {
            SB_Fusion.Begin();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
