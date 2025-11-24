using System.Diagnostics;
using System.Reflection.Emit;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        List<(string, double)> calculationsHistory = new List<(string, double)>();
        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    LogCalculation($"{num1} + {num2} = ", result);
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    LogCalculation($"{num1} - {num2} =", result);
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    LogCalculation($"{num1} * {num2} = ", result);
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        LogCalculation($"{num1} / {num2} = ", result);
                    }
                    writer.WriteValue("Divide");
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;
        }

        public void LogCalculation(string calculation, double result)
        {
            calculationsHistory.Add((calculation, result));
        }

        public List<(string, double)> GetCalculations()
        {
            return calculationsHistory;
        }

        public void ClearCalculations()
        {
            calculationsHistory.Clear();
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }

}