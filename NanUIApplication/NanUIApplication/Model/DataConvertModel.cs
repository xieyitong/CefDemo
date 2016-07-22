using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanUIApplication
{
    public class DataConvertModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string order { get; set; }
        public int num { get; set; }
        public string updateTime { get; set; }
        public string message { get; set; }
        public int errCode { get; set; }
        public int status { get; set; }
        public List<ResponseDetails> data { get; set; }

    }
}
