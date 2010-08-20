//----------------------------------------------------------------------------
//  <copyright file=”Tracer.cs” company=”6bit Inc.”>
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
using System.Diagnostics;

namespace GobiLoader.Util
{
    static class Tracer
    {
        static TraceSwitch _ts;
        static Tracer()
        {
            _ts = new TraceSwitch("TraceLevel", "TraceLevel", "3");
        }

        public static void WriteLineVerbose(string message)
        {
            Trace.WriteLineIf(_ts.TraceVerbose, message);
        }

        public static void WriteLineVerbose(string message, params object[] parms)
        {
            Trace.WriteLineIf(_ts.TraceVerbose, string.Format(message, parms));
        }

        public static void WriteLineInfo(string message)
        {
            Trace.WriteLineIf(_ts.TraceInfo, message);
        }

        public static void WriteLineInfo(string message, params object[] parms)
        {
            Trace.WriteLineIf(_ts.TraceInfo, string.Format(message, parms));
        }

        public static void WriteLineError(string message)
        {
            Trace.WriteLineIf(_ts.TraceError, message);
        }

        public static void WriteLineError(string message, params object[] parms)
        {
            Trace.WriteLineIf(_ts.TraceError, string.Format(message, parms));
        }
    }
}
