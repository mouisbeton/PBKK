using System;
using System.Globalization;

namespace CalculatorApp
{
    public sealed class CalculatorEngine
    {
        private decimal firstNumber;
        private string operation = "";
        private bool startNewNumber = true;
        private bool hasSecondNumber;
        private bool hasError;

        public string Display { get; private set; } = "0";
        public string Expression { get; private set; } = "";

        private static decimal Parse(string text)
        {
            return decimal.Parse(text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture);
        }

        private static string Format(decimal value)
        {
            return value.ToString("0.##############", CultureInfo.InvariantCulture);
        }

        public void EnterDigit(string digit)
        {
            if (digit.Length != 1 || digit[0] < '0' || digit[0] > '9')
                throw new ArgumentException("Masukan harus berupa satu angka.");
            PrepareInput();
            int digitCount = Display.Replace("-", "").Replace(".", "").Length;
            if (digitCount >= 14) return;
            if (Display == "0") Display = digit;
            else if (Display == "-0") Display = "-" + digit;
            else Display += digit;
            hasSecondNumber = true;
        }

        public void EnterDecimal()
        {
            PrepareInput();
            if (!Display.Contains(".")) Display += ".";
            hasSecondNumber = true;
        }

        private void PrepareInput()
        {
            if (hasError) Clear();
            if (!startNewNumber) return;
            Display = "0";
            startNewNumber = false;
            if (operation == "") Expression = "";
        }

        public void ChooseOperation(string nextOperation)
        {
            if (nextOperation != "+" && nextOperation != "−" && nextOperation != "×" && nextOperation != "÷")
                throw new ArgumentException("Operasi tidak tersedia.");
            if (hasError) return;
            if (operation != "" && !startNewNumber && hasSecondNumber)
                Calculate();
            firstNumber = Parse(Display);
            operation = nextOperation;
            Expression = Format(firstNumber) + " " + operation;
            startNewNumber = true;
            hasSecondNumber = false;
        }

        public string Calculate()
        {
            if (hasError || operation == "" || !hasSecondNumber) return null;
            decimal secondNumber = Parse(Display);
            Expression = Format(firstNumber) + " " + operation + " " + Format(secondNumber) + " =";
            decimal result;
            switch (operation)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "−": result = firstNumber - secondNumber; break;
                case "×": result = firstNumber * secondNumber; break;
                case "÷":
                    if (secondNumber == 0) throw new DivideByZeroException("Tidak dapat membagi dengan nol.");
                    result = firstNumber / secondNumber;
                    break;
                default: throw new InvalidOperationException("Operasi tidak tersedia.");
            }
            Display = Format(result);
            string history = Expression + " " + Display;
            operation = "";
            firstNumber = result;
            startNewNumber = true;
            hasSecondNumber = false;
            return history;
        }

        public void Backspace()
        {
            if (hasError) { Clear(); return; }
            if (startNewNumber) return;
            Display = Display.Length > 1 ? Display.Substring(0, Display.Length - 1) : "0";
            if (Display == "-") Display = "0";
        }

        public void ChangeSign()
        {
            if (hasError) return;
            if (startNewNumber && operation != "") PrepareInput();
            if (operation == "") Expression = "";
            Display = Display.StartsWith("-") ? Display.Substring(1) : "-" + Display;
            startNewNumber = false;
            hasSecondNumber = true;
        }

        public void SetError()
        {
            Display = "Error";
            operation = "";
            startNewNumber = true;
            hasSecondNumber = false;
            hasError = true;
        }

        public void Clear()
        {
            firstNumber = 0;
            operation = "";
            Display = "0";
            Expression = "";
            startNewNumber = true;
            hasSecondNumber = false;
            hasError = false;
        }
    }
}
