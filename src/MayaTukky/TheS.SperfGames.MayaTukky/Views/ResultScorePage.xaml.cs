﻿using System;
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
using System.Windows.Navigation;
using TheS.SperfGames.MayaTukky.Controls;

namespace TheS.SperfGames.MayaTukky.Views
{
    /// <summary>
    /// หน้าคำนวณเวลาของเกม state 1
    /// </summary>
    public partial class ResultScorePage : Page
    {
        #region Fields

        private CloudUI _clound;

        #endregion Fields

        #region Events

        /// <summary>
        /// การคำนวณคะแนนเสร็จสิ้น
        /// </summary>
        public static event EventHandler CalculateScoreCompleted;

        #endregion Events

        public ResultScorePage()
        {
            InitializeComponent();
            Sb_Next.RepeatBehavior = RepeatBehavior.Forever;
            _clound = new CloudUI();
            LayoutRoot.Children.Add(_clound);

            poisonSeal.PlayCompleted += new EventHandler(poisonSeal_PlayCompleted);
            Sb_Result.Completed += new EventHandler(Sb_Result_Completed);
            _clound.Sb_CloudOut.Completed += new EventHandler(Sb_CloudOut_Completed);
            btn_Next.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Next_MouseLeftButtonDown);

            _clound.Sb_CloudOut.Begin();
        }

        /// <summary>
        /// แสดงผลเฉพาะเมื่อจบ State 1
        /// </summary>
        public void ShowTrophiesFirstState()
        {
            setItemCollectionsVisibility(
                System.Windows.Visibility.Collapsed, 
                System.Windows.Visibility.Visible, 
                System.Windows.Visibility.Collapsed
                );
        }

        /// <summary>
        /// แสดงผลเฉพาะเมื่อจบ State 2
        /// </summary>
        public void ShowTrophiesSecondState()
        {
            setItemCollectionsVisibility(
                System.Windows.Visibility.Visible,
                System.Windows.Visibility.Collapsed,
                System.Windows.Visibility.Collapsed
                );
        }

        /// <summary>
        /// แสดงผลเฉพาะเมื่อจบ State 3
        /// </summary>
        public void ShowTrophiesThirdState()
        {
            setItemCollectionsVisibility(
                System.Windows.Visibility.Collapsed,
                System.Windows.Visibility.Collapsed,
                System.Windows.Visibility.Visible
                );
        }

        private void setItemCollectionsVisibility(Visibility voodoo,Visibility poision,Visibility monster)
        {
            cv_VoodooItemCollection.Visibility = voodoo;
            cv_VoodooSeal.Visibility = voodoo;

            cv_MonsterItemCollection.Visibility = monster;
            cv_MonsterSeal.Visibility = monster;

            cv_PoisonItemCollection.Visibility = poision;
            cv_PoisonSeal.Visibility = poision;
        }


        private void btn_Next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Sb_Next.Stop();
            Sb_Result.Stop();
            var temp = CalculateScoreCompleted;
            if (temp != null)
            {
                temp(this, null);
            }
        }

        private void Sb_CloudOut_Completed(object sender, EventArgs e)
        {
            poisonSeal.StartPlay();
            Sb_Result.Begin();
        }

        private void Sb_Result_Completed(object sender, EventArgs e)
        {
            Sb_Next.Begin();
        }

        private void poisonSeal_PlayCompleted(object sender, EventArgs e)
        {
            poisonSeal.StartPlay();
        }


        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
