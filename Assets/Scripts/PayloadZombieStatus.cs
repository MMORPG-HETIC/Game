using UnityEngine;

[System.Serializable]
public class PayloadZombieStatus
{
    public string id;
    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float rotationW;


    public void SetPosition(Vector3 vector)
    {
        positionX = vector.x;
        positionY = vector.y;
        positionZ = vector.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(positionX, positionY, positionZ);
    }

    public void SetRotation(Quaternion quaternion)
    {
        rotationX = quaternion.x;
        rotationY = quaternion.y;
        rotationZ = quaternion.z;
        rotationW = quaternion.w;
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(rotationX, rotationY, rotationZ, rotationW);
    }
}