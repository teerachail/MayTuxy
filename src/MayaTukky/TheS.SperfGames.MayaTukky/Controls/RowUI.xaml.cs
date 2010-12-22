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
using System.Windows.Markup;
using System.Text;
using TheS.SperfGames.MayaTukky.Controls;

namespace TheS.SperfGames.MayaTukky.Controls
{
    /// <summary>
    /// การแสดงผลของแถว
    /// </summary>
    public partial class RowUI : UserControl
    {
        #region Fields
        
        private const int ElementCupIndex = 0; // ตำแหน่งของแก้วที่อยู่ภายใน Canvas
        private string _cupRowState; // การแสดงผล state ของ cup
        private string _cupStyleName;
        private string _cupLevel;
        private CupUI _lastClickedCup;
        private Canvas[] _cupCanvases;
        private QuestionRow _question;
        private bool _isAutoAnswerOn;

        #endregion Fields

        #region Events
        
        /// <summary>
        /// การสลับแก้วเสร็จสิ้น
        /// </summary>
        public event EventHandler SwapCompleted;

        /// <summary>
        /// เมื่อมีการคลิกเพื่อทำการตอบคำถาม
        /// </summary>
        public event CupAnswerEventHandler ClickAnswer;

        /// <summary>
        /// แก้วที่อยู่ภายในแถวนี้
        /// </summary>
        public CupUI[] Cups
        {
            get
            {
                CupUI[] cups = new CupUI[_cupCanvases.Count()];
                for (int cupIndex = 0; cupIndex < _cupCanvases.Count(); cupIndex++)
                    cups[cupIndex] = _cupCanvases[cupIndex].Children[ElementCupIndex] as CupUI;

                return cups;
            }
        }

        #endregion Events

        #region Constructors
        
        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับแถว
        /// </summary>
        public RowUI()
        {
            InitializeComponent();

            // กำหนดแก้วทั้งหมด
            _cupCanvases = new Canvas[] {
                canvas1,
                canvas2,
                canvas3,
                canvas4,
                canvas5
            };
            foreach (var canvas in _cupCanvases) canvas.Children.Add(new CupUI());

            initializeEvents();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// กำหนดคำถาม
        /// </summary>
        /// <param name="question">คำถาม</param>
        /// <param name="cupStyleName">ชนิดรูปทรงของแก้ว</param>
        /// <param name="cupLevel">ชนิดระดับความยากของแก้ว</param>
        public void SetQuestionRow(QuestionRow question, string cupStyleName, string cupLevel)
        {
            _lastClickedCup = null;
            _cupStyleName = cupStyleName;
            _cupLevel = cupLevel;
            _question = question;

            // กำหนด state เริ่มต้นจากจำนวนแก้ว
            const int Easy = 3;
            const int Normal = 4;
            const int Hard = 5;
            switch (question.CupCount)
            {
                case Easy: _cupRowState = "threeCup"; break;
                case Normal: _cupRowState = "fourCup"; break;
                case Hard: _cupRowState = "fiveCup"; break;
                default: break;
            }

            // กำหนดการใช้งานของ state manager เพื่อกำหนดการแสดงผลของแก้วที่จะนำไปใช้งาน
            VisualStateManager.GoToState(this, _cupRowState, false);

            // เคลียแก้ว
            foreach (var canvas in _cupCanvases) (canvas.Children[ElementCupIndex] as CupUI).ResetState();


            // กำหนดลายแก้ว และวัตถุภายในแก้ว
            for (int canvasIndex = 0; canvasIndex < _cupCanvases.Count(); canvasIndex++)
            {
                if (canvasIndex < question.BeforeCup.Count)
                {
                    // กำหนดข้อมูลของแก้วที่นำไปแสดงผล
                    (_cupCanvases[canvasIndex].Children[ElementCupIndex] as CupUI)
                        .Initialize(_question.BeforeCup[canvasIndex], getCupStyleName());
                }
                else
                {
                    // กำหนดวัตถุที่ไม่ได้ถูกนำมาแสดงผลให้ไม่สามารถเปิดแก้วได้
                    (_cupCanvases[canvasIndex].Children[ElementCupIndex] as CupUI).HasOpened = true;
                }
            }

            // ทำการเปิดแสดงวัตถุที่อยู่ภายในแก้ว
            foreach (var canvas in _cupCanvases) (canvas.Children[ElementCupIndex] as CupUI).Sb_ShowItem.Begin();
        }

        /// <summary>
        /// ทำการครอบแก้ว
        /// </summary>
        public void PlayCupDown()
        {
            foreach (var canvas in _cupCanvases) (canvas.Children[0] as CupUI).CupDown();
        }

        /// <summary>
        /// กำหนดวัตถุที่อยู่ภายในแก้ว
        /// </summary>
        public void SetAfterCupItem()
        {
            for (int canvasIndex = 0; canvasIndex < _question.AfterCup.Count; canvasIndex++)
            {
                var cup = _cupCanvases[canvasIndex].Children[ElementCupIndex] as CupUI;
                cup.Initialize(_question.AfterCup[canvasIndex], getCupStyleName());
                cup.SetCupClick();
            }
        }

        /// <summary>
        /// ตรวจสอบคำตอบเพื่อใช้ในการแสดงผล
        /// </summary>
        /// <param name="result">ผลลัพธ์</param>
        public void PlayAnswerResult(AnswerResult result)
        {
            if (result.IsCorrect == true && _lastClickedCup != null)
                _lastClickedCup.CupCorrect();

            if (result.IsFinish) _isAutoAnswerOn = true;
        }

        // ทำการเปิดแก้วทุกใบ
        private void Sb_Correct_Completed(object sender, EventArgs e)
        {
            if (_isAutoAnswerOn)
            {
                _isAutoAnswerOn = false;

                foreach (var canvas in _cupCanvases)
                {
                    var cup = (canvas.Children[ElementCupIndex] as CupUI);
                    if (!cup.HasOpened) cup.CupUp();
                }
            }
        }

        // ทำการสลับแก้วหลังจากเล่นอนิเมชันการครอบแก้วเสร็จสิ้น
        private void swapCup(object sender, EventArgs e)
        {
            Storyboard1.Stop();
            Storyboard2.Stop();

            // กำหนดตำแหน่งของแก้วใหม่
            VisualStateManager.GoToState(this, "none", false);

            // กำหนดการใช้งานของ state manager เพื่อกำหนดการแสดงผลของแก้วที่จะนำไปใช้งาน
            VisualStateManager.GoToState(this, _cupRowState, false);

            // ตรวจสอบจำนวนคำถามที่เหลือ
            const int EmptyQuestion = 0;
            if (_question.Sequence.Count > EmptyQuestion)
            {
                // เริ่มทำการสลับแก้ว
                var sequence = _question.Sequence.Dequeue();

                // กำหนดแก้วที่จะนำมาทำการสลับ
                Canvas first = _cupCanvases[sequence.First];
                Canvas second = _cupCanvases[sequence.Second];

                // ตรวจสอบวิธีการสลับ
                if (sequence.First < sequence.Second) setStoryBoardFront(first, second);
                else setStoryBoardBack(first, second);
            }
            else
            {
                // กระจายข่าวว่า แถวนี้ทำการสลับแก้วเสร็จสิ้น
                var temp = SwapCompleted;
                if (temp != null)
                {
                    temp(_question.IsFronRow, null);
                }
            }
        }

        // สร้างที่อยู่ path ของชนิดลายแก้ว
        private string getCupStyleName()
        {
            const string concatenateCupName = "_";
            StringBuilder cupStyle = new StringBuilder();
            cupStyle.Append(_cupStyleName).Append(concatenateCupName).Append(_cupLevel);
            return cupStyle.ToString();
        }

        // สลับแก้วชนิด ทวนเข็ม
        private void setStoryBoardFront(Canvas can1, Canvas can2)
        {
            Storyboard.SetTargetName(DbKeyFrame1, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame2, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame3, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame4, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame5, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame6, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame7, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame8, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame9, can2.Name);

            EsKeyFrameX1.Value = Canvas.GetLeft(can2);
            EsKeyFrameX2.Value = Canvas.GetLeft(can1);

            Storyboard1.Begin();
        }

        // สลับแก้วชนิด ตามเข็ม
        private void setStoryBoardBack(Canvas can1, Canvas can2)
        {
            Storyboard.SetTargetName(DbKeyFrame10, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame11, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame12, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame13, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame14, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame15, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame16, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame17, can2.Name);
            Storyboard.SetTargetName(DbKeyFrame18, can1.Name);
            Storyboard.SetTargetName(DbKeyFrame19, can2.Name);

            EsKeyFrameX3.Value = Canvas.GetLeft(can2);
            EsKeyFrameX4.Value = Canvas.GetLeft(can1);

            Storyboard2.Begin();
        }

        // เมื่อแก้วถูกคลิก
        private void OnClickAnswer(object sender, CupAnswerEventArgs objName)
        {
            _lastClickedCup = (CupUI)sender;
            var temp = ClickAnswer;
            if (temp != null)
            {
                temp(sender, objName);
            }
            _lastClickedCup.Sb_Correct.Completed -= new EventHandler(Sb_Correct_Completed);
            _lastClickedCup.Sb_Correct.Completed += new EventHandler(Sb_Correct_Completed);
        }

        // กำหนดเหตุการณ์ต่างๆภายในเกม
        private void initializeEvents()
        {
            // กำหนดเหตุการณ์เมื่อแก้วถูกคลิก
            foreach (var canvas in _cupCanvases) (canvas.Children[ElementCupIndex] as CupUI).Click += new CupAnswerEventHandler(OnClickAnswer);

            // กำหนดเหตุการณ์เมื่อแก้วถูกครอบเสร็จสิ้น
            (_cupCanvases.Last().Children[ElementCupIndex] as CupUI).Sb_Down.Completed += new EventHandler(swapCup);

            // กำหนดเหตุการณ์เมื่อการสลับแก้วทั้งสองแบบจบลง
            Storyboard1.Completed += new EventHandler(swapCup);
            Storyboard2.Completed += new EventHandler(swapCup);
        }

        #endregion Methods
    }
}
