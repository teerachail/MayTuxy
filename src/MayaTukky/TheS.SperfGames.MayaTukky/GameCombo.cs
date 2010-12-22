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
    /// ตัวจัดการการให้คะแนนพิเศษ
    /// </summary>
    public class GameCombo
    {
        #region Fields

        private const int Pluse = 1;
        private readonly float AddPluse;
        private int _comboCount;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับตัวจัดการการให้คะแนนพิเศษ
        /// </summary>
        /// <param name="addPluse">ค่าของคะแนนที่จะถูกเพิ่มให้เมื่อตอบถูก</param>
        public GameCombo(float addPluse)
        {
            AddPluse = addPluse;
        }

        #endregion Constructors

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
                    ans.Score = ans.Score * (Pluse + (_comboCount * AddPluse));
                    _comboCount++;
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
