using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class QuestUIContainer : MonoBehaviour
{
    public TextMeshProUGUI QuestName;
    public TextMeshProUGUI Description;
    public void updateQuestName(string name)
    {
        QuestName.text = name;
    }

    public void updateQuestDescription(string description)
    {
        Description.text = description;
    }
}
