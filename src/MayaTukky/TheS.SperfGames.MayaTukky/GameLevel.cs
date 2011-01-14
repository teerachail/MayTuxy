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
    /// ระดับความยาก
    /// </summary>
    public abstract class GameLevel
    {
        #region Fields

        protected float MaximumSwapSpeed = 8;
        protected int _level;
        protected int _cupCount;
        protected int _swapCount;
        protected int _roundPoint;
        protected int _cupPoint;
        protected float _swapSpeed;
        protected string _cupLevel;

        #endregion Fields

        #region Properties

        /// <summary>
        /// ระดับความยากถูกเพิ่มขึ้น
        /// true: ถูกเพิ่ม
        /// </summary>
        public bool IsLevelUp { get; set; }

        #endregion Properties

        #region Methods

        // สร้างรอบเกมใหม่
        public abstract GameRound CreateGameRound(GameRound previousGameRound);

        #endregion Methods
    }
}
