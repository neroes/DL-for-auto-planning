using System;

namespace RPGModel
{
    public class StateRandom : System.Random
    {
        int numberOfInvokes, seed;

        public int NumberOfInvokes { get { return numberOfInvokes; } }
        public int Seed { get { return seed; } }

        public StateRandom(int Seed, int forward = 0)
            : base(Seed)
        {
            this.seed = Seed;
            numberOfInvokes = forward;
            for (int i = 0; i < forward; ++i)
                Next(0);
        }

        public override Int32 Next(Int32 maxValue)
        {
            numberOfInvokes += 1;
            return base.Next(maxValue);
        }

        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            numberOfInvokes += 1;
            return base.Next(minValue, maxValue);
        }
    }
}
