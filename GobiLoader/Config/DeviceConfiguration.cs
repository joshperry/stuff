using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GobiLoader.Config
{
    public class DeviceConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("brand", IsKey = true, IsRequired = true)]
        public string Brand
        {
            get { return this["brand"] as string; }
            set { this["brand"] = value; }
        }

        [ConfigurationProperty("deviceId", IsRequired = true)]
        public string DeviceId
        {
            get { return this["deviceId"] as string; }
            set { this["deviceId"] = value; }
        }
    }
}
