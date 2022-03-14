namespace TemboAI.Models
{
    public class State
    {
        public double[] Values { get; set; }

        public int Output { get; set; }

        public int Occurrence { get; set; }

        public double Reward { get; set; }

        public int TimesAdjusted { get; set; }

        public bool EqualsTo(double[] state)
        {
            if (state != null && state.Length != Values.Length)
                return false;
            for (int index = 0; index < Values.Length; ++index)
            {
                if (Values[index] != state[index])
                    return false;
            }
            return true;
        }
    }
}
