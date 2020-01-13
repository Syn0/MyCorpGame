using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BTN_CMD : MonoBehaviour
{
    public string Command;
    private Button btn;

    private void Awake()
    {
        if (btn == null) btn = GetComponent<Button>();
        if (Command == null || Command == "") Command = name;
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        MNG_Game.instance.sendCommand(Command);
    }
}
