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
    /// ระดับความยากของ Stage 2 Fix
    /// </summary>
    public class GameLevelSecondFix : GameLevelSecond
    {
        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให่้กับ Stage 2 ชนิดที่มีการกำหนดไว้ก่อนแล้ว
        /// </summary>
        /// <param name="backCupCount">จำนวนแก้วแถวหลัง</param>
        /// <param name="backSwapCount">จำนวนครั้งที่ต้องทำการสลับแถวหลัง</param>
        /// <param name="maximumCorrect">จำนวนครั้งที่ต้องตอบถูกจึงจะผ่านระดับความยากนี้</param>
        /// <param name="cupCount">จำนวนแก้วแถวหน้า</param>
        /// <param name="swapCount">จำนวนครั้งที่ต้องทำการสลับแถวหน้า</param>
        /// <param name="currentPoint">คะแนนที่จะได้เมื่อผลลัพธ์ถูกต้อง</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="level">ระดับความยาก</param>
        /// <param name="cupLevel">ถ้วยที่จะนำมาใช้ในการแสดงผล</param>
        public GameLevelSecondFix(int backCupCount, int backSwapCount, int maximumCorrect, int cupCount, int swapCount, int currentPoint, float swapSpeed, int level, string cupLevel)
        {
            _backCupCount = backCupCount;
            _backSwapCount = backSwapCount;
            _maximumCorrect = maximumCorrect;
            _cupCount = cupCount;
            _swapSpeed = swapSpeed;
            _currentPoint = currentPoint;
            _swapCount = swapCount;
            _level = level;
            _cupLevel = cupLevel;
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
            return new GameRoundSecond(_currentPoint, _swapSpeed, _swapCount, _cupCount, _backSwapCount, _backCupCount, _maximumCorrect, _cupLevel);
        }

        #endregion Methods
    }
}
