using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GobiLoader.Config
{
    public abstract class ConfigurationElementCollection<K, V> : ConfigurationElementCollection where V : ConfigurationElement, new()
    {
        public ConfigurationElementCollection()
        {
        }

        public abstract override ConfigurationElementCollectionType CollectionType
        {
            get;
        }

        protected abstract override string ElementName
        {
            get;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new V();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return GetElementKey((V)element);
        }

        protected abstract K GetElementKey(V element);

        public void Add(V path)
        {
            BaseAdd(path);
        }

        public void Remove(K key)
        {
            BaseRemove(key);
        }

        public V this[K key]
        {
            get { return (V)BaseGet(key); }
        }

        public V this[int index]
        {
            get { return (V)BaseGet(index); }
        }
    }
}
