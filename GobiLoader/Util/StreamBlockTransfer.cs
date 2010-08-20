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
//  along with bbPress; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GobiLoader.Util
{
    /// <summary>
    /// Transfers data from one stream to another in atomic blocks of a specified size for the total length of data specified
    /// </summary>
    class StreamBlockTransfer
    {
        Stream _from;
        Stream _to;
        int _blocksize;
        int _length;
        byte[] _fbuf;

        int _totalWritten = 0;
        int _currentBlock = 0;

        public StreamBlockTransfer(Stream from, Stream to, int blocksize, int length)
        {
            _from = from;
            _to = to;
            _blocksize = blocksize;
            _length = length;

            // Calculate the number of blocks we need to transfer
            BlockCount = _length / _blocksize;
            if (_length % _blocksize > 0) BlockCount++;

            _fbuf = new byte[_blocksize];
        }

        public void Transfer()
        {
            for (; _currentBlock < BlockCount; _currentBlock++)
            {
                // Write a block

                // Read up to a full block from the firmware file
                int read = _from.Read(_fbuf, 0, _fbuf.Length);

                // Make sure that we don't write more data than the requested length
                if ((_totalWritten + read) > _length)
                    read = _length - _totalWritten;

                // Write out the read data
                _to.Write(_fbuf, 0, read);
                _totalWritten += read;

                // Flush the stream
                _to.Flush();

                // Notify the callsite
                if(ProgressCallback != null)
                    ProgressCallback(this);

                // Pause if necissary
                if(InterblockPause > 0)
                    System.Threading.Thread.Sleep(InterblockPause);
            }
        }

        public Action<StreamBlockTransfer> ProgressCallback { get; set; }
        public int InterblockPause { get; set; }
        public int CurrentBlock { get { return _currentBlock + 1; } }
        public int TotalTransferred { get { return _totalWritten; } }
        public int BlockCount { get; private set; }
    }


}
