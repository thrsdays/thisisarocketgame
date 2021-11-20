using UnityEngine;

public class RocketParameters : MonoBehaviour
{
    public static float CalculateDistanceBetweenTwoPositions(Vector3 pos1, Vector3 pos2)
    {
        float dist = Mathf.Sqrt(Mathf.Pow(pos1.x-pos2.x,2) + Mathf.Pow(pos1.y - pos2.y,2) + Mathf.Pow(pos1.z - pos2.z,2));

        return dist;
    }

    public static float CalculateAngleBetweenTwoVectors(Vector3 pos1, Vector3 pos2)
    {
        float angle = Mathf.Acos((pos1.x * pos2.x + pos1.y * pos2.y + pos1.z * pos2.z) / (Mathf.Sqrt(pos1.x * pos1.x + pos1.y * pos1.y + pos1.z * pos1.z) * Mathf.Sqrt(pos2.x * pos2.x + pos2.y * pos2.y + pos2.z * pos2.z)));

        return angle;
    }
}
