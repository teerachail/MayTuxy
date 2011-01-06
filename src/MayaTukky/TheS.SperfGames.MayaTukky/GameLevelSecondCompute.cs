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
    /// ระดับความยากของ Stage 2 Compute
    /// </summary>
    public class GameLevelSecondCompute : GameLevelSecond
    {
        #region Fields

        private const int AddPoint = 2;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให่้กับระดับความยาก Stage 2 
        /// </summary>
        /// <param name="isLevelUp">เป็นการเพิ่มระดับความยากหรือไม่</param>
        /// <param name="level">ระดับความยาก</param>
        public GameLevelSecondCompute(bool isLevelUp, int level)
        {
            IsLevelUp = isLevelUp;
            _level = level;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// สร้างรอบเกมใหม่
        /// </summary>
        /// <param name="previousGameRound">รอบของเกมก่อนหน้า</param>
        /// <returns>รอบเกมใหม่</returns>
        public override GameRound CreateGameRound(GameRound previousGameRound)
        {
            if (IsLevelUp) _level++;
            else _level--;

            var previous = previousGameRound as GameRoundSecond;

            const int Point = 5;
            int gamePoint = Point + (_level * AddPoint);

            const string cupStyle = "4";
            return new GameRoundSecond(gamePoint, previous.SwapSpeed, previous.SwapCount
                , previous.CupCount, previous.BackSwapCount, previous.BackCupCount, previous.MaximumCorrect, cupStyle);
        }

        #endregion Methods
    }
}
