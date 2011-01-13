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
    /// ตัวจัดการการให้เวลาพิเศษ
    /// </summary>
    public class TimeCombo
    {
        #region Fields

        private readonly int MaximumPluse;
        private readonly int AddTime;
        private int _pluse;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับตัวจัดการการให้เวลาพิเศษ
        /// </summary>
        /// <param name="addTime">เวลาที่จะเพิ่มให้เมื่อตอบถูกครบจำนวนที่กำหนด</param>
        /// <param name="maximumPluse">จำนวนครั้งที่จะได้เวลาเพิ่ม</param>
        public TimeCombo(int addTime,int maximumPluse)
        {
            AddTime = addTime;
            MaximumPluse = maximumPluse;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// นำผลลัพธ์ไปคำนวณการให้เวลาเพิ่ม
        /// </summary>
        /// <param name="ans">ผลลัพธ์</param>
        /// <returns>ผลลัพธ์ที่ถูกคำนวณเวลาพิเศษแล้ว</returns>
        public AnswerResult Process(AnswerResult ans)
        {
            if (ans.IsCorrect != null)
            {
                if (ans.IsCorrect.Equals(true)) _pluse++;

                ans.TimeCombo = _pluse;

                const int ResetPluse = 0;
                if (_pluse >= MaximumPluse)
                {
                    _pluse = ResetPluse;
                    ans.TimeAdvantage = AddTime;
                }
            }
            return ans;
        }

        #endregion Methods
    }
}
