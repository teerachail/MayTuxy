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
    /// รายละเอียดคำถามในแต่ละแถว
    /// </summary>
    public class QuestionRow
    {
        #region Properties

        /// <summary>
        /// จำนวนแก้ว
        /// </summary>
        public int CupCount { get; set; }

        /// <summary>
        /// จำนวนครั้งในการสลับ
        /// </summary>
        public int SwapCount { get; set; }

        /// <summary>
        /// ลำดับก่อนการสลับ
        /// </summary>
        public List<string> BeforeCup { get; set; }

        /// <summary>
        /// ลำดับหลังจากมีการสลับ
        /// </summary>
        public List<string> AfterCup { get; set; }

        /// <summary>
        /// ลำดับในการสลับ
        /// </summary>
        public Queue<SwapSequence> Sequence { get; set; }

        /// <summary>
        /// แถวที่คำถามนี้อยู่
        /// true: แถวหน้า--
        /// false: แถวหลัง
        /// </summary>
        public bool IsFronRow { get; set; }

        #endregion Properties

        public QuestionRow()
        {
            AfterCup = new List<string>(CupCount);
            BeforeCup = new List<string>(CupCount);
            Sequence = new Queue<SwapSequence>();
        }
    }
}
