using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace MultiTagSystem
{
    public class MultiTagComponent : MonoBehaviour
    {
        [HideInInspector]
        public List<string> Tags => _Tags;

        [SerializeField]
        private List<string> _Tags;

        private void Awake()
        {
            AddDict();
        }

        public void AddDict()
        {
            MultiTag.MultiTagsAddToDict(_Tags.ToArray(), gameObject);
        }

        private void OnDestroy()
        {
            MultiTag.RemoveToDict(_Tags.ToArray(), gameObject);
        }

        /// <summary>
        /// Adds a Tag from the Set.
        /// <para>You cannot create a new Tag.</para>
        /// <para>To create do it via the Editor and Rebuild.</para>
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(string tag)
        {
            if (MultiTagSystem.Tags.Set.Contains(tag))
            {
                var hashSet = new HashSet<string>(_Tags);
                if (hashSet.Add(tag))
                {
                    _Tags = hashSet.ToList();
                    MultiTag.SingleTagAddToDict(tag, gameObject);
                }
            }
        }

        /// <summary>
        /// Removes a Tag.
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(string tag)
        {
            if (MultiTagSystem.Tags.Set.Contains(tag))
            {
                var hashSet = new HashSet<string>(_Tags);
                if (hashSet.Remove(tag))
                {
                    _Tags = hashSet.ToList();
                    MultiTag.SingleRemoveFromDict(tag, gameObject);
                }
            }
        }
    }
}