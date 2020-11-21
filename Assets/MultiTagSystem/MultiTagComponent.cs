using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace MultiTagSystem
{
    public class MultiTagComponent : MonoBehaviour
    {
        [HideInInspector]
        public List<string> tags => _tags;

        [SerializeField]
        private List<string> _tags;

        private void Awake()
        {
            AddDict();
        }

        public void AddDict()
        {
            MultiTag.MultiTagsAddToDict(_tags.ToArray(), gameObject);
        }

        private void OnDestroy()
        {
            MultiTag.RemoveToDict(_tags.ToArray(), gameObject);
        }

        /// <summary>
        /// Adds a Tag from the Set.
        /// <para>You cannot create a new Tag.</para>
        /// <para>To add a Tag via the Editor and rebuild.</para>
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(string tag)
        {
            if (Tags.Set.Contains(tag))
            {
                var hashSet = new HashSet<string>(_tags);
                if (hashSet.Add(tag))
                {
                    _tags = hashSet.ToList();
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
            if (Tags.Set.Contains(tag))
            {
                var hashSet = new HashSet<string>(_tags);
                if (hashSet.Remove(tag))
                {
                    _tags = hashSet.ToList();
                    MultiTag.SingleRemoveFromDict(tag, gameObject);
                }
            }
        }
    }
}