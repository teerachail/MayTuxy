using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PerfEx.Infrastructure;

namespace TheS.SperfGames.MayaTukky.Controls
{
    public partial class ItemReward : UserControl,IAnime
    {
        public ItemReward()
        {
            InitializeComponent();
            Sb_ItemMove.RepeatBehavior = RepeatBehavior.Forever;
            Sb_ShowItemReward.Completed += new EventHandler(Sb_ShowItemReward_Completed);
        }

        private void Sb_ShowItemReward_Completed(object sender, EventArgs e)
        {
            StopPlay();
            var temp = PlayCompleted;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        #region IAnime Members

        public string AnimationName
        {
            get;
            set;
        }

        public bool AutoPlay
        {
            get;
            set;
        }

        public TimeSpan? BeginTime
        {
            get
            {
                return Sb_ShowItemReward.BeginTime;
            }
            set
            {
                Sb_ShowItemReward.BeginTime = value;
            }
        }

        public event EventHandler PlayCompleted;

        public double SpeedRatio
        {
            get
            {
                return Sb_ShowItemReward.SpeedRatio;
            }
            set
            {
                Sb_ItemMove.SpeedRatio = value;
                Sb_ShowItemReward.SpeedRatio = value;
            }
        }

        public void StartPlay()
        {
            Sb_ShowItemReward.Begin();
            Sb_ItemMove.Begin();
        }

        public void StopPlay()
        {
            Sb_ShowItemReward.Stop();
            Sb_ShowItemReward.Stop();
        }

        #endregion
    }
}
