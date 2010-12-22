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
using System.Windows.Threading;

namespace TheS.SperfGames.MayaTukky.Views
{
    public partial class SecondStatePage : Page
    {
        private const int PlayTukkyAnimation = 5;
        private int _timeCombo;
        private bool _resetGame;
        private bool _isGameStart;
        private int _correctCount;
        private string[] _cupStyles;
        private int _incorrectCount;
        private int _timeLeftSecond;
        private string _cupStyleName;
        private int _swapCompletedCount;
        private RowUI _fronRow;
        private RowUI _backRow;
        private TimeOutLayerUI _timeOutLayer;
        private TrueFalseMarkUI _trueFalseMark;
        private DispatcherTimer _timer;
        private GameStageManager _gameManager;
        private PrepareLayerUI _prepareLayer;

        public SecondStatePage()
        {
            InitializeComponent();

            _prepareLayer = new PrepareLayerUI();
            LayoutRoot.Children.Add(_prepareLayer);
            _prepareLayer.Sb_Start.Begin();
            _prepareLayer.Sb_Start.Completed += new EventHandler(Sb_Start_Completed);

            // กำหนดชนิดของแก้ว
            _cupStyles = new string[]{
                "CylindricalCup",
                "TallCup",
                "TriangleCup",
            };

            // สุ่มถ้วยที่จะนำมาใช้ในเกม
            _cupStyleName = randomCupStyle();

            // กำหนดตัวแปรเริ่มต้น
            _gameManager = new GameStageManagerSecond();
            _fronRow = new RowUI();
            _backRow = new RowUI();
            _trueFalseMark = new TrueFalseMarkUI();
            
            Canvas.SetTop(_fronRow, 35);

            // ทำการติดตามข้อมูลเมื่อมีการคลิกตัวแก้ว
            _fronRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);
            _backRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);

            // จัดการการแสดงผลของ tukky และ 3เกลอ
            tukkyLose.PlayCompleted += new EventHandler(tukkyLose_PlayCompleted);
            tukkyWin.ThreeTopNormal.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopLose.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopWin.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);

            // เล่น 3 เกลอ Normal
            tukkyWin.ThreeTopNormal.StartPlay();

            RowUI.SwapCompleted += new EventHandler(Row_SwapCompleted);

            clock.Sb_Clock5.Completed += (s, e) =>
            {
                clock.Sb_Clock1.Stop();
                clock.Sb_Clock2.Stop();
                clock.Sb_Clock3.Stop();
                clock.Sb_Clock4.Stop();
                clock.Sb_Clock5.Stop();

                //clock.Sb_TimeUp.Begin();
            };

            _trueFalseMark.Sb_Good.Completed += (s, e) => { _trueFalseMark.Sb_Good.Stop(); };
            _trueFalseMark.Sb_Fail.Completed += (s, e) => { _trueFalseMark.Sb_Fail.Stop(); };

            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);
        }

        /// <summary>
        /// ทำการเล่นคำถาม
        /// </summary>
        public void PlayQuestion()
        {
            _isGameStart = true;
            _fronRow.PlayCupDown();
            _backRow.PlayCupDown();
        }

        // สุ่มแก้วที่จะนำมาแสดงผลใน state นี้
        private string randomCupStyle()
        {
            const int MaximumStyle = 3;
            var _random = new Random();
            return _cupStyles[_random.Next(MaximumStyle)];
        }

        private void Row_SwapCompleted(object sender, EventArgs e)
        {
            _swapCompletedCount++;
            const int SetAfterItem = 2;
            if (_swapCompletedCount >= SetAfterItem)
            {
                const int Reset = 0;
                _swapCompletedCount = Reset;
                _fronRow.SetAfterCupItem();
                _backRow.SetAfterCupItem();

                tukkyHand.StopPlay();
            }
        }

        private void Sb_Start_Completed(object sender, EventArgs e)
        {
            // สร้างตัวจับเวลา
            _timer = new DispatcherTimer();
            const int OneSecond = 1;
            _timer.Interval = TimeSpan.FromSeconds(OneSecond);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();

            // ย่อขนาดของแถวหลัง
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = scale.ScaleX * 0.8;
            scale.ScaleY = scale.ScaleY * 0.8;
            Canvas.SetLeft(_backRow, 60);
            Canvas.SetTop(_backRow, 25);
            _backRow.RenderTransform = scale;

            // นำ Row เข้ามาภายในตัวเกม
            LayoutRoot.Children.Add(_backRow);
            LayoutRoot.Children.Add(_fronRow);

            // นำเครื่องหมาย True Flase เข้ามา
            LayoutRoot.Children.Add(_trueFalseMark);

            // เรียกขอคำถาม
            GetQuestion();
        }

        // เรียกคำถาม
        private void GetQuestion()
        {
            _resetGame = false;
            _isGameStart = false;
            // เรียกคำถาม
            var question = _gameManager.GetNextQuestion();

            // กำหนดข้อมูลของแถวหน้าและแถวหลัง
            _fronRow.SetQuestionRow(question.FrontRow, _cupStyleName, question.CupLevel);
            _backRow.SetQuestionRow(question.BackRow, _cupStyleName, question.CupLevel);
        }

        // ตรวจสอบคำตอบ
        private void CheckAnswer(object sender, CupAnswerEventArgs objName)
        {
            // ตรวจสอบผลลัพธ์
            var result = _gameManager.CheckAnswer(objName.ItemName);
            objName.AnswerResult = result;
            if (result != null)
            {
                _timeCombo = result.TimeCombo;

                const int ResetAnswerCount = 0;
                if (result.IsCorrect == false)
                {
                    // จัดการตัวนับการตอบผิด
                    _incorrectCount++;
                    _correctCount = ResetAnswerCount;

                    // ตอบผิด ทำการเรียกคำถามใหม่
                    _resetGame = true;

                    // เล่น animation 3 เกลอ
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopWin.StartPlay();

                    // เล่น animation False Mark
                    _trueFalseMark.Sb_Fail.Begin();

                }
                else if (result.IsCorrect == true)
                {
                    // จัดการตัวนับการตอบถูก
                    _correctCount++;
                    _incorrectCount = ResetAnswerCount;

                    // จัดการการแสดงผลคะแนนและเวลา
                    _timeLeftSecond += result.TimeAdvantage;
                    GlobalScore.Second += (int)result.Score;
                    scoreBoard.txt_Score.Text = Convert.ToString(GlobalScore.Second);

                    _fronRow.PlayAnswerResult(result);
                    _backRow.PlayAnswerResult(result);

                    scoreBoard.Sb_ScoreUp.Begin();
   
                    if (result.IsFinish == true)
                    {
                        // ตอบถูก ทำการตรวจสอบการเลื่อนระดับความยาก
                        _resetGame = true;

                        scoreBoard.Sb_RoundEnd.Stop();
                        scoreBoard.Sb_RoundEnd.Begin();
                    }

                    // เล่น อนิมเชั่นนาฬิกา
                    timeComboAnimate(_timeCombo);

                    // เล่น animation 3 เกลอ
                    tukkyWin.ThreeTopLose.StartPlay();
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Visible;

                    // เล่น animation True Mark
                    _trueFalseMark.Sb_Good.Begin();
                }

                // จัดการการแสดงผลของ tukky
                if (_incorrectCount >= PlayTukkyAnimation)
                {
                    // เล่น aniamtion ทักกีี้หัวเราะ
                    tukkyWin.StartPlay();
                    _incorrectCount = ResetAnswerCount;
                }
                else if (_correctCount >= PlayTukkyAnimation)
                {
                    // เล่น aniamtion ทักกีี้ร้องไห้
                    tukkyWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyLose.StartPlay();
                    _correctCount = ResetAnswerCount;
                }
            }
        }

        // เวลาเดิน
        private void _timer_Tick(object sender, EventArgs e)
        {
            var result = _gameManager.Tick();
            // แสดงผลเวลา
            _timeLeftSecond = _gameManager.TimeLeftSecond;
            clock.txt_Timer.Text = Convert.ToString(_timeLeftSecond);

            // เมื่อเวลาหมด
            if (result)
            {
                _timeOutLayer = new TheS.SperfGames.MayaTukky.Controls.TimeOutLayerUI();
                LayoutRoot.Children.Add(_timeOutLayer);
                _timeOutLayer.Sb_TimeOut.Begin();
                _timer.Stop();
            }
        }

        // จัดการการแสดงผลของ tukky
        private void tukkyLose_PlayCompleted(object sender, EventArgs e)
        {
            tukkyWin.Visibility = System.Windows.Visibility.Visible;
            tukkyLose.Visibility = System.Windows.Visibility.Collapsed;
        }

        // กำหนดการแสดงผลของ 3 เกลอ
        private void ThreeTop_PlayCompleted(object sender, EventArgs e)
        {
            tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Visible;
            tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
            tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;

            const int EndGame = 0;
            if (_resetGame || (_fronRow.CurrentCup == EndGame && _backRow.CurrentCup == EndGame))
            {
                GetQuestion();
            }
        }

        private void timeComboAnimate(int combo)
        {
            if (combo.Equals(1)) clock.Sb_Clock1.Begin();
            else if (combo.Equals(2)) clock.Sb_Clock2.Begin();
            else if (combo.Equals(3)) clock.Sb_Clock3.Begin();
            else if (combo.Equals(4)) clock.Sb_Clock4.Begin();
            else { clock.Sb_Clock5.Begin(); clock.Sb_TimeUp.Begin(); }
        }

        private void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isGameStart)
            {
                PlayQuestion();
                tukkyHand.StartPlay();
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}
