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
    /// รอบเกมของ Stage 2
    /// </summary>
    public class GameRoundSecond : GameRound
    {
        #region Fields

        private int _backSwapCount;
        private int _backCupCount;
        private int _maximumCorrect;
        private int _correctCount;
        private string _answer;
        private bool _isIncorrect;

        #endregion Fields

        #region Properties

        /// <summary>
        /// จำนวนครั้งในการสลับแก้วของแถวหลัง
        /// </summary>
        public int BackSwapCount
        {
            get { return _backSwapCount; }
        }

        /// <summary>
        /// จำนวนแก้วของแถวหลัง
        /// </summary>
        public int BackCupCount
        {
            get { return _backCupCount; }
        }

        /// <summary>
        /// จำนวนครั้งที่ต้องตอบถูกจึงจะผ่านรอบเกมนี้
        /// </summary>
        public int MaximumCorrect
        {
            get { return _maximumCorrect; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับรอบของเกม
        /// </summary>
        /// <param name="currentPoint">คะแนนที่ได้เมื่อตอบถูก</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับแก้วของแถวหน้า</param>
        /// <param name="cupCount">จำนวนแก้วของแถวหน้า</param>
        /// <param name="backSwapCount">จำนวนครั้งในการสลับแก้วของแถวหลัง</param>
        /// <param name="backCupCount">จำนวนแก้วของแถวหลัง</param>
        /// <param name="maximumCorrect">จำนวนครั้งที่ต้องตอบถูกจึงจะผ่านรอบเกมนี้</param>
        /// <param name="cupLevel">ชนิดลายแก้ว</param>
        public GameRoundSecond(int currentPoint, float swapSpeed, int swapCount, int cupCount, int backSwapCount, int backCupCount, int maximumCorrect, string cupLevel)
            : base(currentPoint, swapSpeed, swapCount, cupCount)
        {
            _backSwapCount = backSwapCount;
            _backCupCount = backCupCount;
            _maximumCorrect = maximumCorrect;
            _cupLevel = cupLevel;
            CreateQuestion();

            // TODO: Voodoo 7 error
            _items = new List<string>{
                "voodoo1",
                "voodoo2",
                "voodoo3",
                "voodoo4",
                "voodoo5",
                "voodoo6",
                //"voodoo7",
                "voodoo8",
            };
            
            _items = _questionManager.CreateQuestionBefore(_items, backCupCount);
            Question.BackRow.BeforeCup = _questionManager.CreateQuestionBefore(_items, backCupCount);
            Question.BackRow.Sequence = _questionManager.CreateSwapSequence(backCupCount, backSwapCount);
            Question.BackRow.AfterCup = _questionManager.GetQuestionAfter(Question.BackRow.BeforeCup, Question.BackRow.Sequence);

            const string Empty = "EmptyItem";
            if (cupCount != backCupCount)
            {
                _items.Add(Empty);
            }
            Question.FrontRow.BeforeCup = _questionManager.CreateQuestionBefore(_items, cupCount);
            Question.FrontRow.Sequence = _questionManager.CreateSwapSequence(cupCount, swapCount);
            Question.FrontRow.AfterCup = _questionManager.GetQuestionAfter(Question.FrontRow.BeforeCup, Question.FrontRow.Sequence);
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
            var answer = new AnswerResult();

            if ((!_isIncorrect))
            {
                if (string.IsNullOrEmpty(_answer)) _answer = objName;
                else
                {
                    if (_answer.Equals(objName))
                    {
                        // ตอบถูก
                        answer.IsCorrect = true;
                        answer.Score = _currentPoint;
                        _correctCount++;
                        _answer = string.Empty;

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
            }

            return answer;
        }

        /// <summary>
        /// สร้างคำถาม
        /// </summary>
        protected override void CreateQuestion()
        {
            Question = new Question()
            {
                CupLevel = _cupLevel
            };
            Question.FrontRow = Question.AddQuestionRow(_cupCount, _swapCount,_swapSpeed, true);
            Question.BackRow = Question.AddQuestionRow(_backCupCount, _backSwapCount, _swapSpeed, false);
        }

        #endregion Methods
    }
}
