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
                // TODO: ระดับความยาก State 2 ยังไม่รอบรับการสลับระหว่างที่ทำการเลือกแก้ว
                new GameLevelSecondFix(2,0,1,2,3,0,10,1.000f,01,"1"),
                new GameLevelSecondFix(2,0,1,2,3,1,15,1.665f,02,"1"),
                new GameLevelSecondFix(3,0,2,3,3,0,20,1.000f,03,"1"),
                new GameLevelSecondFix(3,0,2,3,3,1,25,1.665f,04,"1"),
                new GameLevelSecondFix(3,0,2,3,3,1,30,1.665f,05,"2"),
                new GameLevelSecondFix(4,0,3,4,3,0,35,1.000f,06,"2"),
                new GameLevelSecondFix(4,0,3,4,3,1,401,1.665f,07,"2"),
                new GameLevelSecondFix(4,0,3,4,3,1,45,1.665f,08,"2"),
                new GameLevelSecondFix(4,0,3,4,3,2,50,1.665f,09,"3"),
                new GameLevelSecondFix(5,0,4,5,3,0,55,1.665f,10,"3"),
                new GameLevelSecondFix(5,0,4,5,3,1,60,1.665f,11,"3"),
                new GameLevelSecondFix(5,1,4,5,3,1,65,1.665f,12,"3"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 3;
            const int MaximumTimePluse = 5;
            const float AddGameComboPluse = 0.05f;
            const float GameComboPluse = 0.95f;
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
                newGameLevel = new GameLevelSecondCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }

        #endregion Methods
    }
}
