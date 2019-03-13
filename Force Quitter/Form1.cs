using System;
using System.Windows.Forms;


namespace Force_Killer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            if (Properties.Settings.Default.even_launched == false)
            {
                Properties.Settings.Default.context_items.Clear();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.even_launched = true;
            }
            context_menu_reflesh();
            listbox1_refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Properties.Settings.Default.now_running = true;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.now_running = false;
            Properties.Settings.Default.Save();
            Environment.Exit(0);
        }

        private void toolStripMenuItem_New_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.now_running)
            {
                Form1 form1 = new Form1();
                form1.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "実行ファイル (.exe)|*.exe|all|*.*";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] filename;
                filename = openFileDialog1.FileName.Split('\\');
                Array.Reverse(filename);
                filename = filename[0].Split('.');

                Properties.Settings.Default.context_items.Add(filename[0]);
                Properties.Settings.Default.Save();

                listbox1_refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    int sel = listBox1.SelectedIndex;
                    Properties.Settings.Default.context_items.RemoveAt(sel);
                    Properties.Settings.Default.Save();
                    context_menu_reflesh();
                    listbox1_refresh();

                    contextMenuStrip1.Refresh();
                    listBox1.Refresh();
                }
            }
        }

        private void context_menu_reflesh()
        {
            if (contextMenuStrip1.Items.Count > 3)
            {
                for (int i = 0; i < contextMenuStrip1.Items.Count - 3; i++)
                {
                    contextMenuStrip1.Items.RemoveAt(i);
                }
            }
            if (Properties.Settings.Default.context_items.Count > 0)
            {
                for (int j = 0; j < Properties.Settings.Default.context_items.Count; j++)
                {
                    ToolStripMenuItem context_menu_name = new ToolStripMenuItem();
                    context_menu_name.Text = Properties.Settings.Default.context_items[j];
                    contextMenuStrip1.Items.Insert(contextMenuStrip1.Items.Count - 3, context_menu_name);
                }
                give_event_to_control();
                contextMenuStrip1.Refresh();
            }
            else
            {
                contextMenuStrip1.Refresh();
            }
        }

        private void listbox1_refresh()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < Properties.Settings.Default.context_items.Count; i++)
            {
                listBox1.Items.Add(Properties.Settings.Default.context_items[i]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.now_running = false;
            Properties.Settings.Default.Save();

            Application.Exit();
            Application.Restart();
        }

        private void give_event_to_control()
        {
            for (int i = 0; i < Properties.Settings.Default.context_items.Count; i++)
            {
                contextMenuStrip1.Items[i].Click += buttonNoShow_Click;
            }
        }

        private void buttonNoShow_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("アプリケーションを強制終了しますか？", "確認", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(((ToolStripMenuItem)sender).Text);
                int i = 0;
                foreach (System.Diagnostics.Process p in ps)
                {
                    i++;
                    p.Kill();
                }

                if (i == 0)
                {
                    MessageBox.Show("選択したアプリケーションは起動していませんでした。", "実行結果");
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fm = new Form2();
            fm.Show();
        }
    }
}
