using System;
using RustCrossbreed.BusinessLogic;

namespace RustCrossbreed.Factories
{
    public class BreedFactory
    {
        public static EGene[] ParseGenes(string genes)
        {
            genes = genes.ToUpper();
            EGene[] output = new EGene[genes.Length];

            for (int i = 0; i < genes.Length; i++)
            {
                switch (genes[i])
                {
                    case 'X':
                    case 'W':
                    case 'G':
                    case 'Y':
                    case 'H':
                    case '?':
                        output[i] = (EGene)genes[i];
                        break;
                    default:
                        throw new ArgumentException(nameof(genes));
                }
            }

            return output;
        }
        public static bool TryParseGenes(string gene, out EGene[] output)
        {
            try
            {
                output = ParseGenes(gene);
                return true;
            }
            catch (ArgumentException)
            {
                output = null;
                return false;
            }
        }
        public static Breed ParseBreed(string genes, string[] parentGenes = null, int generation = 0)
        {
            return new Breed(ParseGenes(genes), parentGenes, generation);
        }
        public static bool TryParseBreed(string genes, out Breed breed, string[] parentGenes = null, int generation = 0)
        {
            if (TryParseGenes(genes, out EGene[] output))
            {
                breed = new Breed(output, parentGenes, generation);
                return true;
            }
            breed = null;
            return false;
        }
    }
}
