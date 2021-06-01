using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GrabInfo
    {

        /// <summary>
        /// GrabID
        /// </summary>		
        public int GrabID { get; set; }
        /// <summary>
        /// UniqueID
        /// </summary>		
        public string UniqueID { get; set; }
        /// <summary>
        /// ProName
        /// </summary>		
        public string ProName { get; set; }
        /// <summary>
        /// ProPrice
        /// </summary>		
        public string ProPrice { get; set; }
        /// <summary>
        /// WinCompany
        /// </summary>		
        public string WinCompany { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// URL
        /// </summary>		
        public string URL { get; set; }

    }
}
