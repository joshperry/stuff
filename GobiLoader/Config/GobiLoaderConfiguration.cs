using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GobiLoader.Config
{
    public class GobiLoaderConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("devices")]
        [ConfigurationCollection(typeof(DeviceConfigurationCollection))]
        public DeviceConfigurationCollection DeviceIds
        {
            get
            {
                return base["devices"] as DeviceConfigurationCollection;
            }
        }
    }
}
