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

        public void UpdateObservableCollection(Collection<T> models)
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

            models.Remove(this.Model);
            models.Add(this.Model);
        }
    }
}