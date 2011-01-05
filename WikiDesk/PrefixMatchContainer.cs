
namespace WikiDesk
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Matches entries with partial, prefix matching.
    /// </summary>
    public class PrefixMatchContainer<TValue> where TValue : class
    {
        public event CollectionChangeEventHandler OnCollectionChanged;

        public PrefixMatchContainer()
        {
            strings_ = new AutoCompleteStringCollection();
            strings_.CollectionChanged += strings__CollectionChanged;
        }

        public int Count
        {
            get { return strings_.Count; }
        }

        public string this[int index]
        {
            get { return strings_[index]; }
        }

        public void Add(string key, TValue value)
        {
            strings_.Add(key);
        }

        public int Find(string key, bool ignoreCase, bool exact)
        {
            if (!ignoreCase && exact)
            {
                return strings_.IndexOf(key);
            }

            for (int i = 0; i < strings_.Count; ++i)
            {
                string s = strings_[i];
                if (exact)
                {
                    if (String.Compare(key, s, ignoreCase, CultureInfo.CurrentCulture) == 0)
                    {
                        return i;
                    }
                }
                else
                {
                    if (String.Compare(key, 0, s, 0, key.Length, ignoreCase, CultureInfo.CurrentCulture) == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #region implementation

        private void InvokeOnCollectionChanged(CollectionChangeEventArgs e)
        {
            CollectionChangeEventHandler handler = OnCollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void strings__CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            InvokeOnCollectionChanged(e);
        }

        #endregion // implementation

        #region representation

        private readonly AutoCompleteStringCollection strings_;

        #endregion // representation
    }
}
