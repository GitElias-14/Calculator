using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

class Evaluator: IVisitor {
   public Evaluator(IVisitable t) 
   {
      s = new Stack<int>();
      t.Accept(this);
   }
   public void Visit(Literal e) 
   {
      s.Push(e.Value);
   }
   public void Visit(Add e) 
   {
      var (left,right) = AcceptAndPop(e.Left, e.Right);
      s.Push(left+right);
   }
   public void Visit(Mul e) 
   {
      var (left,right) = AcceptAndPop(e.Left, e.Right);
      s.Push(left*right);
   }

   public void Visit(Div e)
   {
      var (left,right) = AcceptAndPop(e.Left, e.Right);
      if (right == 0) throw new DivideByZeroException("cannot divide by zero");
      s.Push(left/right);
   }

   public void Visit(Fct e)
   {
      int Value = AcceptAndPop(e.Expr);
      if(Value < 0) throw new FactorialOfNegativeException();
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
         int value = AcceptAndPop(arg);

         if (first || value > currentMax)
         {
            currentMax = value;
            first = false;
         }  
      }
      s.Push(currentMax);
   }

   private int AcceptAndPop(IVisitable expr)
   {
      expr.Accept(this);
      return s.Pop();
   }
   private (int, int) AcceptAndPop(IVisitable expr1,IVisitable expr2)
   {
      expr1.Accept(this);
      int e1 = s.Pop();
      expr2.Accept(this);
      int e2 = s.Pop();
      return (e1,e2);
   }
   
   public void Clear() => s.Clear();
   public override string ToString() => s.Peek().ToString();
   private Stack<int> s;
}
