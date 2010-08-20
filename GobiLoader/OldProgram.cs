using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace OldGobiLoader
{
    class Program
    {
        // The final 0xFF 0xFF in each of these messages is replaced with a calculated checksum when the message is sent.
        // These messages seem to conform somewhat to standard PPP messages, using a 16-bit CCITT CRC at the end, and being wrapped with a beginning and ending 0x7E bytes

        // The protocol seems to use a simple sequential handshake, each message's initial byte is an identifier, and a successfull repsonse will be that identifier + 1
        // e.g. the initial handshake message id is 0x01 and a successfull transmission will result in a response message with an identifier of 0x02
        static byte[] s1 = new byte[] {0x01, 0x51, 0x43, 0x4f, 0x4d, 0x20, 0x68, 0x69, 0x67, 0x68,
                0x20, 0x73, 0x70, 0x65, 0x65, 0x64, 0x20, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x63,
                0x6f, 0x6c, 0x20, 0x68, 0x73, 0x74, 0x00, 0x00, 0x00, 0x00, 0x04, 0x04, 0x30, 0xFF, 0xFF};

        // This message is sent with the size of the first (amss.mbn) firmware file's size embedded as 4 bytes starting at byte 3
        // This message is formated with the wrapped 0x7E message guards
        // A valid response is also wrapped in 0x7E bytes and byte 2 will be 0x26
        // NOTE: The size of the first file needs to be shortened in all uses by 8 bytes
        static byte[] s2 = new byte[] { 0x25, 0x05, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0xFF, 0xFF };

        // This message is sent immediately preceeding the file upload, the size of the file is embedded starting at byte 8
        // This message is not wrapped in the 0x7E guards
        // Once the firmware file is uploaded successfully a valid response message will wrapped in 0x7E bytes and byte 2 will be 0x28
        // The card seems to like a pause of a couple hundred milliseconds between this message and the start of the file transfer
        static byte[] s3 = new byte[] { 0x27, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF };

        // The next two messages work the same as s2 and s3 except they contain the size for the second (apps.mbn) firmware file
        static byte[] s4 = new byte[] { 0x25, 0x06, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0xFF, 0xFF };

        static byte[] s5 = new byte[] { 0x27, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF };

        // This message is sent after the successfull transmission of both firmware files
        // This message is wrapped in the 0x7E guards
        // When this message is sent the QDLoader port goes away and the card loads its firmware and the other communication ports pop up
        static byte[] s6 = new byte[] { 0x29, 0xFF, 0xFF };

        private static Stream ss;
        static byte[] rbuf = new byte[1024];
        static void OldMain(string[] args)
        {
            SerialPort sp = null;
            try
            {
                string[] firmware = GetFirmwarePaths();
                InjectFirmwareLengths(firmware);

                string comPort = FindComPort();
                sp = new SerialPort(comPort, 115200);
                sp.WriteTimeout = 1000;
                sp.Open();
                ss = sp.BaseStream;

                Send(s1, true);
                Receive(0x02);

                Send(s2, true);
                Receive(0x26);

                Send(s3, false);
                System.Threading.Thread.Sleep(200);
                // Send amss file
                SendFile(firmware[0]);
                Receive(0x28);

                Send(s4, true);
                Receive(0x26);

                Send(s5, false);
                System.Threading.Thread.Sleep(200);
                // Send apps file
                SendFile(firmware[1]);
                Receive(0x28);

                Send(s6, true);
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
            finally
            {
                if(sp != null)
                    sp.Close();
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }

        private static void InjectFirmwareLengths(string[] paths)
        {
            // Get the length of the firmware files and inject them into the messages at the appropriate positions
            FileInfo fi1 = new FileInfo(paths[0]);
            FileInfo fi2 = new FileInfo(paths[1]);

            // The first firmware file needs to be truncated by 8 bytes
            int l1 = (int)fi1.Length - 8;
            int l2 = (int)fi2.Length;

            byte[] b1 = BitConverter.GetBytes(l1);
            byte[] b2 = BitConverter.GetBytes(l2);

            Array.Copy(b1, 0, s2, 2, 4);
            Array.Copy(b1, 0, s3, 7, 4);

            Array.Copy(b2, 0, s4, 2, 4);
            Array.Copy(b2, 0, s5, 7, 4);
        }

        private static string[] GetFirmwarePaths()
        {
            string[] paths = File.ReadAllLines(@"C:\Users\All Users\QUALCOMM\QDLService\Options.txt");
            if (paths.Length >= 2)
            {
                if (!paths[0].Contains("amss") || !paths[1].Contains("apps"))
                    throw new ApplicationException(@"Firmware paths in C:\Users\All Users\QUALCOMM\QDLService\Options.txt are missing or not valid");
            }
            return paths;
        }

        private static string FindComPort()
        {
            // The driver writes the QDLoader com port to the registry, we'll go look for it there
            string comport = string.Empty;

            // Goto the VID/PID for the card
            RegistryKey k = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Enum\USB\VID_05C6&PID_9201\");
            if (k != null)
            {
                // The subkeys are any of this VID/PID attached to USB, there could be more than one, or none, we will just use the first
                string[] subkeys = k.GetSubKeyNames();
                if (subkeys.Length > 0)
                {
                    // The com port is under the "Device Parameters" subkey, "PortName" value
                    RegistryKey devkey = k.OpenSubKey(subkeys[0] + @"\Device Parameters\");
                    comport = devkey.GetValue("PortName") as string;
                    devkey.Close();
                }
                k.Close();
            }

            if (comport == string.Empty)
                throw new ApplicationException("Could not divine the com port from the registry, are you sure you have a Gobi card installed? Make sure that the \"Qualcomm HS-USB QDLoader 9201\" port is showing in device manager under \"Ports (COM & LPT)\".");
            return comport;
        }

        private static void Receive(byte expect)
        {
            // Receive any data from the port and make sure that byte 2 is the expected value
            int read = ss.Read(rbuf, 0, rbuf.Length);
            Console.Write(">> { "); PrintByteArray(rbuf, read); Console.WriteLine("}");

            // If we didn't get what we were expecting, then punt
            if (rbuf[1] != expect)
                throw new ApplicationException(string.Format("Invalid Response recieved; expected {0:X2} got {1:X2}", expect, rbuf[1]));
        }

        static void PrintByteArray(byte[] buf, int len)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                sb.AppendFormat("{0:X2} ", buf[i]);
            }

            Console.Write(sb.ToString());
        }

        static byte[] fbuf = new byte[1048576];
        private static void SendFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            long blocks = fi.Length / 1048576;
            if (fi.Length % 1048576 > 0) blocks++;
            Console.WriteLine("Sending {0} blocks for file {1}", blocks, path);
            
            Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            
            for (int j = 0; j < blocks; j++)
            {
                // Write a 1048576 byte block
                //HACK: truncate the larger file by 8 bytes
                bool trunc = (j == blocks - 1) // last block
                    && path.Contains("amss"); // amss file

                if (trunc)
                    Console.WriteLine("\tTruncating last block by 8 bytes");

                // Read up to 1MB from the firmware file
                int read = fs.Read(fbuf, 0, fbuf.Length);

                Console.WriteLine("\tWriting {0} bytes for block {1}", read, j);
                // Write out the read data, unless we are on the last block, then write out everything except the last 8 bytes
                ss.Write(fbuf, 0, trunc ? read - 8 : read);

                // Flush the stream
                Console.WriteLine("\tFlushing the block");
                //ss.Write(fbuf, 0, 0);
                ss.Flush();
                System.Threading.Thread.Sleep(100);
            }
        }

        static readonly byte[] wrapbyte = new byte[] { 0x7E };
        static void Send(byte[] buf, bool wrap)
        {
            Console.Write("<< { ");
            if (wrap)
            {
                Console.Write("{0:X2} ", wrapbyte[0]);
                ss.Write(wrapbyte, 0, 1);
            }

            InjectCRC(buf);
            PrintByteArray(buf, buf.Length);
            ss.Write(buf, 0, buf.Length);

            if (wrap)
            {
                Console.Write("{0:X2} ", wrapbyte[0]);
                ss.Write(wrapbyte, 0, 1);
            }

            Console.WriteLine("}");
            ss.Flush();
        }

        private static void InjectCRC(byte[] buf)
        {
            ushort crc = CalcCRC(buf, buf.Length - 2);
            byte[] bcrc = BitConverter.GetBytes(crc);

            Array.Copy(bcrc, 0, buf, buf.Length - 2, 2);
        }

        // CRC calculation code uncerimoniously ripped off from the Linux Kernel source code and "ported" to C# by moi
        static ushort CalcCRC(byte[] buf, int len)
        {
            ushort crc = 0xffff;
            for(int i = 0; i < len; i++)
                crc = crc_ccitt_byte(crc, buf[i]);
            return (ushort)(crc ^ 0xFFFF);
        }

        static ushort crc_ccitt_byte(ushort crc, byte c)
        {
            return (ushort)((crc >> 8) ^ crc_ccitt_table[(crc ^ c) & 0xff]);
        }

        static readonly ushort[] crc_ccitt_table = new ushort[] {
            0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
        };
    }
}
