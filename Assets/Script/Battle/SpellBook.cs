using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellBook : MonoBehaviour
{
    private Button[] buttons;

    void Start()
    {
        this.InitButtons();
    }

    private void InitButtons() {
        this.buttons = this.GetComponentsInChildren<Button>();        
    }

    public void SetActions(Action[] actions) {
        for (int i = 0; i < this.buttons.Length; i++)
        {
            this.buttons[i].onClick.AddListener(() => actions[i].Act());
        }
    }


    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
