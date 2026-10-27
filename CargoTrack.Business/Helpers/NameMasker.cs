namespace CargoTrack.Business.Helpers
{
    public static class NameMasker
    {
        private const int VisibleCharCount = 2;
        private const string Mask = "***";   // Sabit uzunluk: ismin harf sayısı belli olmasın

        // "Murat Yücedağ"   -> "Mu*** Yü***"
        // "Ahmet Can Demir" -> "Ah*** Ca*** De***"
        public static string MaskFullName(string? firstName, string? lastName)
        {
            var maskedParts = $"{firstName} {lastName}"
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(MaskWord);

            return string.Join(" ", maskedParts);
        }

        private static string MaskWord(string word)
        {
            // Kısa isimlerde ismin tamamı görünmesin, sadece ilk harf kalsın
            var visible = word.Length > VisibleCharCount ? word[..VisibleCharCount] : word[..1];
            return visible + Mask;
        }
    }
}
