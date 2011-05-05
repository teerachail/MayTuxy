using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// สถิติการใช้งานของผู้เล่น
    /// </summary>
    public class PlayerStatistics
    {
        /// <summary>
        /// คะแนนเฉลี่ยของด่าน 1
        /// </summary>
        public int FirstAveragePoint { get; set; }
        
        /// <summary>
        /// คะแนนสูงสุดของด่าน 1
        /// </summary>
        public int FirstHighScorePoint { get; set; }

        /// <summary>
        /// คะแนนเฉลี่ยของด่าน 2
        /// </summary>
        public int SecondAveragePoint { get; set; }

        /// <summary>
        /// คะแนนสูงสุดของด่าน 2
        /// </summary>
        public int SecondHighScorePoint { get; set; }

        /// <summary>
        /// คะแนนเฉลี่ยของด่าน 3
        /// </summary>
        public int ThirdAveragePoint { get; set; }

        /// <summary>
        /// คะแนนสูงสุดของด่าน 3
        /// </summary>
        public int ThirdHighScorePoint { get; set; }

        /// <summary>
        /// คะแนนที่ได้เมื่อเทียบกับผู้เล่นทั้งหมด
        /// </summary>
        public double Percentile { get; set; }

        /// <summary>
        /// จำนวนครั้งที่เล่น
        /// </summary>
        public int Time { get; set; }
    }
}
