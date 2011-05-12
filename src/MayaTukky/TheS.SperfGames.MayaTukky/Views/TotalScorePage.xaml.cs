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
    /// หน้าคำนวณเวลาของเกมทั้งหมด
    /// </summary>
    public partial class TotalScorePage : Page
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

        #region Constructors

        /// <summary>
        /// Initialize total score page
        /// </summary>
        public TotalScorePage()
        {
            InitializeComponent();
            initializeObjects();
            initializeEvents();

            LayoutRoot.Children.Add(_clound);
            SB_Calculate.Begin();

            // SCalculate state 1
            const int TotalRunningScoreKeyFrame = 8;
            const string FirstScore = "PoisonDiscreteObjectKeyFrameScore";
            calculateGameScoreRunner(FirstScore, TotalRunningScoreKeyFrame, GlobalScore.FirstScore);

            // SCalculate state 2
            const string SecondScore = "VoodooDiscreteObjectKeyFrameScore";
            calculateGameScoreRunner(SecondScore, TotalRunningScoreKeyFrame, GlobalScore.SecondScore);

            // SCalculate state 3
            const string ThirdScore = "MonsterDiscreteObjectKeyFrameScore";
            calculateGameScoreRunner(ThirdScore, TotalRunningScoreKeyFrame, GlobalScore.ThirdScore);

            // SCalculate total score
            const string TotalScore = "AllDiscreteObjectKeyFrameScore";
            int totalSocre = GlobalScore.FirstScore + GlobalScore.SecondScore + GlobalScore.ThirdScore;
            calculateGameScoreRunner(TotalScore, TotalRunningScoreKeyFrame, totalSocre);
        }

        #endregion Constructors

        #region Methods

        // กำหนดค่าข้อมูลพื้นฐาน
        private void initializeObjects()
        {
            _clound = new CloudUI();
        }

        // กำหนดเหตุการณ์ต่างๆ
        private void initializeEvents()
        {
            // เมื่อแสดงคะแนนรวมเสร็จสิ้น จะทำการแสดงปุ่มไปต่อ
            TotalScoreObjectAnimation.Completed += (s, e) => Sb_Next.Begin();

            // เมื่อทำการคลิกปุ่มไปต่อ
            btn_Next.MouseLeftButtonDown += (s, e) => {
                var temp = CalculateScoreCompleted;
                if (temp != null) {
                    temp(this, null);
                }
            };
        }

        // กำหนดข้อมูลในการแสดงคะแนน
        private void calculateGameScoreRunner(string objectName, int keyFrame, int scoreValue)
        {
            int score = (int)(scoreValue / keyFrame);
            for (int keyFrameValues = 1; keyFrameValues <= keyFrame; keyFrameValues++) {
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

        #endregion Methods
    }
}
