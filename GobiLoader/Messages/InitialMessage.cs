//----------------------------------------------------------------------------
//  <copyright file=”InitialMessage.cs” company=”6bit Inc.”>
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
using System.IO;

namespace GobiLoader.Messages
{
    class InitialMessage : Message
    {
        private static readonly byte[] msg = new byte[] {0x01, 0x51, 0x43, 0x4f, 0x4d, 0x20, 0x68, 0x69, 0x67, 0x68,
                0x20, 0x73, 0x70, 0x65, 0x65, 0x64, 0x20, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x63,
                0x6f, 0x6c, 0x20, 0x68, 0x73, 0x74, 0x00, 0x00, 0x00, 0x00, 0x04, 0x04, 0x30};

        protected override byte[] MessageBytes
        {
            get { return msg; }
        }

        protected override byte ExpectedResponse
        {
            get { return 0x02; }
        }
    }
}
