using System;

namespace Queue
{
    public class JobA : IJob
    {
        private readonly string _string;

        public JobA(string s)
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