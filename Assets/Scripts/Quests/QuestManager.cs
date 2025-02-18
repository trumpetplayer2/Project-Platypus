using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using UnityEngine;
public class QuestObjective
{
    public List<string> names = new List<string>();
    public List<string> descriptions = new List<string>();

}
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> QuestList = new List<Quest>();
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
        for(int i = 0; i < QuestList.Count; i++)
        {
            QuestList[i].initializeUID(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Quest trigger in QuestList)
        {
            if (trigger.hidden) { return; }
            trigger.QuestUpdate();
        }
    }

    public List<QuestObjective> updateMenu(int zone)
    {
        List<QuestObjective> quests = new List<QuestObjective>();
        foreach (Quest trigger in QuestList)
        {
            if(trigger.zone == zone)
            {
                QuestObjective obj = new QuestObjective();
                obj.names.Add(trigger.GetQuestName());
                obj.descriptions.Add(trigger.GetQuestDescription());
            }
        }
        return quests;
    }

    public Quest getQuest(int id)
    {
        return QuestList[id];
    }

    public void unlockQuest(int id)
    {
        QuestList[id].unlock();
    }
}
