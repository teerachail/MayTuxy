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
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
                new GameLevelThirdFix(5,5,275,2.765f,10,4,"4"),
                new GameLevelThirdFix(3,0,050,1.000f,2,2,"1"),
                new GameLevelThirdFix(3,1,075,1.665f,2,2,"1"),
                new GameLevelThirdFix(3,2,100,1.940f,3,2,"1"),
                new GameLevelThirdFix(4,1,125,1.665f,4,3,"1"),
                new GameLevelThirdFix(4,2,150,1.940f,5,3,"2"),
                new GameLevelThirdFix(4,3,175,2.215f,6,3,"2"),
                new GameLevelThirdFix(5,2,200,1.940f,7,4,"3"),
                new GameLevelThirdFix(5,3,225,2.215f,8,4,"3"),
                new GameLevelThirdFix(5,4,250,2.490f,9,4,"4"),
                new GameLevelThirdFix(5,5,275,2.765f,10,4,"4"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 5;
            const float AddPluseCombo = 0.1f;
            _timeCombo = new TimeCombo(AddTimeSecond);
            _gameCombo = new GameCombo(AddPluseCombo);
        }

        #endregion Constructors

        //เพิ่มความยาก
        protected override GameLevel nextLevel()
        {
            GameLevel newGameLevel = null;
            const int MaximumLevel = 9;
            if (_currentLevelIndex <= MaximumLevel)
            {
                _currentLevelIndex++;
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
