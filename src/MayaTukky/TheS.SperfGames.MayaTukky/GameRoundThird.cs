using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// รอบของเกม Stage 3
    /// </summary>
    public class GameRoundThird : GameRound
    {
        #region Fields

        private int _backCupCount;
        private int _maximumCorrect;
        private int _correctCount;
        private int _questionIndex;
        private bool _isIncorrect;

        #endregion Fields

        /// <summary>
        /// จำนวนครั้งที่ต้องตอบถูกจึงจะผ่านรอบเกมนี้
        /// </summary>
        public int MaximumCorrect
        {
            get { return _maximumCorrect; }
        }

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับรอบของเกม
        /// </summary>
        /// <param name="currentPoint">คะแนนที่ได้เมื่อตอบถูก</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับแก้วของแถวหน้า</param>
        /// <param name="cupCount">จำนวนแก้วของแถวหน้า</param>
        /// <param name="backCupCount">จำนวนแก้วของแถวหลัง</param>
        /// <param name="maximumCorrect">จำนวนครั้งที่ต้องตอบถูกจึงจะผ่านรอบเกมนี้</param>
        /// <param name="cupLevel">ถ้วยที่จะนำมาใช้ในการแสดงผล</param>
        public GameRoundThird(int currentPoint, float swapSpeed, int swapCount, int cupCount, int backCupCount, int maximumCorrect,string cupLevel)
            : base(currentPoint, swapSpeed, swapCount, cupCount)
        {
            _backCupCount = backCupCount;
            _maximumCorrect = maximumCorrect;
            _cupLevel = cupLevel;

            CreateQuestion();

            // TODO: Monster 3 error
            _items = new List<string>{
                "monster1",
                "monster2",
                "monster3",
                "monster4",
                "monster5",
                "monster6",
                "monster7",
                "monster8",
            };

            _items = _questionManager.CreateQuestionBefore(_items, cupCount);
            Question.FrontRow.BeforeCup = _questionManager.CreateQuestionBefore(_items, cupCount);
            Question.FrontRow.Sequence = _questionManager.CreateSwapSequence(cupCount, swapCount);
            Question.FrontRow.AfterCup = _questionManager.GetQuestionAfter(Question.FrontRow.BeforeCup, Question.FrontRow.Sequence);

            Question.BackRow.BeforeCup = _questionManager.CreateQuestionBefore(_items, backCupCount);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ตรวจคำตอบ
        /// </summary>
        /// <param name="objName">ชื่อของคำตอบที่ต้องการตรวจ</param>
        /// <returns>ผลลัพธ์ของการตรวจ</returns>
        public override AnswerResult CheckAnswer(string objName)
        {
            AnswerResult answer = new AnswerResult();

            if (!_isIncorrect)
            {
                if (objName.Equals(Question.BackRow.BeforeCup[_questionIndex]))
                {
                    // ตอบถูก
                    _questionIndex++;
                    _correctCount++;
                    answer.IsCorrect = true;
                    answer.Score = _currentPoint;

                    // ตรวจสอบการจบรอบเกมนี้
                    if ((_correctCount >= _maximumCorrect) && (!_isHasFinished))
                    {
                        answer.IsFinish = true;
                        _isHasFinished = true;
                    }
                }
                else
                {
                    // ตอบผิด
                    answer.IsCorrect = false;
                    _isIncorrect = true;
                } 
            }

            return answer;
        }

        /// <summary>
        /// สร้างคำถาม
        /// </summary>
        protected override void CreateQuestion()
        {
            Question = new Question
            {
                CupLevel = _cupLevel
            };

            const int backSwapCount = 0;
            Question.FrontRow = Question.AddQuestionRow(CupCount, SwapCount,_swapSpeed, true);
            Question.BackRow = Question.AddQuestionRow(CupCount, backSwapCount, _swapSpeed, false);
        }

        #endregion Methods
    }
}
