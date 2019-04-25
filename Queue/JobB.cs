using System;

namespace Queue
{
    public class JobB : IJob
    {
        private readonly string _string;

        public JobB(string s)
        {
            _string = s;
        }

        public void Invoke()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return _string;
        }
    }
}