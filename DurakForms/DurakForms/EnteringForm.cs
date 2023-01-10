using System.Net;

namespace DurakForms
{
    public partial class EnteringForm : Form
    {
        string nickname;
        IPAddress ip;
        Button currentCard;

        private Point Origin_Cursor;
        private Point Origin_Control;
        private bool BtnDragging = false;
        private Point[] cardButtonsPos = new Point[5];
        List<string> ourCardsNames = new List<string>();
        List<Button> add_buttons = new List<Button>();
        bool isAttack = false;       
        internal delegate void _UpdateTable(string[] names);
        internal _UpdateTable UpdateTableDelegat;


        public EnteringForm()
        {
            InitializeComponent();

        }

        private void MouseEvent(object? sender, MouseEventArgs e)
        {
            // if(currentCard != null)
            //      currentCard.GetType().GetProperty("Location").SetValue(currentCard,new Point(Cursor.Position.X,Cursor.Position.Y));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //    MessageBox.Show("alarm");
            // CardButtonGenerate();
            //this.MouseMove += new MouseEventHandler(MouseEvent);
            //ourCardsNames.Add("00");
            //ourCardsNames.Add("30");
            //ourCardsNames.Add("21");
            //ourCardsNames.Add("17");
            //CardButtonGenerate(ourCardsNames.ToArray());
            //ShowTurn(new string[] {"", "10" }, new string[] { "04", "14" });
            //EnemyCardShow(3);
            Client.form = this;
            UpdateTableDelegat = new _UpdateTable(UpdateTable);
        }


        private void HostingButton_Click(object sender, EventArgs e)
        {
            if (CheckNickname())
            {
                Client.StartHosting(nickname);
                ShowGameTable();
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {

            string _ip = IpInputBox.Text;//.Replace(' ', '.');                
            if (IPAddress.TryParse(_ip, out ip))
            {
                if (CheckNickname())
                {
                    Client.Connect(nickname, ip);
                    ShowGameTable();
                }
            }
            else
                MessageBox.Show("input ip adress");

        }


        bool CheckNickname()
        {
            if (NickNameBox.Text == "")
            {
                MessageBox.Show("input nickname");
                return false;
            }
            else
                return true;
        }


        void ShowEnteringForm()
        {
            HideObjectsWithTag("Game");
            ShowObjectsWithTag("Enter");
        }

        void ShowGameTable()
        {
            HideObjectsWithTag("Enter");
            ShowObjectsWithTag("Game");
        }

        void HideObjectsWithTag(string tag)
        {
            foreach (Control obj in Controls)
                if (obj.Tag == tag)
                    obj.Hide();
        }
        void ShowObjectsWithTag(string tag)
        {
            foreach (Control obj in Controls)
                if (obj.Tag == tag)
                    obj.Hide();
        }

        internal void CardButtonGenerate(string[] card_names)//string[] card_names)
        {
            int x = 20, y = 20;

            for (int i = 0; i < card_names.Length; i++)
            {
                if(card_names[i] != "")
                    CreateCardButton(x, y, i, card_names[i] + ".gif");
                x += 168;
            }
            x = 20;
            y += 235;


            //foreach (var name in card_names)
            //{

            //}
        }

        public void CreateCardButton(int x, int y, int i, string card_name) // 1 - Создание фуккции создание кнопки
        {
            Button btn = new Button();
            btn.Text = null;
            //i.ToString();
            btn.Size = new Size(158, 225);
            btn.Location = new Point(x, y);
            btn.Click += new EventHandler(CardButton_Click);
            btn.BackgroundImage = GetCardImage(card_name);
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatStyle = FlatStyle.Flat;
            //  btn.MouseUp += new MouseEventHandler(CardButton_MouseUp);
            btn.MouseDown += new MouseEventHandler(CardButton_MouseDown);
            //      btn.MouseMove += new MouseEventHandler(CardButton_MouseMove);
            Controls.Add(btn);
        }

        private void CardButton_Click(object sender, EventArgs e) // 2 - Обработчик кнопки
        {
            //   // MessageBox.Show("alarm");
            //  currentCard = (Button)sender;
        }

        private Bitmap GetCardImage(string card_name)
        {
            string path = string.Format(@"cards images\{0}", card_name);
            return new Bitmap(path);
        }

        //private void CardButton_MouseUp(object sender, MouseEventArgs e)
        //{
        //    BtnDragging = false;
        //}

        private void CardButton_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                currentCard = (Button)sender;


                foreach (Control button in this.Controls)
                {
                    if (button.GetType() == typeof(Button))
                    {
                        ((Button)button).FlatAppearance.BorderColor = Color.Black;
                        ((Button)button).FlatAppearance.BorderSize = 1;
                    }
                }
                currentCard.FlatAppearance.BorderColor = Color.Gold;
                currentCard.FlatAppearance.BorderSize = 5;
            }
            catch { }
            //   currentCard.
            //Button ct = sender as Button;
            //ct.Capture = true;
            //Origin_Cursor = Cursor.Position;
            //Origin_Control = ct.Location;
            //BtnDragging = true;
        }

        //private void CardButton_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (BtnDragging)
        //    {
        //        Button ct = sender as Button;
        //        ct.Left = Origin_Control.X - (Origin_Cursor.X - Cursor.Position.X);
        //        ct.Top = Origin_Control.Y - (Origin_Cursor.Y - Cursor.Position.Y);
        //    }
        //}


        internal void UpdateTable(string[] messages)
        {
            isAttack = bool.Parse(messages[0]);
            CardButtonGenerate(messages[1].Split(' '));
            ShowTurn(messages[2].Split(' '), messages[3].Split(' '));
            EnemyCardShow(int.Parse(messages[4]));

        }

        void ShowTurn(string[] deffend_cards, string[] attack_cards)
        {
            int x = 20, y = 255;
                     
            for (int i = 0; i < deffend_cards.Length; i++)
            {
                if(deffend_cards[i] != "")
                    CreatePictureBox(x, y, deffend_cards[i] + ".gif");
                else if(!isAttack)
                    CreateAddButton(x, y);
                x += 168;
            }
            x = 20;
            y += 240;
            for (int i = 0; i < attack_cards.Length; i++)
            {
                if (deffend_cards[i] != "")
                    CreatePictureBox(x, y, attack_cards[i] + ".gif");
                x += 168;
            }
            if (isAttack)
                CreateAddButton(x, y);
        }

        void CreateAddButton(int x, int y)
        {

            Button btn = new Button();
            btn.Text = null;
            btn.Size = new Size(158, 225);
            btn.Location = new Point(x, y);
            btn.Click += new EventHandler(AddButton_Click);
            btn.BackgroundImage = GetCardImage("Add.png");
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(btn);
            add_buttons.Add(btn);
        }

        void EnemyCardShow(int count)
        {
            int x = 20, y = 700;
            for (int i = 0; i < count; i++)
            {
                CreatePictureBox(x, y, "CardBackGround.png");
                x += 168;
            }

        }

        void AddButton_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < add_buttons.Count(); i++)
            {
                if (add_buttons[i] == sender)
                    if (currentCard != null)
                        Client.SendCommand(String.Format("t {0} {1}", i, currentCard.Name));
                    else
                        Client.SendCommand(String.Format("t {0}", i));
            }

        }

        void CreatePictureBox(int x, int y, string card_name)
        {
            PictureBox btn = new PictureBox();
            btn.Text = null;
            btn.Size = new Size(158, 225);
            btn.Location = new Point(x, y);

            btn.BackgroundImage = GetCardImage(card_name);
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            //  btn.MouseUp += new MouseEventHandler(CardButton_MouseUp);
            btn.MouseDown += new MouseEventHandler(CardButton_MouseDown);
            //      btn.MouseMove += new MouseEventHandler(CardButton_MouseMove);
            Controls.Add(btn);
        }

        private void IpInputBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void NickNameBox_TextChanged(object sender, EventArgs e)
        {
            nickname = NickNameBox.Text;
        }

        private void EndTurn_Click(object sender, EventArgs e)
        {
            Client.SendCommand("e");
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Client.SendCommand("r");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}