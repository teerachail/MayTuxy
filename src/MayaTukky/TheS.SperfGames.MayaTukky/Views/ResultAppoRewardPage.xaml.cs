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
    public partial class ResultAppoRewardPage : Page
    {
        private const int MaximumDisplayScoreRound = 10;
        private const int DisplaySmokeEffectRound = 9;
        private const int DisplayScoreOneCircleMilisecond = 300;

        private int _currentRound;
        private int _totalScore;
        private int _scorePerRound;
        private bool _isFinished;

        private AppoTable _table;
        private CardInformation _currentCard;
        private DispatcherTimer _displayScoreTimer;

        public ResultAppoRewardPage()
        {
            InitializeComponent();
            initializeObject();
            initializeEvents();
            //calculateGameScoreRunner();
            StartShowScore();
        }

        public void StartShowScore()
        {
            _displayScoreTimer.Start();
        }

        private void initializeObject()
        {
            _totalScore = GlobalScore.FirstScore + GlobalScore.SecondScore + GlobalScore.ThirdScore;
            
            _scorePerRound= (int)(_totalScore / MaximumDisplayScoreRound);

            _displayScoreTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(DisplayScoreOneCircleMilisecond)
            };

            // TODO: กำหนดค่า Appo table ตัวอย่าง พี่บุ๊งยังไม่ได้ตัดสินใจ
            _table = new AppoTable {
                CardLevelList = new List<CardInformation> {
                    new CardInformation{
                        RequireScore = 0,
                        Rank = 1,
                        ImageUrl = "ToadCard"
                    },
                    new CardInformation{
                        RequireScore = 100,
                        Rank = 1,
                        ImageUrl = "CrowCard"
                    },
                    new CardInformation{
                        RequireScore = 200,
                        Rank = 1,
                        ImageUrl = "GhostCard"
                    },
                    new CardInformation{
                        RequireScore = 300,
                        Rank = 1,
                        ImageUrl = "WolfCard"
                    },
                    new CardInformation{
                        RequireScore = 400,
                        Rank = 2,
                        ImageUrl = "TreeCard"
                    },
                    new CardInformation{
                        RequireScore = 500,
                        Rank = 2,
                        ImageUrl = "GolemCard"
                    },
                    new CardInformation{
                        RequireScore = 600,
                        Rank = 2,
                        ImageUrl = "GobinCard"
                    },
                    new CardInformation{
                        RequireScore = 700,
                        Rank = 2,
                        ImageUrl = "TribeCard"
                    },
                    new CardInformation{
                        RequireScore = 800,
                        Rank = 3,
                        ImageUrl = "KnightCard"
                    },
                    new CardInformation{
                        RequireScore = 900,
                        Rank = 3,
                        ImageUrl = "AstrologerCard"
                    },
                    new CardInformation{
                        RequireScore = 1000,
                        Rank = 3,
                        ImageUrl = "HerbalistsCard"
                    },
                    new CardInformation{
                        RequireScore = 1100,
                        Rank = 3,
                        ImageUrl = "PriestCard"
                    },
                    new CardInformation{
                        RequireScore = 1200,
                        Rank = 4,
                        ImageUrl = "SorcererFirstCard"
                    },
                    new CardInformation{
                        RequireScore = 1300,
                        Rank = 4,
                        ImageUrl = "SorcererSecondCard"
                    },
                    new CardInformation{
                        RequireScore = 1400,
                        Rank = 4,
                        ImageUrl = "SorcererThirdCard"
                    },
                    new CardInformation{
                        RequireScore = 1500,
                        Rank = 4,
                        ImageUrl = "BerserkCard"
                    },
                    new CardInformation{
                        RequireScore = 1600,
                        Rank = 5,
                        ImageUrl = "HeimdellCard"
                    },
                    new CardInformation{
                        RequireScore = 1700,
                        Rank = 5,
                        ImageUrl = "ThorCard"
                    },
                    new CardInformation{
                        RequireScore = 1800,
                        Rank = 5,
                        ImageUrl = "OdinCard"
                    },
                },
            };

            _currentCard = _table.CardLevelList.First();
            FirstImage.Source = _currentCard.ImageSource;
            displayCardRank(_currentCard);
        }

        private void initializeEvents()
        {
            _displayScoreTimer.Tick += (s, e) => {

                if (_currentRound == DisplaySmokeEffectRound) CloundBomberStoryboard.Begin();

                AllStateScoreTextBlock.Text = (_scorePerRound * _currentRound).ToString();

                _currentRound++;

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

                // ตรวจสอบการแสดงจบ และ effect ควัน
                if (_currentRound >= MaximumDisplayScoreRound) {
                    _displayScoreTimer.Stop();
                    AllStateScoreTextBlock.Text = _totalScore.ToString();
                    _isFinished = true;
                }
            };

            #region การเลื่อนภาพเสร็จสิ้น
            
            AppoSlideStoryboard.Completed += (s, e) => {
                FirstImage.Source = _currentCard.ImageSource;
                AppoSlideStoryboard.Stop();
            };

            #endregion การเลื่อนภาพเสร็จสิ้น
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

        private void calculateGameScoreRunner()
        {
            const string ObjectName = "AllScoreDiscreteObjectKeyFrame";
            const int MaximumKeyFrame = 10;
            var totalScore = GlobalScore.FirstScore + GlobalScore.SecondScore + GlobalScore.ThirdScore;

            int score = (int)(totalScore / MaximumKeyFrame);
            for (int keyFrameValues = 1; keyFrameValues <= MaximumKeyFrame; keyFrameValues++) {
                (LayoutRoot.FindName(string.Format("{0}{1}", ObjectName, keyFrameValues)) as DiscreteObjectKeyFrame)
                    .Value = (score * keyFrameValues).ToString();
            }
            (LayoutRoot.FindName(string.Format("{0}{1}", ObjectName, MaximumKeyFrame)) as DiscreteObjectKeyFrame)
                .Value = totalScore.ToString();

            RunningScoreStoryboard.Begin();
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
    }
}
