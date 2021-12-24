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
        /// 
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