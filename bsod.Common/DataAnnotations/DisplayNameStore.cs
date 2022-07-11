using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.DataAnnotations
{

    public class DisplayNameStore
    {
        private static List<DisplayNameStore> _store = new List<DisplayNameStore>();

        public string DisplayName { get; }
        public Type ModelType { get; }
        public PropertyInfo Property { get; }

        private DisplayNameStore(string displayName, Type modelType, PropertyInfo prop)
        {
            ModelType = modelType;
            DisplayName = displayName;
            Property = prop;
        }
        public static DisplayNameStore GetDisplayName(Type type, PropertyInfo prop)
        {
            DisplayNameStore foundStore = _store.Find(x => x.Property == prop);
            if (foundStore != null) return foundStore;
            string displayName = type.GetDisplayName(prop, false);
            foundStore = new DisplayNameStore(displayName, type, prop);
            _store.Add(foundStore);
            return foundStore;
        }

        public bool IsHidden(bool isTable = false, bool isList = false, bool isExport = false)
        {
            var att = ModelType.GetMetaDataAttribute<HideAttribute>(Property);
            if (isTable)
                return att?.ShowTable == false;
            else if (isList)
                return att?.ShowList == false;
            else if (isExport)
                return att?.ShowExport == false;
            else
                return att != null;
        }
    }
}
