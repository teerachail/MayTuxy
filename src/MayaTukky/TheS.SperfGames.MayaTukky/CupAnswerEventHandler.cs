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
    public class CupAnswerEventArgs
    {
        public string ItemName { get; private set; }
        public AnswerResult AnswerResult { get; set; }

        public CupAnswerEventArgs(string itemName)
        {
            ItemName = itemName;
        }
    }

    public delegate void CupAnswerEventHandler(object sender, CupAnswerEventArgs e);
}
