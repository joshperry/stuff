//----------------------------------------------------------------------------
//  <copyright file=”AmssFirmwareFile.cs” company=”6bit Inc.”>
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

namespace GobiLoader.Firmware
{
    class AmssFirmwareFile : FirmwareFile
    {
        public AmssFirmwareFile(string path)
            : base(path)
        {
            if (!path.ToLowerInvariant().Contains("amss"))
                throw new ApplicationException("Invalid firmware file for this object, expected amss.mbn");
        }

        public override int Length
        {
            get { return (int)Info.Length - 8; }
        }

        public override byte FirmwareIDByte
        {
            get { return 0x05; }
        }
    }
}
