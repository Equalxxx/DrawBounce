#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("rQHPAXCKhoaCgoe35baMt46BhNLjsqSSzJLemjQTcHEbGUjXPUbf1za332vdg7UL7zQImlni9Hjg2eI7B5OsV+7AE/GOeXPsCqnHIXDAyvjr4qfO6eSptqG3o4GE0oOMlJrG9xIZ/YsjwAzcU5GwtExDiMpJk+5Wo2VsVjD3WIjCZqBNdur/amAykJCn6OGn8+/ip/Pv4umn5vf36+7k5vXm5PPu5OKn9PPm8+Lq4unz9Km3Tp71ctqJUvjYHHWihD3SCMrainaRt5OBhNKDhJSKxvf36+Kn1ejo8y9b+aWyTaJSXohR7FMlo6SWcCYr8+/o9e7z/raRt5OBhNKDhJSKxvcsJPYVwNTSRiioxjR/fGT3SmEkywWGh4GOrQHPAXDk44KGtwZ1t62BmBZcmcDXbIJq2f4DqmyxJdDL0mv4xi8fflZN4Ruj7JZXJDxjnK1EmPDwqeb39+viqeTo6qjm9/fr4uTm1eLr7ubp5OKn6Omn8+/u9Kfk4vWBhNKaiYORg5OsV+7AE/GOeXPsCo+sgYaCgoCFhpGZ7/Pz9/S9qKjwg4GUhdLUtpS3loGE0oONlI3G9/desfhGANJeIB4+tcV8X1L2Gfkm1YG3iIGE0pqUhoZ4g4K3hIaGeLea/qfm9PTy6uL0p+bk5OL38+bp5OK6oeCnDbTtcIoFSFlsJKh+1O3c44qBjq0BzwFwioaGgoKHhAWGhofbMr0qc4iJhxWMNqaRqfNSu4pc5ZGAa/q+BAzUp1S/QzY4HciN7Hise7cFgzy3BYQkJ4SFhoWFhoW3ioGOR+S08HC9gKvRbF2IpoldPfSeyDLl6+Kn9PPm6ePm9eOn8+L16vSn5s5f8Ri0k+Im8BNOqoWEhoeGJAWGt5aBhNKDjZSNxvf36+Knzunkqbb9twWG8beJgYTSmoiGhniDg4SFhogaunSszq+dT3lJMj6JXtmbUUy66eOn5Ojp4+7z7ujp9Kfo4afy9OKP2bcFhpaBhNKap4MFho+3BYaDt/Pu4e7k5vPip+X+p+bp/qf35vXzqcchcMDK+I/Zt5iBhNKapIOft5H36+KnxOL18+7h7uTm8+7o6afG8qfExrcFhqW3ioGOrQHPAXCKhoaGMJw6FMWjla1AiJoxyhvZ5E/MB5CotwZEgY+sgYaCgoCFhbcGMZ0GNPfr4qfV6Ojzp8TGt5mQirext7O1sR7Lqv8wagscW3TwHHXxVfC3yEbu4e7k5vPu6OmnxvLz7+j17vP+trSx3bfltoy3joGE0oOBlIXS1LaUmAIEApweusCwdS4cxwmrUzYXlV+n5unjp+Ti9fPu4e7k5vPu6Omn96G3o4GE0oOMlJrG9/fr4qfE4vXzgoeEBYaIh7cFho2FBYaGh2MWLo4Mng5ZfszrcoAspbeFb5+5f9eOVLK1trO3tLHdkIq0sre1t761trO34AiPM6dwTCurp+j3MbiGtwswxEg5c/QcaVXjiEz+yLNfJbl+/3jsT94ggo77kMfRlpnzVDAMpLzAJFLowvmYy+zXEcYOQ/PljJcExgC0DQarp+Ti9fPu4e7k5vPip/fo6+7k/gj0BudBnNyOqBU1f8PPd+e/GZJy1y0NUl1je1eOgLA38vKm");
        private static int[] order = new int[] { 1,20,26,32,32,18,53,41,46,19,12,54,18,24,34,15,17,36,38,30,35,26,58,29,40,56,47,29,51,35,57,42,35,57,51,58,43,57,39,42,58,58,59,57,53,48,54,54,49,49,59,51,57,58,54,59,57,57,58,59,60 };
        private static int key = 135;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
