//----------------------------------------------------------------------------
//  <copyright file=”PortProvider.cs” company=”6bit Inc.”>
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
using Microsoft.Win32;
using System.IO.Ports;
using GobiLoader.Util;

namespace GobiLoader
{
    class PortProvider : IDisposable
    {
        const string USB_BASE_PATH = @"System\CurrentControlSet\Enum\USB\{0}\";
        bool _disposed = false;

        public PortProvider(IEnumerable<Config.DeviceConfiguration> usbDevIds)
        {
            Port = new SerialPort(FindComPort(usbDevIds), 115200);
            Port.ReadTimeout = 2000;
            Port.WriteTimeout = 2000;
        }

        private string FindComPort(IEnumerable<Config.DeviceConfiguration> usbDevIds)
        {
            // The driver writes the QDLoader com port to the registry, we'll go look for it there
            string comport = string.Empty;

            IEnumerator<Config.DeviceConfiguration> devenum = usbDevIds.GetEnumerator();
            while(devenum.MoveNext())
            {
                comport = FindComPort(devenum.Current.DeviceId);
                if (!string.IsNullOrEmpty(comport))
                {
                    Tracer.WriteLineInfo("Found loader comport {0} for a {1} Qualcomm card", comport, devenum.Current.Brand);
                    break;
                }
            }

            if (string.IsNullOrEmpty(comport))
                throw new ApplicationException("Could not divine the com port from the registry, are you sure you have a Gobi card and drivers installed? Make sure that the \"Qualcomm HS-USB QDLoader 9201\" port is showing in device manager under \"Ports (COM & LPT)\".");

            return comport;
        }

        private string FindComPort(string devpath)
        {
            string comport = string.Empty;

            // Goto the VID/PID for the card
            RegistryKey k = Registry.LocalMachine.OpenSubKey(string.Format(USB_BASE_PATH, devpath));
            if (k != null)
            {
                // The subkeys are any of this VID/PID attached to USB, there could be more than one, or none, we will just use the first
                string[] subkeys = k.GetSubKeyNames();
                if (subkeys.Length > 0)
                {
                    // The com port is under the "Device Parameters" subkey, "PortName" value
                    RegistryKey devkey = k.OpenSubKey(subkeys[0] + @"\Device Parameters\");
                    comport = devkey.GetValue("PortName") as string;
                    devkey.Close();
                }
                k.Close();
            }

            return comport;
        }

        public SerialPort Port { get; private set; }

        public void  Dispose()
        {
            if (!_disposed)
            {
                Tracer.WriteLineInfo("Connection to card closed");
                if (Port != null)
                    Port.Dispose();

                Port = null;
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
