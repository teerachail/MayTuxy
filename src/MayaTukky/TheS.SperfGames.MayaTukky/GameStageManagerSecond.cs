﻿using System;
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
            // สร้างระดับความยากของเกมในช่วง (Level 1-10)
            _gameLevels = new System.Collections.Generic.List<GameLevel>()
            {
               new GameLevelSecondFix(3,0,2,3,0,050,1.0f,1,"1"),
               new GameLevelSecondFix(3,0,2,3,1,75,1.5f,2,"1"),
               new GameLevelSecondFix(4,0,3,4,0,100,2.0f,3,"1"),
               new GameLevelSecondFix(4,0,3,4,1,125,2.5f,4,"1"),
               new GameLevelSecondFix(4,0,3,4,2,150,3.0f,5,"2"),
               new GameLevelSecondFix(4,0,4,5,0,175,3.5f,6,"2"),
               new GameLevelSecondFix(4,0,4,5,1,200,4.0f,7,"3"),
               new GameLevelSecondFix(4,0,4,5,2,225,4.5f,8,"3"),
               new GameLevelSecondFix(5,0,4,5,1,250,5.0f,9,"4"),
               new GameLevelSecondFix(5,0,4,5,2,275,5.5f,10,"4"),
            };
            _currentLevel = _gameLevels.First();

            const int AddTimeSecond = 5;
            const float AddPluseCombo = 0.1f;
            _timeCombo = new TimeCombo(AddTimeSecond);
            _gameCombo = new GameCombo(AddPluseCombo);
        }

        #endregion Constructors

        #region Methods

        // เพิ่มความยาก
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
                newGameLevel = new GameLevelSecondCompute(true, _currentLevelIndex);
            }

            return newGameLevel;
        }

        // ลดความยาก
        protected override GameLevel previousLevel()
        {
            GameLevel newGameLevel = null;

            // ตรวจสอบการลดระดับความยากของเกม
            const int MinimumLevel = 0;
            if (_currentLevelIndex > MinimumLevel)
            {
                _currentLevelIndex--;
            }

            // ตรวจสอบช่วงระดับความยากของเกม เพื่อทำการสร้างระดับความยากใหม่
            const int MaximumLevel = 9;
            if (_currentLevelIndex <= MaximumLevel)
            {
                // ระดับความยากของเกมอยู่ในระดับมาตรฐานที่กำหนดไว้ (Level 1-10)
                _gameLevels[_currentLevelIndex].IsLevelUp = false;
                newGameLevel = _gameLevels[_currentLevelIndex];
            }
            else
            {
                // ระดับความยากของเกมเกินระดับมาตรฐานที่กำหนดไว้ (Level 10+)
                _currentLevel.IsLevelUp = false;
                newGameLevel = _currentLevel;
            }

            return newGameLevel;
        }

        #endregion Methods
    }
}