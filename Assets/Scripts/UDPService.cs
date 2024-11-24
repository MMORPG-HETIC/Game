using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class UDPService : MonoBehaviour
{
    UdpClient udp;
    IPEndPoint localEP;

    public delegate void UDPMessageReceive(byte[] message, IPEndPoint sender);

    public event UDPMessageReceive OnMessageReceived;

    public bool Listen(int port) {
        if (udp != null) {
            Debug.LogWarning("Socket already initialized! Close it first.");
            return false;
        }

        try
        {
            // Local End-Point
            localEP = new IPEndPoint(IPAddress.Any, port);
            
            // Create the listener
            udp = new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.ExclusiveAddressUse = false;
            udp.Client.Bind(localEP);

            Debug.Log("Server listening on port: " + port);

            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Error creating UDP listener on port: " + port + ": " + ex.Message);
            CloseUDP();
            return false;
        }
    }

    public bool InitClient() {
        if (udp != null) {
            Debug.LogWarning("Socket already initialized! Close it first.");
            return false;
        }

        try
        {
            udp = new UdpClient();
            localEP = new IPEndPoint(IPAddress.Any, 0);
            udp.Client.Bind(localEP);
        } catch (System.Exception ex)
        {
            Debug.LogWarning("Error creating UDP client: " + ex.Message);
            CloseUDP();
            return false;
        }
        return true;
    }

    private void CloseUDP() {
        if (udp != null) {
            udp.Close();
            udp = null;
        }
    }

    void Update() {
        ReceiveUDP();
    }

    private void ReceiveUDP() {
        if (udp == null) { return; }

        while (udp.Available > 0)
		{
            IPEndPoint sourceEP = new IPEndPoint(IPAddress.Any, 0);
			byte[] data = udp.Receive(ref sourceEP);

			try
			{
                OnMessageReceived.Invoke(data, sourceEP);
            }
			catch (System.Exception ex)
			{
				Debug.LogWarning("Error receiving UDP message: " + ex.Message);
			}
		}
    }

    public void SendUDPMessage(string message, IPEndPoint destination) {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
        SendUDPBytes(bytes, destination);
    }

    public void SendUDPBytes(byte[] bytes, IPEndPoint destination) {
        if (udp == null) { 
            Debug.LogWarning("Trying to send a message on socket that is not yet open");
            return; 
        }

        try {
            udp.Send(bytes, bytes.Length, destination);
            
        } catch (SocketException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public T FromByteArray<T>(byte[] data)
    {
        if (data == null)
            return default(T);
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data, 1, data.Length -1))
        {
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }

    public byte[] ObjectToByteArray<T>(byte type, T obj)
    {
        if (obj != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                byte[] bytes = { type };
                byte[] objBytes = ms.ToArray();

                byte[] combinedBytes = new byte[bytes.Length + objBytes.Length];
                Array.Copy(bytes, 0, combinedBytes, 0, bytes.Length);
                Array.Copy(objBytes, 0, combinedBytes, bytes.Length, objBytes.Length);

                return combinedBytes;
            }
        }
        return null;
    }
}
