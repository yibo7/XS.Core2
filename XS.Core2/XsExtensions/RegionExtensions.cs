
namespace Mustang
{
    using System.Collections;

    public static class RegionExtensions
    {
        /// <summary>
        /// Contains all the valid chars for token names.
        /// </summary>
        private static BitArray s_validRegionCodeChars;
        private const int MaxTokenCharBits = 128;

        /// <summary>
        /// Determines if the specified token is valid or not.
        /// </summary>
        public static bool IsValidRegionCode(this string region)
        {
            if (!string.IsNullOrEmpty(region) && region.Length >= 2 && region.Length <= 8)
            {
                BitArray bits = s_validRegionCodeChars;
                if (bits == null)
                    bits = s_validRegionCodeChars = GetValidCharBits();

                foreach (char ch in region)
                {
                    if (ch >= MaxTokenCharBits || !bits.Get(ch))
                        return false;
                }
                return true;
            }

            return false;
        }

        private static BitArray GetValidCharBits()
        {
            BitArray bits = new BitArray(MaxTokenCharBits);

            // Valid chars include:
            // a-z, A-Z
            // Special chars: -, _
            //
            //
            for (char ch = 'a'; ch <= 'z'; ch++)
                bits.Set(ch, true);
            for (char ch = 'A'; ch <= 'Z'; ch++)
                bits.Set(ch, true);

            return bits;
        }
    }
}
