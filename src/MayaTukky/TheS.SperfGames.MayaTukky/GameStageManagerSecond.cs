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
    /// ตัวควบคุม Stage 2
    /// </summary>
    public class GameStageManagerSecond : GameStageManager
    {
        #region Constructors

        /// <summary>
        /// สร้างระดับความยากของเกมตอนเริ่มต้น
        /// </summary>
        public GameStageManagerSecond()
        {
            // สร้างระดับความยากของเกมในช่วง (Level 1-12)
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
                // TODO: แก้ไขระดับความยากของเกม State 2 ใหม่
                new GameLevelSecondFix(3,0,2,3,0,050,1.0f,01,"1"),
                new GameLevelSecondFix(3,0,2,3,1,075,2.0f,02,"1"),
                new GameLevelSecondFix(4,0,3,4,0,100,1.0f,03,"1"),
                new GameLevelSecondFix(4,0,3,4,1,125,2.0f,04,"1"),
                new GameLevelSecondFix(4,0,3,4,2,150,4.0f,05,"2"),
                new GameLevelSecondFix(4,0,4,5,0,175,1.0f,06,"2"),
                new GameLevelSecondFix(4,0,4,5,1,200,2.0f,07,"2"),
                new GameLevelSecondFix(4,0,4,5,2,225,4.0f,08,"2"),
                new GameLevelSecondFix(5,0,4,5,1,250,2.0f,09,"3"),
                new GameLevelSecondFix(5,0,4,5,2,275,4.0f,10,"3"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 5;
            const float AddGameComboPluse = 0.05f;
            const float GameComboPluse = 0.95f;
            _timeCombo = new TimeCombo(AddTimeSecond);
            _gameCombo = new GameCombo
            {
                Pluse = GameComboPluse,
                AddPluse = AddGameComboPluse
            };
        }

        #endregion Constructors

        #region Methods

        // เพิ่มความยาก
        protected override GameLevel nextLevel()
        {
            GameLevel newGameLevel = null;
            _currentLevelIndex++;

            // ตรวจสอบช่วงระดับความยากของเกม เพื่อทำการสร้างระดับความยากใหม่
            const int MaximumLevel = 11;
            if (_currentLevelIndex <= MaximumLevel)
            {
                // ระดับความยากของเกมอยู่ในระดับมาตรฐานที่กำหนดไว้ (Level 1-12)
                _gameLevels[_currentLevelIndex].IsLevelUp = true;
                newGameLevel = _gameLevels[_currentLevelIndex];
            }
            else
            {
                // ระดับความยากของเกมเกินระดับมาตรฐานที่กำหนดไว้ (Level 13+)
                newGameLevel = new GameLevelSecondCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }

        #endregion Methods
    }
}
