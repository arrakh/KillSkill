using System;

namespace Arr.SDS
{
    public class SearchFilterAttribute : Attribute
    {
        public string filter;

        public SearchFilterAttribute(string filter)
        {
            this.filter = filter;
        }
    }
}