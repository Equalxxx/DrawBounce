#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("L++8aHPt8wB4S0HG00auHwjcpA33WFIFwB9JxtlUkrs/V65w53nh485eiOeUCez55iqv7pAnJ+AxNUR16TCnubOz8idJhE4ybium/5smOpQbSbJnHSyDnvC1CfMBsrIRdwXh+sWg9591BXdeicpYuhFW1rC4JuHXL/osLv4PARelQPrYTv1/JtIVWjl2SlOW4kWSaruOunXW7pg1f2B0jMJBT0BwwkFKQsJBQUDgtoAHgabbWozCsQVlO5t8P18wJa4QBy5f0mRwwkFicE1GSWrGCMa3TUFBQUVAQ0mlJezhWBljP/6ZIm65wgdv0CByMK6rPBPwI/ePzcfr+B5TA6cIj2W/F3YZvqXME7AdbU8uUHKiA6Mob0PW4kRWVfwDc0JDQUBB");
        private static int[] order = new int[] { 3,4,11,6,9,8,8,13,9,11,11,13,12,13,14 };
        private static int key = 64;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
