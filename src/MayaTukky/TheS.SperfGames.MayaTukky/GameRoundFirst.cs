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
    /// รอบเกมของ Stage 1
    /// </summary>
    public class GameRoundFirst : GameRound
    {
        #region Fields
        
        private const string IncorrectAnswerItem = "EmptyItem";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับรอบของเกม
        /// </summary>
        /// <param name="currentPoint">คะแนนที่ได้เมื่อตอบถูก</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับแก้ว</param>
        /// <param name="cupCount">จำนวนแก้ว</param>
        /// <param name="cupPoint">คะแนนที่ได้เมื่อตอบถูกต่อหนึ่งคู่</param>
        /// <param name="cupLevel">ชนิดลายแก้ว</param>
        public GameRoundFirst(int currentPoint, float swapSpeed, int swapCount, int cupCount,int cupPoint,string cupLevel)
            : base(currentPoint, swapSpeed, swapCount, cupCount,cupPoint)
        {
            _cupLevel = cupLevel;

            CreateQuestion();
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

            if (_isHasFinished == false)
            {
                _isHasFinished = true;

                if (objName.Equals(IncorrectAnswerItem))
                {
                    // ตอบผิด
                    answer.IsCorrect = false;
                }
                else
                {
                    // ตอบถูก
                    answer.IsCorrect = true;
                    answer.Score = _roundPoint;
                    answer.IsFinish = true;
                } 
            }

            return answer;
        }

        /// <summary>
        /// สร้างคำถาม
        /// </summary>
        protected override void CreateQuestion()
        {
            // สร้างคำถาม
            Question = new Question()
            {
                CupLevel = _cupLevel
            };
            Question.FrontRow = Question.AddQuestionRow(_cupCount, _swapCount,_swapSpeed, true);

            // กำหนดชื่อวัตถุที่อยู่ภายในแก้ว
            _items = new List<string>{
                "poison2",
                "poison6",
                "poison4",
            };

            int cupLevel = int.Parse(_cupLevel);
            const int Easy = 2;
            const int Normal = 3;
            const int Hard = 4;

            if(cupLevel >= Easy)
            {
                _items.Add("poison7");
                _items.Add("poison5");
            }

            if(cupLevel >= Normal)
            {
                _items.Add("poison8");
                _items.Add("poison3");
            }

            if (cupLevel >= Hard) _items.Add("poison1");
            
            _items = _questionManager.CreateQuestionBefore(_items, _cupCount, IncorrectAnswerItem);
            Question.FrontRow.BeforeCup = _questionManager.CreateQuestionBefore(_items, _cupCount);
            Question.FrontRow.Sequence = _questionManager.CreateSwapSequence(_cupCount, _swapCount);
            Question.FrontRow.AfterCup = _questionManager.GetQuestionAfter(Question.FrontRow.BeforeCup, Question.FrontRow.Sequence);
        }

        #endregion Methods
    }
}
