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

namespace TheS.SperfGames.MayaTukky.Models
{
    public class CupItems : UserControl
    {
        private Dictionary<string, Type> _itemTypes = new Dictionary<string, Type>()
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

        Dictionary<string, UserControl> _cachedItems = new Dictionary<string, UserControl>();

        public UserControl this[string index]
        {
            get {
                const int MaxEmptyItemSuffix = 7;
                const string EmptyItemKey = "EmptyItem";

                if (!_cachedItems.ContainsKey(index))
                {
                    var uc = Activator.CreateInstance(_itemTypes[index]) as UserControl;
                    _cachedItems[index] = uc;

                    if (index == EmptyItemKey)
                    {
                        for (int i = 0; i < MaxEmptyItemSuffix; i++)
                        {
                            _cachedItems[index + i] = Activator.CreateInstance(_itemTypes[index]) as UserControl;
                        }
                    }
                }

                var item = _cachedItems[index];
                if (index == EmptyItemKey && item.Parent != null)
                {
                    for (int i = 0; i < MaxEmptyItemSuffix; i++)
                    {
                        item = _cachedItems[index + i];
                        if (item.Parent == null)
                        {
                            return item;
                        }
                    }
                }
                return item;
            }
        }
    }
}
