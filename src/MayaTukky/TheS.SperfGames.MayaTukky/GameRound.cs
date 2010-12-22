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
    /// รอบเกม
    /// </summary>
    public abstract class GameRound
    {
        #region Fields

        protected int _currentPoint;
        protected float _swapSpeed;
        protected int _swapCount;
        protected int _cupCount;
        protected string _cupLevel;
        protected List<string> _items;
        protected SwapHelper _questionManager;

        #endregion Fields

        #region Properties

        /// <summary>
        /// จำนวนครั้งในการสลับแก้วด้านหน้า
        /// </summary>
        public int SwapCount
        {
            get { return _swapCount; }
        }

        /// <summary>
        /// จำนวนแก้วด้านหน้า
        /// </summary>
        public int CupCount
        {
            get { return _cupCount; }
        }

        /// <summary>
        /// คะแนนที่ได้เมื่อชนะในระดับความยากนี้
        /// </summary>
        public int CurrentPoint
        {
            get { return _currentPoint; }
        }

        /// <summary>
        /// ความเร๋็วในการสลับแก้วในหน้า
        /// </summary>
        public float SwapSpeed
        {
            get { return _swapSpeed; }
        }

        /// <summary>
        /// ชนิดลายแก้วที่นำมาแสดงผล
        /// </summary>
        public string CupLevel
        {
            get { return _cupLevel; }
        }

        /// <summary>
        /// คำถาม
        /// </summary>
        public Question @Question { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับรอบของเกม
        /// </summary>
        /// <param name="currentPoint">คะแนนที่ได้เมื่อตอบถูก</param>
        /// <param name="swapSpeed">ความเร็วในการสลับแก้ว</param>
        /// <param name="swapCount">จำนวนครั้งในการสลับแก้ว</param>
        /// <param name="cupCount">จำนวนแก้ว</param>
        public GameRound(int currentPoint, float swapSpeed, int swapCount, int cupCount)
        {
            _questionManager = new SwapHelper();
            _currentPoint = currentPoint;
            _swapSpeed = swapSpeed;
            _swapCount = swapCount;
            _cupCount = cupCount;
        }

        #endregion Constructors

        #region Methods

        // ตรวจคำตอบ
        public abstract AnswerResult CheckAnswer(string objName);

        // สร้างคำถาม
        protected abstract void CreateQuestion();

        #endregion Methods
    }
}
