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
using System.Windows.Threading;

namespace TheS.SperfGames.MayaTukky.Views
{
    /// <summary>
    /// หน้าแสดงผลลัพท์จากการเล่นเกมทั้ง 3 ด่าน
    /// </summary>
    public partial class ResultAppoRewardPage : Page
    {
        #region Fields

        private const int DisplayScoreOneCircleMillisecond = 280;
        private const int HoldTimeMillisecond = 720;
        private const int MaximumDisplayScoreRound = 10;
        private const int DisplaySmokeEffectRound = 9;
        private const int RestartScoreRound = 1;
        private const int DisplayScorePerRoundMillisecond = 30;

        private int _currentRound;
        private int _totalScore;
        private int _scorePerRound;
        private int _miniRound;
        private int _currentDisplayScoreRound = RestartScoreRound;
        private double _scorePerMiniRound;
        private bool _isFinished;

        private ScoreTableResponse _table;
        private CardInformation _currentCard;
        private DispatcherTimer _displayCardTimer;
        private DispatcherTimer _holdingTimer;
        private DispatcherTimer _displayScoreTimer;

        private IGameService _svc;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize objects and events
        /// </summary>
        public ResultAppoRewardPage()
        {
            InitializeComponent();
            initializeObject();
            initializeEvents();
            StartShowScore();
        }

        #endregion Constructors

        #region Events

        public static event EventHandler Completed;

        #endregion Events

        #region Methods

        /// <summary>
        /// เริ่มทำการแสดงคะแนนและการ์ด
        /// </summary>
        public void StartShowScore()
        {
            _svc.RequestScoreTable(getScoreTableCallback);
        }

        // Initialize objects
        private void initializeObject()
        {
            _svc = new GameService();

            _totalScore = GlobalScore.FirstScore + GlobalScore.SecondScore + GlobalScore.ThirdScore;

            _scorePerRound = (int)(_totalScore / MaximumDisplayScoreRound);
            _scorePerMiniRound = _totalScore / MaximumDisplayScoreRound/ MaximumDisplayScoreRound;

            _displayCardTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DisplayScoreOneCircleMillisecond) };
            _holdingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(HoldTimeMillisecond) };
            _displayScoreTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DisplayScorePerRoundMillisecond) };
        }

        // Initialize events
        private void initializeEvents()
        {
            #region กดปุ่มไปต่อ

            btn_Next.MouseLeftButtonDown += (s, e) => {
                var temp = Completed;
                if (temp != null) temp(null, null);
            };

            #endregion กดปุ่มไปต่อ

            #region ถึงเวลาในการทำเล่นคะแนนวิ่ง

            _displayScoreTimer.Tick += (s, e) => {

                if (_currentDisplayScoreRound >= MaximumDisplayScoreRound) {
                    _currentDisplayScoreRound = RestartScoreRound;
                    _displayScoreTimer.Stop();
                    _holdingTimer.Start();
                }
                else {
                    _miniRound++;
                    AllStateScoreTextBlock.Text = ((int)(_miniRound * _scorePerMiniRound)).ToString();
                    _currentDisplayScoreRound++;
                }
            };

            #endregion ถึงเวลาในการทำเล่นคะแนนวิ่ง

            #region ถึงเวลาที่ต้องเปลี่ยนการ์ดใหม่

            _displayCardTimer.Tick += (s, e) => {

                // ตรวจการเล่น effect ควันที่ภาพรองสุดท้าย
                if (_currentRound == DisplaySmokeEffectRound) ;

                // แสดงคะแนนที่ได้ออกมา
                if (_isFinished == false) _displayScoreTimer.Start();

                _currentRound++;

                // แสดงภาพการ์ดที่ได้
                var nextLevelScore = _scorePerRound * _currentRound;
                var nextCard = getCardInformationByScore(nextLevelScore);
                if (nextCard != null) {
                    if (nextCard != _currentCard) {

                        var oldCard = _currentCard;
                        FirstImage.Source = oldCard.ImageSource;

                        _currentCard = nextCard;
                        SecondImage.Source = _currentCard.ImageSource;
                        AppoSlideStoryboard.Begin();

                        displayCardRank(_currentCard);
                    }
                }

                // ตรวจสอบการแสดงจบ
                if (_currentRound >= MaximumDisplayScoreRound) {
                    AllStateScoreTextBlock.Text = _totalScore.ToString();
                    _isFinished = true;
                    Sb_Next.Begin();
                }

                _displayCardTimer.Stop();
            };

            #endregion ถึงเวลาที่ต้องเปลี่ยนการ์ดใหม่

            #region หมดเวลาในการแสดงการ์ด

            _holdingTimer.Tick += (s, e) => {
                _holdingTimer.Start();
                _displayCardTimer.Start();
            };

            #endregion หมดเวลาในการแสดงการ์ด

            #region การเลื่อนการ์ดเสร็จสิ้น

            AppoSlideStoryboard.Completed += (s, e) => {
                FirstImage.Source = _currentCard.ImageSource;
                AppoSlideStoryboard.Stop();
                CardNameTextBlock.Text = _currentCard.Name;
            };

            #endregion การเลื่อนการ์ดเสร็จสิ้น
        }

        // แสดงการ์ดทีได้จากคะแนน
        private CardInformation getCardInformationByScore(int score)
        {
            CardInformation result = null;

            foreach (var card in _table.CardLevelList) {
                if (score >= card.RequireScore) result = card;
                else if (score < card.RequireScore) break;
            }

            return result;
        }

        // แสดงผล Rank ของ card
        private void displayCardRank(CardInformation card)
        {
            if (card != null) {
                switch (card.Rank) {
                    case 1: VisualStateManager.GoToState(this, OneStar.Name, true); break;
                    case 2: VisualStateManager.GoToState(this, TwoStar.Name, true); break;
                    case 3: VisualStateManager.GoToState(this, ThreeStar.Name, true); break;
                    case 4: VisualStateManager.GoToState(this, FourStar.Name, true); break;
                    case 5: VisualStateManager.GoToState(this, FiveStar.Name, true); break;
                    default: VisualStateManager.GoToState(this, NoneStar.Name, true); break;
                }
            }
        }

        // ได้รับข้อตารางลำดับคะแนนกลับไป
        private void getScoreTableCallback(ScoreTableResponse scoreTable)
        {
            if (scoreTable != null) {

                _table = scoreTable;
                _currentCard = _table.CardLevelList.FirstOrDefault();

                if (_currentCard != null)
                {
                    FirstImage.Source = _currentCard.ImageSource;
                    CardNameTextBlock.Text = _currentCard.Name;
                    displayCardRank(_currentCard);
                    _displayCardTimer.Start(); 
                }
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion Methods
    }
}