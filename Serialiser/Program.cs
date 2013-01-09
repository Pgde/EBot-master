using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serializer
{
    public class Program
    {
        static string InnerspaceDirectory = @"C:\Program Files (x86)\InnerSpace\.NET Programs";
        static void Main()
        {
            // Agents
            Dictionary<string, int> _agents = new Dictionary<string, int>();
            foreach (var item in File.ReadLines(@"..\..\agents.txt"))
            {
                string[] parts = item.Split(',');
                _agents.Add(parts[0], int.Parse(parts[1]));
            }
            Serialise(_agents, "agents");

            // WarpScramblers
            List<int> _ws = new List<int>();
            foreach (var item in File.ReadLines(@"..\..\WarpScramblers.txt"))
            {
                _ws.Add(int.Parse(item));
            }
            Serialise(_ws, "warpScramblers");

            // Commander Wrecks
            List<int> _cw = new List<int>();
            foreach (var item in File.ReadLines(@"..\..\CommanderWrecks.txt"))
            {
                _cw.Add(int.Parse(item));
            }
            Serialise(_cw, "CommanderWrecks");

            // Officer Wrecks
            List<int> _ow = new List<int>();
            foreach (var item in File.ReadLines(@"..\..\OfficerWrecks.txt"))
            {
                _ow.Add(int.Parse(item));
            }
            Serialise(_ow, "OfficerWrecks");

            // Jammers
            List<int> _jam = new List<int>();
            foreach (var item in File.ReadLines(@"..\..\Jammers.txt"))
            {
                _jam.Add(int.Parse(item));
            }
            Serialise(_jam, "Jammers");
        }
        static void Serialise(object obj2serialise, string name)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Create(InnerspaceDirectory + @"\" + name + ".bin");
            formatter.Serialize(stream, obj2serialise);
            stream.Close();
        }
    }
}
