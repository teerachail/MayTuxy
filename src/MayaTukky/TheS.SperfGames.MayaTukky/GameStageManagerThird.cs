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
                // TODO: ระดับความยาก State 3 ยังไม่รอบรับการสลับระหว่างที่ทำการเลือกแก้ว
                new GameLevelThirdFix(2,0,07,1.000f,01,1,"1"),
                new GameLevelThirdFix(2,1,09,1.665f,02,1,"1"),
                new GameLevelThirdFix(3,0,11,1.000f,03,2,"1"),
                new GameLevelThirdFix(3,1,13,1.665f,04,2,"1"),
                new GameLevelThirdFix(3,1,15,1.665f,05,2,"2"),
                new GameLevelThirdFix(4,0,17,1.000f,06,3,"2"),
                new GameLevelThirdFix(4,1,19,1.665f,07,3,"2"),
                new GameLevelThirdFix(4,1,21,1.665f,08,3,"2"),
                new GameLevelThirdFix(4,2,23,1.665f,09,3,"3"),
                new GameLevelThirdFix(5,0,25,1.665f,10,4,"3"),
                new GameLevelThirdFix(5,1,27,1.665f,11,4,"3"),
                new GameLevelThirdFix(5,2,29,1.665f,12,4,"3"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 3;
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
