using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MultiTagSystem
{
    public static class MultiTag
    {
        private static Dictionary<string, HashSet<GameObject>> MtDict;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (MtDict == null)
            {
                MtDict = new Dictionary<string, HashSet<GameObject>>();
            }
        }

        /// <summary>
        /// Rebuilds the Dictionary based on the current state of all MultiTagComponents
        /// </summary>
        public static void RebuildDict()
        {
            MtDict.Clear();
            var list = Object.FindObjectsOfType<MultiTagComponent>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].AddDict();
            }
        }

        /// <summary>
        /// Don't call this unless you know what you're doing.
        /// <para>This does not update the current state of the MultiTagComponent.</para>
        /// <para>It's best to add a tag from a gameobject's MultiTagComponent.</para>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="gameObject"></param>
        public static void MultiTagsAddToDict(string[] tags, GameObject gameObject)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                SingleTagAddToDict(tag, gameObject);
            }
        }

        /// <summary>
        /// Don't call this unless you know what you're doing.
        /// <para>This does not update the current state of the MultiTagComponent.</para>
        /// <para>It's best to add a tag from a gameobject's MultiTagComponent.</para>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="gameObject"></param>
        public static void SingleTagAddToDict(string tag, GameObject gameObject)
        {
            if (!MtDict.ContainsKey(tag))
            {
                MtDict.Add(tag, new HashSet<GameObject>());
            }

            if (MtDict[tag] == null)
            {
                MtDict[tag] = new HashSet<GameObject>();
            }

            MtDict[tag].Add(gameObject);
        }

        /// <summary>
        /// Don't call this unless you know what you're doing.
        /// <para>This does not update the current state of the MultiTagComponent.</para>
        /// <para>It's best to remove a tag from a gameobject's MultiTagComponent.</para>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="gameObject"></param>
        public static void RemoveToDict(string[] tags, GameObject gameObject)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                SingleRemoveFromDict(tag, gameObject);
            }
        }

        /// <summary>
        /// Don't call this unless you know what you're doing.
        /// <para>This does not update the current state of the MultiTagComponent.</para>
        /// <para>It's best to add a tag from a gameobject's MultiTagComponent.</para>
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="gameObject"></param>
        public static void SingleRemoveFromDict(string tag, GameObject gameObject)
        {
            if (MtDict.ContainsKey(tag))
            {
                if (MtDict[tag] == null || MtDict[tag].Count == 1)
                {
                    MtDict.Remove(tag);
                }
                else
                {
                    MtDict[tag].Remove(gameObject);
                }
            }
        }

        /// <summary>
        /// Runs a query on the current state of gameobjects with MultiTagComponent.
        /// </summary>
        /// <param name="multiTagFilter">Query parameters</param>
        /// <returns>A shallow List of GameObjects</returns>
        public static List<GameObject> QueryMultiple(MultiTagFilter multiTagFilter)
        {
            //Creates a shallow copy
            return RunQuery(multiTagFilter).ToList();
        }

        /// <summary>
        /// Runs a query on the current state of gameobjects with MultiTagComponent.
        /// </summary>
        /// <param name="multiTagFilter"></param>
        /// <returns>Returns the first index</returns>
        public static GameObject QuerySingle(MultiTagFilter multiTagFilter)
        {
            return RunQuery(multiTagFilter)[0];
        }

        private static List<GameObject> RunQuery(MultiTagFilter multiTagFilter)
        {
            List<GameObject> result = null;

            var any = multiTagFilter.Any;
            var all = multiTagFilter.All;
            var none = multiTagFilter.None;

            var resultHash = new HashSet<GameObject>();
            if (any != null)
            {
                for (int i = 0; i < any.Length; i++)
                {
                    var key = any[i];
                    if (MtDict.ContainsKey(key))
                    {
                        var tempResult = MtDict[key];

                        if (tempResult != null && tempResult.Count != 0)
                        {
                            resultHash.UnionWith(tempResult);
                        }
                    }
                }
                result = resultHash.ToList();
            }

            if (all != null)
            {
                for (int i = 0; i < all.Length; i++)
                {
                    var key = all[i];
                    if (MtDict.ContainsKey(key))
                    {
                        var tempResult = MtDict[key];

                        if (tempResult != null && tempResult.Count != 0)
                        {
                            result = result == null ? tempResult.ToList() : tempResult.Intersect(result).ToList();
                        }
                    }
                }
            }

            if (none != null)
            {
                if (result == null)
                {
                    if (resultHash.Count > 0)
                    {
                        resultHash.Clear();
                    }

                    foreach (var dict in MtDict)
                    {
                        var list = dict.Value;
                        resultHash.UnionWith(list);
                    }
                    result = resultHash.ToList();
                }

                for (int i = 0; i < none.Length; i++)
                {
                    var key = none[i];
                    if (MtDict.ContainsKey(key))
                    {
                        var toRemove = MtDict[key];

                        if (toRemove != null && toRemove.Count != 0)
                        {
                            var theSet = new HashSet<GameObject>(toRemove);
                            result.RemoveAll(item => theSet.Contains(item));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the list from a key
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>Shallow copy of List. null if false<GameObject></returns>
        public static List<GameObject> GetList(string tag)
        {
            return MtDict.ContainsKey(tag) ? MtDict[tag].ToList() : null;
        }

        /// <summary>
        /// Gets the int of elements of list from a tag
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>int of elements from a key. 0 if tag doesn't exist</returns>
        public static int GetListCount(string tag)
        {
            return MtDict.ContainsKey(tag) ? MtDict[tag].Count : 0;
        }

        /// <summary>
        /// Gets the first gameobject from a tag
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>GameObject if tag exist. null if false</returns>
        public static GameObject GetGameObject(string tag)
        {
            return MtDict.ContainsKey(tag) ? MtDict[tag].ToArray()[0] : null;
        }
    }
}