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
    /// ระดับความยากของ Stage 1 Fix
    /// </summary>
    public class GameLevelFirstFix : GameLevelFirst
    {
        #region Constructor

        /// <summary>
        /// กำหนดค่าเริ่มต้นให่้กับ Stage 1 ชนิดที่มีการกำหนดไว้ก่อนแล้ว
        /// </summary>
        /// <param name="cupCount">จำนวนแก้วแถวหน้า</param>
        /// <param name="swapCount">จำนวนครั้งที่ต้องทำการสลับแถวหน้า</param>
        /// <param name="currentPoint">คะแนนที่จะได้เมื่อผลลัพธ์ถูกต้อง</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="level">ระดับความยาก</param>
        /// <param name="cupLevel">ถ้วยที่จะนำมาใช้ในการแสดงผล</param>
        public GameLevelFirstFix(int cupCount, int swapCount, int currentPoint, float swapSpeed, int level,string cupLevel)
        {
            _cupCount = cupCount;
            _swapCount = swapCount;
            _currentPoint = currentPoint;
            _swapSpeed = swapSpeed;
            _level = level;
            _cupLevel = cupLevel;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// สร้างรอบเกมใหม่
        /// </summary>
        /// <param name="previousGameRound">รอบของเกมก่อนหน้า</param>
        /// <returns>รอบเกมใหม่</returns>
        public override GameRound CreateGameRound(GameRound previousGameRound)
        {
            return new GameRoundFirst(_currentPoint, _swapSpeed, _swapCount, _cupCount, _cupLevel);
        }

        #endregion Methods
    }
}
