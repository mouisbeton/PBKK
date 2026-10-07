using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class CalculatorForm
    {
        private TextBox txtDisplay;
        private Label lblExpression;
        private Label lblStatus;
        private ListBox lstHistory;

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Kalkulator | PBKK Pertemuan 3";
            ClientSize = new Size(680, 590);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(241, 245, 249);
            Font = new Font("DejaVu Sans", 10F);
            AutoScaleMode = AutoScaleMode.None;

            var title = new Label {
                Text = "KALKULATOR", Location = new Point(24, 22), Size = new Size(400, 32),
                Font = new Font("DejaVu Sans", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42)
            };
            var subtitle = new Label {
                Text = "Windows Forms dan C#", Location = new Point(26, 62), Size = new Size(400, 25),
                ForeColor = Color.FromArgb(71, 85, 105)
            };
            lblExpression = new Label {
                Name = "lblExpression", Text = "", Location = new Point(26, 100), Size = new Size(380, 26),
                TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(71, 85, 105)
            };
            txtDisplay = new TextBox {
                Name = "txtDisplay", Text = "0", ReadOnly = true, TabStop = false,
                Location = new Point(26, 134), Size = new Size(380, 52),
                Font = new Font("DejaVu Sans", 25F, FontStyle.Bold), TextAlign = HorizontalAlignment.Right,
                BackColor = Color.White, ForeColor = Color.FromArgb(15, 23, 42), BorderStyle = BorderStyle.FixedSingle
            };
            lblStatus = new Label {
                Name = "lblStatus", Text = "Siap menghitung", Location = new Point(26, 197),
                Size = new Size(380, 27), ForeColor = Color.FromArgb(71, 85, 105)
            };
            Controls.AddRange(new Control[] { title, subtitle, lblExpression, txtDisplay, lblStatus });

            string[,] keys = {
                { "C", "⌫", "±", "÷" },
                { "7", "8", "9", "×" },
                { "4", "5", "6", "−" },
                { "1", "2", "3", "+" },
                { "0", ".", "=", "=" }
            };
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (row == 4 && col == 3) continue;
                    string key = keys[row, col];
                    bool isOperator = key == "+" || key == "−" || key == "×" || key == "÷";
                    var button = new Button {
                        Name = "btn" + ButtonName(key), Text = key,
                        Location = new Point(26 + col * 98, 236 + row * 64),
                        Size = new Size(key == "=" ? 184 : 86, 52),
                        FlatStyle = FlatStyle.Flat, Font = new Font("DejaVu Sans", 16F, FontStyle.Bold),
                        BackColor = key == "=" ? Color.FromArgb(37, 99, 235) : isOperator ? Color.FromArgb(219, 234, 254) : Color.White,
                        ForeColor = key == "=" ? Color.White : Color.FromArgb(15, 23, 42),
                        UseVisualStyleBackColor = false
                    };
                    button.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
                    if (key == "C") button.Click += ClearButton_Click;
                    else if (key == "⌫") button.Click += BackspaceButton_Click;
                    else if (key == "±") button.Click += SignButton_Click;
                    else if (key == ".") button.Click += DecimalButton_Click;
                    else if (key == "=") button.Click += EqualsButton_Click;
                    else if (isOperator) button.Click += OperatorButton_Click;
                    else button.Click += NumberButton_Click;
                    Controls.Add(button);
                }
            }

            var historyTitle = new Label {
                Text = "RIWAYAT", Location = new Point(432, 100), Size = new Size(218, 26),
                Font = new Font("DejaVu Sans", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42)
            };
            lstHistory = new ListBox {
                Name = "lstHistory", Location = new Point(432, 134), Size = new Size(218, 416),
                BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White,
                Font = new Font("DejaVu Sans", 10F), HorizontalScrollbar = true, IntegralHeight = false
            };
            Controls.AddRange(new Control[] { historyTitle, lstHistory });
            ResumeLayout(false);
            PerformLayout();
        }

        private static string ButtonName(string key)
        {
            switch (key)
            {
                case "C": return "Clear";
                case "⌫": return "Backspace";
                case "±": return "Sign";
                case ".": return "Decimal";
                case "=": return "Equals";
                case "+": return "Plus";
                case "−": return "Minus";
                case "×": return "Multiply";
                case "÷": return "Divide";
                default: return key;
            }
        }
    }
}
