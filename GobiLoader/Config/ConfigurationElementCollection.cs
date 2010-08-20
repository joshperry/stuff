//----------------------------------------------------------------------------
//  <copyright file=”ConfigurationElementCollection.cs” company=”6bit Inc.”>
//      Copyright 2009
//  </copyright>
//  <author>Joshua Perry (josh@6bit.com)</author>
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; version 2 of the License.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with GobiLoader; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//----------------------------------------------------------------------------
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
