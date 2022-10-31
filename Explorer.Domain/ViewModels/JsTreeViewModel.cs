using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Explorer.Domain.ViewModels
{
    public class Attr
    {
        public string type { get; set; }
        public string title { get; set; }

    }

  
    public class JsTreeViewModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; } 
        public Attr a_attr  { get; set; }
        public string icon { get; set; }
    }


}
