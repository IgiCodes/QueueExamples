namespace TPLQueue
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
            // Do a thing...
        }

        public override string ToString()
        {
            return _string;
        }
    }
}