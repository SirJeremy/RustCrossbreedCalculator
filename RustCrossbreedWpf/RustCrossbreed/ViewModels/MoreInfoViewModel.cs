using System;
using System.Collections.Generic;
using System.Text;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.ViewModels
{
    public class MoreInfoViewModel
    {
        #region Properties
        public Breed Breed { get; }

        //no need for setters since Breed is mostly immutable
        public string GenesString { get => Breed.GenesString; }
        public string[] ParentGenes { get => Breed.ParentGenes; }
        public string Generation { get => string.Format("Gen {0}", Breed.Generation.ToString("00")); }

        //no setters since I want info to be static
        public (string genes, int generation)[] Children { get; }
        //when int? is null, it means that the parent could not be found
        public int?[] ParentGenerations { get; }
        #endregion

        #region Constructors
        public MoreInfoViewModel(Breed breed, (string, int)[] children)
        {
            Breed = breed ?? throw new ArgumentNullException(nameof(breed));
            //more validation should be done, but I put that off for a later task
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }
        #endregion
    }
}
