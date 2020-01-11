using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysticLights
{
    public static class SaveFileManager {
    
        /// <summary>
        /// Use BinaryFormatter and PersistentDataPath
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save<T>(T data, string fileName, string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = Application.persistentDataPath;

            string path = Path.Combine(filePath, fileName);

            FileStream stream = new FileStream(path, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);

            stream.Close();

            Debug.Log("Save complete : " + fileName + " Path : " + path);
        }

        /// <summary>
        /// Use BinaryFormatter and PersistentDataPath
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Load<T>(string fileName, string filePath = "")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = Application.persistentDataPath;

            string path = Path.Combine(filePath, fileName);
            object data = null;

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                data = (T)formatter.Deserialize(stream);
                stream.Close();

                Debug.Log("Load complete : " + fileName + " Path : " + path);
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
            }

            return (T)data;
        }
    }
}
