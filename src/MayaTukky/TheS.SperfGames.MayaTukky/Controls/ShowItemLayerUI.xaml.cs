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

        #endregion Fields

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

        public void Initialize(List<string> question)
        {
            foreach (var canvas in _itemCanvas) canvas.Children.Clear();

            for (int itemIndex = 0; itemIndex < question.Count; itemIndex++)
            {
                UserControl item = (UserControl)Activator.CreateInstance(_items[question[itemIndex]]);
                _itemCanvas[itemIndex].Children.Add(item);
            }
        }
    }
}
