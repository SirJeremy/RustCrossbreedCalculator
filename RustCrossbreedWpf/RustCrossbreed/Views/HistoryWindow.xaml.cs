using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RustCrossbreed.Models;
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
            vm.HistorySelected = History.SelectedItems.Cast<HistoryModel>().ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteSelected();
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteAll();
        }

        private void MoreInfo_Click(object sender, RoutedEventArgs e)
        {
            vm.OnMoreInfoClick();
        }
    }
}
