﻿using System;
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
    /// ตัวจัดการการให้คะแนนพิเศษ
    /// </summary>
    public class GameCombo
    {
        #region Fields

        public float Pluse { get; set; }
        public float AddPluse { get; set; }
        private int _comboCount;

        #endregion Fields

        #region Methods

        /// <summary>
        /// นำผลลัพธ์ไปคำนวณการให้คะแนนเพิ่ม
        /// </summary>
        /// <param name="ans">ผลลัพธ์</param>
        /// <returns>ผลลัพธ์ที่ถูกคำนวณเวลาพิเศษแล้ว</returns>
        public AnswerResult Process(AnswerResult ans)
        {
            if (ans.IsCorrect != null)
            {
                const int ResetPluse = 0;
                if (ans.IsCorrect.Equals(true))
                {
                    // ตอบถูก
                    if (ans.IsFinish) _comboCount++;

                    ans.Score = ans.Score * (Pluse + (_comboCount * AddPluse));
                }
                else
                {
                    // ตอบผิด
                    _comboCount = ResetPluse;
                }
            }
            return ans;
        }

        #endregion Methods
    }
}
