using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireBase.Database.Extensions
{
  public static  class TaskExtensions
    {
        public static async Task WithAggregateException(this Task source)
        {
            try
            {
                await source.ConfigureAwait(false);
            }
            catch
            {
                throw source.Exception;
            }
        }
    }
}
