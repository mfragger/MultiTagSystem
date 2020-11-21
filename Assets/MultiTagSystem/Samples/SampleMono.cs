using MultiTagSystem;
using UnityEngine;
using UnityEngine.Video;

public class SampleMono : MonoBehaviour
{
    private void Start()
    {
        //Querying for GameObjects
        var list = MultiTag.QueryMultiple(new MultiTagFilter
        {
            //you can use string literal
            Any = new string[] { "EditorOnly", "Player" },
            //or you can use const variable
            None = new string[] { Tags.Finish }
        });

        foreach (var val in list)
        {
            Debug.Log($"Query {val.name}");

            var mtcomp = val.GetComponent<MultiTagComponent>();

            //Adding and removing tags via MultiTagComponent;
            mtcomp.RemoveTag("EditorOnly");
            mtcomp.AddTag("GameController");
        }

        //Getting the first gameObject from a list of gameobjects with Player tag
        var gameObject = MultiTag.GetGameObject(Tags.Player);
        Debug.Log($"GetGameObject {gameObject.name}");

        //Getting only a GameObject based on query
        gameObject = MultiTag.QuerySingle(new MultiTagFilter
        {
            All = new string[] { Tags.Player },
            None = new string[] { Tags.Untagged }
        });

        Debug.Log($"QuerySingleton {gameObject.name}");
    }
}