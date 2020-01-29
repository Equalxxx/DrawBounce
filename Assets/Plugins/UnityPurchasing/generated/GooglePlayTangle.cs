#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("qygmKRmrKCMrqygoKYuq1VynqkYZqygLGSQvIAOvYa/eJCgoKCwpKiAHEBtyFS2SnPEGdBbTHVoUsLnt7l1zkeuOVWCsVIZ1SgwOnpEUK5ndHsBRyc9u9gRpBJj7aj4NswHRfqolrUgoRnAAFr2tXQ6SJoALrK2ZI2zihzWY/H9Z6Z995hvL53ebsZlSndkKniCd6M8DXKjNl5QIYEk2B4NLyHJ1VHcyDa3FZynuoct2ynrDiXvbN+vWn6ZsM/nDp2zyMJQYw4zVqjnFwrAYlLgLGNYWfn2ELml1ddD9ETwghAr4TA70jkhs+YgWYuPZvbXKwgDMrzXCG3b5Q9EFF24kKTsC0QYasg8KCDaCubozeiNrdWujCdL9k7p4R6QF0CsqKCko");
        private static int[] order = new int[] { 1,1,12,10,8,9,8,7,13,11,11,11,12,13,14 };
        private static int key = 41;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
