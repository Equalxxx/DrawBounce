#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8yVrGKzMkjLVlvaZjAe5rof2e82y4BvOtIUqN1kcoFqoGxu43qxIU2vo5unZa+jj62vo6OlJHymuKA9ybAleNtys3vcgY/ETuP9/GRGPSH4Wvt+wFwxluhm0xOaH+dsLqgqBxoZThYdXpqi+DOlTcedU1o97vPOQ3+P6P0vsO8MSJxPcf0cxnNbJ3SXZa+jL2eTv4MNvoW8e5Ojo6Ozp6mf3IU49oEVQT4MGRzmOjkmYnO3cQJkOEBoaW47gLeebx4IPVjKPkz2GRhXB2kRaqdHi6G967we2oXUNpJkHApW6WYpeJmRuQlG3+qoOoSbMXvH7rGm24G9w/TsSlv4H2U7QSErgDIxFSPGwypZXMIvHEGuuxnmJ2+p/S+3//FWq2uvq6Ono");
        private static int[] order = new int[] { 2,9,9,8,10,5,13,9,11,13,11,12,12,13,14 };
        private static int key = 233;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
