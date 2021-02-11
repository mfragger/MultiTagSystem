# Multi-Tag
a multi-tagging system that overides unity's default tagging system

## To install:
Download the package from [here](https://github.com/mfragger/MultiTagSystem/releases). And import the package to your project.

## How to use in editor:
### Rebuild the tags via MultiTag > Rebuild Tags
    - this takes in the tags you set in the UnityEditor and puts it in a static class Tags.
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
        
        //Check if gameobject tagged with Finish tag
        MultiTag.IsGameObjectTaggedWith(Tags.Finish, gameObject);
    }
}
```
## [License](https://github.com/mfragger/MultiTagSystem/blob/main/LICENSE)

MIT License

Copyright (c) 2021 mfragger

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
