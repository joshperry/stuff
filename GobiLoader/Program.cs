//----------------------------------------------------------------------------
//  <copyright file=”Program.cs” company=”6bit Inc.”>
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
using System.Diagnostics;
using GobiLoader.Util;
using System.Configuration;

namespace GobiLoader
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tracer.WriteLineInfo("GobiLoader for Windows 7\n---------------------------------------\n");

                Config.GobiLoaderConfiguration config = ConfigurationManager.GetSection("gobiLoader") as Config.GobiLoaderConfiguration;
                GobiLoader loader = new GobiLoader(config);
                loader.LoadFirmware();

                Tracer.WriteLineInfo("Done...");
            }
            catch (Exception ex) { Tracer.WriteLineError("!Boom: " + ex.Message); }

            Console.WriteLine("Press the any key to continue...");
            Console.ReadKey();
        }
    }
}
