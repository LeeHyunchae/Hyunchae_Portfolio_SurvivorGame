using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ObbInfo
{
    public Vector2 center; // Transform Position
    public Vector2 size; // Image Width, Height
    public float rot; // Transform Rotation
}


public class ObbTest : MonoBehaviour
{
    public ObbTest target;

    public ObbInfo myInfo = new ObbInfo();

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        myInfo.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
    }

    private void Update()
    {
        myInfo.center = transform.localPosition;
        myInfo.rot = transform.eulerAngles.z;

        IsCollisionTest(target.myInfo);
    }

    public bool IsCollisionTest(ObbInfo target)
    {
        Vector2 distance = GetCenterDistanceVector(target);
        Vector2[] vec = new Vector2[4]
        {
            GetHeightVector(myInfo),
            GetHeightVector(target),
            GetWidthVector(myInfo ),
            GetWidthVector(target)
        };

        Vector2 unitVec;
        for (int i = 0; i < 4; i++)
        {
            float sum = 0f;
            unitVec = GetUnitVector(vec[i]);
            for (int j = 0; j < 4; j++)
            {
                sum += Mathf.Abs(vec[j].x * unitVec.x + vec[j].y * unitVec.y);
            }

            if (Mathf.Abs(distance.x * unitVec.x + distance.y * unitVec.y) > sum)
            {
                Debug.Log("Dont Collision");
                return false;
            }
        }
        Debug.Log("collision");
        return true;
    }

    private Vector2 GetCenterDistanceVector(ObbInfo target)
    {
        return myInfo.center - target.center;
    }
    private Vector2 GetHeightVector(ObbInfo box)
    {
        float x = box.size.y * Mathf.Cos(Deg2Rad(box.rot - 90f)) / 2;
        float y = box.size.y * Mathf.Sin(Deg2Rad(box.rot - 90f)) / 2;

        return new Vector2(x, y);
    }

    private Vector2 GetWidthVector(ObbInfo box)
    {
        float x = box.size.x * Mathf.Cos(Deg2Rad(box.rot)) / 2;
        float y = box.size.x * Mathf.Sin(Deg2Rad(box.rot)) / 2;

        return new Vector2(x, y);
    }

    private Vector2 GetUnitVector(Vector2 v)
    {
        float len = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2));

        return new Vector2(v.x / len, v.y / len);
    }

    private float Deg2Rad(float deg)
    {
        return deg / 180 * Mathf.PI;
    }
}
