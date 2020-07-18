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

        //no setters since I want info to be static
        //public (string parentGenes, string parentGeneration)[] ParentGenes { get; }
        //public (string genes, int generation)[] Children { get; }

        //Tuple<string parentGenes, string parentGeneration>
        public Tuple<string, string>[] ParentGenes { get; }
        //Tuple<string childGenes, string childGeneration>
        public Tuple<string, int>[] Children { get; }
        #endregion

        #region Constructors
        public MoreInfoViewModel(Breed breed, (string, int)[] children, int?[] parentGenerations)
        {
            //more validation should be done, but I put that off for a later task

            Breed = breed ?? throw new ArgumentNullException(nameof(breed));
            //Children = children ?? new (string, int)[0];

            ////ew
            ////ParentGenes = (ValueTuple<string, string>[])breed.ParentGenes.Zip(parentGenerations, (genes, generation) => (genes, generation?.ToString() ?? "Missing"));

            //var parentGenes = new (string, string)[breed.ParentGenes.Length];
            //for(int i = 0; i < breed.ParentGenes.Length; i++)
            //{
            //    parentGenes[i] = (breed.ParentGenes[i], parentGenerations[i]?.ToString() ?? "Missing");
            //}
            //ParentGenes = parentGenes;

            //OOPSIE POOPSIES
            //cant bind to value tuples, fix later

            var parentGenes = new Tuple<string, string>[breed.ParentGenes.Length];
            for (int i = 0; i < breed.ParentGenes.Length; i++)
            {
                parentGenes[i] = Tuple.Create(breed.ParentGenes[i], parentGenerations[i]?.ToString() ?? "Missing");
            }
            ParentGenes = parentGenes;

            Children = children.Select(x => Tuple.Create(x.Item1, x.Item2)).ToArray();
        }
        #endregion
    }
}
