using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class CalculatorForm : Form
    {
        private readonly CalculatorEngine engine = new CalculatorEngine();

        public CalculatorForm()
        {
            InitializeComponent();
            RefreshDisplay();
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            engine.EnterDigit(((Button)sender).Text);
            RefreshDisplay();
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Execute(delegate { engine.ChooseOperation(((Button)sender).Text); });
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            Execute(delegate
            {
                string history = engine.Calculate();
                if (history != null)
                {
                    lstHistory.Items.Insert(0, history);
                    if (lstHistory.Items.Count > 20) lstHistory.Items.RemoveAt(20);
                }
            });
        }

        private void DecimalButton_Click(object sender, EventArgs e)
        {
            engine.EnterDecimal();
            RefreshDisplay();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            engine.Clear();
            RefreshDisplay();
        }

        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            engine.Backspace();
            RefreshDisplay();
        }

        private void SignButton_Click(object sender, EventArgs e)
        {
            engine.ChangeSign();
            RefreshDisplay();
        }

        private void Execute(Action action)
        {
            try
            {
                action();
                lblStatus.Text = "Siap menghitung";
                lblStatus.ForeColor = Color.FromArgb(71, 85, 105);
            }
            catch (DivideByZeroException ex)
            {
                ShowError(ex.Message);
            }
            catch (OverflowException)
            {
                ShowError("Hasil terlalu besar. Tekan C.");
            }
            RefreshDisplay();
        }

        private void ShowError(string message)
        {
            engine.SetError();
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.FromArgb(185, 28, 28);
        }

        private void RefreshDisplay()
        {
            txtDisplay.Text = engine.Display;
            lblExpression.Text = engine.Expression;
            if (engine.Display != "Error")
            {
                lblStatus.Text = "Siap menghitung";
                lblStatus.ForeColor = Color.FromArgb(71, 85, 105);
            }
        }
    }
}
