using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

class MaxIterator : IEnumerator<IVisitable>
{
   private readonly List<IVisitable> list;
   private int _index = -1;

   public MaxIterator(List<IVisitable> list)
   {
      this.list = list;
   
   }
      
   public IVisitable Current => list[_index];
   object System.Collections.IEnumerator.Current => Current;

   public bool MoveNext()
   {
     _index ++;
     return _index < list.Count;
   }

   public void Reset() => _index = -1;


   public void Dispose(){}
   
}
