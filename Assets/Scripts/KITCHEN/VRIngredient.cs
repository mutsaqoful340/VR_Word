using UnityEngine;

public class VRIngredient : MonoBehaviour
{
    public string namaBahan;

    Vector3 startPos;
    Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void Kembali()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
