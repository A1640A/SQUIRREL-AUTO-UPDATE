using DevExpress.XtraWaitForm;
using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Squirrel;
using System.Linq;

namespace SQUIRREL_AUTO_UPDATE
{
    public partial class frmUpdate : WaitForm
    {
        public frmUpdate()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;

            SetCaption("Uygulama başlatılıyor...");
            SetDescription("Lütfen bekleyiniz...");
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            SetCaption("Güncellemeler kontrol ediliyor.");
            CheckForUpdates();
        }

        private async void CheckForUpdates()
        {
            try
            {
                using (var updateManager = new UpdateManager("https://github.com/A1640A/SQUIRREL-AUTO-UPDATE"))
                {
                    var updateInfo = await updateManager.CheckForUpdate();

                    if (updateInfo.ReleasesToApply.Any() && updateInfo.ReleasesToApply.First().Version != updateManager.CurrentlyInstalledVersion())
                    {
                        SetCaption("Yeni versiyon bulundu.");
                        var downloadUpdate = MessageBox.Show($"SquirrelAutoUpdate v.{updateInfo.ReleasesToApply.First().Version} mevcut.\n\nŞimdi güncellemek ister misiniz ?", "SquirrelAutoUpdate", MessageBoxButtons.YesNo);
                        if (downloadUpdate == DialogResult.Yes)
                        {
                            SetCaption("Dosyalar indiriliyor...");

                            var newRelease = await updateManager.UpdateApp();

                            SetCaption("Uygulama güncellendi.");
                            SetDescription("Lütfen bekleyiniz...");

                            Application.Restart();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uygulama güncellenirken hata oluştu.");
            }
            finally
            {
                Close();
            }
        }
    }
}