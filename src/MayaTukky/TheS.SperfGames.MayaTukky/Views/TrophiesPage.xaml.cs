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
    public partial class TrophiesPage : Page
    {
        #region Fields
        
        private string _username;
        private bool _callbackCompleted;
        private IGameService _svc;
        private ScoreTableResponse _table;
        private PlayerStatistics _statistics;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize objects
        /// </summary>
        public TrophiesPage()
        {
            InitializeComponent();
            _svc = new GameService();
            InitializeInformation("");
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initialize player information
        /// </summary>
        /// <param name="username">username</param>
        public void InitializeInformation(string username)
        {
            _svc.RequestScoreTable(getScoreTableCallback);
            _svc.RequestStatistics(username, getStatisticsCallback);
        }

        // ได้รับข้อมูลสถิติการเล่นกลับมา
        private void getStatisticsCallback(PlayerStatistics statistics)
        {
            if (statistics != null) {
                _statistics = statistics;

                FirstAveragePointTextBlock.Text = _statistics.FirstAveragePoint.ToString();
                FirstHighScorePointTextBlock.Text = _statistics.FirstHighScorePoint.ToString();

                SecondAveragePointTextBlock.Text = _statistics.SecondAveragePoint.ToString();
                SecondHighScorePointTextBlock.Text = _statistics.SecondHighScorePoint.ToString();

                ThirdAveragePointTextBlock.Text = _statistics.ThirdAveragePoint.ToString();
                ThirdHighScorePointTextBlock.Text = _statistics.ThirdHighScorePoint.ToString();

                PercentileTextBlock.Text = _statistics.Percentile.ToString();
                TimeTextBlock.Text = _statistics.Time.ToString();

                AllAveragePointTextBlock.Text = Convert.ToString(_statistics.FirstAveragePoint + _statistics.SecondAveragePoint + _statistics.ThirdAveragePoint);
                AllHighScorePointTextBlock.Text = Convert.ToString(_statistics.FirstHighScorePoint + _statistics.SecondHighScorePoint + _statistics.ThirdHighScorePoint);

                if (_callbackCompleted)
                    displayCardInformationByScore(
                    _statistics.FirstHighScorePoint
                    + _statistics.SecondHighScorePoint
                    + _statistics.ThirdHighScorePoint);
                else _callbackCompleted = true;
            }
        }

        // ได้รับตารางคะแนนกลับมา
        private void getScoreTableCallback(ScoreTableResponse scoreTable)
        {
            if (scoreTable != null) {
                _table = scoreTable;

                if (_callbackCompleted)
                    displayCardInformationByScore(
                   _statistics.FirstHighScorePoint
                   + _statistics.SecondHighScorePoint
                   + _statistics.ThirdHighScorePoint);
                else _callbackCompleted = true;
            }
        }

        // แสดงการ์ดทีได้จากคะแนน
        private void displayCardInformationByScore(int score)
        {
            CardInformation result = null;

            foreach (var card in _table.CardLevelList) {
                if (score >= card.RequireScore) result = card;
                else if (score < card.RequireScore) break;
            }

            if (result != null) CardImage.Source = result.ImageSource;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion Methods
    }
}
