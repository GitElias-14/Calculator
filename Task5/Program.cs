using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

interface IVisitable {
   void Accept(IVisitor vtor);
}

interface IVisitor {
   void Visit(Literal elem);
   void Visit(Add elem);
   void Visit(Mul elem);
   void Visit(Div elem);
   void Visit(Fct elem);
   void Visit(Max elem);
}

class Evaluator: IVisitor {
   public Evaluator(IVisitable t) {
      s = new Stack<int>();
      t.Accept(this);
   }
   public void Visit(Literal e) {
      s.Push(e.Value);
   }
   public void Visit(Add e) {
      e.Left.Accept(this);
      int left = s.Pop();
      e.Right.Accept(this);
      int right = s.Pop();
      s.Push(left+right);
   }
   public void Visit(Mul e) {
      e.Left.Accept(this);
      int left = s.Pop();
      e.Right.Accept(this);
      int right = s.Pop();
      s.Push(left*right);
   }

  public void Visit(Div e)
  {
       e.Left.Accept(this);
      int left = s.Pop();
      e.Right.Accept(this);
      int right = s.Pop();
       if (right == 0)
        throw new DivideByZeroException();
      s.Push(left/right);
  }    


    public void Visit(Fct e)
    {
        e.Expr.Accept(this);
        int Value = s.Pop();
        if(Value < 0)
            throw new FactorialOfNegativeException();
        int result = 1;

        for(int i = 2; i <= Value ; i++)
        result *= i;
        
         s.Push(result);
    }
   public void Visit(Max e)
   {
      bool first = true;
      int currentMax = 0;

      foreach(var arg in e)
      {
            arg.Accept(this);
        int value = s.Pop();

        if (first || value > currentMax)
        {
            currentMax = value;
            first = false;
        }
         
      }

      s.Push(currentMax);
      


   }
   


   public void Clear() => s.Clear();
   public override string ToString() => s.Peek().ToString();
   private Stack<int> s;
}

class Stringifier: IVisitor {
   public Stringifier(IVisitable t) {
      s = new StringBuilder();
      t.Accept(this);
   }
   public void Visit(Literal e) {
      s.Append(e.Value);
   }
   public void Visit(Add e) {
      s.Append("(");
      e.Left.Accept(this);
      s.Append("+");
      e.Right.Accept(this);
      s.Append(")");
   }
   public void Visit(Mul e) {
      e.Left.Accept(this);
      s.Append("*");
      e.Right.Accept(this);
   }
  public void Visit(Div e)
{  
    e.Left.Accept(this);
    s.Append("/");
    e.Right.Accept(this);
    
}

public void Visit(Fct e)
{
    s.Append("(");
    e.Expr.Accept(this);
    s.Append(")!");
}

public void Visit(Max e)
{
   s.Append("max{");
   bool first = true;

    foreach (var arg in e)
   {
      if (!first) s.Append(",");
      arg.Accept(this);
      first = false;
   }

   s.Append("}");
      
}
   public void Clear() => s.Clear();
   public override string ToString() => s.ToString();
   private StringBuilder s;
}

class Literal : IVisitable {
   public readonly int Value;
   public Literal(int Value) {
      this.Value = Value;
   }
   public void Accept(IVisitor vtor) => vtor.Visit(this);
}

class Mul : IVisitable {
   public Mul(IVisitable Left, IVisitable Right) {
      this.Left = Left;
      this.Right = Right;
   }
   public void Accept(IVisitor vtor) => vtor.Visit(this);
   public readonly IVisitable Left, Right;
}

class Add : IVisitable {
   public Add(IVisitable Left, IVisitable Right) {
      this.Left = Left;
      this.Right = Right;
   }
   public void Accept(IVisitor vtor) => vtor.Visit(this);
   public readonly IVisitable Left, Right;
}

class Div : IVisitable
{
     public Div(IVisitable Left, IVisitable Right) {
      this.Left = Left;
      this.Right = Right;
   }
   public void Accept(IVisitor vtor) => vtor.Visit(this);
   public readonly IVisitable Left, Right;

}
 
class Fct : IVisitable
{
    public readonly IVisitable Expr;

    public Fct(IVisitable expr)
    {
        Expr = expr;
    }
    public void Accept(IVisitor vtor) => vtor.Visit(this);
}

class  Max : IVisitable , IEnumerable<IVisitable>
{
   public readonly List<IVisitable> Expr;

   public Max(List<IVisitable> expr)
   {
      if (expr == null || expr.Count == 0)
      throw new ArgumentException("passed arguments are not correct");

      Expr = expr;
   }

    public void Accept(IVisitor vtor) => vtor.Visit(this);

    public IEnumerator<IVisitable> GetEnumerator()
   {
       return new MaxItrator(Expr);
   }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
   
}
class FactorialOfNegativeException : ArgumentException
{
    public FactorialOfNegativeException()
        : base("cannot calculate factorial of negatives")
    {
    }
}

class MaxItrator : IEnumerator<IVisitable>
{
   private readonly List<IVisitable> list;
   private int _index = -1;

   public MaxItrator(List<IVisitable> list)
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



class Program {
   static void Main()
   {
       IVisitable t = new Max(new List<IVisitable> {
            new Add(new Literal(-2), new Literal(9)),  // = 7
            new Literal(6),
            new Literal(0)
        });

         Console.WriteLine($"{new Stringifier(t)} = {new Evaluator(t)}");
   }
}

