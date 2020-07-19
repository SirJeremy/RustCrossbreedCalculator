using System;
using System.Linq;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.ViewModels
{
    public class MoreInfoViewModel
    {
        #region Properties
        public Breed Breed { get; }

        //no need for setters since Breed is mostly immutable
        public string GenesString { get => Breed.GenesString; }
        public string Generation { get => string.Format("Gen {0}", Breed.Generation.ToString()); }

        //Tuple<string parentGenes, string parentGeneration>
        public Tuple<string, string>[] ParentGenes { get; }
        public Breed[] Children { get; }
        #endregion

        #region Constructors
        public MoreInfoViewModel(Breed breed, Breed[] children, int?[] parentGenerations)
        {
            //more validation should be done, but I put that off for a later task

            Breed = breed ?? throw new ArgumentNullException(nameof(breed));
            Children = children;

            ParentGenes = breed.ParentGenes.Zip(parentGenerations, (genes, generation) 
                => Tuple.Create(genes, generation?.ToString() ?? "Missing"))
                .ToArray();
        }
        #endregion
    }
}
