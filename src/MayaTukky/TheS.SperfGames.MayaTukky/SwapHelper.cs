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
using System.Linq;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// ตัวช่วยสร้างการสลับแก้ว
    /// </summary>
    public class SwapHelper
    {
        private Random _randomElementIndex;

        /// <summary>
        /// กำหนดค่าเริ่มต้นที่ใช้ในการสร้างการสลับแก้ว
        /// </summary>
        public SwapHelper()
        {
            _randomElementIndex = new Random();
        }

        /// <summary>
        /// สร้างคำถามก่อนทำการสลับ
        /// </summary>
        /// <param name="itemNames">รายชื่อสิ่งของที่นำมาแสดงผล</param>
        /// <param name="cupCount">จำนวนแก้ว</param>
        /// <returns>คำถามก่อนการสลับแก้ว</returns>
        public List<string> CreateQuestionBefore(IEnumerable<string> itemNames, int cupCount)
        {
            if (itemNames.Count() < cupCount) throw new ArgumentException("List of name can't low than cup count.");

            var sourceNames = itemNames.ToList<string>();
            var result = new List<string>();
            while (result.Count < cupCount)
            {
                int elementIndex = _randomElementIndex.Next(sourceNames.Count);
                result.Add(sourceNames[elementIndex]);
                sourceNames.Remove(sourceNames[elementIndex]);
            }
            return result;
        }

        /// <summary>
        /// สร้างลำดับการสลับแก้ว
        /// </summary>
        /// <param name="cupCount">จำนวนแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับ</param>
        /// <returns>ลำดับในการสลับแก้ว</returns>
        public Queue<SwapSequence> CreateSwapSequence(int cupCount, int swapCount)
        {
            const int CanNotFindAnswer = 1;
            if (cupCount <= CanNotFindAnswer) throw new ArgumentException("Cup count can't low than 2");
            var result = new Queue<SwapSequence>();

            while (result.Count < swapCount)
            {
                int first = _randomElementIndex.Next(cupCount);
                int second = _randomElementIndex.Next(cupCount);
                while (first == second) second = _randomElementIndex.Next(cupCount);

                result.Enqueue(new SwapSequence { First = first, Second = second });
            }

            return result;
        }

        /// <summary>
        /// หาผลลัพธ์จากลำดับในการสลับแก้ว
        /// </summary>
        /// <param name="questionBefore">คำถามก่อนการสลับแก้ว</param>
        /// <param name="swapSequences">ลำดับการสลับแก้ว</param>
        /// <returns>ผลลัพธ์หลังจากสลับแก้ว</returns>
        public List<string> GetQuestionAfter(IEnumerable<string> questionBefore, IEnumerable<SwapSequence> swapSequences)
        {
            var sourceItem = questionBefore.ToList<string>();

            foreach (var sequence in swapSequences)
            {
                var itemNameBuffer = sourceItem[sequence.First];
                sourceItem[sequence.First] = sourceItem[sequence.Second];
                sourceItem[sequence.Second] = itemNameBuffer;
            }

            return sourceItem;
        }
    }
}
