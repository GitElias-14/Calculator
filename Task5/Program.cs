using System;
using System.Text;
using System.Collections.Generic;

interface IVisitable {
   void Accept(IVisitor vtor);
}

interface IVisitor {
   void Visit(Literal elem);
   void Visit(Add elem);
   void Visit(Mul elem);
   void Visit(Div elem);
   void Visit(Fct elem);
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
{   s.Append("(");
    e.Left.Accept(this);
    s.Append("/");
    e.Right.Accept(this);
    s.Append(")");
}

public void Visit(Fct e)
{
    s.Append("(");
    e.Expr.Accept(this);
    s.Append(")!");
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

class FactorialOfNegativeException : ArgumentException
{
    public FactorialOfNegativeException()
        : base("cannot calculate factorial of negatives")
    {
    }
}

class Program {
   static void Main() {
   IVisitable t = new Fct(new Literal(-5));
Console.WriteLine(new Stringifier(t) + "=" + new Evaluator(t));
   }
}

