using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

abstract class LeftRightCalculations : IVisitable
{
    public readonly IVisitable Left, Right;
    public LeftRightCalculations(IVisitable left, IVisitable right)
    {
        Left = left;
        Right = right;
    }
    public abstract void Accept(IVisitor vtor);
}

// Visitable classes for expressions
class Mul : LeftRightCalculations {
    public Mul(IVisitable left, IVisitable right) : base (left, right) {}
    public override void Accept(IVisitor vtor) => vtor.Visit(this);
}

class Add : LeftRightCalculations {
    public Add(IVisitable left, IVisitable right) : base (left, right) {}
    public override void Accept(IVisitor vtor) => vtor.Visit(this);
}

class Div : LeftRightCalculations {
    public Div(IVisitable left, IVisitable right) : base (left, right) {}
    public override void Accept(IVisitor vtor) => vtor.Visit(this);
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

class Max : IVisitable , IEnumerable<IVisitable>
{
   private readonly List<IVisitable> Expr;

   public Max(List<IVisitable> expr)
   {
      if (expr == null || expr.Count == 0)
        throw new ArgumentException("passed arguments are not correct");
      Expr = expr;
   }
    public Max(IVisitable[] args) : this(new List<IVisitable>(args)) { }
    public void Accept(IVisitor vtor) => vtor.Visit(this);

    public IEnumerator<IVisitable> GetEnumerator() => new MaxIterator(Expr);

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
   
}

// Exception class
class FactorialOfNegativeException : ArgumentException
{
    public FactorialOfNegativeException()
        : base("cannot calculate factorial of negatives")
    {
    }
}