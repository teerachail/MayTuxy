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
using System.Windows.Markup;
using System.Text;
using System.IO;

namespace TheS.SperfGames.MayaTukky.Controls
{
    /// <summary>
    /// แก้ว
    /// </summary>
    public partial class CupUI : UserControl
    {
        private const string CupPath = @"Assets/CupStyles/";
        private string _itemName;
        private string _cupName;
        private bool _isCanClick;

        private bool _hasOpened;

        public bool HasOpened
        {
            get { return _hasOpened; }
            set { _hasOpened = value; }
        }

        private UserControl _item;
        private Dictionary<string, Type> _items;

        public event CupAnswerEventHandler Click;

        public CupUI()
        {
            InitializeComponent();
            Sb_ShowItem.SpeedRatio = 0.2;
            RowUI.SwapCompleted += new EventHandler(Row_SwapCompleted);

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

            Sb_Click.Completed += new EventHandler(Sb_Click_Completed);
            Sb_Up.Completed += new EventHandler(Sb_Up_Completed);
            Sb_ShowItem.Begin();
        }

        /// <summary>
        /// กำหนดค่าเริ่มต้นของแก้ว
        /// </summary>
        /// <param name="item">ชื่อวัตถุที่อยู่ในแก้ว</param>
        /// <param name="cupName">ชื่อของแก้วที่จะนำมาแสดงผล</param>
        public void Initialize(string item, string cupName)
        {
            cv_Cup.Children.Clear();
            cv_Item.Children.Clear();
            _itemName = item;
            _cupName = loadElement(cupName);
            cv_Cup.Children.Add(XamlReader.Load(_cupName) as UIElement);
            cv_Cup.MouseLeftButtonDown += new MouseButtonEventHandler(Cup_MouseLeftButtonDown);

            _item = (UserControl)Activator.CreateInstance(_items[item]);
            cv_Item.Children.Add(_item);
        }

        /// <summary>
        /// สั่งให้แก้วกลับไปอยู่สถานะเริ่มต้น
        /// </summary>
        public void ResetState()
        {
            _hasOpened = false;
            _isCanClick = false;
            Sb_ShowItem.Stop();
            Sb_Click.Stop();
            Sb_Up.Stop();
            Sb_Down.Stop();
            Sb_Correct.Stop();
        }

        /// <summary>
        /// สั่งให้แก้วครอบลง
        /// </summary>
        public void CupDown()
        {
            Sb_Down.Begin();
        }

        public void CupUp()
        {
            Sb_Up.Begin();
        }

        public void CupCorrect()
        {
            Sb_Correct.Begin();
        }

        // เมื่อ Canvas ถูกคลิก
        private void Cup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isCanClick)
            {
                _hasOpened = true;

                _isCanClick = !_isCanClick;
                Sb_Click.Begin();
            }
        }

        // load cup
        private string loadElement(string cupName)
        {
            const string FileType = ".txt";
            StringBuilder cupPath = new StringBuilder();
            cupPath.Append(CupPath).Append(cupName).Append(FileType);

            Stream stream = Application.GetResourceStream(new Uri(cupPath.ToString(), UriKind.RelativeOrAbsolute)).Stream;
            return (new StreamReader(stream)).ReadToEnd();
        }

        private int _rowSwapCount;
        // เมื่อแก้วปิดแล้วจึงจะสามารถกดคลิกได้
        void Row_SwapCompleted(object sender, EventArgs e)
        {
            _rowSwapCount++;
            const int SwapFinish = 2;
            if (_rowSwapCount >= SwapFinish)
            {
                const int Reset = 0;
                _rowSwapCount = Reset;
                _isCanClick = true;
            }
        }

        private void Sb_Click_Completed(object sender, EventArgs e)
        {
            Sb_Up.Begin();
        }

        // ส่งคำตอบ
        private void Sb_Up_Completed(object sender, EventArgs e)
        {
            var temp = Click;
            if (temp != null)
            {
                temp(this, new CupAnswerEventArgs(_itemName));
            }
        }
    }
}
