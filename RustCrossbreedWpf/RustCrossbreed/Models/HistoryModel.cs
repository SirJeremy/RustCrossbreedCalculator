using RustCrossbreed.BusinessLogic;
using System;
using System.Windows.Documents;

namespace RustCrossbreed.Models
{
    //maybe in the future allow undo using this, but for now its just for info
    public class HistoryModel
    {
        #region Properties
        public EHistoryAction Action { get; }
        public Breed Breed { get; }
        public int Generation { get => Breed.Generation; }
        #endregion

        #region Constructors
        public HistoryModel(EHistoryAction action, Breed breed)
        {
            Action = action;
            Breed = breed ?? throw new ArgumentNullException(nameof(breed));
        }
        #endregion
    }
}
