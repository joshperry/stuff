//----------------------------------------------------------------------------
//  <copyright file=”GobiLoader.cs” company=”6bit Inc.”>
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
using System.IO;

namespace GobiLoader
{
    public class GobiLoader
    {
        Config.GobiLoaderConfiguration _config;

        public GobiLoader(Config.GobiLoaderConfiguration config)
        {
            _config = config;
        }

        public void LoadFirmware()
        {
            var firmwares = new Firmware.FirmwareProvider();
            using (var txer = new Messages.MessageTransceiver(new PortProvider(_config.DeviceIds.Cast<Config.DeviceConfiguration>())))
            {
                txer.Tranceive(new Messages.InitialMessage());

                txer.Tranceive(new Messages.FirmwarePreludeMessage(firmwares.AmssFirmware));
                txer.Tranceive(new Messages.SendFirmwareFileMessage(firmwares.AmssFirmware));

                txer.Tranceive(new Messages.FirmwarePreludeMessage(firmwares.AppsFirmware));
                txer.Tranceive(new Messages.SendFirmwareFileMessage(firmwares.AppsFirmware));

                txer.Tranceive(new Messages.ConclusionMessage());
            }
        }
    }
}
