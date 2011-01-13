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
        #region Fields
        
        private const string CupPath = @"Assets/CupStyles/";
        private string _itemName;
        private string _cupName;
        private bool _isCanClick;
        private bool _hasOpened;
        private UserControl _item;
        private TheS.SperfGames.MayaTukky.Models.CupItems _items = new Models.CupItems();

        #endregion Fields

        #region Properties

        /// <summary>
        /// เปิดการแสดงผลเมื่อเมาส์เลื่อนมาชี้แก้ว
        /// true: เปิด
        /// false: ปิด
        /// </summary>
        public bool IsEnableMouseOverAnimation
        {
            get { return _isCanClick; }
        }

        /// <summary>
        /// แก้วนี้ถูกคลิ๊กแล้วหรือไม่
        /// </summary>
        public bool HasOpened
        {
            get { return _hasOpened; }
            set { _hasOpened = value; }
        }

        #endregion Properties

        #region Events
        
        /// <summary>
        /// แก้วถูกคลิกแล้ว
        /// </summary>
        public event CupAnswerEventHandler Click;

        #endregion Events

        #region Constructors

        /// <summary>
        /// กำหนดค่าเริ่มต้นให้กับแก้ว
        /// </summary>
        public CupUI(TheS.SperfGames.MayaTukky.Models.CupItems cupItems)
        {
            InitializeComponent();

            _items = cupItems;

            Sb_Click.Completed += new EventHandler(Sb_Click_Completed);
            Sb_Up.Completed += new EventHandler(Sb_Up_Completed);
            cv_Cup.MouseLeftButtonDown += new MouseButtonEventHandler(Cup_MouseLeftButtonDown);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// กำหนดค่าเริ่มต้นของแก้ว
        /// </summary>
        /// <param name="item">ชื่อวัตถุที่อยู่ในแก้ว</param>
        /// <param name="cupName">ชื่อของแก้วที่จะนำมาแสดงผล</param>
        public void Initialize(string item, string cupName)
        {
            if (_cupName != cupName)
            {
                cv_Cup.Children.Clear();
                _cupName = loadElement(cupName);
                cv_Cup.Children.Add(XamlReader.Load(_cupName) as UIElement);
            }

            ClearItem();
            _itemName = item;
            _item = _items[item];
            cv_Item.Children.Add(_item);
        }

        /// <summary>
        /// ลบวัตถุที่อยู่ภายในแก้ว
        /// </summary>
        public void ClearItem()
        {
            cv_Item.Children.Clear();
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

            ClearItem();
        }

        /// <summary>
        /// อนิเมชันตอบผิด
        /// </summary>
        public void StartCupIncorrect()
        {
            const int ItemElementIndex = 0;
            var item = (cv_Item.Children[ItemElementIndex] as PerfEx.Infrastructure.IAnime);
            if(item!=null) item.StartPlay();
        }

        /// <summary>
        /// หยุดการเล่นอนิเมชัน
        /// </summary>
        public void StopCupIncorrect()
        {
            const int ItemElementIndex = 0;
            var item = (cv_Item.Children[ItemElementIndex] as PerfEx.Infrastructure.IAnime);
            if (item != null) item.StopPlay();
        }

        /// <summary>
        /// อนิเมชันแก้วครอบลง
        /// </summary>
        public void CupDown()
        {
            Sb_Down.Begin();
        }

        /// <summary>
        /// อนิเมชันแก้วลอยขึ้น
        /// </summary>
        public void CupUp()
        {
            Sb_Up.Begin();
        }

        /// <summary>
        /// อนิเมชันตอบถูก
        /// </summary>
        public void CupCorrect()
        {
            Sb_Correct.Begin();
        }

        /// <summary>
        /// กำหนดให้สามารถคลิกได้
        /// </summary>
        public void SetCupClick()
        {
            _isCanClick = true;
        }

        // เมื่อเกมเล่นคำถามเสร็จสิ้น
        private void Cup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isCanClick)
            {
                _hasOpened = true;

                _isCanClick = !_isCanClick;
                Sb_Click.Begin();
            }
        }

        // โหลดข้อมูลของแก้ว
        private string loadElement(string cupName)
        {
            const string FileType = ".txt";
            StringBuilder cupPath = new StringBuilder();
            cupPath.Append(CupPath).Append(cupName).Append(FileType);

            Stream stream = Application.GetResourceStream(new Uri(cupPath.ToString(), UriKind.RelativeOrAbsolute)).Stream;
            return (new StreamReader(stream)).ReadToEnd();
        }

        // ทำการเล่นอนิเมชันแก้วลอยขึ้น
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

        #endregion Methods
    }
}
