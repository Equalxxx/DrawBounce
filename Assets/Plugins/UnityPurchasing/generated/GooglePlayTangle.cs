#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("VacH6zcKQ3qw7yUfe7Au7EjEH1DeDdrGbtPW1OpeZWbvpv+3qbd/1V+XFK6piKvu0XEZu/UyfReqFqYfCXblGR5sxEhk18QKyqKhWPK1qanFd/TXxfjz/N9zvXMC+PT09PD19nf0+vXFd/T/93f09PVXdgmAe3aaYWkWHtwQc+kex6olnw3Zy7L49ef828zHrsnxTkAt2qjKD8GGyGxlMQHCHI0VE7Iq2LXYRCe24tFv3Q2idvlxlPSarNzKYXGB0k76XNdwcUWOQQXWQvxBNBPfgHQRS0jUvJXq2zKBr003Uom8cIhaqZbQ0kJNyPdFDCHN4PxY1iSQ0ihSlLAlVMq+PwX/sD5b6UQgo4U1Q6E6xxc7q0dtRQ4hT2akm3jZDPf29PX0");
        private static int[] order = new int[] { 11,6,13,5,11,6,13,12,8,9,12,12,13,13,14 };
        private static int key = 245;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
