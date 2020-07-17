using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RustCrossbreed.BusinessLogic
{
    //kinda immutable
    //after spending some time on this, I don't remember why I bothered making this immutable, but here it is
    public class Breed : IEquatable<Breed>
    {
        #region Variables
        private readonly EGene[] _genes;
        private readonly string[] _parentGenes;
        #endregion

        #region Properties
        public EGene[] Genes
        {
            get
            {
                EGene[] copy = new EGene[_genes.Length];
                _genes.CopyTo(copy, 0);
                return copy;
            }
        }

        //parrentGenes is string, since I don't want direct refreces and it makes comparisons/searching easier
        public string[] ParentGenes
        {
            get
            {
                string[] copy = new string[_parentGenes.Length];
                _parentGenes.CopyTo(copy, 0);
                return copy;
            }
        }

        //0 = intial, 1 = bred from 0, 2 = bred using a 1
        public int Generation { get; }

        public string GenesString { get; }
        #endregion

        #region Constructors
        public Breed(EGene[] genes, string[] parentGenes = null, int generation = 0)
        {
            _genes = genes ?? throw new ArgumentNullException(nameof(genes));
            Generation = generation;

            if (parentGenes == null)
                _parentGenes = new string[0];
            else
            {
                string[] tmp = parentGenes.ToArray();
                Array.Sort(tmp);
                _parentGenes = tmp;
            }

            //create _geneString
            if (Genes.Length == 0)
                GenesString = string.Empty;
            else
            {
                StringBuilder output = new StringBuilder(Genes.Length);
                foreach (EGene gene in Genes)
                {
                    output.Append((char)gene);
                }
                GenesString = output.ToString();
            }
        }
        #endregion


        #region Overrides
        public override string ToString()
        {
            return GenesString;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Breed);
        }
        public override int GetHashCode()
        {
            //just what VS spat out, not sure if correct
            return HashCode.Combine(_parentGenes, GenesString);
        }
        #endregion

        #region Operators
        public static bool operator ==(Breed breed1, Breed breed2)
        {
            if (breed1 is null)
                return false;
            return breed1.Equals(breed2);
        }
        public static bool operator !=(Breed breed1, Breed breed2)
        {
            if (breed1 is null)
                return true;
            return !breed1.Equals(breed2);
        }
        #endregion

        #region IEquatable
        public bool Equals([AllowNull] Breed other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (this.ToString() != other.ToString())
                return false;

            for (int i = 0; i < ParentGenes.Length; i++)
            {
                if (ParentGenes[i] != other.ParentGenes[i])
                    return false;
            }

            return true;
        }
        #endregion

        #region Static Methods
        public static string ToString(EGene[] genes)
        {
            if (genes == null) throw new ArgumentNullException(nameof(genes));
            if (genes.Length == 0) return string.Empty;

            StringBuilder output = new StringBuilder(genes.Length);
            foreach (EGene gene in genes)
            {
                output.Append((char)gene);
            }
            return output.ToString();
        }
        #endregion
    }
}
