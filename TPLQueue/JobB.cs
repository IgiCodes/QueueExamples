namespace TPLQueue
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
            // Do a thing...
        }

        public override string ToString()
        {
            return _string;
        }
    }
}