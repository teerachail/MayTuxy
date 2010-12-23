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
        /// <param name="cupLevel">ชนิดลายแก้ว</param>
        public GameRoundFirst(int currentPoint, float swapSpeed, int swapCount, int cupCount,string cupLevel)
            : base(currentPoint, swapSpeed, swapCount, cupCount)
        {
            _cupLevel = cupLevel;
            CreateQuestion();

            // TODO: Items name
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
            
            _items = _questionManager.CreateQuestionBefore(_items, cupCount,IncorrectAnswerItem);
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
            AnswerResult answer = new AnswerResult();

            if (objName.Equals(IncorrectAnswerItem))
            {
                // ตอบผิด
                answer.IsCorrect = false;
            }
            else
            {
                // ตอบถูก
                answer.IsCorrect = true;
            }

            answer.IsFinish = true;
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
            Question.FrontRow = Question.AddQuestionRow(CupCount, SwapCount, true);
        }

        #endregion Methods
    }
}
