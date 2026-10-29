namespace CargoTrack.Business.Helpers
{
    public static class NameMasker
    {
        private const int VisibleCharCount = 2;
        private const string Mask = "***";

        public static string MaskFullName(string? firstName, string? lastName)
        {
            var maskedParts = $"{firstName} {lastName}"
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(MaskWord);

            return string.Join(" ", maskedParts);
        }

        private static string MaskWord(string word)
        {
            var visible = word.Length > VisibleCharCount ? word[..VisibleCharCount] : word[..1];
            return visible + Mask;
        }
    }
}
