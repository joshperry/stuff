using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GobiLoader.Config
{
    public class DeviceConfigurationCollection : ConfigurationElementCollection<string, DeviceConfiguration>
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "device"; }
        }

        protected override string GetElementKey(DeviceConfiguration element)
        {
            return element.Brand;
        }
    }
}
