using System.Windows;
using System.Linq;
using RustCrossbreed.BusinessLogic;
using RustCrossbreed.ViewModels;
using RustCrossbreed.Factories;
using RustCrossbreed.Services;

namespace RustCrossbreed.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel vm;
        public MainWindow()
        {
            InitializeComponent();

            var genesRepo = new BreedRepository();
            genesRepo.TryAdd(BreedFactory.ParseBreed("GYHXWW"));
            genesRepo.TryAdd(BreedFactory.ParseBreed("GGYYYY"));
            genesRepo.TryAdd(BreedFactory.ParseBreed("GGGGYY"));
            genesRepo.TryAdd(BreedFactory.ParseBreed("GGYYHH"));

            vm = new MainWindowViewModel(new RustCrossbreeder(), genesRepo, new BreedRepository(true), 
                new BreedRepository(), new HistoryRepository());
            this.DataContext = vm;
        }

        #region Button Handlers
        private void GeneSubmit_Click(object sender, RoutedEventArgs e)
        {
            vm.SubmitGene();
        }
        private void DeleteBreed_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteSelectedBreeds();
        }
        private void DeleteAllBreeds_Click(object sender, RoutedEventArgs e)
        {
            vm.DeleteAllBreeds();
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenHistoryWindow();
        }
        private void MoreInformation_Click(object sender, RoutedEventArgs e)
        {
            vm.OnMoreInfoClick();
        }
        private void AddBreed_Click(object sender, RoutedEventArgs e)
        {
            vm.AddSelectedBreeds();
        }

        private void RemoveCrossbreeding_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveSelectedBreeds();
        }
        private void ClearCrossbreeding_Click(object sender, RoutedEventArgs e)
        {
            vm.ClearSelectedGenes();
        }
        private void Crossbreed_Click(object sender, RoutedEventArgs e)
        {
            vm.CrossbreedSelectedBreeds();
        }

        private void OutputSave_Click(object sender, RoutedEventArgs e)
        {
            vm.SaveOutputBreed();
        }
        private void OutputClear_Click(object sender, RoutedEventArgs e)
        {
            vm.ClearGenesOutput();
        }
        #endregion 

        #region Selection Changed Handlers
        private void GenesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItems = GenesList.SelectedItems.Cast<Breed>().ToList();

            vm.BreedsListSelectedItems = selectedItems;
            //only want 1 listview to have a selection
            if (selectedItems.Count != 0)
            {
                SelectedList.UnselectAll();
                OutputList.UnselectAll();
            }
        }

        private void SelectedList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItems = SelectedList.SelectedItems.Cast<Breed>().ToList();

            vm.SelectedListSelectedItems = selectedItems;
            //only want 1 listview to have a selection
            if (selectedItems.Count != 0)
            {
                GenesList.UnselectAll();
                OutputList.UnselectAll();
            }
        }

        private void OutputList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItems = OutputList.SelectedItems.Cast<Breed>().ToList();

            vm.OutputListSelectedItems = selectedItems;
            //only want 1 listview to have a selection
            if (selectedItems.Count != 0)
            {
                GenesList.UnselectAll();
                SelectedList.UnselectAll();
            }
        }
        #endregion
    }
}
