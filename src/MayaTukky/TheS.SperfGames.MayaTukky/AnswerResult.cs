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
    /// ผลลัพธ์ของคำตอบ
    /// </summary>
    public class AnswerResult
    {
        #region Properties

        /// <summary>
        /// ผลลัพธ์ของคำตอบ
        /// true: ถูก
        /// false: ผิด
        /// null: ยังไม่สามารถสรุปได้
        /// </summary>
        public bool? IsCorrect { get; set; }

        /// <summary>
        /// คะแนนที่ได้
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// เวลาที่เพิ่มขึ้น
        /// </summary>
        public int TimeAdvantage { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ได้ combo
        /// </summary>
        public int TimeCombo { get; set; }

        /// <summary>
        /// จบรอบเกมนี้แล้วหรือไม่
        /// true: จบแล้ว
        /// false: ยังไม่จบ
        /// </summary>
        public bool IsFinish { get; set; }

        #endregion Properties
    }
}
