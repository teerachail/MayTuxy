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
using System.Collections.Generic;

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// ตัวจัดการ Stage
    /// </summary>
    public abstract class GameStageManager
    {
        #region Fields

        protected const int MaximumIncorrectAnswer = 2;
        private const int StartTimeSecond = 6;
        protected float _score;
        protected string _username;
        private int _timeLeftSecond = StartTimeSecond;
        protected int _incorrectAnswer;
        protected int _currentLevelIndex;
        protected GameLevel _currentLevel;
        protected GameRound _gameRound;
        protected TimeCombo _timeCombo;
        protected GameCombo _gameCombo;
        protected List<GameLevel> _gameLevels;

        #endregion Fields

        #region Properties

        /// <summary>
        /// เวลาที่เหลือ
        /// </summary>
        public int TimeLeftSecond
        {
            get { return _timeLeftSecond; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// เรียกขอคำถาม
        /// </summary>
        /// <returns>คำถาม</returns>
        public Question GetNextQuestion()
        {
            _gameRound = _currentLevel.CreateGameRound(_gameRound);
            return _gameRound.Question;
        }

        /// <summary>
        /// ตรวจสอบคำตอบ
        /// </summary>
        /// <returns>ผลลัพธ์
        /// </returns>
        public AnswerResult CheckAnswer(string objName)
        {
            var result = _gameRound.CheckAnswer(objName);
            if (result.IsCorrect != null)
            {
                // นำผลลัพธ์ไปประมวลผลปรับปรุงค่าคะแนนพิเศษต่างๆ
                applyAnswer(result);

                const int ResetIncorrectAnswer = 0;
                if (result.IsCorrect.Equals(true))
                {
                    // ตอบถูก
                    _incorrectAnswer = ResetIncorrectAnswer;

                    if (result.IsFinish)
                    {
                        // จบเกมแล้ว
                        _currentLevel = nextLevel();
                    }
                }
                else
                {
                    // ตอบผิด
                    _incorrectAnswer++;
                    if (_incorrectAnswer >= MaximumIncorrectAnswer)
                    {
                        // ตอบผิดจนครบจำนวนครั้งที่กำหนด
                        _incorrectAnswer = ResetIncorrectAnswer;
                        _currentLevel = previousLevel();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// ตรวจเวลาหมด
        /// </summary>
        /// <returns>
        /// true: หมดเวลา
        /// false: ยังไม่หมดเวลา
        /// </returns>
        public bool Tick()
        {
            bool isTimeOver = false;
            _timeLeftSecond--;
            const int TimeOver = 0;
            if (_timeLeftSecond <= TimeOver) isTimeOver = true;

            return isTimeOver;
        }

        // นำผลลัพธ์ไปใช้งาน
        private void applyAnswer(AnswerResult ans)
        {
            // คำนวนคะแนนพิเศษจาก TimeCombo และ GameCombo
            ans = _timeCombo.Process(ans);
            ans = _gameCombo.Process(ans);

            // ปรับปรุงข้อมูลคะแนนและเวลาของเกม
            _score += ans.Score;
            _timeLeftSecond += ans.TimeAdvantage;
        }

        // ลดความยาก
        private GameLevel previousLevel()
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

        // เพิ่มความยาก
        protected abstract GameLevel nextLevel();

        #endregion Methods
    }
}
