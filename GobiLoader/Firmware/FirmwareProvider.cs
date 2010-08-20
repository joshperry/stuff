//----------------------------------------------------------------------------
//  <copyright file=”FirmwareProvider.cs” company=”6bit Inc.”>
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

namespace GobiLoader.Firmware
{
    class FirmwareProvider
    {
        public FirmwareProvider()
        {
            LoadFirmwareFiles();
        }

        private void LoadFirmwareFiles()
        {
            string[] paths = File.ReadAllLines(@"C:\Users\All Users\QUALCOMM\QDLService\Options.txt");
            if (paths.Length >= 2)
            {
                foreach (string path in paths)
                {
                    if (path.ToLowerInvariant().Contains("amss"))
                    {
                        AmssFirmware = new AmssFirmwareFile(path);
                    }
                    else if (path.ToLowerInvariant().Contains("apps"))
                    {
                        AppsFirmware = new AppsFirmwareFile(path);
                    }
                }

                if (AmssFirmware == null || AppsFirmware == null)
                    throw new ApplicationException(@"Firmware paths in C:\Users\All Users\QUALCOMM\QDLService\Options.txt are missing or not valid");
            }
        }

        public FirmwareFile AmssFirmware { get; set; }
        public FirmwareFile AppsFirmware { get; set; }
    }
}
