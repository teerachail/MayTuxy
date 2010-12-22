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

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// คำถาม
    /// </summary>
    public class Question
    {
        #region Properties

        /// <summary>
        /// ชนิดลายแก้ว
        /// </summary>
        public string CupLevel { get; set; }

        /// <summary>
        /// คำถามของแถวหน้า
        /// </summary>
        public QuestionRow FrontRow { get; set; }

        /// <summary>
        /// คำถามของแถวหลัง
        /// </summary>
        public QuestionRow BackRow { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// สร้างคำถาม
        /// </summary>
        /// <param name="cupCount">จำนวนของแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับ</param>
        /// <param name="isFrontRow">ชนิดแถว
        /// true: แถวหน้า
        /// false: แถวหลัง
        /// </param>
        /// <returns></returns>
        public QuestionRow AddQuestionRow(int cupCount, int swapCount, bool isFrontRow)
        {
            return new QuestionRow
            {
                CupCount = cupCount,
                SwapCount = swapCount,
                IsFronRow = isFrontRow
            };
        }

        #endregion Methods
    }
}
