using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemoryLeakTest.Forms.Views
{
    /// <summary>
    /// SubWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SubWindow : Window
    {

        /// <summary>SubWindow.View</summary>
        /// <param name="isClearCollections">一覧の解放処理を実行するか</param>
        public SubWindow(bool isClearCollections)
        {

            InitializeComponent();

            if (DataContext is ViewModels.SubWindow viewModel)
            {
                viewModel.IsClearCollections = isClearCollections;
            }

        }

    }
}
