using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.Commom.SMS
{
    public class Entity
    {
        public string Ext { get; set; }
        public string Extend { get; set; }
        public string[] Params { get; set; }
        public string Sig { get; set; }
        public string Sign { get; set; }
        public Phone Tel { get; set; }
        public string Time { get; set; }
        public string Tpl_id { get; set; }
    }
    public class Phone
    {
        public string Mobile { get; set; }
        public string Nationcode { get; set; }
    }
}
