namespace Library
{

    internal class MathUtils
    {

        public static bool AnyNonPositive(params decimal[] values) => values.Any(s => s <= 0m);

        public static float Lerp(float a, float b, float t) => a + (b - a) * Math.Clamp(t, 0f, 1f);

        public static float InverseLerp(float a, float b, float value)
        {
            if (a == b) return 0f;
            return Math.Clamp((value - a) / (b - a), 0f, 1f);
        }

    }

}