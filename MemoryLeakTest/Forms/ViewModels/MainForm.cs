using AYam.Common.IO;
using AYam.Common.MVVM;
using System;
using System.Windows;

namespace MemoryLeakTest.Forms.ViewModels
{

    /// <summary>MainForm.ViewModel</summary>
    public class MainForm : ViewModelBase, IDisposable
    {

        #region Model

        /// <summary>MainForm.Model</summary>
        private Models.MainForm _Model;

        #endregion

        #region Property

        /// <summary>SubWindow.View</summary>
        public Window Dialog { get; set; }

        /// <summary>一覧の解放処理を実行するか</summary>
        public bool IsClearCollections { get; set; }

        /// <summary>メモリ使用量</summary>
        public string MemoryUsage { get; set; } = "0 MB";

        /// <summary>ボタン用コマンド</summary>
        public DelegateCommand<string> ButtonCommand { get; private set; }

        /// <summary>スタートボタン操作許可</summary>
        public bool IsEnabledStartButton { get; private set; } = true;

        /// <summary>ストップボタン操作許可</summary>
        public bool IsEnabledStopButton { get; private set; } = false;

        /// <summary>ステータス</summary>
        public string Status { get; set; } = "STOP";

        /// <summary>ステータス</summary>
        public string WaitTime { get; set; } = "";

        #endregion

        /// <summary>MainForm.ViewModel</summary>
        public MainForm()
        {

            Log.Initialize();

            _Model = new Models.MainForm(UpdateMemoryUsage, ShowNewDialog, ChangeEnabled, UpdateStatus);

            ButtonCommand = new DelegateCommand<string>(
                (parameter) => 
                {

                    switch (parameter)
                    {

                        case "start":
                            _Model.OnStart(IsClearCollections);
                            break;

                        case "stop":
                            IsEnabledStopButton = false;
                            CallPropertyChanged(nameof(IsEnabledStopButton));
                            _Model.IsLoop = false;
                            break;

                    }

                }, 
                () => true);

        }

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            if (Dialog != null)
            {
                Dialog = null;
                CallPropertyChanged(nameof(Dialog));
            }

            _Model.Dispose();

        }

        /// <summary>メモリ使用量更新</summary>
        private void UpdateMemoryUsage()
        {

            var memory = (double)Environment.WorkingSet / (1024 ^ 2);

            MemoryUsage = memory.ToString("N0") + " MB";
            CallPropertyChanged(nameof(MemoryUsage));

            // ログ出力
            Log.WriteLog("Memory = " + MemoryUsage, "Memory");

        }

        /// <summary>新規Dialog表示</summary>
        private void ShowNewDialog()
        {

            _Model.IsClosed = false;

            if (Dialog != null)
            {
                Dialog = null;
                CallPropertyChanged(nameof(Dialog));
            }

            Dialog = new Views.SubWindow(IsClearCollections);
            CallPropertyChanged(nameof(Dialog));

        }

        /// <summary>ボタン操作許可の切り替え</summary>
        /// <param name="value">スタートボタンの操作許可</param>
        private void ChangeEnabled(bool value)
        {

            IsEnabledStartButton = value;
            IsEnabledStopButton = !value;

            CallPropertyChanged(nameof(IsEnabledStartButton));
            CallPropertyChanged(nameof(IsEnabledStopButton));

        }

        /// <summary>ステータス更新</summary>
        /// <param name="status">ステータス</param>
        /// <param name="waitTime">待機時間(秒)</param>
        private void UpdateStatus(string status, string waitTime)
        {

            Status = status;
            WaitTime = waitTime;

            CallPropertyChanged(nameof(Status));
            CallPropertyChanged(nameof(WaitTime));

        }

    }

}
