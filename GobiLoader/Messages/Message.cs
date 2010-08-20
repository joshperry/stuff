//----------------------------------------------------------------------------
//  <copyright file=”Message.cs” company=”6bit Inc.”>
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
using System.Diagnostics;
using GobiLoader.Util;

namespace GobiLoader.Messages
{
    abstract class Message
    {
        protected byte[] rbuf = new byte[1024];
        protected const byte wrapbyte = 0x7E;

        protected abstract byte[] MessageBytes { get; }
        protected abstract byte ExpectedResponse { get; }

        protected virtual bool ShouldWrap { get { return true; } }

        public virtual void Send(Stream stream)
        {
            // Calculate the total length of the output message
            int msglen = MessageBytes.Length
                + 2                 // for CRC
                + (ShouldWrap ? 2 : 0);   // for wrap bytes
            // offset for the message
            int msgoffset = (ShouldWrap ? 1 : 0);
            // offset for the CRC
            int crcoffset = msgoffset + MessageBytes.Length;
            
            // Create our output buffer
            byte[] sbuf = new byte[msglen];

            // Add the wrap bytes to the buffer
            if (ShouldWrap) { sbuf[0] = wrapbyte; sbuf[sbuf.Length - 1] = sbuf[0]; }

            // Copy the message bytes into the buffer
            Array.Copy(MessageBytes, 0, sbuf, msgoffset, MessageBytes.Length);

            // Copy the CRC into the buffer
            Array.Copy(GetCRC(MessageBytes), 0, sbuf, crcoffset, 2);

            Tracer.WriteLineVerbose("<< {{ {0}}}", PrintByteArray(sbuf));

            // Send the buffer
            stream.Write(sbuf, 0, sbuf.Length);
        }

        protected byte[] GetCRC(byte[] buf)
        {
            ushort crc = CcittCrc16.Calc(buf);
            byte[] bcrc = BitConverter.GetBytes(crc);

            return bcrc;
        }

        public virtual void Receive(Stream stream)
        {
            Tracer.WriteLineVerbose("Expecting {0:X2} response", ExpectedResponse);

            // Receive any data from the port and make sure that byte 2 is the expected value
            int read = stream.Read(rbuf, 0, rbuf.Length);
            Tracer.WriteLineVerbose(">> {{ {0}}}", PrintByteArray(rbuf, read));

            // If we didn't get what we were expecting, then punt
            if (rbuf[1] != ExpectedResponse)
                throw new ApplicationException(string.Format("Invalid Response recieved; expected {0:X2} got {1:X2}", ExpectedResponse, rbuf[1]));
        }

        protected string PrintByteArray(byte[] buf){ return PrintByteArray(buf, buf.Length); }
        protected string PrintByteArray(byte[] buf, int len)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                sb.AppendFormat("{0:X2} ", buf[i]);
            }

            return sb.ToString();
        }
    }
}
