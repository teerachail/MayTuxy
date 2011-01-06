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
    /// ตัวควบคุม Stage 3
    /// </summary>
    public class GameStageManagerThird : GameStageManager
    {
        #region Constructors

        /// <summary>
        /// สร้างระดับความยากของเกมตอนเริ่มต้น
        /// </summary>
        public GameStageManagerThird()
        {
            // สร้างระดับความยากของเกมในช่วง (Level 1-12)
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
                // TODO: แก้ไขระดับความยากของเกม State 3 ใหม่
                new GameLevelThirdFix(3,0,050,1.000f,02,2,"1"),
                new GameLevelThirdFix(3,1,075,1.665f,02,2,"1"),
                new GameLevelThirdFix(3,2,100,1.940f,03,2,"1"),
                new GameLevelThirdFix(4,1,125,1.665f,04,3,"1"),
                new GameLevelThirdFix(4,2,150,1.940f,05,3,"2"),
                new GameLevelThirdFix(4,3,175,2.215f,06,3,"2"),
                new GameLevelThirdFix(5,2,200,1.940f,07,4,"2"),
                new GameLevelThirdFix(5,3,225,2.215f,08,4,"2"),
                new GameLevelThirdFix(5,4,250,2.490f,09,4,"3"),
                new GameLevelThirdFix(5,5,275,2.765f,10,4,"3"),
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

        // เพิ่มความยาก
        protected override GameLevel nextLevel()
        {
            GameLevel newGameLevel = null;
            _currentLevelIndex++;

            // ตรวจสอบช่วงระดับความยากของเกม เพื่อทำการสร้างระดับความยากใหม่
            const int MaximumLevel = 11;
            if (_currentLevelIndex <= MaximumLevel)
            {
                _gameLevels[_currentLevelIndex].IsLevelUp = true;
                newGameLevel = _gameLevels[_currentLevelIndex];
            }
            else
            {
                newGameLevel = new GameLevelThirdCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }
    }
}
