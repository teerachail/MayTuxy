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
        private const int MinimumIncorrectCountForDisplayFail = 5; // จำนวนครั้งที่จะทำการแสดงเครื่องหมายผิดที่มีจำนวนครั้งที่ผิด
        private const int DisplayGameCombo = 5; // จำนวนครั้งที่จะทำการแสดงผล Combo ที่ได้
        private const int QuestionTimeMilisecond = 700; // เวลาในการที่ต้องรอดูโจทย์ มิลิวินาที
        private const string CupStyleName = "CylindricalCup";
        private bool _isWaitingClickForPlayQuestion; // กำลังรอให้คลิกเพื่อเล่นคำถาม
        private int _correctCount;
        private int _incorrectCount;
        private int _timeLeftSecond;
        private int _timeCombo;
        private int _gameCombo;
        private RowUI _frontRow;
        private TimeOutLayerUI _timeOutLayer;
        private TrueFalseMarkUI _trueFalseMark;
        private DispatcherTimer _timer;
        private DispatcherTimer _autoPlayQuestionTimer;
        private DispatcherTimer _displayQuestionTimer;
        private GameStageManager _gameManager;
        private PrepareLayerUI _prepareLayer;
        private CloudUI _clound;

        #endregion Fields

        #region Events

        public static event EventHandler GameFinish;

        #endregion Events

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นของเกม State 1
        /// </summary>
        public FirstStatePage()
        {
            InitializeComponent();

            // เหตุการณ์ในการรอให้แสดงคำถามเสร็จสิ้นก่อน
            _displayQuestionTimer = new DispatcherTimer();
            _displayQuestionTimer.Interval = TimeSpan.FromMilliseconds(QuestionTimeMilisecond);

            // ตัวนับเวลาก่อนเกมเริ่ม
            _prepareLayer = new PrepareLayerUI();
            LayoutRoot.Children.Add(_prepareLayer);

            // ค่าเริ่มต้น
            _gameManager = new GameStageManagerFirst();
            _frontRow = new RowUI();
            _trueFalseMark = new TrueFalseMarkUI();

            // กำหนดค่าให้ตัวแจ้งเวลาจบเกม
            _timeOutLayer = new TheS.SperfGames.MayaTukky.Controls.TimeOutLayerUI();

            // สร้างตัวจับเวลา
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(TimeTickSecond);
            _autoPlayQuestionTimer = new DispatcherTimer();
            _autoPlayQuestionTimer.Interval = TimeSpan.FromSeconds(AutoPlayQuestionTimeSecond);

            // สร้างหน้าก้อนเมฆในการแสดงการเปลี่ยนฉาก
            _clound = new CloudUI();

            // กำหนดเหตุการณ์ของเกม
            initializeEvents();

            // เริ่มเล่นตัวนับเวลาก่อนเข้าเล่นเกม
            _prepareLayer.Sb_Start.Begin();
            Sb_Dark.Begin();
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

            // เมื่อเวลาในการรอดูคำถามเสร็จสิ้น
            _displayQuestionTimer.Tick += new EventHandler(_displayQuestionTimer_Tick);

            // ทำการติดตามข้อมูลเมื่อมีการคลิกตัวแก้ว
            _frontRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);

            // เมื่อแก้วสลับเสร็จสิ้น
            _frontRow.SwapCompleted += new EventHandler(_frontRow_SwapCompleted);

            // เมื่อแก้วโชว์เสร็จสิ้น
            _frontRow.ShowItemCompleted += new EventHandler(_frontRow_ShowItemCompleted);

            // กำหนดเหตุการณ์เมื่อก้อนเมฆเปลี่ยนฉากเล่นจบ
            _clound.Sb_CloudIn.Completed += new EventHandler(Sb_CloudIn_Completed);

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

            // กำหนดเหตุการณ์เมื่อทักกี้แสดงอารมณ์เสร็จ
            tukkyLose.PlayCompleted += new EventHandler(Tukky_emotion_Completed);
            tukkyWin.PlayCompleted += new EventHandler(Tukky_emotion_Completed);

            // กำหนดเหตุการณ์เมื่อเล่นการนับเวลาจบ
            _timeOutLayer.Sb_TimeOut.Completed += new EventHandler(Sb_TimeOut_Completed);

            // กำหนดการเล่นอนิเมชันเมื่อได้รับเวลาเพิ่ม
            clock.Sb_TimeUp.Completed += new EventHandler(Sb_TimeUp_Completed);
        }

        // เมื่อเวลาในการรอดูคำถามเสร็จสิ้น
        private void _displayQuestionTimer_Tick(object sender, EventArgs e)
        {
            _displayQuestionTimer.Stop();
            _isWaitingClickForPlayQuestion = true;
        }

        // เมื่อได้รับเวลาเพิ่ม
        private void Sb_TimeUp_Completed(object sender, EventArgs e)
        {
            clock.Sb_TimeUp.Stop();
        }

        // แสดงการเล่นเมฆในการเปลี่ยนฉาก
        private void Tukky_emotion_Completed(object sender, EventArgs e)
        {
            LayoutRoot.Children.Add(_clound);
            _clound.Sb_CloudIn.Begin();
        }

        // แจ้งเหตุการณ์ว่าเกมจบแล้ว
        private void Sb_CloudIn_Completed(object sender, EventArgs e)
        {
            var temp = GameFinish;
            if (temp != null)
            {
                temp(this, null);
            }
        }

        // เรียกคำถามใหม่
        private void GetQuestion()
        {
            // เรียกคำถาม
            var question = _gameManager.GetNextQuestion();

            // กำหนดข้อมูลของแถวหน้า
            _frontRow.SetQuestionRow(question.FrontRow, CupStyleName, question.CupLevel);

            // ตั้งเวลาในการดูคำถาม
            _displayQuestionTimer.Start();
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

                // แสดงผลอนิเมชันตอบของ item
                _frontRow.PlayAnswerResult(result);

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
                    if (_gameCombo >= MinimumIncorrectCountForDisplayFail)
                    {
                        // TODO: แสดงกราฟฟิคจำนวน Combo ที่เสียไป
                    }
                    else _trueFalseMark.Sb_Fail.Begin();

                    // จัดการตัวนับการตอบถูกติดต่อกัน
                    const int ResetGameCombo = 0;
                    _gameCombo = ResetGameCombo;
                }
                else if (result.IsCorrect == true)
                {
                    // จัดการตัวนับการตอบถูกติดต่อกัน
                    _gameCombo++;
                    if (GlobalScore.FirstMaximumCombo < _gameCombo) GlobalScore.FirstMaximumCombo = _gameCombo;

                    // จัดการตัวนับการตอบถูก
                    _correctCount++;

                    result.IsFinish = false;

                    // จัดการการแสดงผลคะแนนและเวลา
                    _timeLeftSecond += result.TimeAdvantage;
                    GlobalScore.FirstScore += (int)result.Score;
                    scoreBoard.txt_Score.Text = Convert.ToString(GlobalScore.FirstScore);
                    scoreBoard.Sb_ScoreUp.Begin();

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่นอนิเมชัน
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopLose.StartPlay();

                    // แสดงอนิเมชันการตอบถูก
                    const int DisplayCorrectAnswerAndCombo = 0;
                    if (_gameCombo % DisplayGameCombo == DisplayCorrectAnswerAndCombo)
                    {
                        // TODO: แสดงกราฟฟิคจำนวน Combo ที่ได้
                    }
                    else _trueFalseMark.Sb_Good.Begin();

                    // เล่นอนิเมชันแสดงคะแนน
                    scoreBoard.Sb_ScoreUp.Stop();
                    scoreBoard.Sb_ScorePlus.Stop();
                    scoreBoard.Sb_ScoreUp.Begin();
                    scoreBoard.Sb_ScorePlus.Begin();
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

        // เมื่อตัวนับเวลาก่อนเริ่มเล่นเกมจบลง
        private void Sb_Start_Completed(object sender, EventArgs e)
        {
            // เคลียร์ขัวนับ
            _prepareLayer.Sb_Start.Stop();
            LayoutRoot.Children.Remove(_prepareLayer);

            Sb_Dark.Stop();

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

                // แสดงผลอนิเมชันหมดเวลา
                LayoutRoot.Children.Add(_timeOutLayer);

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
            if (_isWaitingClickForPlayQuestion) PlayQuestion();
        }

        // แสดงผลการเล่นอนิเมชันของทักกี้
        private void Sb_TimeOut_Completed(object sender, EventArgs e)
        {
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
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}
