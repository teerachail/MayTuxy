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
    /// <summary>
    /// เกม State 1
    /// </summary>
    public partial class FirstStatePage : Page
    {
        #region Fields

        private const int TimeTickSecond = 1;   // เวลาในการเดินของนาฬิกา ต่อวินาที
        private const int AutoPlayQuestionTimeSecond = 2; // เวลาในการรอให้จำโจทย์ วินาที
        private bool _isWaitingClickForPlayQuestion; // กำลังรอให้คลิกเพื่อเล่นคำถาม
        private string _cupStyleName;
        private string[] _cupStyles;
        private int _correctCount;
        private int _incorrectCount;
        private int _timeLeftSecond;
        private int _timeCombo;
        private RowUI _frontRow;
        private TimeOutLayerUI _timeOutLayer;
        private TrueFalseMarkUI _trueFalseMark;
        private DispatcherTimer _timer;
        private DispatcherTimer _autoPlayQuestionTimer;
        private GameStageManager _gameManager;
        private PrepareLayerUI _prepareLayer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นของเกม State 1
        /// </summary>
        public FirstStatePage()
        {
            InitializeComponent();

            // ตัวนับเวลาก่อนเกมเริ่ม
            _prepareLayer = new PrepareLayerUI();
            LayoutRoot.Children.Add(_prepareLayer);

            // ค่าเริ่มต้น
            _gameManager = new GameStageManagerFirst();
            _frontRow = new RowUI();
            _trueFalseMark = new TrueFalseMarkUI();

            // กำหนดชนิดของแก้ว และทำการสุ่มลายแก้วที่จะนำมาใช้ในเกม
            _cupStyles = new string[] { "CylindricalCup", "TallCup", "TriangleCup" };
            _cupStyleName = randomCupStyle();

            // สร้างตัวจับเวลา
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(TimeTickSecond);
            _autoPlayQuestionTimer = new DispatcherTimer();
            _autoPlayQuestionTimer.Interval = TimeSpan.FromSeconds(AutoPlayQuestionTimeSecond);

            // กำหนดเหตุการณ์ของเกม
            initializeEvents();

            // เริ่มเล่นตัวนับเวลาก่อนเข้าเล่นเกม
            _prepareLayer.Sb_Start.Begin();
        }

        #endregion Constructors

        /// <summary>
        /// ทำการเล่นคำถาม
        /// </summary>
        public void PlayQuestion()
        {
            _isWaitingClickForPlayQuestion = false;
            tukkyHand.StartPlay();
            _frontRow.PlayCupDown();
        }

        // กำหนดเหตุการณ์ของเกม
        private void initializeEvents()
        {
            // ตัวนับเวลาก่อนเริ่มเล่นเกม
            _prepareLayer.Sb_Start.Completed += new EventHandler(Sb_Start_Completed);

            // เหตุการณ์เมื่อเวลาเดิน
            _timer.Tick += new EventHandler(_timer_Tick);
            _autoPlayQuestionTimer.Tick += new EventHandler(_autoPlayQuestionTimer_Tick);

            // ทำการติดตามข้อมูลเมื่อมีการคลิกตัวแก้ว
            _frontRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);

            // เมื่อแก้วสลับเสร็จสิ้น
            _frontRow.SwapCompleted += new EventHandler(_frontRow_SwapCompleted);

            // เมื่อแก้วโชว์เสร็จสิ้น
            _frontRow.ShowItemCompleted += new EventHandler(_frontRow_ShowItemCompleted);

            // กำหนดเหตุกาณ์ในการแสดงผลทักกี้ และสามเกลอ
            tukkyWin.ThreeTopNormal.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopLose.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopWin.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);

            // เครื่องหมายที่แสดงผลการตอบถูกหรือตอบผิด
            _trueFalseMark.Sb_Good.Completed += (s, e) => { _trueFalseMark.Sb_Good.Stop(); };
            _trueFalseMark.Sb_Fail.Completed += (s, e) => { _trueFalseMark.Sb_Fail.Stop(); };

            // คลิกเพื่อเล่นการแสดงคำถาม
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);

            // กำหนดเหตุการ์ณ์ของนาฬิกาจับเวลา
            clock.Sb_Clock5.Completed += new EventHandler(Sb_Clock5_Completed);
        }

        // เรียกคำถามใหม่
        private void GetQuestion()
        {
            _isWaitingClickForPlayQuestion = true;

            // เรียกคำถาม
            var question = _gameManager.GetNextQuestion();

            // กำหนดข้อมูลของแถวหน้า
            _frontRow.SetQuestionRow(question.FrontRow, _cupStyleName, question.CupLevel);
        }

        // ตรวจสอบคำตอบ
        private void CheckAnswer(object sender, CupAnswerEventArgs objName)
        {
            // ตรวจสอบผลลัพธ์
            var result = _gameManager.CheckAnswer(objName.ItemName);

            if (result != null)
            {
                // กำหนดค่าให้กับคะแนนความต่อเนื่องของเวลา
                _timeCombo = result.TimeCombo;

                if (result.IsCorrect == false)
                {
                    // จัดการตัวนับการตอบผิด
                    _incorrectCount++;

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่นอนิเมชัน
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopWin.StartPlay();

                    // แสดงอนิเมชันการตอบผิด
                    _trueFalseMark.Sb_Fail.Begin();
                }
                else if (result.IsCorrect == true)
                {
                    // จัดการตัวนับการตอบถูก
                    _correctCount++;

                    // จัดการการแสดงผลคะแนนและเวลา
                    _timeLeftSecond += result.TimeAdvantage;
                    GlobalScore.First += (int)result.Score;
                    scoreBoard.txt_Score.Text = Convert.ToString(GlobalScore.First);
                    scoreBoard.Sb_ScoreUp.Begin();

                    // แสดงผลอนิเมชันตอบถูกของ item
                    _frontRow.PlayAnswerResult(result);

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่นอนิเมชัน
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopLose.StartPlay();

                    // แสดงอนิเมชันการตอบถูก
                    _trueFalseMark.Sb_Good.Begin();

                    // เล่นอนิเมชันดาว
                    scoreBoard.Sb_RoundEnd.Stop();
                    scoreBoard.Sb_RoundEnd.Begin();
                }

                const int First = 1;
                const int Second = 2;
                const int Third = 3;
                const int Fourth = 4;
                const int Fifth = 5;
                if (_timeCombo == First) clock.Sb_Clock1.Begin();
                else if (_timeCombo == Second) clock.Sb_Clock2.Begin();
                else if (_timeCombo == Third) clock.Sb_Clock3.Begin();
                else if (_timeCombo == Fourth) clock.Sb_Clock4.Begin();
                else if (_timeCombo == Fifth)
                {
                    clock.Sb_Clock5.Begin();
                    clock.Sb_TimeUp.Begin();
                }
            }
        }

        // เมื่อการแก้วถูกสลับเสร็จสิ้น
        private void _frontRow_SwapCompleted(object sender, EventArgs e)
        {
            tukkyHand.StopPlay();
            _frontRow.SetAfterCupItem();
        }

        // กำหนดเหตุการ์ณ์ของนาฬิกาจับเวลา
        private void Sb_Clock5_Completed(object sender, EventArgs e)
        {
            clock.Sb_Clock1.Stop();
            clock.Sb_Clock2.Stop();
            clock.Sb_Clock3.Stop();
            clock.Sb_Clock4.Stop();
            clock.Sb_Clock5.Stop();
        }

        // สุ่มแก้วที่จะนำมาแสดงผลใน state นี้
        private string randomCupStyle()
        {
            const int MaximumStyle = 3;
            var _random = new Random();
            return _cupStyles[_random.Next(MaximumStyle)];
        }

        // เมื่อตัวนับเวลาก่อนเริ่มเล่นเกมจบลง
        private void Sb_Start_Completed(object sender, EventArgs e)
        {
            // เรียกขอคำถาม
            GetQuestion();

            LayoutRoot.Children.Remove(_prepareLayer);
            LayoutRoot.Children.Add(_frontRow);

            // นำเครื่องหมาย True Flase เข้ามา
            LayoutRoot.Children.Add(_trueFalseMark);

            // เริ่มทำการจับเวลา
            _timer.Start();
        }

        // เมื่อการเล่นอนิเมชันของสามเกลอเสร็จสิ้น
        private void ThreeTop_PlayCompleted(object sender, EventArgs e)
        {
            tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Visible;
            tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
            tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;

            GetQuestion();
        }

        // เมื่อเวลาเดิน
        private void _timer_Tick(object sender, EventArgs e)
        {
            var result = _gameManager.Tick();
            // แสดงผลเวลา
            _timeLeftSecond = _gameManager.TimeLeftSecond;
            clock.txt_Timer.Text = Convert.ToString(_timeLeftSecond);

            // เมื่อเวลาหมด
            if (result)
            {
                _timer.Stop();

                // ปิดการแสดงผลของแถวหน้าและแถวหลัง
                _frontRow.Visibility = System.Windows.Visibility.Collapsed;

                _timeOutLayer = new TheS.SperfGames.MayaTukky.Controls.TimeOutLayerUI();
                LayoutRoot.Children.Add(_timeOutLayer);

                // จัดการการแสดงผลของทักกี้
                if (_incorrectCount >= _correctCount)
                {
                    // เล่น aniamtion ทักกีี้หัวเราะ
                    tukkyWin.StartPlay();
                }
                else
                {
                    // เล่น aniamtion ทักกีี้ร้องไห้
                    tukkyWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyLose.StartPlay();
                }

                _timeOutLayer.Sb_TimeOut.Begin();
            }
        }

        // เมื่อทำการคลิกเพื่อทำการเล่นคำถาม
        private void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isWaitingClickForPlayQuestion)
            {
                PlayQuestion();
            }
        }

        // เมื่อเล่นอนิเมชันโชว์วัตถุในแก้วเสร็จสิ้น
        private void _frontRow_ShowItemCompleted(object sender, EventArgs e)
        {
            _autoPlayQuestionTimer.Start();
        }

        // เมื่อหมดเวลาในการดูวัตถุในแก้ว
        private void _autoPlayQuestionTimer_Tick(object sender, EventArgs e)
        {
            _autoPlayQuestionTimer.Stop();
            PlayQuestion();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}
