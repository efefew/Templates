using System;

namespace BreakInfinity
{
    public partial struct BigDouble
    {
        public BigDouble Clamp(BigDouble delta, BigDouble min, BigDouble max)
        {        
            if (delta < Zero && this > max) return max;
            if (delta > Zero && this < min) return min;
            return this;
        }
        public bool TryConvertToFloat(out float result)
        {
            double value = _mantissa * Math.Pow(10, Exponent);
            // Если число слишком большое или маленькое — не помещается в float
            if (double.IsInfinity(value) || double.IsNaN(value) || value > float.MaxValue)
            {
                result = float.MaxValue;
                return false;
            }
            if(value < float.MinValue)
            {
                result = float.MinValue;
                return false;
            }
            result = (float)value;
            return true;
        }
        public string Format()
        {
            string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
            long tier = Exponent / 3;
            return tier < suffixes.Length ? $"{Mantissa * Math.Pow(10, Exponent % 3):F2}{suffixes[tier]}" : ToString(); // fallback
        }
    }
}

