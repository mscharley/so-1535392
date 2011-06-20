using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swizzle
{
    /// <summary>
    /// This implements the Vector4 class as described in the question, based on our totally generic
    /// Vector class.
    /// </summary>
    class Vector4 : Vector<int>
    {
        public Vector4(int val0, int val1, int val2, int val3)
            : base(new Dictionary<char, int> { {'t', 0}, {'x', 1}, {'y', 2}, {'z', 3},
                                               {'a', 0}, {'r', 1}, {'g', 2}, {'b', 3}},
                   new int[] { val0, val1, val2, val3 })
        { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            dynamic v1, v2, v3;

            v1 = new Vector4(1, 2, 3, 4);

            v2 = v1.rgb;
            // Prints: v2.r: 2
            Console.WriteLine("v2.r: {0}", v2.r);
            // Prints: red: 2
            int red = v2.r;
            Console.WriteLine("red: {0}", red);
            // Prints: v2 has 3 elements.
            Console.WriteLine("v2 has {0} elements.", v2.Length);

            v3 = new Vector4(5, 6, 7, 8);
            v3.ar = v2.gb; // yes, the names are preserved! v3 = (3, 4, 7, 8)

            v2.r = 5; // fails: need a length check in TrySetMember to get this working
            //v2.a = 5; // fails: v2 has no 'a' element, only 'r', 'g', and 'b'

            // Something fun that will also work
            Console.WriteLine("v3.gr: {0}", v3.gr);
            v3.rg = v3.gr; // switch green and red
            Console.WriteLine("v3.gr: {0}", v3.gr);

            Console.WriteLine("\r\nPress any key to continue.");
            Console.ReadKey(true);
        }
    }
}
