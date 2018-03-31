using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace chouqian
{
    public partial class Form1 : Form
    {
        private List<String> textList;
        private List<String> tmpList;
        Random rand = new Random();
        int currentIndex = -1;
        int randomIndex = 0;
        public Form1()
        {
            InitializeComponent();
        }


        private bool loadFile(String filename,out List<String> textList) {
            FileStream fileStream = null;
            StreamReader streamReader = null;
            textList = new List<String>();
            int i = 0;
            try
            {
                fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

                streamReader = new StreamReader(fileStream, Encoding.Default);

                fileStream.Seek(0, SeekOrigin.Begin);

                string content = streamReader.ReadLine();

                i = 0;
                while (content != null)
                {
                    string line = content.Trim();
                    if (line == "")
                    {
                        //content = streamReader.ReadLine();
                        goto cont;
                        continue;
                    }
                    else
                    {
                        textList.Add(line);
                    }
                    
                    //MessageBox.Show(number.ToString() + "," + address);

                    /*
                    if (content.Contains("="))  
                    {  
                    string key = content.Substring(0, content.LastIndexOf("=")).Trim();  
                    string value = content.Substring(content.LastIndexOf("=") + 1).Trim();  
                    }  */
                    i++;
                cont:
                    content = streamReader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                textList = null;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
            if (textList == null)
                return false;
            return true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "抽签助手 - 咸阳市公安消防支队";
            this.btnStart.Text = "开始";
            this.btnStart.Enabled = false;
            this.btnRestart.Text = "清除并重新加载";
            this.timer.Interval = 10;
            this.label1.Text = "";
            
        }



        private void btnRestart_Click(object sender, EventArgs e)
        {
            if(timer.Enabled == true)
            {
                this.timer.Enabled = false;
                
            }
            this.listBox1.Items.Clear();
            this.label1.Text = "";
            if(loadFile("names.txt", out textList))
            {
                if (textList.Count == 0)
                {
                    MessageBox.Show("数据加载失败：文件为空\n请正确填写抽签文件后重新加载", "抽签助手", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnStart.Enabled = false;
                }
                else
                {
                    MessageBox.Show("加载成功，共加载 " + Convert.ToString(textList.Count) + " 个对象", "抽签助手", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnStart.Enabled = true;
                    tmpList = new List<string>(textList);
                }
            }
            else
            {
                this.btnStart.Enabled = false;
            }
            this.btnStart.Text = "开始";
            
            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(this.timer.Enabled == true)
            {
                
                string obj = tmpList[randomIndex];
                tmpList.RemoveAt(randomIndex);
                listBox1.Items.Add(Convert.ToString(currentIndex + 1) +" - " + obj);
                currentIndex++;
                if (tmpList.Count <= 1)
                {

                    timer.Stop();
                    listBox1.Items.Add(Convert.ToString(currentIndex + 1) +" - " + tmpList[0]);
                    
                    this.btnStart.Text = "重新开始";
                    currentIndex = -1;
                    tmpList = new List<string>(textList);

                    MessageBox.Show(this, "抽签过程实施完毕", "抽签助手", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.label1.Text = "";
                }
                else
                {
                    btnStart.Text = "抽取" + Convert.ToString(currentIndex + 1) + "号";
                }
            }
            else
            {
                this.timer.Start();
                this.btnStart.Enabled = false;//等待产生第一个随机数
                currentIndex = -1;
                this.listBox1.Items.Clear();
            }
                
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
            if(currentIndex == -1)
            {
                btnStart.Text = "抽取 1 号";
                btnStart.Enabled = true;
                currentIndex = 0;
            }
            else
            {
                int i = rand.Next(tmpList.Count);
                if (i == randomIndex)
                {
                    randomIndex = (i + 1) % tmpList.Count;
                    
                }
                else
                    randomIndex = i;
                Debug.Print(Convert.ToString(randomIndex));
                //currentIndex ++;
            }
            label1.Text = tmpList[randomIndex];
            /*
            currentIndex++;
            label1.Text = Convert.ToString(currentIndex);
            Debug.Print(Convert.ToString(currentIndex));
            */
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            btnRestart_Click(null, null);
        }
    }
}
