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
using System.Windows.Media.Imaging;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// รายละเอียดของการ์ด
    /// </summary>
    public class CardInformation
    {
        #region Fields

        private const string ImageType = ".png";
      
        #endregion Fields
        
        #region Properties

        /// <summary>
        /// ความต้องการของคะแนน
        /// </summary>
        public double RequireScore { get; set; }

        /// <summary>
        /// ที่อยู่ของตำแหน่งภาพ
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Image display
        /// </summary>
        public BitmapImage ImageSource
        {
            get
            {
                return new BitmapImage(new Uri(string.Format("../Assets/Images/{0}{1}", ImageUrl,ImageType), UriKind.RelativeOrAbsolute));
            }
        }

        /// <summary>
        /// ระดับของการ์ด
        /// </summary>
        public int Rank { get; set; }
        

        #endregion Properties
    }
}
