using System;

[Serializable]
public struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
{
    public double Mantissa;
    public int Exponent;

    public BigNumber(double mantissa, int exponent = 0)
    {
        Mantissa = mantissa;
        Exponent = exponent;
        Normalize();
    }

    // --- Основные операции ---
    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        // если разница экспонент слишком велика — возвращаем большее число
        if (a.Exponent > b.Exponent + 10) return a;
        if (b.Exponent > a.Exponent + 10) return b;

        int diff = a.Exponent - b.Exponent;
        double resultMantissa = a.Mantissa + b.Mantissa * Math.Pow(10, -diff);
        return new BigNumber(resultMantissa, a.Exponent);
    }

    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        if (a.Exponent > b.Exponent + 10) return a;
        if (b.Exponent > a.Exponent + 10) return a; // вычитание очень маленького числа почти ничего не меняет

        int diff = a.Exponent - b.Exponent;
        double resultMantissa = a.Mantissa - b.Mantissa * Math.Pow(10, -diff);
        return new BigNumber(resultMantissa, a.Exponent);
    }

    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        return new BigNumber(a.Mantissa * b.Mantissa, a.Exponent + b.Exponent);
    }

    public static BigNumber operator /(BigNumber a, BigNumber b)
    {
        if (b.Mantissa == 0)
            throw new DivideByZeroException("Division by zero in BigNumber");

        return new BigNumber(a.Mantissa / b.Mantissa, a.Exponent - b.Exponent);
    }

    // --- Сравнения ---
    public static bool operator >(BigNumber a, BigNumber b)
    {
        if (a.Exponent != b.Exponent)
            return a.Exponent > b.Exponent;
        return a.Mantissa > b.Mantissa;
    }

    public static bool operator <(BigNumber a, BigNumber b) => b > a;
    public static bool operator >=(BigNumber a, BigNumber b) => !(a < b);
    public static bool operator <=(BigNumber a, BigNumber b) => !(a > b);
    public static bool operator ==(BigNumber a, BigNumber b) => a.Exponent == b.Exponent && Math.Abs(a.Mantissa - b.Mantissa) < 1e-10;
    public static bool operator !=(BigNumber a, BigNumber b) => !(a == b);

    public bool Equals(BigNumber other)
    {
        return Mantissa.Equals(other.Mantissa) && Exponent == other.Exponent;
    }
    public override bool Equals(object obj) => obj is BigNumber n && this == n;
    public override int GetHashCode() => Mantissa.GetHashCode() ^ Exponent.GetHashCode();

    // --- Нормализация ---
    private void Normalize()
    {
        if (Mantissa == 0)
        {
            Exponent = 0;
            return;
        }

        while (Math.Abs(Mantissa) >= 10)
        {
            Mantissa /= 10;
            Exponent++;
        }
        while (Math.Abs(Mantissa) < 1)
        {
            Mantissa *= 10;
            Exponent--;
        }
    }

    // --- Форматирование ---
    public override string ToString()
    {
        if (Exponent < 6)
            return (Mantissa * Math.Pow(10, Exponent)).ToString("F0");

        return $"{Mantissa:F2}e{Exponent}";
    }
    public string Format(BigNumber num)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc" };
        int tier = num.Exponent / 3;
        return tier < suffixes.Length ? $"{num.Mantissa * Math.Pow(10, num.Exponent % 3):F2}{suffixes[tier]}" : num.ToString(); // fallback
    }

    public int CompareTo(BigNumber other)
    {
        if (this > other) return 1;
        if (this < other) return -1;
        return 0;
    }

    // --- Утилиты ---
    public static BigNumber Pow(BigNumber a, double power)
    {
        double newMantissa = Math.Pow(a.Mantissa, power);
        int newExponent = (int)(a.Exponent * power);
        return new BigNumber(newMantissa, newExponent);
    }

    public static BigNumber Zero => new BigNumber(0, 0);
    public static BigNumber One => new BigNumber(1, 0);


}
