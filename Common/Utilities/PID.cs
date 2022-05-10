using System;

namespace Common.Utilities
{
    public static class PID
    {
        public static string NewId()
        {
            string str = "";

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                int n = rnd.Next() % 10;
                str += n.ToString();
            }

            return str;
        }
        public static string ProductNewId()
        {
            string str = "";

            var rnd = new Random();

            for (int i = 0; i < 8; i++)
            {
                int n = rnd.Next() % 10;
                str += n.ToString();
            }
            return "TB-" + str;
        }
    }
}
