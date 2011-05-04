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

namespace TheS.SperfGames.MayaTukky
{
    public static class GlobalScore
    {
        public static int FirstScore { get; set; }
        public static int SecondScore { get; set; }
        public static int ThirdScore { get; set; }

        public static int FirstMaximumCombo { get; set; }
        public static int SecondMaximumCombo { get; set; }
        public static int ThirdMaximumCombo { get; set; }

        public static int FirstCorrectAnswer { get; set; }
        public static int SecondCorrectAnswer { get; set; }
        public static int ThirdCorrectAnswer { get; set; }

        public static int FirstIncorrectAnswer { get; set; }
        public static int SecondIncorrectAnswer { get; set; }
        public static int ThirdIncorrectAnswer { get; set; }

        public static bool FirstCompleted { get; set; }
        public static bool SecondCompleted { get; set; }
        public static bool ThirdCompleted { get; set; }

        static GlobalScore()
        {
            // TODO: ตัวอย่างให้พี่พายตรวจสอบการเล่น animation เมื่อตรวจแล้วให้ลบ Constructor นี้ทิ้ง
            //FirstScore = 50;
            //SecondScore= 100;
            //ThirdScore = 553;

            FirstScore = 600;
            SecondScore = 600;
            ThirdScore = 600;

            //FirstScore = 56;
            //SecondScore = 192;
            //ThirdScore = 423;
        }
    }
}
