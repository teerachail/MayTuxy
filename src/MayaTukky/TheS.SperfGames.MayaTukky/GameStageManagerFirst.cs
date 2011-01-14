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
            // สร้างระดับความยากของเกมในช่วง (Level 1-12)
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
                new GameLevelFirstFix(3,03,13,0,1.665f,01,"1"),
                new GameLevelFirstFix(3,04,16,0,2.220f,02,"1"),
                new GameLevelFirstFix(3,05,19,0,2.775f,03,"1"),
                new GameLevelFirstFix(3,06,22,0,3.330f,04,"1"),
                new GameLevelFirstFix(4,07,25,0,3.885f,05,"2"),
                new GameLevelFirstFix(4,08,28,0,4.440f,06,"2"),
                new GameLevelFirstFix(4,09,31,0,4.995f,07,"2"),
                new GameLevelFirstFix(4,10,34,0,5.550f,08,"2"),
                new GameLevelFirstFix(5,11,37,0,6.105f,09,"3"),
                new GameLevelFirstFix(5,12,40,0,6.670f,10,"3"),
                new GameLevelFirstFix(5,13,43,0,7.225f,11,"3"),
                new GameLevelFirstFix(5,14,46,0,7.780f,12,"3"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 6;
            const int MaximumTimePluse = 5;
            const float AddGameComboPluse = 0.1f;
            const float GameComboPluse = 0.9f;
            _timeCombo = new TimeCombo(AddTimeSecond,MaximumTimePluse);
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
                newGameLevel = new GameLevelFirstCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }

        #endregion Methods
    }
}
