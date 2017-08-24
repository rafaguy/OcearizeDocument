using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBase.Database
{
  public  class FirebaseObject<T>
    {
        internal FirebaseObject(string key,T obj)
        {
            this.Key = key;
            this.Object = obj;
        }
        public string Key { get; }
        public T Object { get; }
    }
}
