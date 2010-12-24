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
    /// หน้าคำนวณเวลาของเกม state 1
    /// </summary>
    public partial class TotalScoreFirstPage : Page
    {
        #region Events

        /// <summary>
        /// การคำนวณคะแนนเสร็จสิ้น
        /// </summary>
        public static event EventHandler CalculateScoreCompleted;

        #endregion Events

        public TotalScoreFirstPage()
        {
            InitializeComponent();
            poisonSeal.StartPlay();
            SB_SumScore.Begin();

            poisonSeal.PlayCompleted += new EventHandler(poisonSeal_PlayCompleted);
            SB_SumScore.Completed += new EventHandler(SB_SumScore_Completed);
        }

        private void SB_SumScore_Completed(object sender, EventArgs e)
        {
            var temp = CalculateScoreCompleted;
            if (temp != null)
            {
                temp(this,null);
            }
        }

        private void poisonSeal_PlayCompleted(object sender, EventArgs e)
        {
            poisonSeal.StartPlay();
        }


        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
