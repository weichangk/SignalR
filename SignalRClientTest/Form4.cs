using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignalRClientTest
{
    public partial class Form4 : Form
    {
        readonly HubClient hubClient = new HubClient(@"https://localhost:44342/", "ChatsHub", "Form4");
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hubClient.CallMethod("sendMsgAll", hubClient.UserId, this.textBox1.Text, DateTime.Now, false);//发送信息
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hubClient.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            hubClient.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hubClient.CallMethod("sendMsgByUserId", hubClient.UserId, "Form2", this.textBox1.Text, DateTime.Now, false);//发送信息
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            hubClient.Stop();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            hubClient.ConnectionError = (errorMsg) => { MessageBox.Show(errorMsg); };//连接异常提示
            hubClient.RevMsgAllEvent += RevMsgAll;//注册接收信息事件
            hubClient.RevMsg();//监听接收信息
        }
        public void RevMsgAll(object sender, RevMsgAllEventArgs e)
        {
            Task.Run(() =>
            {
                this.richTextBox1.AppendText($"{e.Time.ToString()} isSysMsg:{e.IsSysMsg} Id:{e.SendId} Send Msg:{e.Msg}" + Environment.NewLine);//异步输出接收信息
            });
        }
    }
}
