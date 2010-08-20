//----------------------------------------------------------------------------
//  <copyright file=”FirmwarePreludeMessage.cs” company=”6bit Inc.”>
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
//  along with bbPress; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GobiLoader.Firmware;

namespace GobiLoader.Messages
{
    class FirmwarePreludeMessage : Message
    {
        private byte[] msg = { 0x25, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00 };

        public FirmwarePreludeMessage(FirmwareFile file)
        {
            // Set byte 1 to the firmware file id
            msg[1] = file.FirmwareIDByte;

            // Copy the file size (4 bytes) into the message starting at byte 2
            Array.Copy(file.LengthBytes, 0, msg, 2, 4);
        }

        protected override byte[] MessageBytes
        {
            get { return msg; }
        }

        protected override byte ExpectedResponse
        {
            get { return 0x26; }
        }
    }
}
