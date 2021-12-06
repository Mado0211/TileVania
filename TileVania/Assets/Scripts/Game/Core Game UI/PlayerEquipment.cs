using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] int arrowNumber = 3;
    [SerializeField] TMP_Text arrowText;
    
    public int ArrowNumber
    {
        get { return arrowNumber; }
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowText.text = arrowNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HaveArrows()
    {
        if (arrowNumber > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseArrow()
    {
        arrowNumber--;
        arrowText.text = arrowNumber.ToString();
    }

    public void ReceiveArrow(int amount)
    {
        arrowNumber += amount;
        arrowText.text = arrowNumber.ToString();
    }
}
