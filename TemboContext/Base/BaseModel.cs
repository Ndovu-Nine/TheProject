using System;
using System.Collections;
using TemboContext.Connection;

namespace TemboContext.Base
{
	public abstract class BaseModel : IDisposable
	{
		#region variables
        private Hashtable _items;
		#endregion

		#region properties
		public object this[string name]
        {
            get => _items?[name];
            set
            {
                if (_items == null)
                    _items = new Hashtable();
                _items[name] = value;
            }
        }

        internal ConnectionContainer DbConnection { get; set; }
		#endregion

		#region public methods
		public void Dispose()
		{
		}
		#endregion
	}
}
