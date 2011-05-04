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
        #region Properties

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

        private AppoTable _table;
        private CardInformation _currentCard;
        private DispatcherTimer _displayCardTimer;
        private DispatcherTimer _holdingTimer;
        private DispatcherTimer _displayScoreTimer;

        #endregion Properties

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

        #region Methods

        /// <summary>
        /// เริ่มทำการแสดงคะแนนและการ์ด
        /// </summary>
        public void StartShowScore()
        {
            _displayCardTimer.Start();
        }

        // Initialize objects
        private void initializeObject()
        {
            _totalScore = GlobalScore.FirstScore + GlobalScore.SecondScore + GlobalScore.ThirdScore;

            _scorePerRound = (int)(_totalScore / MaximumDisplayScoreRound);
            _scorePerMiniRound = _totalScore / MaximumDisplayScoreRound/ MaximumDisplayScoreRound;

            _displayCardTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DisplayScoreOneCircleMillisecond) };
            _holdingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(HoldTimeMillisecond) };
            _displayScoreTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(DisplayScorePerRoundMillisecond) };

            // TODO: กำหนดค่า Appo table ตัวอย่าง พี่บุ๊งยังไม่ได้ตัดสินใจ
            _table = new AppoTable {
                CardLevelList = new List<CardInformation> {
                    new CardInformation{
                        RequireScore = 0,
                        Rank = 1,
                        Name = "Toad",
                        ImageUrl = "ToadCard",
                    },
                    new CardInformation{
                        RequireScore = 100,
                        Rank = 1,
                        Name = "Crow",
                        ImageUrl = "CrowCard"
                    },
                    new CardInformation{
                        RequireScore = 200,
                        Rank = 1,
                        Name = "Ghost",
                        ImageUrl = "GhostCard"
                    },
                    new CardInformation{
                        RequireScore = 300,
                        Rank = 1,
                        Name = "Wolf",
                        ImageUrl = "WolfCard"
                    },
                    new CardInformation{
                        RequireScore = 400,
                        Rank = 2,
                        Name = "Tree",
                        ImageUrl = "TreeCard"
                    },
                    new CardInformation{
                        RequireScore = 500,
                        Rank = 2,
                        Name = "Golem",
                        ImageUrl = "GolemCard"
                    },
                    new CardInformation{
                        RequireScore = 600,
                        Rank = 2,
                        Name = "Gobin",
                        ImageUrl = "GobinCard"
                    },
                    new CardInformation{
                        RequireScore = 700,
                        Rank = 2,
                        Name = "Tribe",
                        ImageUrl = "TribeCard"
                    },
                    new CardInformation{
                        RequireScore = 800,
                        Rank = 3,
                        Name = "Knight",
                        ImageUrl = "KnightCard"
                    },
                    new CardInformation{
                        RequireScore = 900,
                        Rank = 3,
                        Name = "Astrologer",
                        ImageUrl = "AstrologerCard"
                    },
                    new CardInformation{
                        RequireScore = 1000,
                        Rank = 3,
                        Name = "Herbalists",
                        ImageUrl = "HerbalistsCard"
                    },
                    new CardInformation{
                        RequireScore = 1100,
                        Rank = 3,
                        Name = "Priest",
                        ImageUrl = "PriestCard"
                    },
                    new CardInformation{
                        RequireScore = 1200,
                        Rank = 4,
                        Name = "Sorcerer first",
                        ImageUrl = "SorcererFirstCard"
                    },
                    new CardInformation{
                        RequireScore = 1300,
                        Rank = 4,
                        Name = "Sorcerer second",
                        ImageUrl = "SorcererSecondCard"
                    },
                    new CardInformation{
                        RequireScore = 1400,
                        Rank = 4,
                        Name = "Sorcerer third",
                        ImageUrl = "SorcererThirdCard"
                    },
                    new CardInformation{
                        RequireScore = 1500,
                        Rank = 4,
                        Name = "Berserk",
                        ImageUrl = "BerserkCard"
                    },
                    new CardInformation{
                        RequireScore = 1600,
                        Rank = 5,
                        Name = "Heimdell",
                        ImageUrl = "HeimdellCard"
                    },
                    new CardInformation{
                        RequireScore = 1700,
                        Rank = 5,
                        Name = "Thor",
                        ImageUrl = "ThorCard"
                    },
                    new CardInformation{
                        RequireScore = 1800,
                        Rank = 5,
                        Name = "Odin",
                        ImageUrl = "OdinCard"
                    },
                },
            };

            _currentCard = _table.CardLevelList.First();
            FirstImage.Source = _currentCard.ImageSource;
            CardNameTextBlock.Text = _currentCard.Name;
            displayCardRank(_currentCard);
        }

        // Initialize events
        private void initializeEvents()
        {
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

            #region ถึงเวลาที่ต้องเปลี่ยนการ์ดใหม่

            _displayCardTimer.Tick += (s, e) => {

                // ตรวจการเล่น effect ควันที่ภาพรองสุดท้าย
                if (_currentRound == DisplaySmokeEffectRound) CloundBomberStoryboard.Begin();

                // แสดงคะแนนที่ได้ออกมา
                if (_isFinished == false) _displayScoreTimer.Start();

                _currentRound++;

                // แสดงภาพการ์ดที่ได้
                var nextLevelScore = _scorePerRound * _currentRound;
                var nextCard = getCardInformation(nextLevelScore);
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

        public CardInformation getCardInformation(int score)
        {
            CardInformation result = null;

            foreach (var card in _table.CardLevelList) {
                if (score >= card.RequireScore) result = card;
                else if (score < card.RequireScore) break;
            }

            return result;
        }

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

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion Methods
    }
}