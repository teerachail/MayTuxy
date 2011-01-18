﻿using System;
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
    /// เกม State 2
    /// </summary>
    public partial class SecondStatePage : Page
    {
        #region Fields

        private const int TimeTickSecond = 1;   // เวลาในการเดินของนาฬิกา ต่อวินาที
        private const int QuestionTimeMilisecond = 700; // เวลาในการที่ต้องรอดูโจทย์ มิลิวินาที
        private const int MinimumIncorrectCountForDisplayFail = 5; // จำนวนครั้งที่จะทำการแสดงเครื่องหมายผิดที่มีจำนวนครั้งที่ผิด
        private const int DisplayGameCombo = 5; // จำนวนครั้งที่จะทำการแสดงผล Combo ที่ได้
        private const int TimeAlertSecond = 10; // แจ้งเตือนเวลาใกล้หมด
        private const string CupStyleName = "TriangleCup";
        private bool _isRoundFinish; // จบ Round ที่กำลังเล่นนี้แล้วหรือยัง
        private bool _isGetNextQuestion; // เมื่อเล่นอนิเมชันสามเกลอจบจะทำการสร้างคำถามใหม่หรือไม่
        private bool _isWaitingClickForPlayQuestion; // กำลังรอให้คลิกเพื่อเล่นคำถาม
        private int _cupAutoAnswerCount; // ตัวนับแก้วที่ถูกตอบอัตโนมัติ
        private int _timeCombo;
        private int _incorrectCount;
        private int _correctCount;
        private int _timeLeftSecond;
        private int _gameCombo;
        private RowUI _frontRow;
        private RowUI _backRow;
        private DispatcherTimer _timer;
        private DispatcherTimer _timerAfterPlayQuestion;
        private DispatcherTimer _displayQuestionTimer;
        private GameStageManager _gameManager;
        private TimeOutLayerUI _timeOutLayer;
        private TrueFalseMarkUI _trueFalseMark;
        private PrepareLayerUI _prepareLayer;
        private CloudUI _clound;

        private const int FirstHandAnimationSecond = 3;
        private const int SecondHandAnimationSecond = 8;
        private int _doNotingTime;
        private DispatcherTimer _doNotingHandTimer;

        #endregion Fields

        #region Events

        public static event EventHandler GameFinish;

        #endregion Events

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นของเกม State 2
        /// </summary>
        public SecondStatePage()
        {
            InitializeComponent();

            GlobalScore.SecondScore = GlobalScore.FirstScore;

            // เหตุการณ์ในการรอให้แสดงคำถามเสร็จสิ้นก่อน
            _displayQuestionTimer = new DispatcherTimer();
            _displayQuestionTimer.Interval = TimeSpan.FromMilliseconds(QuestionTimeMilisecond);

            // ตัวนับเวลาก่อนเกมเริ่ม
            _prepareLayer = new PrepareLayerUI();
            LayoutRoot.Children.Add(_prepareLayer);

            // กำหนดตัวควบคุมเกม และ แถวหน้ากับแถวหลัง
            _gameManager = new GameStageManagerSecond();
            _frontRow = new RowUI();
            _backRow = new RowUI();

            // กำหนดค่าให้ตัวแจ้งเวลาจบเกม
            _timeOutLayer = new TheS.SperfGames.MayaTukky.Controls.TimeOutLayerUI();

            // กำหนดตำแหน่งของแถวหน้า
            Canvas.SetTop(_frontRow, 35);

            // ย่อขนาดของแถวหลัง
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = scale.ScaleX * 0.8;
            scale.ScaleY = scale.ScaleY * 0.8;
            Canvas.SetLeft(_backRow, 60);
            Canvas.SetTop(_backRow, 25);
            _backRow.RenderTransform = scale;

            // เครื่องหมายที่แสดงผลการตอบถูกหรือตอบผิด
            _trueFalseMark = new TrueFalseMarkUI();

            // สร้างตัวจับเวลา
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(TimeTickSecond);

            // timer for after play question
            _timerAfterPlayQuestion = new DispatcherTimer();
            _timerAfterPlayQuestion.Interval = TimeSpan.FromSeconds(0.08);

            // สร้างหน้าก้อนเมฆในการแสดงการเปลี่ยนฉาก
            _clound = new CloudUI();

            // กำหนดเหตุการณ์ของเกม
            initializeEvents();

            // เริ่มเล่นตัวนับเวลาก่อนเข้าเล่นเกม
            _prepareLayer.Sb_Start.Begin();
            Sb_Dark.Begin();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ทำการเล่นคำถาม
        /// </summary>
        public void PlayQuestion()
        {
            _isWaitingClickForPlayQuestion = false;
            _frontRow.PlayCupDown();
            _backRow.PlayCupDown();
        }

        // เรียกคำถาม
        private void GetQuestion()
        {
            _doNotingHandTimer.Stop();

            const int Reset = 0;
            _cupAutoAnswerCount = Reset;
            _isRoundFinish = false;
            _isGetNextQuestion = false;

            // เรียกคำถาม
            Question question = _gameManager.GetNextQuestion();
            string cupLevel = question.CupLevel;

            // กำหนดข้อมูลของแถวหน้าและแถวหลัง พร้อมกับกำหนดคำถาม
            _frontRow.SetQuestionRow(question.FrontRow, CupStyleName, cupLevel);
            _backRow.SetQuestionRow(question.BackRow, CupStyleName, cupLevel);

            // ตั้งเวลาในการดูคำถาม
            _displayQuestionTimer.Start();
        }

        // กำหนดเหตุการณ์ของเกม
        private void initializeEvents()
        {
            // ตัวนับเวลาก่อนเริ่มเล่นเกม
            _prepareLayer.Sb_Start.Completed += new EventHandler(Sb_Start_Completed);

            // เหตุการณ์เมื่อเวลาเดิน
            _timer.Tick += new EventHandler(_timer_Tick);

            // ตัวนับเวลาแสดงมือทักกี้
            _doNotingHandTimer = new DispatcherTimer();
            _doNotingHandTimer.Interval = TimeSpan.FromSeconds(TimeTickSecond);

            // เมื่อเวลาในการรอดูคำถามเสร็จสิ้น
            _displayQuestionTimer.Tick += new EventHandler(_displayQuestionTimer_Tick);

            // delay for set after cup items
            _timerAfterPlayQuestion.Tick += new EventHandler(_timerAfterPlayQuestion_Tick);

            // ทำการติดตามข้อมูลเมื่อมีการคลิกตัวแก้ว
            _frontRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);
            _backRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);

            // กำหนดเหตุการณ์เมื่อก้อนเมฆเปลี่ยนฉากเล่นจบ
            _clound.Sb_CloudIn.Completed += new EventHandler(Sb_CloudIn_Completed);

            // ทำการติดตามเมื่อแก้วถูกเปิดเสร็จสิ้น
            foreach (var cup in _frontRow.Cups) cup.Sb_Up.Completed += new EventHandler(Sb_Up_Completed);
            foreach (var cup in _backRow.Cups) cup.Sb_Up.Completed += new EventHandler(Sb_Up_Completed);

            // กำหนดเหตุกาณ์ในการแสดงผลทักกี้ และสามเกลอ
            tukkyWin.ThreeTopNormal.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopLose.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopWin.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);

            // เครื่องหมายที่แสดงผลการตอบถูกหรือตอบผิด
            _trueFalseMark.Sb_Good.Completed += (s, e) => { _trueFalseMark.Sb_Good.Stop(); };
            _trueFalseMark.Sb_Fail.Completed += (s, e) => { _trueFalseMark.Sb_Fail.Stop(); };
            _trueFalseMark.Sb_ComboContinuing.Completed += (s, e) => { _trueFalseMark.Sb_ComboContinuing.Stop(); };
            _trueFalseMark.Sb_ComboLost.Completed += (s, e) => { _trueFalseMark.Sb_ComboLost.Stop(); };

            // เมื่อแก้วสลับเสร็จสิ้น
            _frontRow.SwapCompleted += new EventHandler(Row_SwapCompleted);

            // คลิกเพื่อเล่นการแสดงคำถาม
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);

            // กำหนดเหตุการณ์เมื่อทักกี้แสดงอารมณ์เสร็จ
            tukkyLose.PlayCompleted += new EventHandler(Tukky_emotion_Completed);
            tukkyWin.PlayCompleted += new EventHandler(Tukky_emotion_Completed);

            // กำหนดเหตุการณ์เมื่อเล่นการนับเวลาจบ
            _timeOutLayer.Sb_TimeOut.Completed += new EventHandler(Sb_TimeOut_Completed);

            // กำหนดการเล่นอนิเมชันเมื่อได้รับเวลาเพิ่ม
            clock.Sb_TimeUp.Completed += new EventHandler(Sb_TimeUp_Completed);

            // เหตุการณ์เมื่อไม่มีการทำบางสิ่งบางอย่างนานๆ
            _doNotingHandTimer.Tick += new EventHandler(_doNotingHandTimer_Tick);
        }

        // เหตุการณ์เมื่อไม่มีการทำบางสิ่งบางอย่างนานๆ
        private void _doNotingHandTimer_Tick(object sender, EventArgs e)
        {
            _doNotingTime++;

            if (_doNotingTime >= SecondHandAnimationSecond)
            {
                tukkyHand.Sb_HandWaitThreeSecond.Stop();
                tukkyHand.Sb_HandWaitFiveSecond.Begin();
            }
            else if (_doNotingTime >= FirstHandAnimationSecond)
            {
                tukkyHand.Sb_HandWaitThreeSecond.Begin();
            }

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

        private void _timerAfterPlayQuestion_Tick(object sender, EventArgs e)
        {
            _timerAfterPlayQuestion.Stop();
            completePlayQuestion();
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

        // ตรวจสอบคำตอบ
        private void CheckAnswer(object sender, CupAnswerEventArgs objName)
        {
            // ตรวจสอบผลลัพธ์
            var result = _gameManager.CheckAnswer(objName.ItemName);

            // หยุดการเล่่นอินเมชันมือทักกี้
            tukkyHand.Sb_HandWaitThreeSecond.Stop();
            tukkyHand.Sb_HandWaitFiveSecond.Stop();
            _doNotingHandTimer.Stop();
            const int ResetTimer = 0;
            _doNotingTime = ResetTimer;

            if (result != null)
            {
                // กำหนดค่าให้กับคะแนนความต่อเนื่องของเวลา และแสดงผลเวลาเกมที่เหลือ
                _timeCombo = result.TimeCombo;
                _timeLeftSecond += result.TimeAdvantage;
                clock.txt_TimePlus.Text = result.TimeAdvantage.ToString();

                // แสดงผลอนิเมชันตอบของ item
                _frontRow.PlayAnswerResult(result);
                _backRow.PlayAnswerResult(result);

                const int IncorrectAnswer = 0;
                if ((int)result.Score > IncorrectAnswer)
                {
                    // คำนวณการนำคะแนนที่ได้ไปทำการแสดงผล
                    const int Proportion = 5;
                    const string ScoreBoardName = "DokValue";
                    calculateScoreRunner(ScoreBoardName, Proportion, (int)result.Score);

                    scoreBoard.txt_ScorePlus.Text = ((int)result.Score).ToString();

                    // เล่นอนิเมชันแสดงคะแนน
                    //scoreBoard.Sb_ScoreUp.Stop();
                    scoreBoard.Sb_ScorePlus.Stop();
                    scoreBoard.Sb_ScoreUp.Begin();
                    scoreBoard.Sb_ScorePlus.Begin();

                    GlobalScore.SecondScore += (int)result.Score;

                    // จัดการตัวนับการตอบถูกติดต่อกัน
                    if (GlobalScore.SecondMaximumCombo < _gameCombo) GlobalScore.SecondMaximumCombo = _gameCombo;
                }

                if (result.IsCorrect == false)
                {
                    // จัดการตัวนับการตอบผิด
                    _incorrectCount++;

                    // ตอบผิดทำการเรียกคำถามใหม่
                    _isGetNextQuestion = true;

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่นอนิเมชัน
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopWin.StartPlay();

                    // แสดงอนิเมชันการตอบผิด
                    if (_gameCombo >= MinimumIncorrectCountForDisplayFail)
                    {
                        // TODO: แสดงกราฟฟิคจำนวน Combo ที่เสียไป State 2
                        _trueFalseMark.Sb_ComboLost.Begin();
                    }
                    else _trueFalseMark.Sb_Fail.Begin();

                    // จัดการตัวนับการตอบถูกติดต่อกัน
                    const int ResetGameCombo = 0;
                    _gameCombo = ResetGameCombo;
                }
                else if (result.IsCorrect == true)
                {
                    // เพิ่มรายชื่อวัตถุที่อยู่ภายในแก้วที่ตอบถูก
                    if (!GlobalScore.SecondItemsFound.Any(c => c.Equals(objName.ItemName)))
                    {
                        GlobalScore.SecondItemsFound.Add(objName.ItemName);
                    }

                    // ตรวจสอบการจบระดับความยากนี้
                    if (result.IsFinish == true)
                    {
                        // จัดการตัวนับการตอบถูกติดต่อกัน
                        _gameCombo++;

                        // ตอบถูก ทำการตรวจสอบการเลื่อนระดับความยาก
                        _isGetNextQuestion = true;
                        _isRoundFinish = true;
                    }
                }

                const int First = 1;
                const int Second = 2;
                const int Third = 3;
                const int Fourth = 4;
                const int Fifth = 5;
                if (_timeCombo >= First) clock.PlayClockOne();
                if (_timeCombo >= Second) clock.PlayClockTwo();
                if (_timeCombo >= Third) clock.PlayClockThree();
                if (_timeCombo >= Fourth) clock.PlayClockFour();
                if (_timeCombo >= Fifth)
                {
                    clock.PlayClockFive();
                    clock.Sb_TimeUp.Begin();
                }
            }
        }

        // คำนวณการนำคะแนนที่ได้ไปทำการแสดงผล
        private void calculateScoreRunner(string objectName, int keyFrame, int score)
        {
            int scoreProportion = (score / keyFrame);
            for (int keyFrameValues = 1; keyFrameValues <= keyFrame; keyFrameValues++)
            {
                (scoreBoard.LayoutRoot.FindName(string.Format("{0}{1}", objectName, keyFrameValues)) as DiscreteObjectKeyFrame)
                    .Value = (GlobalScore.SecondScore + scoreProportion * keyFrameValues).ToString();
            }
            (scoreBoard.LayoutRoot.FindName(string.Format("{0}{1}", objectName, keyFrame)) as DiscreteObjectKeyFrame)
                    .Value = (GlobalScore.SecondScore + (int)score).ToString();
        }

        // เมื่อเวลาเดิน 1 ติ๊ก
        private void _timer_Tick(object sender, EventArgs e)
        {
            var result = _gameManager.Tick();

            // แสดงผลเวลา
            _timeLeftSecond = _gameManager.TimeLeftSecond;
            clock.txt_Timer.Text = Convert.ToString(_timeLeftSecond);
            clock.Sb_TikTok.Begin();

            // เปลี่ยนสีของเวลาเมื่อเวลาใกล้หมด State 2
            if (_timeLeftSecond <= TimeAlertSecond)
            {
                clock.txt_Timer.Foreground = new SolidColorBrush(Colors.Red);
            }

            // เมื่อเวลาหมด
            if (result)
            {
                _timer.Stop();

                // ปิดการแสดงผลของแถวหน้าและแถวหลัง
                _frontRow.Visibility = System.Windows.Visibility.Collapsed;
                _backRow.Visibility = System.Windows.Visibility.Collapsed;

                // แสดงผลอนิเมชันหมดเวลา
                LayoutRoot.Children.Add(_timeOutLayer);

                _timeOutLayer.Sb_TimeOut.Begin();
            }
        }

        // กำหนดการแสดงผลของสามเกลอ
        private void ThreeTop_PlayCompleted(object sender, EventArgs e)
        {
            tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Visible;
            tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
            tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;

            // ตรวจสอบการเรียกคำถามใหม่
            if (_isGetNextQuestion) GetQuestion();
        }

        // คลิกเพื่อทำการเล่นคำถาม
        private void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isWaitingClickForPlayQuestion)
            {
                PlayQuestion();
                tukkyHand.StartPlay();
            }
        }

        // เมื่อแถวสลับเสร็จสิ้น
        private void Row_SwapCompleted(object sender, EventArgs e)
        {
            // แถวที่สลับเสร็จเป็นแถวหน้า
            tukkyHand.StopPlay();

            _timerAfterPlayQuestion.Start();

            // เริ่มทำการจับเวลาเผื่อเล่นอนิเมชันมือทักกี้
            _doNotingHandTimer.Start();
        }

        private void completePlayQuestion()
        {
            // กำหนดข้อมูลให้แถวหลัง
            _backRow.SetAfterCupItem();
            _frontRow.SetAfterCupItem();
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

            // นำแถวหน้าและแถวหลังเข้ามาภายในตัวเกม
            LayoutRoot.Children.Add(_backRow);
            LayoutRoot.Children.Add(_frontRow);

            // นำเครื่องหมาย True Flase เข้ามา
            LayoutRoot.Children.Add(_trueFalseMark);

            // เริ่มทำการจับเวลา
            _timer.Start();
        }

        // เมื่อแก้วลอยขึ้นจนสุดแล้ว
        private void Sb_Up_Completed(object sender, EventArgs e)
        {
            _doNotingHandTimer.Start();

            if (_isGetNextQuestion)
            {
                _cupAutoAnswerCount++;
                const int AutoCupAmount = 2;

                if (_cupAutoAnswerCount >= AutoCupAmount && _isRoundFinish)
                {
                    _isRoundFinish = false;

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่นอนิเมชัน
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopLose.StartPlay();

                    // แสดงอนิเมชันการตอบถูก State 2
                    const int DisplayCorrectAnswerAndCombo = 0;
                    const int DisplayCorrectAnswerForLowLevel = 3;
                    if (((_gameCombo % DisplayGameCombo == DisplayCorrectAnswerAndCombo)
                        && (_gameCombo != DisplayCorrectAnswerAndCombo))
                        || (_gameCombo == DisplayCorrectAnswerForLowLevel))
                    {
                        _trueFalseMark.txt_TrueCombo.Text = _gameCombo.ToString();
                        _trueFalseMark.Sb_ComboContinuing.Begin();
                    }
                    else _trueFalseMark.Sb_Good.Begin();

                    _correctCount++;
                }
            }
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

        #endregion Methods

    }
}
