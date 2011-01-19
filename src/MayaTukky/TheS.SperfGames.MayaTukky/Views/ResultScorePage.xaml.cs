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
using TheS.SperfGames.MayaTukky.Controls;

namespace TheS.SperfGames.MayaTukky.Views
{
    /// <summary>
    /// หน้าคำนวณเวลาของเกม state 1
    /// </summary>
    public partial class ResultScorePage : Page
    {
        #region Fields

        private CloudUI _clound;

        #endregion Fields

        #region Events

        /// <summary>
        /// การคำนวณคะแนนเสร็จสิ้น
        /// </summary>
        public static event EventHandler CalculateScoreCompleted;

        #endregion Events

        public ResultScorePage()
        {
            InitializeComponent();
            Sb_Next.RepeatBehavior = RepeatBehavior.Forever;
            _clound = new CloudUI();
            LayoutRoot.Children.Add(_clound);

            poisonSeal.PlayCompleted += new EventHandler(poisonSeal_PlayCompleted);
            Sb_Result.Completed += new EventHandler(Sb_Result_Completed);
            _clound.Sb_CloudOut.Completed += new EventHandler(Sb_CloudOut_Completed);
            btn_Next.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Next_MouseLeftButtonDown);

            _clound.Sb_CloudOut.Begin();

            const string FirstFinished = "firstStateResult";
            const string SecondFinished = "secondStateResult";
            const string ThirdFinished = "thirdStateResult";

            int firstScore = GlobalScore.FirstScore;
            int secondScore = GlobalScore.SecondScore;
            int thirdScore = GlobalScore.ThirdScore;

            const int KeyFrame = 3;
            const string FalseDok = "DokFalseValue";
            const string TrueDok = "DokTrueValue";

            int correctAnswerCount = 0;
            int incorrectAnswerCount = 0;
            int maximumCombo = 0;

            const int EmptyScore = 0;
            if (GlobalScore.ThirdItemsFound.Count != EmptyScore)
            {
                // ผ่านเกม State 3
                VisualStateManager.GoToState(this,ThirdFinished , false);
                incorrectAnswerCount = GlobalScore.ThirdIncorrectAnswer;
                correctAnswerCount = GlobalScore.ThirdMaximumCombo;
                maximumCombo = GlobalScore.ThirdMaximumCombo;

                foreach (var item in GlobalScore.ThirdItemsFound)
                {
                    (thirdCollection.LayoutRoot.FindName(item) as Canvas)
                        .Visibility = System.Windows.Visibility.Visible;
                }
            }
            else if (GlobalScore.SecondItemsFound.Count != EmptyScore)
            {
                // ผ่านเกม State 2
                VisualStateManager.GoToState(this,SecondFinished , false);
                incorrectAnswerCount = GlobalScore.SecondIncorrectAnswer;
                correctAnswerCount = GlobalScore.SecondMaximumCombo;
                maximumCombo = GlobalScore.SecondMaximumCombo;

                foreach (var item in GlobalScore.SecondItemsFound)
                {
                    (secondCollection.LayoutRoot.FindName(item) as Canvas)
                        .Visibility = System.Windows.Visibility.Visible;
                }
            }
            else
            {
                // ผ่านเกม State 1
                VisualStateManager.GoToState(this, FirstFinished, false);
                incorrectAnswerCount = GlobalScore.FirstIncorrectAnswer;
                correctAnswerCount = GlobalScore.FirstMaximumCombo;
                maximumCombo = GlobalScore.FirstMaximumCombo;

                foreach (var item in GlobalScore.FirstItemsFound)
                {
                    (firstCollection.LayoutRoot.FindName(item) as Canvas)
                        .Visibility = System.Windows.Visibility.Visible;
                }
            }

            calculateGameScoreRunner(FalseDok, KeyFrame, incorrectAnswerCount);
            calculateGameScoreRunner(TrueDok, KeyFrame, correctAnswerCount);
            txt_Combo.Text = maximumCombo.ToString();
        }

        private void btn_Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Sb_Next.Stop();
            Sb_Result.Stop();
            var temp = CalculateScoreCompleted;
            if (temp != null)
            {
                temp(this, null);
            }
        }

        private void Sb_CloudOut_Completed(object sender, EventArgs e)
        {
            poisonSeal.StartPlay();
            Sb_Result.Begin();
        }

        private void Sb_Result_Completed(object sender, EventArgs e)
        {
            Sb_Next.Begin();
        }

        private void poisonSeal_PlayCompleted(object sender, EventArgs e)
        {
            poisonSeal.StartPlay();
        }

        private void calculateGameScoreRunner(string objectName, int keyFrame, int scoreValue)
        {
            int score = (int)(scoreValue / keyFrame);
            for (int keyFrameValues = 1; keyFrameValues <= keyFrame; keyFrameValues++)
            {
                (LayoutRoot.FindName(string.Format("{0}{1}", objectName, keyFrameValues)) as DiscreteObjectKeyFrame)
                    .Value = (score * keyFrameValues).ToString();
            }
            (LayoutRoot.FindName(string.Format("{0}{1}", objectName, keyFrame)) as DiscreteObjectKeyFrame)
                .Value = scoreValue.ToString();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
