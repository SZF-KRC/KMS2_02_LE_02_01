using KMS2_02_LE_02_01.Manager;
using System;

namespace KMS2_02_LE_02_01
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FilterManager filterManager = new FilterManager();
            filterManager.ToDo();
        }
    }
}
