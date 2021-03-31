using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static int classSelect;
    public GameObject Grenadier1;
    public GameObject Builder1;
    public GameObject Guard1;
    public GameObject Thief1;
    public static GameObject Grenadier;
    public static GameObject Builder;
    public static GameObject Guard;
    public static GameObject Thief;




    // Start is called before the first frame update
    void Start()
    {
        Grenadier = Grenadier1;
        Builder = Builder1;
        Guard = Guard1;
        Thief = Thief1;
    }

    public static void classSetting(int classSelected)
    {
        classSelect = classSelected;
        Debug.Log("Class Changed Value: " + classSelect);
    }
    public static GameObject ClassSpawning()
    {
        Debug.Log("I am in class spawning");
        if (classSelect == 1)
        {
            Debug.Log("return builder. " + Builder.name);

            return Builder;
        }
        else if (classSelect == 2)
        {
            Debug.Log("return Grenadier. " + Grenadier.name);

            return Grenadier;
        }
        else if (classSelect == 3)
        {
            Debug.Log("return Guard. " + Guard.name);

            return Guard;
        }
        else if (classSelect == 4)
        {
            Debug.Log("return Thief. " + Thief.name);

            return Thief;
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
