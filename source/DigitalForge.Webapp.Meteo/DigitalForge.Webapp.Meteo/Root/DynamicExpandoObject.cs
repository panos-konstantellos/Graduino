// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace WebApplication1.Root
{
    public class DynamicExpandoObject : DynamicObject, IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _dictionaryImpl;

        public DynamicExpandoObject()
        {
            this._dictionaryImpl = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this._dictionaryImpl.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this._dictionaryImpl[binder.Name] = value;

            return true;
        }

        #region IDictionary<string, object>

        object IDictionary<string, object>.this[string key]
        {
            get { return this._dictionaryImpl[key]; }
            set { this._dictionaryImpl[key] = value; }
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { return this._dictionaryImpl.Keys; }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { return this._dictionaryImpl.Values; }
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return this._dictionaryImpl.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).IsReadOnly; }
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            this._dictionaryImpl.Add(key, value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).Contains(item);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return this._dictionaryImpl.ContainsKey(key);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return (this._dictionaryImpl as IEnumerable<KeyValuePair<string, object>>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this._dictionaryImpl as IEnumerable).GetEnumerator();
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return this._dictionaryImpl.Remove(key);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return (this._dictionaryImpl as ICollection<KeyValuePair<string, object>>).Remove(item);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return this._dictionaryImpl.TryGetValue(key, out value);
        }

        #endregion
    }
}
