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
    /// ระดับความยากของ Stage 1 Compute
    /// </summary>
    public class GameLevelFirstCompute : GameLevelFirst
    {
        #region Fields

        private const int AddPoint = 50;

        #endregion Fields

        /// <summary>
        /// กำหนดค่าเริ่มต้นให่้กับระดับความยาก Stage 1
        /// </summary>
        /// <param name="isLevelUp">เป็นการเพิ่มระดับความยากหรือไม่</param>
        /// <param name="level">ระดับความยาก</param>
        public GameLevelFirstCompute(bool isLevelUp, int level)
        {
            IsLevelUp = isLevelUp;
            _level = level;
        }

        /// <summary>
        /// สร้างรอบเกมใหม่
        /// </summary>
        /// <param name="previousGameRound">รอบของเกมก่อนหน้า</param>
        /// <returns>รอบเกมใหม่</returns>
        public override GameRound CreateGameRound(GameRound previousGameRound)
        {
            if (IsLevelUp) _level++;
            else _level--;
            int gamePoint = AddPoint + (_level * AddPoint);

            GameRoundFirst previous = previousGameRound as GameRoundFirst;
            _swapSpeed = previous.SwapSpeed + AddSwapSpeed;

            return new GameRoundFirst(gamePoint, _swapSpeed, previous.SwapCount, previous.CupCount,previous.CupLevel);
        }
    }
}
