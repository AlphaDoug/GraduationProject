using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{

    [SerializeField]
    private GameObject[] chestPosition;
    private OOFormArray mFormTbCollection = null;
    private List<TaskController.CollectionAttribute> collectionAttribute = new List<TaskController.CollectionAttribute>();

    private void Awake()
    {
        if (mFormTbCollection == null)
        {
            mFormTbCollection = OOFormArray.ReadFromResources("Data/Tables/TbCollection");
        }

        for (int i = 1; i < mFormTbCollection.mRowCount; i++)
        {
            var m_collectionAttribute = mFormTbCollection.GetObject<TaskController.CollectionAttribute>(i);
            collectionAttribute.Add(m_collectionAttribute);
        }
        
    }
    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < collectionAttribute.Count; i++)
        {
            var collectionPrefab = (GameObject)Resources.Load(collectionAttribute[i].Path);
            var collection = Instantiate(collectionPrefab) as GameObject;
            collection.transform.parent = chestPosition[i].transform;
            collection.transform.localPosition = Vector3.zero;
            collection.GetComponent<ActivateChest>().enabled = true;
            collection.GetComponent<Collection>().ID = (collectionAttribute[i].ID);
            collection.GetComponent<Collection>().DES = (collectionAttribute[i].DES);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}
