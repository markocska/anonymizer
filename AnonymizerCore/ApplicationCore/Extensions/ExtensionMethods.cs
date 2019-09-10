using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrambler.Extensions
{
    public static class ExtensionMethods
    {
        public static List<T> GetSubListElementsOfIndex<T>(this List<List<T>> listOfLists, int index)
        {
            return listOfLists.SelectMany(list => list, (list, listElement) => list.ElementAt(index)).ToList();
        }

    }
}
