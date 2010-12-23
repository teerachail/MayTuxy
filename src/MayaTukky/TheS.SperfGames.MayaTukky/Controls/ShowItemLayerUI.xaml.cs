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

namespace TheS.SperfGames.MayaTukky.Controls
{
    public partial class ShowItemLayerUI : UserControl
    {
        #region Fields

        private Dictionary<string, Type> _items;
        private Canvas[] _itemCanvas;
        private string _cupRowState;
        private int _correctCount;

        #endregion Fields

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับตัวแสดงคำถาม
        /// </summary>
        public ShowItemLayerUI()
        {
            InitializeComponent();

            _itemCanvas = new Canvas[]{
                cv_ShowItemOne,
                cv_ShowItemTwo,
                cv_ShowItemThree,
                cv_ShowItemFour,
                cv_ShowItemFive,
            };

            _items = new Dictionary<string, Type>()
            {
                { "voodoo1", typeof(testVoodooAnime.Voodoo.Voodoo1.Voodoo1_Place) },
                { "voodoo2", typeof(testVoodooAnime.Voodoo.Voodoo2.Voodoo2_Place) },
                { "voodoo3", typeof(testVoodooAnime.Voodoo.Voodoo3.Voodoo3_Place) },
                { "voodoo4", typeof(testVoodooAnime.Voodoo.Voodoo4.Voodoo4_Place) },
                { "voodoo5", typeof(testVoodooAnime.Voodoo.Voodoo5.Voodoo5_Place) },
                { "voodoo6", typeof(testVoodooAnime.Voodoo.Voodoo6.Voodoo6_Place) },
                { "voodoo7", typeof(testVoodooAnime.Voodoo.Voodoo7.Voodoo7_Place) },
                { "voodoo8", typeof(testVoodooAnime.Voodoo.Voodoo8.Voodoo8_Place) },

                { "monster1", typeof(MonsterAnimation.Item.Monster_Crab.Crab) },
                { "monster2", typeof(MonsterAnimation.Item.Monster_Duckking.Duckking) },
                { "monster3", typeof(MonsterAnimation.Item.Monster_GearDodo.GearDodo) },
                { "monster4", typeof(MonsterAnimation.Item.Monster_GhostJellyFish.GhostJellyFish) },
                { "monster5", typeof(MonsterAnimation.Item.Monster_Snail.Snail) },
                { "monster6", typeof(MonsterAnimation.Item.Monster_Spider.Spider) },
                { "monster7", typeof(MonsterAnimation.Item.Monster_Squdy.Squdy) },
                { "monster8", typeof(MonsterAnimation.Item.Monster_TheTree.TheTree) },

                { "poison1", typeof(PoisonAnimation.Item.Poison1) },
                { "poison2", typeof(PoisonAnimation.Item.Poison2) },
                { "poison3", typeof(PoisonAnimation.Item.Poison3) },
                { "poison4", typeof(PoisonAnimation.Item.Poison4) },
                { "poison5", typeof(PoisonAnimation.Item.Poison5) },
                { "poison6", typeof(PoisonAnimation.Item.Poison6) },
                { "poison7", typeof(PoisonAnimation.Item.Poison7) },
                { "poison8", typeof(PoisonAnimation.Item.Poison8) },

                { "EmptyItem", typeof(Controls.EmptyItemUI) },
            };
        }

        /// <summary>
        /// กำหนดการแสดงผลของคำถาม
        /// </summary>
        /// <param name="question">คำถามที่ต้องการนำมาแสดง</param>
        public void SetQuestion(List<string> question)
        {
            const int Reset = 0;
            _correctCount = Reset;
            resetAnimation();

            foreach (var canvas in _itemCanvas) canvas.Children.Clear();

            const int Easy = 3;
            const int Normal = 4;
            const int Hard = 5;

            switch (question.Count)
            {
                case Easy: _cupRowState = "threeCup"; break;
                case Normal: _cupRowState = "fourCup"; break;
                case Hard: _cupRowState = "fiveCup"; break;
                default: break;
            }

            // กำหนดการใช้งานของ state manager เพื่อกำหนดการแสดงผลของแก้วที่จะนำไปใช้งาน
            VisualStateManager.GoToState(this, "none", false);
            VisualStateManager.GoToState(this, _cupRowState, false);

            for (int itemIndex = 0; itemIndex < question.Count; itemIndex++)
            {
                UserControl item = (UserControl)Activator.CreateInstance(_items[question[itemIndex]]);
                _itemCanvas[itemIndex].Children.Add(item);
            }
        }

        /// <summary>
        /// แสดงการเล่นอนิเมชันเมื่อตอบถูก
        /// </summary>
        /// <param name="result">ผลลัพธ์ของการเล่นเกม</param>
        public void PlayAnswerResult(AnswerResult result)
        {
            if (result.IsCorrect == true)
            {
                _correctCount++;

                const int GotoQuestionTwo = 1;
                const int GotoQuestionThree = 2;
                const int GotoQuestioFour = 3;
                const int GotoQuestionFive = 4;
                const int Finish = 5;

                switch (_correctCount)
                {
                    case GotoQuestionTwo: Sb_NextItemTwo.Begin(); break;
                    case GotoQuestionThree: Sb_NextItemThree.Begin(); break;
                    case GotoQuestioFour: Sb_NextItemFour.Begin(); break;
                    case GotoQuestionFive: Sb_NextItemFive.Begin(); break;
                    case Finish: Sb_FadeAway.Begin(); break;
                    default: break;
                }
            }
        }

        // กำหนดการแสดงคำถามใหม่
        private void resetAnimation()
        {
            Sb_FadeAway.Stop();
            Sb_NextItemFive.Stop();
            Sb_NextItemFour.Stop();
            Sb_NextItemThree.Stop();
            Sb_NextItemTwo.Stop();
            Sb_ShowLayer.Stop();
        }
    }
}
