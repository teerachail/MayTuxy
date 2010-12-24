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
    /// หน้าคำนวณเวลาของเกม state 2
    /// </summary>
    public partial class TotalScoreSecondPage : Page
    {
        #region Events

        /// <summary>
        /// การคำนวณคะแนนเสร็จสิ้น
        /// </summary>
        public static event EventHandler CalculateScoreCompleted;

        #endregion Events

        public TotalScoreSecondPage()
        {
            InitializeComponent();

            poisonSeal.StartPlay();
            voodooSeal.StartPlay();

            SB_SumScore.Begin();
            SB_SumScore.Completed += new EventHandler(SB_SumScore_Completed);

            poisonSeal.PlayCompleted += (s, e) =>
                {
                    poisonSeal.StartPlay();
                };
            voodooSeal.PlayCompleted += (s, e) =>
              {
                    voodooSeal.StartPlay();
               };
        }

        private void SB_SumScore_Completed(object sender, EventArgs e)
        {
            var temp = CalculateScoreCompleted;
            if (temp != null)
            {
                temp(this,null);
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
