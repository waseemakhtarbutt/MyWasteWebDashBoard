using System;
using System.Collections.Generic;
using System.Text;

namespace DrTech.Common.Helpers
{
    public class GreenPointHelper
    {
        public static double GetGreenPointsAgainstRecycle(double Weight)
        {
            double result = Weight * 5;
            result = double.Parse(result.ToString());
            return result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }

        public static double GetGreenPointsAgainstRePlant(double Height, double TreeCount)
        {
            double result =  TreeCount * 5;
            result = double.Parse(result.ToString());
            return result = Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }
    }
}
