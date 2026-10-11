using Backend;

var infix = "4*5/(4+6)";
Console.WriteLine($"Infix = {infix}, Result = {ExpressionEvaluator.Evaluate(infix):N5}"); // 2

var infix2 = "4*(5+6-(8/2^3)-7)-1";
Console.WriteLine($"Infix = {infix2}, Result = {ExpressionEvaluator.Evaluate(infix2):N5}"); // 11

var infix3 = "4*7^(1/3)*7*((1+9)/3*7^4)";
Console.WriteLine($"Infix = {infix3}, Result = {ExpressionEvaluator.Evaluate(infix3):N5}"); // 428,675.12518474100 

var infix4 = "144^(1/2)";
Console.WriteLine($"Infix = {infix4}, Result = {ExpressionEvaluator.Evaluate(infix4):N5}"); // 12


static void Probar(string expresion)
{
    try
    {
        List<string> resultado = ExpressionEvaluator.Decompose(expresion);
        Console.WriteLine($"\"{expresion}\"  =>  [{string.Join(", ", resultado)}]");
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"\"{expresion}\"  =>  ERROR: {ex.Message}");
    }
}

Probar("12+3.5");
Probar("12 + 3 * 2");
Probar("(10.5-0.5)/4");
Probar("2^3^2");
Probar("   7   ");
Probar("100");
Probar("");
Probar("2 & 3");      // debe dar error
Probar("3 + a");      // debe dar error