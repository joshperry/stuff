using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyGeek.WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public override bool Equals(object obj)
        {
            var other = obj as PagingInfo;
            return other != null
                && this.TotalItems == other.TotalItems
                && this.ItemsPerPage == other.ItemsPerPage
                && this.CurrentPage == other.CurrentPage;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ TotalItems.GetHashCode() ^ ItemsPerPage.GetHashCode() ^ CurrentPage.GetHashCode();
        }
    }
}