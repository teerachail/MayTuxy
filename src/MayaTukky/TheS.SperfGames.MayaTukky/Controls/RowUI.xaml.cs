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
    public partial class RowUI : UserControl
    {
        private const int CupElementIndex = 0;
        private string _cupRowState;
        private string _cupStyleName;
        private string _cupLevel;
        private CupUI _lastClickedCup;
        private Canvas[] _cupCanvases;
        private QuestionRow _question;
        //private List<Cup> _cups;

        /// <summary>
        /// ถ้วยที่เหลือ
        /// </summary>
        public int CurrentCup { get; set; }

        /// <summary>
        /// การสลับแก้วเสร็จสิ้น
        /// </summary>
        public static event EventHandler SwapCompleted;

        /// <summary>
        /// เมื่อมีการคลิกเพื่อทำการตอบคำถาม
        /// </summary>
        public event CupAnswerEventHandler ClickAnswer;

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับแถว
        /// </summary>
        public RowUI()
        {
            InitializeComponent();

            // กำหนดแก้วทั้งหมด
            //_cups = new List<Cup>()
            //{
            //    new Cup(),
            //    new Cup(),
            //    new Cup(),
            //    new Cup(),
            //    new Cup(),
            //};

            _cupCanvases = new Canvas[] {
                canvas1,
                canvas2,
                canvas3,
                canvas4,
                canvas5
            };
            foreach (var canvas in _cupCanvases) canvas.Children.Add(new CupUI());

            // กำหนดเหตุการณ์เมื่อแก้วถูกคลิก
            foreach (var canvas in _cupCanvases)
                (canvas.Children[CupElementIndex] as CupUI).Click += new CupAnswerEventHandler(OnClickAnswer);

            // กำหนดเหตุการณ์เมื่อแก้วถูกครอบเสร็จสิ้น
            (_cupCanvases.Last().Children[CupElementIndex] as CupUI).Sb_Down.Completed += new EventHandler(swapCup);

            // กำหนดเหตุการณ์เมื่อการสลับแก้วทั้งสองแบบจบลง
            Storyboard1.Completed += new EventHandler(swapCup);
            Storyboard2.Completed += new EventHandler(swapCup);
        }

        /// <summary>
        /// กำหนดคำถาม
        /// </summary>
        /// <param name="question">คำถาม</param>
        /// <param name="cupStyleName">ชนิดรูปทรงของแก้ว</param>
        /// <param name="cupLevel">ชนิดระดับความยากของแก้ว</param>
        public void SetQuestionRow(QuestionRow question, string cupStyleName, string cupLevel)
        {
            _cupStyleName = cupStyleName;
            _cupLevel = cupLevel;
            CurrentCup = question.BeforeCup.Count;
            _question = question;
            _lastClickedCup = null;

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
            setCupRowState();

            // เคลียแก้ว
            foreach (var canvas in _cupCanvases) (canvas.Children[CupElementIndex] as CupUI).ResetState();

            // เคลียแก้ว
            //foreach (var canvas in _cupCanvases) canvas.Children.Clear();
            //foreach (var cup in _cups) cup.ResetState();

            //// กำหนดค่าของแก้วที่จะนำมาแสดงผล
            //foreach (var itemName in _question.BeforeCup)
            //{
            //    Cup cup = new Cup();
            //    cup.Initialize(itemName, getCupStyleName());
            //    _cups.Add(cup);

            //}
            for (int canvasIndex = 0; canvasIndex < question.BeforeCup.Count; canvasIndex++)
            {
                (_cupCanvases[canvasIndex].Children[CupElementIndex] as CupUI)
                    .Initialize(_question.BeforeCup[canvasIndex], getCupStyleName());
            }
            for (int canvasIndex = question.BeforeCup.Count; canvasIndex < _cupCanvases.Count(); canvasIndex++)
            {
                (_cupCanvases[canvasIndex].Children[CupElementIndex] as CupUI).HasOpened = true;
            }

            //// นำแก้วที่ได้รับไปใส่ลงใน canvas
            //for (int cupIndex = 0; cupIndex < _cups.Count; cupIndex++)
            //{
            //    Canvas canvas = _cupCanvases[cupIndex];
            //    canvas.Children.Add(_cups[cupIndex]);
            //}

            //// กำหนดเหตุการณ์เมื่อแก้วถูกคลิก
            //foreach (var cup in _cups) cup.Click += new CupAnswerEventHandler(OnClickAnswer);

            //// กำหนดเหตุการณ์เมื่อแก้วถูกครอบเสร็จสิ้น
            //_cups.Last().Sb_Down.Completed += new EventHandler(swapCup);

            foreach (var canvas in _cupCanvases) (canvas.Children[CupElementIndex] as CupUI).Sb_ShowItem.Begin();
        }

        /// <summary>
        /// ทำการครอบแก้ว
        /// </summary>
        public void PlayCupDown()
        {
            //foreach (var cup in _cups) cup.CupDown();
            foreach (var canvas in _cupCanvases)
            {
                (canvas.Children[0] as CupUI).CupDown();
            }
        }

        /// <summary>
        /// กำหนดวัตถุที่อยู่ภายในแก้ว
        /// </summary>
        public void SetAfterCupItem()
        {
            //for (int cupIndex = 0; cupIndex < _cups.Count; cupIndex++)
            //{
            //    _cups[cupIndex].Initialize(_question.AfterCup[cupIndex], getCupStyleName());
            //}

            for (int canvasIndex = 0; canvasIndex < _question.AfterCup.Count; canvasIndex++)
            {
                (_cupCanvases[canvasIndex].Children[CupElementIndex] as CupUI)
                    .Initialize(_question.AfterCup[canvasIndex], getCupStyleName());
            }
        }

        /// <summary>
        /// ตรวจสอบคำตอบเพื่อใช้ในการแสดงผล
        /// </summary>
        /// <param name="result">ผลลัพธ์</param>
        public void PlayAnswerResult(AnswerResult result)
        {
            if (result.IsCorrect == true && _lastClickedCup != null)
            {
                _lastClickedCup.CupCorrect();
            }
            if (result.IsFinish)
            {
                _isFinish = true;
            }
        }

        // แสดงจำนวนแก้ว
        private void setCupRowState()
        {
            VisualStateManager.GoToState(this, _cupRowState, false);
        }

        // เกมจบลงแล้ว
        private bool _isFinish;

        // เปิดแก้วทุกใบ
        private void Sb_Correct_Completed(object sender, EventArgs e)
        {
            if (_isFinish)
            {
                _isFinish = false;

                foreach (var canvas in _cupCanvases)
                {
                    var cup = (canvas.Children[CupElementIndex] as CupUI);
                    if (!cup.HasOpened)
                        cup.CupUp();

                }

                //foreach (var cup in _cups)
                //{
                //    if (!cup.HasOpened)
                //    {
                //        cup.CupUp();
                //    }
                //}
                _lastClickedCup.Sb_Correct.Completed -= new EventHandler(Sb_Correct_Completed);
            }
        }

        // เล่นอนิเมชันการครอบแก้วเสร็จสิ้น
        private void swapCup(object sender, EventArgs e)
        {
            Storyboard1.Stop();
            Storyboard2.Stop();

            // กำหนดตำแหน่งของแก้วใหม่
            VisualStateManager.GoToState(this, "none", false);
            setCupRowState();

            // ตรวจสอบจำนวนคำถามที่เหลือ
            const int EmptyQueue = 0;
            if (_question.Sequence.Count > EmptyQueue)
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
                // สลับแก้วเสร็จหมด ทำการแจ้งว่าสามารถตอบคำถามได้แล้ว
                var temp = SwapCompleted;
                if (temp != null)
                {
                    temp(null, null);
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
            CurrentCup--;
            var temp = ClickAnswer;
            if (temp != null)
            {
                temp(sender, objName);
            }
            _lastClickedCup.Sb_Correct.Completed += new EventHandler(Sb_Correct_Completed);
        }
    }
}
