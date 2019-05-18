/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Template messages and other things the client/server utilize.
/// 
/// NOTE: Communication does not support Unity's new networking system introduced in Unity 2019 - a new version will be created soon.
///
/// </summary>

#if !UNITY_2019

#pragma warning disable 0618

using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
    //Msgs:
    public enum NetworkMsg { ServerAvailableMsg = 3000, FloatMsg, FloatArrayMsg, IntMsg, IntArrayMsg, Vector2Msg, Vector2ArrayMsg, Vector3Msg, Vector3ArrayMsg, Vector4Msg, Vector4ArrayMsg, RectMsg, RectArrayMsg, StringMsg, StringArrayMsg, ByteMsg, ByteArrayMsg, ColorMsg, ColorArrayMsg, Color32Msg, Color32ArrayMsg, BoolMsg, BoolArrayMsg, QuaternionMsg, QuaternionArrayMsg, Matrix4x4Msg, Matrix4x4ArrayMsg };

    //Custom Messages:
    public class ServerAvailableMessage : MessageBase
    {
        public int port;
        public string deviceId;
        public string fromIp;

        public ServerAvailableMessage() { }

        public ServerAvailableMessage(int port, string deviceId, string fromIp)
        {
            this.fromIp = fromIp;
            this.port = port;
            this.deviceId = deviceId;
        }
    }

    public class Matrix4x4Message : MessageBase
    {
        public Matrix4x4 value;
        public string id;
        public string fromIp;

        public Matrix4x4Message() { }

        public Matrix4x4Message(Matrix4x4 value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Matrix4x4ArrayMessage : MessageBase
    {
        public Matrix4x4[] value;
        public string id;
        public string fromIp;

        public Matrix4x4ArrayMessage() { }

        public Matrix4x4ArrayMessage(Matrix4x4[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class FloatMessage : MessageBase
    {
        public float value;
        public string id;
        public string fromIp;

        public FloatMessage() { }

        public FloatMessage(float value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class FloatArrayMessage : MessageBase
    {
        public float[] value;
        public string id;
        public string fromIp;

        public FloatArrayMessage() { }

        public FloatArrayMessage(float[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class IntMessage : MessageBase
    {
        public int value;
        public string id;
        public string fromIp;

        public IntMessage() { }

        public IntMessage(int value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class IntArrayMessage : MessageBase
    {
        public int[] value;
        public string id;
        public string fromIp;

        public IntArrayMessage() { }

        public IntArrayMessage(int[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector2Message : MessageBase
    {
        public Vector2 value;
        public string id;
        public string fromIp;

        public Vector2Message() { }

        public Vector2Message(Vector2 value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector2ArrayMessage : MessageBase
    {
        public Vector2[] value;
        public string id;
        public string fromIp;

        public Vector2ArrayMessage() { }

        public Vector2ArrayMessage(Vector2[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector3Message : MessageBase
    {
        public Vector3 value;
        public string id;
        public string fromIp;

        public Vector3Message() { }

        public Vector3Message(Vector3 value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector3ArrayMessage : MessageBase
    {
        public Vector3[] value;
        public string id;
        public string fromIp;

        public Vector3ArrayMessage() { }

        public Vector3ArrayMessage(Vector3[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class QuaternionMessage : MessageBase
    {
        public Quaternion value;
        public string id;
        public string fromIp;

        public QuaternionMessage() { }

        public QuaternionMessage(Quaternion value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class QuaternionArrayMessage : MessageBase
    {
        public Quaternion[] value;
        public string id;
        public string fromIp;

        public QuaternionArrayMessage() { }

        public QuaternionArrayMessage(Quaternion[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector4Message : MessageBase
    {
        public Vector4 value;
        public string id;
        public string fromIp;

        public Vector4Message() { }

        public Vector4Message(Vector4 value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Vector4ArrayMessage : MessageBase
    {
        public Vector4[] value;
        public string id;
        public string fromIp;

        public Vector4ArrayMessage() { }

        public Vector4ArrayMessage(Vector4[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class RectMessage : MessageBase
    {
        public Rect value;
        public string id;
        public string fromIp;

        public RectMessage() { }

        public RectMessage(Rect value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class RectArrayMessage : MessageBase
    {
        public Rect[] value;
        public string id;
        public string fromIp;

        public RectArrayMessage() { }

        public RectArrayMessage(Rect[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class ByteMessage : MessageBase
    {
        public byte value;
        public string id;
        public string fromIp;

        public ByteMessage() { }

        public ByteMessage(byte value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class ByteArrayMessage : MessageBase
    {
        public byte[] value;
        public string id;
        public string fromIp;

        public ByteArrayMessage() { }

        public ByteArrayMessage(byte[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class ColorMessage : MessageBase
    {
        public Color value;
        public string id;
        public string fromIp;

        public ColorMessage() { }

        public ColorMessage(Color value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class ColorArrayMessage : MessageBase
    {
        public Color[] value;
        public string id;
        public string fromIp;

        public ColorArrayMessage() { }

        public ColorArrayMessage(Color[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Color32Message : MessageBase
    {
        public Color32 value;
        public string id;
        public string fromIp;

        public Color32Message() { }

        public Color32Message(Color32 value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class Color32ArrayMessage : MessageBase
    {
        public Color32[] value;
        public string id;
        public string fromIp;

        public Color32ArrayMessage() { }

        public Color32ArrayMessage(Color32[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class StringMessage : MessageBase
    {
        public string value;
        public string id;
        public string fromIp;

        public StringMessage() { }

        public StringMessage(string value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class StringArrayMessage : MessageBase
    {
        public string[] value;
        public string id;
        public string fromIp;

        public StringArrayMessage() { }

        public StringArrayMessage(string[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class BoolMessage : MessageBase
    {
        public bool value;
        public string id;
        public string fromIp;

        public BoolMessage() { }

        public BoolMessage(bool value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }

    public class BoolArrayMessage : MessageBase
    {
        public bool[] value;
        public string id;
        public string fromIp;

        public BoolArrayMessage() { }

        public BoolArrayMessage(bool[] value, string id, string fromIp)
        {
            this.value = value;
            this.id = id;
            this.fromIp = fromIp;
        }
    }
}

#endif