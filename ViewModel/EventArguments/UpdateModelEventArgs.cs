using System;
using System.Collections.ObjectModel;
using Model;

namespace ViewModel.EventArguments
{
    public class UpdateModelEventArgs<T> : EventArgs where T : IModel
    {
        public readonly T Model;

        public readonly bool Inserted;

        public readonly bool Removed;

        public UpdateModelEventArgs(T model, bool inserted, bool removed)
        {
            this.Model = model;
            this.Inserted = inserted;
            this.Removed = removed;
        }

        /// <summary>
        /// Updates a collection based on the attributes of the given model.
        ///
        /// When inserted just add it. When removed, just removed it. But when updated do both. This triggers the on
        /// property changed as few times as possible.
        /// </summary>
        /// <param name="models"></param>
        public void UpdateCollection(Collection<T> models)
        {
            if (this.Inserted)
            {
                models.Add(this.Model);
                return;
            }

            if (this.Removed)
            {
                models.Remove(this.Model);
                return;
            }

            int index = models.IndexOf(this.Model);
            models.Remove(this.Model);
            models.Insert(index, this.Model);
        }
    }
}