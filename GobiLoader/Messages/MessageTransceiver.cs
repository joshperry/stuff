//----------------------------------------------------------------------------
//  <copyright file=”MessageTransceiver.cs” company=”6bit Inc.”>
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
using GobiLoader.Util;

namespace GobiLoader.Messages
{
    class MessageTransceiver : IDisposable
    {
        PortProvider _portSource;
        
        public MessageTransceiver(PortProvider portSource)
        {
            _portSource = portSource;
            Tracer.WriteLineInfo("Opening connection to card");
            portSource.Port.Open();
        }

        public Stream PortStream { get { return _portSource.Port.BaseStream; } }

        public void Tranceive(Messages.Message message)
        {
            Tracer.WriteLineInfo("Sending {0}", message.GetType().Name);
            message.Send(PortStream);
            message.Receive(PortStream);
        }

        public void Dispose()
        {
            _portSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
