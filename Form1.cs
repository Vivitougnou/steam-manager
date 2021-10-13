using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace steamManager
{
    public partial class Form1 : Form
    {
        string path = @"D:\data.txt";

        struct user
        {
            public user(string user, string pass)
            {
                username = user;
                password = pass;
            }
            public string username { get; }
            public string password { get; }
        }

        List<user> userlist = new List<user>();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            if(!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Flush();
                sw.Dispose();
            }

            List<string> lines = File.ReadAllLines(path).ToList();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                user newuser = new user(entries[0], entries[1]);
                comboBox1.Items.Add(entries[0]);
                userlist.Add(newuser);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            var User = new user(textBox1.Text, textBox2.Text);
            userlist.Add(User);
            lines.Add(User.username + "," + User.password);
            File.WriteAllLines(path, lines);

            comboBox1.Items.Clear();
            foreach (user v in userlist)
            {
                comboBox1.Items.Add(v.username);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();

            }


            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", "null");
            startInfo.Arguments = "  -login " + userlist[comboBox1.SelectedIndex].username + " " + userlist[comboBox1.SelectedIndex].password;
            Process.Start(startInfo);
        }

        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
        }

        private void label6_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }
    }
}
