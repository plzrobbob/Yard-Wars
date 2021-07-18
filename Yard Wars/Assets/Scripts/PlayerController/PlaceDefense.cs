using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlaceDefense : MonoBehaviour
{
    public GameObject[] defenses;
    public GameObject[] Placeholderdefenses;
    public GameObject m_Camera;
    public GameObject DefenseHolder;
    public PlayerResourceSystem player_resources;

    public float defense_cost;

    private Vector3 defensepos;
    public bool placing;
    public int currentdefense;
    public float newrot;
    private bool deactivate;
    public LayerMask raymask;
    public bool canplace;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.FindWithTag("MainCamera");

        placing = false;
        newrot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("StartPlace") && !placing)
        {
            Debug.Log("placing");
            placing = true;
            for (int i = 0; i < Placeholderdefenses.Length; i++)
            {
                Placeholderdefenses[i].transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            deactivate = true;
            newrot = this.transform.rotation.y;
        }
        else if (Input.GetButtonDown("StartPlace") && placing)
        {
            Debug.Log("notplacing");

            placing = false;
        }

        if (placing)
        {
            SelectDefense();
            RotateDefense();
            SetDefenseHeight();
            if (canplace)
            {
                SetDefense();
            }
        }
        if (!placing && deactivate)
        {
            deactivate = false;
            Placeholderdefenses[currentdefense].SetActive(false);
        }
    }

    private void SelectDefense()
    {
        if (currentdefense == 0 && Input.GetButtonDown("SelectLeftDefense"))
        {
            currentdefense = Placeholderdefenses.Length-1;
        }
        else if (Input.GetButtonDown("SelectLeftDefense"))
        {
            currentdefense--;
        }
        if (currentdefense == Placeholderdefenses.Length-1 && Input.GetButtonDown("SelectRightDefense"))
        {
            currentdefense = 0;
        }
        else if(Input.GetButtonDown("SelectRightDefense"))
        {
            currentdefense++;
        }
        for (int i = 0; i < Placeholderdefenses.Length; i++)
        {
            if (i == currentdefense)
            {
                Placeholderdefenses[i].SetActive(true);
            }
            else 
            {
                Placeholderdefenses[i].SetActive(false);
            }
        }
    }

    private void RotateDefense()
    {
        if (Input.GetAxis("RotateDefenseCClockwise") != 0)
        {
            newrot += 70f * Time.deltaTime;
        }        
        if (Input.GetAxis("RotateDefenseClockwise") != 0)
        {
            newrot -= 70f * Time.deltaTime;
        }

        if (newrot <= -360 || newrot >= 360)
        {
            Debug.Log("Reset rotate");
            newrot = 0;
        }
        Placeholderdefenses[currentdefense].transform.rotation = Quaternion.Euler(0, newrot, 0);
    }

    private void SetDefense()
    {
        if (Input.GetButtonDown("Fire1") && player_resources.player_funds >= defense_cost) // if the player can afford it...
        {
            Instantiate(defenses[currentdefense], Placeholderdefenses[currentdefense].transform.position, Placeholderdefenses[currentdefense].transform.rotation);
            player_resources.Spend(defense_cost);
        }
    }

    private void SetDefenseHeight()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        if (player_resources.player_funds < defense_cost)
        {
            canplace = false;
            return;
        }
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 5.5f, raymask)) //placing against a wall.  defenses will not intersect
        {
            Debug.DrawRay(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up * 20, Color.blue);
            if (Physics.Raycast(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up, out var hit2, 20f, raymask))
            {
                if (hit2.collider.gameObject.layer == 12)
                {
                    canplace = false;
                    return;
                }
                else
                {
                    DefenseHolder.transform.position = hit2.point + (transform.up * 1.8f);
                }
            }
            else
            {
                canplace = false;
                return;
            }
        }
        //if there is no wall player can just place
        else
        {
            Debug.DrawRay(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up * 20, Color.blue);
            if (Physics.Raycast(this.transform.position + (transform.forward * 5) + (transform.up * 10), -transform.up, out var hit2, 20f, raymask))
            {
                if (hit2.collider.gameObject.layer == 12)
                {
                    canplace = false;
                    return;
                }
                else
                {
                    DefenseHolder.transform.position = hit2.point + (transform.up * 1.8f);
                }
            }
            else
            {
                canplace = false;
            }
        }

        //if hit.point exists and it isn't more than 1.5 meters from the player then you can place a defense
        if (Vector3.Distance(this.transform.position, hit.point) < 1.5f)
        {
            canplace = false;
            return;
        }
        else
        { 
            canplace = true;
            return;
        }

    }
}
