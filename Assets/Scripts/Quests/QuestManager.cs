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
    public int Zone = 1;
    public GameObject QuestUIObjectPrefab;
    public GameObject QuestListUI;
    public Vector2 ListStartOffset = new Vector2(15, 15);
    public Vector2 ListObjectOffset = new Vector2(0, 165);
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
        //Generate the list of Quest UI Containers
        List<Quest> quests = updateMenu(Zone);
        GenerateQuestUI(quests);
    }

    void GenerateQuestUI(List<Quest> quests)
    {
        Vector2 offset = ListStartOffset;
        foreach(Quest quest in quests)
        {
            GameObject element = Instantiate(QuestUIObjectPrefab);
            if (!element.TryGetComponent<QuestUIContainer>(out QuestUIContainer container)) continue;
            quest.container = container;
            container.updateQuestName(quest.GetQuestName());
            element.transform.SetParent(QuestListUI.transform);
            element.transform.localPosition = offset;
            offset += ListObjectOffset;
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

    public List<Quest> updateMenu(int zone)
    {
        List<Quest> quests = new List<Quest>();
        foreach (Quest trigger in QuestList)
        {
            if(trigger.zone == zone)
            {
                quests.Add(trigger);
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
