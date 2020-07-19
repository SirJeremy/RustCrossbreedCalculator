using System;
using System.Windows;
using System.Windows.Controls;
using RustCrossbreed.ViewModels;

namespace RustCrossbreed.Views
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private readonly HistoryViewModel vm;

        public HistoryWindow(HistoryViewModel viewModel)
        {
            InitializeComponent();

            vm = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            this.DataContext = vm;
        }

        private void History_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MoreInfo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
