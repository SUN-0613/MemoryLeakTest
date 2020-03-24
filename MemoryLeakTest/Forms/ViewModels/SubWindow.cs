using AYam.Common.MVVM;
using MemoryLeakTest.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryLeakTest.Forms.ViewModels
{

    /// <summary>SubWindow.ViewModel</summary>
    public class SubWindow : ViewModelBase, IDisposable
    {

        #region Property

        /// <summary>一覧の解放処理を実行するか</summary>
        public bool IsClearCollections { get; set; }

        /// <summary>Windowを閉じる</summary>
        public bool IsClose { get; set; }

        /// <summary>表示内容一覧</summary>
        public ObservableCollection<CollectionData> Items { get; set; }

        /// <summary>選択データ</summary>
        public CollectionData SelectedItem { get; set; }

        /// <summary>表示内容一覧</summary>
        public ObservableCollection<CollectionData> Values { get; set; }

        /// <summary>選択データ</summary>
        public CollectionData SelectedValue { get; set; }

        #endregion

        /// <summary>SubWindow.ViewModel</summary>
        public SubWindow()
        {

            Items = new ObservableCollection<CollectionData>();

            var rnd = RandomValue.GetValue();
            for (var iLoop = 0; iLoop < rnd.Next(10000) + 1; iLoop++)
            {
                Items.Add(new CollectionData(iLoop + 1, rnd.Next()));
            }

            SelectedItem = Items[0];

            Values = new ObservableCollection<CollectionData>();

            rnd = RandomValue.GetValue();
            for (var iLoop = 0; iLoop < RandomValue.GetValue().Next(10000) + 1; iLoop++)
            {
                Values.Add(new CollectionData(iLoop + 1, rnd.Next()));
            }

            SelectedValue = Values[0];

            Close();

        }

        /// <summary>解放処理</summary>
        public void Dispose()
        {

            if (IsClearCollections)
            {
                Items.Clear();
                Values.Clear();
            }
            Items = null;
            Values = null;

        }

        /// <summary>Windowを閉じる</summary>
        public async void Close()
        {

            await Task.Run(() => 
            {

                Thread.Sleep(5000);

            });

            IsClose = true;
            CallPropertyChanged(nameof(IsClose));

        }

    }

}
