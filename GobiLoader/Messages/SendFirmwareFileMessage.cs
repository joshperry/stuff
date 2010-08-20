//----------------------------------------------------------------------------
//  <copyright file=”SendFirmwareFileMessage.cs” company=”6bit Inc.”>
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
using System.Diagnostics;
using GobiLoader.Util;
using GobiLoader.Firmware;

namespace GobiLoader.Messages
{
    class SendFirmwareFileMessage : Message
    {
        const int TRANSFER_BLOCK_SIZE = 1024 * 1024;

        byte[] msg = { 0x27, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        FirmwareFile _file;

        public SendFirmwareFileMessage(FirmwareFile file)
        {
            _file = file;

            // Copy the file size (4 bytes) into the message starting at byte 7
            Array.Copy(file.LengthBytes, 0, msg, 7, 4);
        }

        public override void Send(System.IO.Stream stream)
        {
            // let the Message send the header
            base.Send(stream);

            // Pause 200ms and then send the file contents
            System.Threading.Thread.Sleep(200);
            SendFile(stream);
        }

        private void SendFile(Stream ss)
        {
            Tracer.WriteLineInfo("Sending firmware file to card.");

            using (Stream fs = _file.GetStream())
            {
                StreamBlockTransfer xfer = new StreamBlockTransfer(fs, ss, TRANSFER_BLOCK_SIZE, _file.Length)
                {
                    InterblockPause = 100,
                    ProgressCallback = (x => Tracer.WriteLineInfo("\t{2:0%} written, {0} bytes of {1}", x.TotalTransferred, _file.Length, ((double)x.TotalTransferred)/_file.Length)),
                };

                xfer.Transfer();
            }
        }

        private void CheckBlockError(Stream ss)
        {
            byte[] buf = new byte[1024];

            // Change the read timeout to 100ms
            int prevrt = ss.ReadTimeout;
            ss.ReadTimeout = 100;

            try
            {
                // See if we get anything
                int read = ss.Read(buf, 0, buf.Length);

                Tracer.WriteLineVerbose(">> {{ {0}}}", PrintByteArray(buf, read));

                if (buf[1] == 0x0D)
                    throw new ApplicationException("The card responded with an error while sending a block.");
            }
            catch (TimeoutException) { }
            finally
            {
                ss.ReadTimeout = prevrt;
            }
        }

        protected override byte[] MessageBytes
        {
            get { return msg; }
        }

        protected override bool ShouldWrap
        {
            get { return false; }
        }

        protected override byte ExpectedResponse
        {
            get { return 0x28; }
        }
    }
}
