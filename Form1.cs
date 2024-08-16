using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SQUIRREL_AUTO_UPDATE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void SetVersionTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += $" v.{versionInfo.FileVersion}";
        }

        async void CheckForUpdates()
        {
            try
            {
                using (var mgr=await UpdateManager.GitHubUpdateManager("https://github.com/A1640A/SQUIRREL-AUTO-UPDATE"))
                {
                    var release = await mgr.UpdateApp();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Failed to check for updates. \n\n {exc.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetVersionTitle();
            CheckForUpdates();
        }
    }
}
