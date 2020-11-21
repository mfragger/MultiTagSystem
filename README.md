# Multi-Tag
a multi-tagging system that overides unity's default tagging system

## How to use in editor:
### Rebuild the tags via MultiTag > Rebuild Tags
    - this takes in the tags you set in the UnityEditor and puts it in a static class MultiTagTags.
    - this is required each time you add or delete custom tags from the editor.

### Add MultiTagComponent to the gameobject.

This component overrides Unity's own tagging system. If you need to add a new tag, go to drop-down at the top of the inspector besides the Tag label > Add Tag.

#### Best Practice Recommendation

As a general best practice, you should pre-load all the tags you need in editor. You cannot create new tags during runtime, only add existing tags that's within the editor.

## How to use in code:
To be able to query GameObjects that's implementing the MultiTagComponent use the static Query(MultiTagFilter multiTagFilter) function from MultiTag static class.

```C#
public class SimpleMono : MonoBehaviour
{
    private void Start()
    {
        //Querying for GameObjects
        var list = MultiTag.Query(new MultiTagFilter
        {
            //Any gameObject that has these tags
            Any = new string[] { "EditorOnly", "Player" }, //You can use a literal string so long as it exists as a Tag.

            //gameObjects that do not have these tags
            None = new string[] { Tags.Finish } //Tags.Finish is constant for string "Finish"
            
        });

        foreach (var val in list)
        {
            var mtcomp = val.GetComponent<MultiTagComponent>();

            //Adding and removing tags via MultiTagComponent;
            mtcomp.RemoveTag("EditorOnly");
            mtcomp.AddTag(Tags.GameController);
        }

        //Getting a single gameObject
        //This gets the first result.
        var gameObject = MultiTag.GetGameObject(Tags.Player);

        //Getting only a GameObject based on query
        //Another way of getting only the first result with MultiTagFilter
        gameObject = MultiTag.SingletonQuery(new MultiTagFilter
        {
            All = new string[] { Tags.Player },
            None = new string[] { Tags.Untagged }
        });
    }
}
```
