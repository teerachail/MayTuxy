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
    /// รายละเอียดของการ์ด
    /// </summary>
    public class CardInformation
    {
        #region Properties

        /// <summary>
        /// ความต้องการของคะแนน
        /// </summary>
        public double RequireScore { get; set; }

        /// <summary>
        /// ที่อยู่ของตำแหน่งภาพ
        /// </summary>
        public string ImageUrl { get; set; }

        #endregion Properties
    }
}
