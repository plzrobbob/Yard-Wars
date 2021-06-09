using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownManager : MonoBehaviour
{
    public Dropdown ClassDrop;
    // Start is called before the first frame update
    void Start()
    {
        ClassDrop.onValueChanged.AddListener(delegate 
        {
            ClassDropChange(ClassDrop);
        });
    }

    public void ClassDropChange(Dropdown sender)
    {
        GameMaster.classSetting(sender.value);
    }
}
