﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sitecore.Data.Fields;

namespace Fortis.Model.Fields
{
	public class ListFieldWrapper : FieldWrapper, IListFieldWrapper
	{
		public ListFieldWrapper(Field field)
			: base(field)
		{
		}

		public IEnumerable<T> GetItems<T>() where T : IItemWrapper
		{
			var listField = (MultilistField)Field;

			foreach (var id in listField.Items)
			{
                var item = Sitecore.Context.Database.GetItem(id);
                if (item != null)
                {
                    var wrapper = Spawn.FromItem<T>(item);
                    if (wrapper is T)
                    {
                        yield return (T)wrapper;
                    }
                }
			}
		}

        public IEnumerator<IItemWrapper> GetEnumerator()
		{
			return GetItems<IItemWrapper>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerable<Guid> Value
		{
			get
			{
				var listField = (MultilistField)Field;

				foreach (var id in listField.Items)
				{
					Guid guid;
					if (Guid.TryParse(id, out guid))
					{
						yield return guid;
					}
				}
			}
		}
	}
}
