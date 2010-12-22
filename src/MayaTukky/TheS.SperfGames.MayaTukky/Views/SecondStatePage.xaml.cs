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
    /// เกม State 2
    /// </summary>
    public partial class SecondStatePage : Page
    {
        #region Fields

        private const int PlayTukkyAnimation = 5; // จำนวนครั้งที่จะเล่น animation ของทักกี้
        private const int TimeTickSecond = 1;   // เวลาในการเดินของนาฬิกา ต่อวินาที
        private bool _isGetNextQuestion; // เมื่อเล่น animation สามเกลอจบจะทำการสร้างคำถามใหม่หรือไม่
        private bool _isWaitingClickForPlayQuestion; // กำลังรอให้คลิกเพื่อเล่นคำถาม
        private int _timeCombo;
        private int _incorrectCount;
        private int _correctCount;
        private int _timeLeftSecond;
        private string[] _cupStyles;
        private string _cupStyleName;
        private RowUI _fronRow;
        private RowUI _backRow;
        private DispatcherTimer _timer;
        private GameStageManager _gameManager;
        private TimeOutLayerUI _timeOutLayer;
        private TrueFalseMarkUI _trueFalseMark;
        private PrepareLayerUI _prepareLayer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นของเกม State 2
        /// </summary>
        public SecondStatePage()
        {
            InitializeComponent();

            // ตัวนับเวลาก่อนเกมเริ่ม
            _prepareLayer = new PrepareLayerUI();
            LayoutRoot.Children.Add(_prepareLayer);

            // กำหนดตัวควบคุมเกม และ แถวหน้ากับแถวหลัง
            _gameManager = new GameStageManagerSecond();
            _fronRow = new RowUI();
            _backRow = new RowUI();

            // กำหนดตำแหน่งของแถวหน้า
            Canvas.SetTop(_fronRow, 35);

            // ย่อขนาดของแถวหลัง
            ScaleTransform scale = new ScaleTransform();
            scale.ScaleX = scale.ScaleX * 0.8;
            scale.ScaleY = scale.ScaleY * 0.8;
            Canvas.SetLeft(_backRow, 60);
            Canvas.SetTop(_backRow, 25);
            _backRow.RenderTransform = scale;

            // เครื่องหมายที่แสดงผลการตอบถูกหรือตอบผิด
            _trueFalseMark = new TrueFalseMarkUI();

            // กำหนดชนิดของแก้ว และทำการสุ่มลายแก้วที่จะนำมาใช้ในเกม
            _cupStyles = new string[] { "CylindricalCup", "TallCup", "TriangleCup" };
            _cupStyleName = randomCupStyle();

            // สร้างตัวจับเวลา
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(TimeTickSecond);
            _timer.Tick += new EventHandler(_timer_Tick);

            // นำเครื่องหมาย True Flase เข้ามา
            LayoutRoot.Children.Add(_trueFalseMark);

            // กำหนดเหตุการณ์ต่างๆในการเล่นเกม
            initializeEvents();

            // เริ่มเล่นตัวนับเวลาก่อนเข้าเล่นเกม
            _prepareLayer.Sb_Start.Begin();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ทำการเล่นคำถาม
        /// </summary>
        public void PlayQuestion()
        {
            _isWaitingClickForPlayQuestion = false;
            _fronRow.PlayCupDown();
            _backRow.PlayCupDown();
        }

        // สุ่มแก้วที่จะนำมาแสดงผลใน State นี้
        private string randomCupStyle()
        {
            const int MaximumStyle = 3;
            var _random = new Random();
            return _cupStyles[_random.Next(MaximumStyle)];
        }

        // เรียกคำถาม
        private void GetQuestion()
        {
            _isGetNextQuestion = false;
            _isWaitingClickForPlayQuestion = true;

            // เรียกคำถาม
            Question question = _gameManager.GetNextQuestion();
            string cupLevel = question.CupLevel;

            // กำหนดข้อมูลของแถวหน้าและแถวหลัง พร้อมกับกำหนดคำถาม
            _fronRow.SetQuestionRow(question.FrontRow, _cupStyleName, cupLevel);
            _backRow.SetQuestionRow(question.BackRow, _cupStyleName, cupLevel);
        }

        // กำหนดเหตุการณ์ต่างๆ
        private void initializeEvents()
        {
            // ตัวนับเวลาก่อนเริ่มเล่นเกม
            _prepareLayer.Sb_Start.Completed += new EventHandler(Sb_Start_Completed);

            // ทำการติดตามข้อมูลเมื่อมีการคลิกตัวแก้ว
            _fronRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);
            _backRow.ClickAnswer += new CupAnswerEventHandler(CheckAnswer);

            // กำหนดเหตุกาณ์ในการแสดงผลทักกี้ และสามเกลอ
            tukkyLose.PlayCompleted += new EventHandler(tukkyLose_PlayCompleted);
            tukkyWin.ThreeTopNormal.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopLose.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);
            tukkyWin.ThreeTopWin.PlayCompleted += new EventHandler(ThreeTop_PlayCompleted);

            // เครื่องหมายที่แสดงผลการตอบถูกหรือตอบผิด
            _trueFalseMark.Sb_Good.Completed += (s, e) => { _trueFalseMark.Sb_Good.Stop(); };
            _trueFalseMark.Sb_Fail.Completed += (s, e) => { _trueFalseMark.Sb_Fail.Stop(); };

            // เมื่อแก้วสลับเสร็จสิ้น
            _fronRow.SwapCompleted += new EventHandler(Row_SwapCompleted);
            _backRow.SwapCompleted += new EventHandler(Row_SwapCompleted);

            // คลิกเพื่อเล่นการแสดงคำถาม
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainPage_MouseLeftButtonDown);

            // กำหนดเหตุการ์ณ์ของนาฬิกาจับเวลา
            clock.Sb_Clock5.Completed += new EventHandler(Sb_Clock5_Completed);
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

                const int ResetAnswerCount = 0;
                if (result.IsCorrect == false)
                {
                    // จัดการตัวนับการตอบผิด
                    _incorrectCount++;
                    _correctCount = ResetAnswerCount;

                    // ตอบผิด ทำการเรียกคำถามใหม่
                    _isGetNextQuestion = true;

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่น animation
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopWin.StartPlay();

                    // แสดง animation การตอบผิด
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
                    scoreBoard.Sb_ScoreUp.Begin();

                    // แสดงผล animation ตอบถูกของ item
                    _fronRow.PlayAnswerResult(result);
                    _backRow.PlayAnswerResult(result);

                    // ตรวจสอบการจบระดับความยากนี้
                    if (result.IsFinish == true)
                    {
                        // ตอบถูก ทำการตรวจสอบการเลื่อนระดับความยาก
                        _isGetNextQuestion = true;

                        scoreBoard.Sb_RoundEnd.Stop();
                        scoreBoard.Sb_RoundEnd.Begin();
                    }

                    // กำหนดการแสดงผลของสามเกลอ และเริ่มเล่น animation
                    tukkyWin.ThreeTopWin.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopNormal.Visibility = System.Windows.Visibility.Collapsed;
                    tukkyWin.ThreeTopLose.Visibility = System.Windows.Visibility.Visible;
                    tukkyWin.ThreeTopLose.StartPlay();

                    // แสดง animation การตอบถูก
                    _trueFalseMark.Sb_Good.Begin();
                }

                // TODO: การเล่น animation ของนาฬิกา
                const int First = 1;
                const int Second = 2;
                const int Third = 3;
                const int Fourth = 4;
                const int Fifth = 5;
                if (_timeCombo == First)  clock.Sb_Clock1.Begin();
                else if (_timeCombo == Second)  clock.Sb_Clock2.Begin();
                else if (_timeCombo == Third)  clock.Sb_Clock3.Begin();
                else if (_timeCombo == Fourth)  clock.Sb_Clock4.Begin();
                else if (_timeCombo == Fifth)
                {
                    clock.Sb_Clock5.Begin();
                    clock.Sb_TimeUp.Begin();
                }

                // จัดการการแสดงผลของทักกี้
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

        // เมื่อเวลาเดิน 1 ติ๊ก
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

                // แสดงผล animation หมดเวลา
                _timeOutLayer = new TheS.SperfGames.MayaTukky.Controls.TimeOutLayerUI();
                LayoutRoot.Children.Add(_timeOutLayer);
                _timeOutLayer.Sb_TimeOut.Begin();
            }
        }

        // จัดการการแสดงผลของทักกี้
        private void tukkyLose_PlayCompleted(object sender, EventArgs e)
        {
            tukkyWin.Visibility = System.Windows.Visibility.Visible;
            tukkyLose.Visibility = System.Windows.Visibility.Collapsed;
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

        // กำหนดเหตุการณ์ของนาฬิกาจับเวลา
        private void Sb_Clock5_Completed(object sender, EventArgs e)
        {
            clock.Sb_Clock1.Stop();
            clock.Sb_Clock2.Stop();
            clock.Sb_Clock3.Stop();
            clock.Sb_Clock4.Stop();
            clock.Sb_Clock5.Stop();

        }

        // เมื่อแถวสลับเสร็จสิ้น
        private void Row_SwapCompleted(object sender, EventArgs e)
        {
            // แถวที่สลับเสร็จเป็นแถวหน้า
            if ((bool)sender)
            {

                tukkyHand.StopPlay();

                // กำหนดข้อมูลให้แถวหลัง
                _fronRow.SetAfterCupItem();
                _backRow.SetAfterCupItem();
            }
        }

        // เมื่อตัวนับเวลาก่อนเริ่มเล่นเกมจบลง
        private void Sb_Start_Completed(object sender, EventArgs e)
        {
            // เรียกขอคำถาม
            GetQuestion();

            // นำแถวหน้าและแถวหลังเข้ามาภายในตัวเกม
            LayoutRoot.Children.Add(_backRow);
            LayoutRoot.Children.Add(_fronRow);

            // เริ่มทำการจับเวลา
            _timer.Start();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        #endregion Methods

    }
}
