using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
    bool loading;
    float saveDelay = 1f;
    float saveCooldown = 0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        loading = true;
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            instance = this;
        }
        int[] temp;
        if(!GameManager.instance.QuestMap.TryGetValue(Zone, out temp)){
            temp = new int[0];
        }

        for (int i = 0; i < QuestList.Count; i++)
        {
            QuestList[i].initializeUID(i);
        }
        foreach(int i in temp)
        {
            completeQuest(QuestList[i]);
        }
        //Generate the list of Quest UI Containers
        List<Quest> quests = updateMenu(Zone);
        GenerateQuestUI(quests);
        loading = false;
    }

    void completeQuest(Quest q)
    {
        q.forceComplete();
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
        if (saveCooldown > 0)
        {
            saveCooldown = Mathf.Max(0, saveCooldown - Time.deltaTime);
        }
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

    public KeyValuePair<int, int[]> getQuestData()
    {
        List<int> val = new List<int>();
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].getCompleted())
            {
                val.Add(i);
            }
        }
        int[] values = val.ToArray();
        KeyValuePair<int, int[]> pair = new KeyValuePair<int, int[]>(Zone, values);
        return pair;
    }

    public async void saveQuestData()
    {
        //No save while loading
        if (loading) return;
        //Wait for save cooldown
        await Task.Run(() => cooldownCheck());
        //Update Quest data
        GameManager.instance.updateQuestData();
        //Update cooldown
        saveCooldown = saveDelay;
        //Save
        GameManager.instance.save();
    }

    public IEnumerable cooldownCheck()
    {
        while (saveCooldown > 0) {
            Debug.Log("Waiting");
        }
        yield return null;
    }
}
