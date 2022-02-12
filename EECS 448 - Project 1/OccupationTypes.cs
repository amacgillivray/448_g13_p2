using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECS_448___Project_1
{
    internal enum OccupationTypes
    {
        [Description("O")]
        Empty,

        [Description("B")]
        Battleship,

        [Description("R")]
        Raft,

        [Description("D")]
        Destroyer,

        [Description("S")]
        Submarine,

        [Description("A")]
        Carrier,

        [Description("X")]
        Hit,

        [Description("M")]
        Miss
    }
}
