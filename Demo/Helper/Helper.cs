using Demo.Context;
using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class Helper
    {
        public static readonly DimaBaseContext DataBase = new DimaBaseContext();
    }
}
