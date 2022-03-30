using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class TeleportManager : MonoBehaviour
{

    public UIDocument ui;

    private VisualElement uiRoot;
    private ListView listView;

    public VisualTreeAsset listViewTemplate;

    public PrayStatue[] teleportObjectives;
    public bool Display
    {
        get => uiRoot.style.display == DisplayStyle.Flex;
        set
        {
            uiRoot.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            if (this.Display)
            {
                // disable player movement while the list is open
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = false;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = true;

            }
        }
    }

    void OnEnable()
    {

        uiRoot = ui.rootVisualElement;
        listView = uiRoot.Q<ListView>("local-games-list");
        listView.makeItem = MakeItem;
        listView.bindItem = BindItem;
        this.Display = false;

        // find all objects with the tag "TeleportObjective"
        teleportObjectives = FindObjectsOfType<PrayStatue>();

        listView.itemsSource = teleportObjectives;


    }

    void Update()
    {
        if (!this.Display) return;
        // if the player presses the escape key, close the list
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Display = false;
            gameObject.SetActive(false);
        }
    }


    private VisualElement MakeItem()
    {
        var item = listViewTemplate.CloneTree();
        // item.AddToClassList("list-item");
        // item.Add(new Label("Item"));
        return item;
    }
    private void OnClickItem(PrayStatue objective)
    {
        // do the teleport
        Debug.Log("Teleporting to " + objective.objectiveDescription);
        StartCoroutine(objective.doTeleport());

        // close the panel
        this.Display = false;

    }

    private void BindItem(VisualElement item, int index)
    {
        item.Q<Label>("item-label").text = teleportObjectives[index].objectiveDescription;

        item.Q<Button>("item-button").RegisterCallback<ClickEvent>(evt =>
        {
            OnClickItem(teleportObjectives[index]);
        });
    }

}
