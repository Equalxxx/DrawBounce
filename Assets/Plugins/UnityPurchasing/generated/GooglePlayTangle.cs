#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VCu4REMxmRU5iplXl//8Ba/o9PSYKqmKmKWuoYIu4C5fpampqa2oq9McWIsfoRxpToLdKUwWFYnhyLeGou1jBrQZff7YaB78Z5pKZvYaMBgqqaeomCqpoqoqqamoCitU3SYrx1yfQdBITu93heiFGXrrv4wygFD/oYaRmvOUrBMdcIf1l1Kc25UxOGyDUIebM46LibcDODuy+6Lq9OoiiDw0S0OBTS60Q5r3eMJQhJbvpai6CPpatmpXHiftsnhCJu1zsRWZQg1v3PIQag/U4S3VB/TLjY8fEJWqGALKSfP01fazjCxE5qhvIEr3S/tCK6QsyanH8YGXPCzcjxOnAYotLBhRfJC9oQWLec2PdQ/J7XgJl+NiWFN8Ejv5xiWEUaqrqaip");
        private static int[] order = new int[] { 5,5,7,4,5,8,12,12,12,11,10,13,13,13,14 };
        private static int key = 168;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
