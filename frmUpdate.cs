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
            CheckForIllegalCrossThreadCalls = false;

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
                // Wait Form'u başlat
                SetCaption("Güncellemeler kontrol ediliyor...");

                using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/A1640A/SQUIRREL-AUTO-UPDATE"))
                {
                    // Güncellemeleri kontrol et
                    var updateInfo = await updateManager.CheckForUpdate(ignoreDeltaUpdates: false,
                        progress: (percentage) => SetProgress(percentage, "Güncellemeler kontrol ediliyor..."));

                    if (updateInfo.ReleasesToApply != null && updateInfo.ReleasesToApply.Any())
                    {
                        SetCaption("Güncellemeler indiriliyor...");

                        // Güncellemeleri indir ve ilerlemeyi göster
                        var newRelease = await updateManager.UpdateApp(progress: (percentage) => SetProgress(percentage, "Güncellemeler indiriliyor..."));

                        SetCaption("Uygulama güncellendi.");
                        SetDescription("Lütfen bekleyiniz...");

                        Application.Exit();
                    }
                    else
                    {
                        SetCaption("Güncel versiyonu kullanıyorsunuz.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama güncellenirken bir hata oluştu: \n\n{ex.ToString()}", "Güncelleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Close();
            }
        }

        // İlerleme yüzdesini gösteren yardımcı metot
        private void SetProgress(int percentage, string description)
        {
            SetDescription($"{description} %{percentage}");
        }
    }
}