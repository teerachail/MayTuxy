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
using System.Linq;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// ตัวควบคุม Stage 1
    /// </summary>
    public class GameStageManagerFirst : GameStageManager
    {
        #region Constructors

        /// <summary>
        /// สร้างระดับความยากของเกมตอนเริ่มต้น
        /// </summary>
        public GameStageManagerFirst()
        {
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
                new GameLevelFirstFix(3,3,100,1.00f,1,"1"),
                new GameLevelFirstFix(3,4,150,1.35f,2,"1"),
                new GameLevelFirstFix(3,5,200,1.70f,3,"1"),
                new GameLevelFirstFix(4,4,250,1.35f,4,"1"),
                new GameLevelFirstFix(4,5,300,1.70f,5,"2"),
                new GameLevelFirstFix(4,6,350,2.00f,6,"2"),
                new GameLevelFirstFix(4,7,400,2.35f,7,"3"),
                new GameLevelFirstFix(5,5,450,1.70f,8,"3"),
                new GameLevelFirstFix(5,6,500,2.00f,9,"4"),
                new GameLevelFirstFix(5,7,550,2.35f,10,"4"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 10;
            const float AddPluseCombo = 0.2f;
            _timeCombo = new TimeCombo(AddTimeSecond);
            _gameCombo = new GameCombo(AddPluseCombo);
        }

        #endregion Constructors

        #region Methods

        //เพิ่มความยาก
        protected override GameLevel nextLevel()
        {
            GameLevel newGameLevel = null;
            _currentLevelIndex++;

            // ตรวจสอบช่วงระดับความยากของเกม เพื่อทำการสร้างระดับความยากใหม่
            const int MaximumLevel = 9;
            if (_currentLevelIndex <= MaximumLevel)
            {
                // ระดับความยากของเกมอยู่ในระดับมาตรฐานที่กำหนดไว้ (Level 1-10)
                _gameLevels[_currentLevelIndex].IsLevelUp = true;
                newGameLevel = _gameLevels[_currentLevelIndex];
            }
            else
            {
                // ระดับความยากของเกมเกินระดับมาตรฐานที่กำหนดไว้ (Level 10+)
                newGameLevel = new GameLevelFirstCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }

        #endregion Methods
    }
}
