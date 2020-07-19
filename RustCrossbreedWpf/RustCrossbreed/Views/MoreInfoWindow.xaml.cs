using System;
using System.Windows;
using RustCrossbreed.BusinessLogic;
using RustCrossbreed.ViewModels;

namespace RustCrossbreed.Views
{
    /// <summary>
    /// Interaction logic for MoreInfoWindow.xaml
    /// </summary>
    public partial class MoreInfoWindow : Window
    {
        private readonly MoreInfoViewModel vm;

        public MoreInfoWindow(MoreInfoViewModel viewModel)
        {
            InitializeComponent();

            vm = viewModel ?? throw new ArgumentNullException(nameof(vm));
            this.DataContext = vm;
        }
    }
}
