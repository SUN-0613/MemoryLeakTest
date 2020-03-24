using AYam.Common.IO;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryLeakTest.Forms.Models
{

    /// <summary>MainForm.Model</summary>
    public class MainForm : IDisposable
    {

        /// <summary>ループ処理継続</summary>
        public bool IsLoop = false;

        /// <summary>ダイアログを閉じたか</summary>
        public bool IsClosed = false;

        /// <summary>メモリ使用量更新メソッド</summary>
        private Action _UpdateMemoryUsage;

        /// <summary>ダイアログ表示メソッド</summary>
        private Action _ShowNewDialog;

        /// <summary>ボタン操作許可変更メソッド</summary>
        private Action<bool> _ChangeEnabled;

        /// <summary>ステータス更新</summary>
        private Action<string, string> _UpdateStatus;

        /// <summary>MainForm.Model</summary>
        /// <param name="updateMemoryUsage">メモリ使用量更新メソッド</param>
        /// <param name="showNewDialog">ダイアログ表示メソッド</param>
        /// <param name="changeEnabled">ボタン操作許可変更メソッド</param>
        public MainForm(Action updateMemoryUsage, Action showNewDialog, Action<bool> changeEnabled, Action<string, string> updateStatus)
        {
            _UpdateMemoryUsage = updateMemoryUsage;
            _ShowNewDialog = showNewDialog;
            _ChangeEnabled = changeEnabled;
            _UpdateStatus = updateStatus;
        }

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            IsLoop = false;

            _UpdateMemoryUsage = null;
            _ShowNewDialog = null;
            _ChangeEnabled = null;
            _UpdateStatus = null;

        }

        /// <summary>メモリ計測開始</summary>
        /// <param name="isCollectionClear">コレクションクリア実行FLG</param>
        public async void OnStart(bool isCollectionClear)
        {

            var nextCollect = DateTime.Now.AddMinutes(30);

            _ChangeEnabled?.Invoke(false);
            IsLoop = true;

            Log.WriteLog("○コレクションクリア:" + (isCollectionClear ? "あり" : "なし") + "で処理開始", "Memory");

            while (IsLoop)
            {

                for (var iLoop = 0; iLoop < 3; iLoop++)
                {

                    switch (iLoop)
                    {

                        case 0:
                            await WaitTask("Window表示前", 3);
                            break;

                        case 1:
                            _ShowNewDialog();
                            await WaitTask("Window表示中", 5);
                            break;

                        case 2:
                            await WaitTask("メモリ取得前", 2);
                            break;

                        default:
                            break;

                    }

                    if (!IsLoop)
                    {
                        break;
                    }

                }

                // 30分おきにガベージコレクション解放
                if (!IsLoop || nextCollect.AddMinutes(30) >= DateTime.Now)
                {

                    Collect();
                    nextCollect = DateTime.Now.AddMinutes(30);

                }

                _UpdateMemoryUsage?.Invoke();

                if (IsLoop)
                {
                    await WaitTask("次処理準備中", 5);
                }

            }

            Log.WriteLog("処理終了", "Memory");
            _UpdateStatus?.Invoke("STOP", "");
            _ChangeEnabled?.Invoke(true);

        }

        /// <summary>待機メソッド</summary>
        /// <param name="status">ステータス</param>
        /// <param name="second">待機時間(秒)</param>
        private Task WaitTask(string status, int second)
        {

            return Task.Run(() => 
            {

                var stopwatch = new Stopwatch();
                var millSecond = second * 1000;
                var value = -1;

                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < millSecond)
                {

                    var newValue = (int)(stopwatch.ElapsedMilliseconds / 1000);
                    if (!newValue.Equals(value))
                    {

                        value = newValue;
                        _UpdateStatus?.Invoke(status, Convert.ToString(second - value) + "秒待機中...");

                    }

                    Thread.Sleep(10);

                }

                stopwatch.Stop();

            });

        }

        /// <summary>ガベージコレクションの解放</summary>
        private void Collect()
        {

            GC.Collect();
            Log.WriteLog("★GC.Collect()実行", "Memory");

        }

    }

}
