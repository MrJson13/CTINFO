using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReqModel
{
    public class ReqProjectInfoes : ReqPageParams
    {
        public string StrWhere { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string SuccessfulName { get; set; }
    }
}
