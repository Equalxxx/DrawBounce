#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("xxFfLJj4pgbhosKtuDONmrPCT/lYPWoC6JjqwxRXxSeMy0stJbt8SlPDFXoJlHFke7cycw26un2sqNnosmexs2OSnIo43WdF02Diu0+Ix6StMzahjm2+ahJQWnZlg86eOpUS+OvXzgt/2A/3JhMn6EtzBaji/ekRX9zS3e1f3NffX9zc3X0rHZocO0Zqxc+YXYLUW0TJDyaiyjPteuR8frJyIfXucG6d5dbcW07bM4KVQTmQhtQv+oCxHgNtKJRunC8vjOqYfGfUOLhxfMWE/qJjBL/zJF+a8k297yKK64QjOFGOLYDw0rPN7z+ePrXy7V/c/+3Q29T3W5VbKtDc3NzY3d50rTokLi5vutQZ06/ztjtiBrunCd5Lf9nLyGGe7t/e3N3c");
        private static int[] order = new int[] { 2,8,11,5,12,13,8,12,13,9,12,12,12,13,14 };
        private static int key = 221;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
