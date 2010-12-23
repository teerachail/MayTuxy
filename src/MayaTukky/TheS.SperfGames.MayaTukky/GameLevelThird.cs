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
    /// ระดับความยากของ Stage 3
    /// </summary>
    public abstract class GameLevelThird : GameLevel
    {
        #region Fields

        protected int _backCupCount;
        protected int _backSwapCount;
        protected int _maximumCorrect;

        #endregion Fields
    }
}
