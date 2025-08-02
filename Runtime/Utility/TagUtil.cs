using System;
using UnityEngine;

namespace JohaToolkit.UnityEngine.Utility
{
    public static class TagUt
    {
        public static string[] GetAllTags()
        {
#if Unity_Editor
            return UnityEditorInternal.InternalEditorUtility.tags;
#else      
            Debug.Log("[tagHelper] This Method is only available in the Editor!");
            return Array.Empty<string>();
#endif
        }
    }
}
