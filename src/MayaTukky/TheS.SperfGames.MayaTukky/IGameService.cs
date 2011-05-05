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

namespace TheS.SperfGames.MayaTukky
{
    /// <summary>
    /// Game service
    /// </summary>
    public interface IGameService
    {
        #region Methods
        
        /// <summary>
        /// ร้องขอข้อมูลสถิติการเล่นเกม
        /// </summary>
        /// <param name="username">ชื่อผู้ใช้</param>
        /// <param name="callback">Call back</param>
        void RequestStatistics(string username, Action<PlayerStatistics> callback);

        /// <summary>
        /// ร้องขอตาราคะแนน
        /// </summary>
        /// <param name="callback">Call back</param>
        void RequestScoreTable(Action<ScoreTableResponse> callback);

        #endregion Methods
    }
}
