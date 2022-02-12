using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1
{
    internal abstract class Ship
    {
        public string Name { get; set; }
        public string Width { get; set; }
        public int Hits { get; set; }
        public OccupationTypes OccupationType { get; set; }
        public bool IsSunk
        {
            get
            {
                return Hits >= Width;
            }
        }
    }
}
