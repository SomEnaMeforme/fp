using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagCloudDI.CloudVisualize
{
    public class RandomWordColorDistributor : IWordColorDistributor
    {
        public Color GetColor(Color[] possibleColors)
        {
            var rnd = new Random();
            return possibleColors[rnd.Next(0, possibleColors.Length - 1)];
        }
    }
}
