using System;
using System.Collections.Generic;
using System.Linq;

namespace RustCrossbreed.BusinessLogic
{
    public class RustCrossbreeder
    {
        #region Properties
        public int MinBreedsToCrossbreed { get; }
        public int MaxBreedsToCrossbreed { get; }
        #endregion

        #region Constructors
        public RustCrossbreeder(int minBreedsToCrossbreed = 2, int maxBreedsToCrossbreed = 8)
        {
            MinBreedsToCrossbreed = Math.Max(minBreedsToCrossbreed, 2);
            MaxBreedsToCrossbreed = Math.Max(maxBreedsToCrossbreed, 2);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crossbreeds genes.
        /// </summary>
        /// <param name="breeds">
        /// Breeds is a colection of breeds which are a collection of genes.
        /// [x][y] or [breed][gene]
        /// y ->
        /// 123456  x|
        /// 123456   V
        /// </param>
        /// <returns>Returns a colection of breeds that are possible.</returns>
        public List<Breed> Crossbreed(IList<Breed> breeds)
        {
            if (breeds == null) throw new ArgumentNullException(nameof(breeds));
            if (!IsValidCountForCrossbreed(breeds.Count)) throw new ArgumentException(nameof(breeds));

            //check if breeds and genes exist and are valid for crossbreed
            if (breeds.Any(breed => breed == null)) throw new ArgumentNullException(nameof(breeds));
            if (!BreedsAreSameLength(breeds)) throw new ArgumentException(nameof(breeds));
            if (breeds[0].Genes.Length < 1) throw new ArgumentException(nameof(breeds)); //check if breeds have genes

            /// possibleGenes[x][y] or [geneSlot][genePossibility]
            ///  x ->
            /// 123456  y|
            ///  2 4     V
            var possibleGenes = new List<List<EGene>>();

            //crossbreed for each geneSlot
            for (int geneIndex = 0; geneIndex < breeds[0].Genes.Length; geneIndex++)
            {
                var genes = new EGene[breeds.Count];

                for (int breedIndex = 0; breedIndex < breeds.Count; breedIndex++)
                {
                    genes[breedIndex] = breeds[breedIndex].Genes[geneIndex];
                }

                possibleGenes.Add(CrossbreedGenes(genes));
            }

            //then purmuatate all combinations
            /// outputGenes[x][y] or [breed][gene]
            /// [x][y] or [breed][gene]
            /// y ->
            /// 123456  x|
            /// 123456   V
            var outputGenes = PurmutatePossibleGenes(possibleGenes);

            var outputBreeds = new List<Breed>(outputGenes.Count);
            var parentGenes = new string[breeds.Count];
            int generation = GetGeneration(breeds);

            for (int i = 0; i < parentGenes.Length; i++)
            {
                parentGenes[i] = breeds[i].ToString();
            }

            foreach (List<EGene> genes in outputGenes)
            {
                outputBreeds.Add(new Breed(genes.ToArray(), parentGenes, generation));
            }

            return outputBreeds;
        }

        /// <summary>
        /// Crossbreeds genes.
        /// </summary>
        /// <param name="breeds">
        /// Breeds is a colection of breeds which are a collection of genes.
        /// [x][y] or [breed][gene]
        /// y ->
        /// 123456  x|
        /// 123456   V
        /// </param>
        /// <returns>Returns a colection of breeds that are possible.</returns>
        public List<List<EGene>> CrossbreedSimple(List<List<EGene>> breeds)
        {
            if (breeds == null) throw new ArgumentNullException(nameof(breeds));
            if (!IsValidCountForCrossbreed(breeds.Count)) throw new ArgumentException(nameof(breeds));

            //check if breeds and genes exist and are valid for crossbreed
            if (breeds.Any(breed => breed == null)) throw new ArgumentNullException(nameof(breeds));
            if (!BreedsAreSameLength(breeds)) throw new ArgumentException(nameof(breeds));
            if (breeds[0].Count < 1) throw new ArgumentException(nameof(breeds));

            /// possibleGenes[x][y] or [geneSlot][genePossibility]
            ///  x ->
            /// 123456  y|
            ///  2 4     V
            var possibleGenes = new List<List<EGene>>();

            //crossbreed for each geneSlot
            for (int geneIndex = 0; geneIndex < breeds[0].Count; geneIndex++)
            {
                var genes = new EGene[breeds.Count];

                for (int breedIndex = 0; breedIndex < breeds.Count; breedIndex++)
                {
                    genes[breedIndex] = breeds[breedIndex][geneIndex];
                }

                possibleGenes.Add(CrossbreedGenes(genes));
            }

            //then purmuatate all combinations
            /// outputGenes[x][y] or [breed][gene]
            /// [x][y] or [breed][gene]
            /// y ->
            /// 123456  x|
            /// 123456   V
            var outputGenes = PurmutatePossibleGenes(possibleGenes);

            return outputGenes;
        }

        /// <summary>
        /// Crossbreeds a line of genes.
        /// <br>X----- |</br>
        /// <br>X----- V</br>
        /// </summary>
        /// <param name="genes">Genes that get crossbreed.</param>
        /// <returns>Returns a list of possible genes.
        /// If two genes have the equal weight (eg X, W) they will have a 50/50 chance of occuring.</returns>
        public List<EGene> CrossbreedGenes(EGene[] genes)
        {
            if (genes == null) throw new ArgumentNullException(nameof(genes));
            if (genes.Length < MinBreedsToCrossbreed) throw new ArgumentException(nameof(genes));
            if (genes.Length > MaxBreedsToCrossbreed) throw new ArgumentException(nameof(genes));


            var outputGenes = new List<EGene>();
            var geneWeights = CreateEmptyGeneWeights();

            //fill out geneWeights
            foreach (EGene gene in genes)
            {
                //get index of existing geneWeight
                int i = Array.FindIndex(geneWeights, gw => gw.gene == gene);
                //add to weight if geneWeight exists
                if (i >= 0)
                {
                    var a = geneWeights[i].weight;
                    geneWeights[i].weight += GetWeightOfGene(gene);
                }
                else //else if gene not found, scream
                    throw new Exception(nameof(gene));
            }

            //sort by weight
            Array.Sort(geneWeights, (x, y) => y.weight.CompareTo(x.weight));

            //add genes with the highest weight to output
            float highestWeight = -1;
            foreach (var gw in geneWeights)
            {
                if (gw.weight >= highestWeight)
                {
                    highestWeight = gw.weight;
                    outputGenes.Add(gw.gene);
                }
                else
                    break;
            }
            if (outputGenes.Count == 0)
                throw new Exception($"No gene in {nameof(geneWeights)} has a weight.");

            return outputGenes;
        }

        /// <summary>
        /// Gets the weight of a <paramref name="gene"/>.
        /// </summary>
        /// <param name="gene">Gene that you want to the weight for.</param>
        /// <returns>Returns a float that is the wieght of <paramref name="gene"/>.</returns>
        public float GetWeightOfGene(EGene gene)
        {
            switch (gene)
            {
                case EGene.X:
                case EGene.W:
                    return 1;
                case EGene.G:
                case EGene.Y:
                case EGene.H:
                    return .6f;
                case EGene.None:
                    return 0;
                default:
                    throw new ArgumentException(nameof(gene));
            }
        }

        /// <summary>
        /// Figures out the generation based of the <paramref name="generations"/>.
        /// </summary>
        /// <param name="generations"></param>
        /// <returns>Return an int that is +1 to the oldest generation (biggest int).</returns>
        public int GetGeneration(IEnumerable<int> generations)
        {
            int gen = int.MinValue;
            foreach (int generation in generations)
            {
                if (generation > gen)
                    gen = generation;
            }
            gen++;
            return gen;
        }

        /// <summary>
        /// Figures out the generation based of the generations of the <paramref name="breeds"/>.
        /// </summary>
        /// <param name="breeds"></param>
        /// <returns>Return an int that is +1 to the oldest generation (biggest int).</returns>
        public int GetGeneration(IEnumerable<Breed> breeds)
        {
            int gen = int.MinValue;
            foreach (Breed breed in breeds)
            {
                if (breed.Generation > gen)
                    gen = breed.Generation;
            }
            gen++;
            return gen;
        }

        public bool IsValidCountForCrossbreed(int count)
        {
            return (count >= MinBreedsToCrossbreed && count <= MaxBreedsToCrossbreed);
        }

        /// <summary>
        /// Creates geneWeights with its wieghts initialized to 0
        /// <br>and genes initialized to all possible values of EGene.</br>
        /// </summary>
        /// <returns>Returns an array of valuetuple (EGene gene, float wieght), used as a table for tracking wiehgts.</returns>
        /// <remarks>
        /// why isn't geneWeights just a list?
        /// because list return a copy of the value, which makes it unable to modify valuetuples
        /// why not just hard code it? (unroll the loops)
        /// because then I would need to add a new gene to EGene and GetWeightOfGene() if a new gene is ever added
        /// why not just use two seperate arrays/lists?
        /// becasue I like pretty code and valuetuples
        /// </remarks>
        private (EGene gene, float weight)[] CreateEmptyGeneWeights()
        {
            //get all values of enum EGene
            var geneValues = (EGene[])Enum.GetValues(typeof(EGene));

            //create new array of the type of valuetuple
            var geneWeights = new (EGene gene, float weight)[geneValues.Length];

            //copy all values of enum EGene into geneWeights
            for (int i = 0; i < geneValues.Length; i++)
            {
                geneWeights[i].gene = geneValues[i];
                geneWeights[i].weight = 0;
            }

            return geneWeights;
        }

        /// <summary>
        /// Checks to see if breeds in <paramref name="breeds"/> are the same length.
        /// </summary>
        /// <param name="breeds">Breeds is a list of breeds, which are a list of genes.</param>
        /// <returns>Returns true if all breeds are the same length and false if they are not.</returns>
        /// 
        private bool BreedsAreSameLength(List<List<EGene>> breeds)
        {
            int length = breeds[0].Count;
            foreach (List<EGene> breed in breeds)
            {
                if (breed.Count != length)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if breeds in <paramref name="breeds"/> are the same length.
        /// </summary>
        /// <param name="breeds">Breeds is a list of breeds, which are a list of genes.</param>
        /// <returns>Returns true if all breeds are the same length and false if they are not.</returns>
        /// 
        private bool BreedsAreSameLength(IList<Breed> breeds)
        {
            int length = breeds[0].Genes.Length;
            foreach (Breed breed in breeds)
            {
                if (breed.Genes.Length != length)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Purmutates all <paramref name="possibleGenes"/> to a list of breeds.
        /// </summary>
        /// <param name="possibleGenes">List of of possible genes where [x][y] => y is the genes possible for gene slot x.</param>
        /// <returns>Returns a list of breeds, which is a list of genes.</returns>
        private List<List<EGene>> PurmutatePossibleGenes(List<List<EGene>> possibleGenes)
        {
            /// possibleGenes[x][y] or [geneSlot][genePossibility]
            ///  x ->
            /// 123456  y|
            ///  2 4     V

            /// genePermutations[x][y] or [breed][gene]
            /// [x][y] or [breed][gene]
            /// y ->
            /// 123456  x|
            /// 123456   V
            var genePermutations = new List<List<EGene>>();
            var iterators = new int[possibleGenes.Count];
            var stops = new int[possibleGenes.Count]; //technically not necessary, but more readable

            //initialize iterators and stops
            for (int i = 0; i < possibleGenes.Count; i++)
            {
                iterators[i] = 0;
                stops[i] = possibleGenes[i].Count;
            }

            ///now that I look at this, this is kinda like a for loop, but with a more complex iterator
            while (true)
            {
                //form purmutation
                var breed = new List<EGene>(possibleGenes.Count);
                for (int i = 0; i < iterators.Length; i++)
                {
                    //genes slot i, and gene is option interator[i]
                    breed.Add(possibleGenes[i][iterators[i]]);
                }
                genePermutations.Add(breed);

                //if last loop, break
                bool allStopsReached = true;
                for (int i = 0; i < iterators.Length; i++)
                {
                    if (iterators[i] != stops[i] - 1)
                    {
                        allStopsReached = false;
                        break;
                    }
                }
                if (allStopsReached)
                    break;

                //else interate iterators
                ///the basic way this works is that its kinda like counting,
                ///except each digit is of a different base
                iterators[0]++;
                for (int i = 0; i < iterators.Length; i++)
                {
                    //if iterator is at stop
                    if (iterators[i] == stops[i])
                    {
                        //set iterator back to 0
                        iterators[i] = 0;
                        //overflow count to next iterator, if exists
                        if ((i + 1) < iterators.Length)
                            iterators[i + 1]++;
                    }
                }
            }

            return genePermutations;
        }
        #endregion
    }
}
