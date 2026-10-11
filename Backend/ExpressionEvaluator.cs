using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

using System.Globalization;

namespace Backend;

public static class ExpressionEvaluator
{
    public static double Evaluate(string infix) => EvaluatePostfix(ToPostfix(Decompose(infix)));

    public static List<string> Decompose(string infix)
    {
        var parts = new List<string>();
        for (int i = 0; i < infix.Length; i++)
        {
            char c = infix[i];
            if (char.IsWhiteSpace(c)) continue;

            if (char.IsDigit(c) || c == '.')
            {
                int start = i;
                while (i + 1 < infix.Length && (char.IsDigit(infix[i + 1]) || infix[i + 1] == '.'))
                    i++;
                parts.Add(infix[start..(i + 1)]);
            }
            else if (IsOperator(c))
            {
                parts.Add(c.ToString());
            }
            else
            {
                throw new FormatException($"Invalid character: '{c}'.");
            }
        }
        return parts;
    }

    public static List<string> ToPostfix(List<string> parts)
    {
        var postfix = new List<string>();
        var stack = new Stack<char>();

        foreach (var element in parts)
        {
            if (!IsOperator(element[0]))
            {
                postfix.Add(element);
                continue;
            }

            char item = element[0];

            if (item == ')')
            {
                while (stack.Count > 0 && stack.Peek() != '(')
                    postfix.Add(stack.Pop().ToString());

                if (stack.Count == 0)
                    throw new FormatException("Unbalanced parentheses.");

                stack.Pop(); // descarta '('
            }
            else
            {
                while (stack.Count > 0 && PriorityInfix(item) <= PriorityStack(stack.Peek()))
                    postfix.Add(stack.Pop().ToString());

                stack.Push(item);
            }
        }

        while (stack.Count > 0)
        {
            char op = stack.Pop();
            if (op == '(')
                throw new FormatException("Unbalanced parentheses.");
            postfix.Add(op.ToString());
        }

        return postfix;
    }

    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new FormatException("Invalid expression."),
    };

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new FormatException("Invalid expression."),
    };

    private static bool IsOperator(char item) => item == '^' || item == '*' || item == '/' || item == '+' || item == '-' || item == '(' || item == ')';

    private static double EvaluatePostfix(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var element in postfix)
        {
            if (IsOperator(element[0]))
            {
                if (stack.Count < 2)
                    throw new FormatException("Invalid expression.");

                double ope2 = stack.Pop();
                double ope1 = stack.Pop();
                stack.Push(Calculate(ope1, ope2, element[0]));
            }
            else
            {
                stack.Push(double.Parse(element, CultureInfo.InvariantCulture));
            }
        }

        if (stack.Count != 1)
            throw new FormatException("Invalid expression.");

        return stack.Pop();
    }

    private static double Calculate(double ope1, double ope2, char item) => item switch
    {
        '*' => ope1 * ope2,
        '/' => ope1 / ope2,
        '+' => ope1 + ope2,
        '-' => ope1 - ope2,
        '^' => Math.Pow(ope1, ope2),
        _ => throw new FormatException("Invalid expression."),
    };
}