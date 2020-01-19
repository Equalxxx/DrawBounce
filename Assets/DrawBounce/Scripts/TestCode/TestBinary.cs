using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class TestBin
{
	public int a;
	public int b;
	public float c;
}

public class TestBinary : MonoBehaviour
{
	public TestBin testA;
	public TestBin testB;
	public byte[] bytes;

	void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
		{
			bytes = ObjectToByteArraySerialize(testA);
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			testB = Deserialize<TestBin>(bytes);
		}
    }

	byte[] ObjectToByteArraySerialize(object obj)
	{
		using (var stream = new MemoryStream())
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(stream, obj);
			stream.Flush();
			stream.Position = 0;

			return stream.ToArray();
		}
	}

	T Deserialize<T>(byte[] byteData)
	{
		using (var stream = new MemoryStream(byteData))
		{
			BinaryFormatter bf = new BinaryFormatter();
			stream.Seek(0, SeekOrigin.Begin);

			return (T)bf.Deserialize(stream);
		}
	}
}
