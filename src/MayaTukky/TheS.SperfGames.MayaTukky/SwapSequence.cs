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
    /// ลำดับการสลับค่า
    /// </summary>
    public class SwapSequence
    {
        #region Properties

        /// <summary>
        /// ตัวที่จะสลับตัวแรก เริ่มจาก 0
        /// </summary>
        public int First { get; set; }

        /// <summary>
        /// ตัวที่จะสลับตัวที่สอง เริ่มจาก 0
        /// </summary>
        public int Second { get; set; }

        #endregion Properties
    }
}
