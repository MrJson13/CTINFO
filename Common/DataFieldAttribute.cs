using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DataFieldAttribute : Attribute
    {
        public DataFieldAttribute()
        {

        }
        public DataFieldAttribute(string name)
        {
            m_name = name;
        }
        private string m_name = null;

        public string Name { get { return m_name; } set { m_name = value; } }
    }
}
