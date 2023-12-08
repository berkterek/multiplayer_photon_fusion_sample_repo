using UnityEngine;

namespace MultiplayerPhotonFusionSample.Helpers
{
    public static class SessionCodeGeneratorHelper
    {
        public static string Generate(int length = 4)
        {
            char[] chars = "QWERTYUIOPASDFGHJKLZXCVBNM0123456789".ToCharArray();

            string value = string.Empty;
            int charLength = chars.Length;
            for (int i = 0; i < length; i++)
            {
                value += chars[Random.Range(0, charLength)];
            }
            
            return value;
        }
    }
}