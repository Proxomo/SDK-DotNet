using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxomo
{
    public class ListCompletedEventArgs<T>
    {
        public Exception Error { get; set; }
        public T Result { get; set; }
    }

    public class ItemCompletedEventArgs<T>
    {
        public Exception Error { get; set; }
        public T Result { get; set; }
    }
}
