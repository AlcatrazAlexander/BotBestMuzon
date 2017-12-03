using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BestMusonDownloader
{
    public sealed class ProggressBarWithLabel : UserControl
    {
        private Label _label;

        private int _mimimum;
        private int _maximum = 100;
        private int _value;
        private bool _usePercentage;
        private string _text = "";
        private bool _useText;

        [Browsable(true)]
        [DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                if (_value == value) return;

                if (value >= _mimimum && value <= _maximum)
                {
                    _value = value;

                    SetLabelText();
                    Invalidate();
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int Minimum
        {
            get => _mimimum;
            set
            {
                if (_mimimum == value) return;

                _mimimum = value;

                if (_value < _mimimum)
                {
                    _value = _mimimum;

                    SetLabelText();
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (_maximum == value) return;

                _maximum = value;

                if (_value > _maximum)
                    _value = _maximum;
                
                SetLabelText();
                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        public bool UsePercentage
        {
            get => _usePercentage;
            set
            {
                if (value == _usePercentage) return;

                _usePercentage = value;
                SetLabelText();
                Invalidate();
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        public override string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = "";
                if (_text == value) return;

                _text = value;
                SetLabelText();
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        public bool UseText
        {
            get => _useText;
            set
            {
                if (_useText == value) return;

                _useText = value;
                SetLabelText();
            }
        }

        public ProggressBarWithLabel()
            : base()
        {
            InitializeComponent();

            _label.ForeColor = Color.Black;
            this.ForeColor = SystemColors.Highlight;
            _label.SizeChanged += _label_SizeChanged;

            SetLabelText();
        }

        private void InitializeComponent()
        {
            this._label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _label
            // 
            this._label.AutoSize = true;
            this._label.BackColor = System.Drawing.Color.Transparent;
            this._label.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._label.Location = new System.Drawing.Point(136, 11);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(42, 15);
            this._label.TabIndex = 0;
            this._label.Text = "0/100";
            this._label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ProggressBarWithLabel
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._label);
            this.DoubleBuffered = true;
            this.Name = "ProggressBarWithLabel";
            this.Size = new System.Drawing.Size(300, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (SolidBrush sb = new SolidBrush(this.ForeColor))
            {
                using (LinearGradientBrush lgb = new LinearGradientBrush(
                        new Rectangle(0, 0, this.Width, this.Height), 
                        Color.FromArgb(255, Color.White), Color.FromArgb(50, Color.White), 
                        LinearGradientMode.ForwardDiagonal))
                {
                    int width = (int)((float)_value / _maximum * this.Width);
                    e.Graphics.FillRectangle(sb, 0, 0, width, this.Height);
                    e.Graphics.FillRectangle(lgb, 0, 0, width, this.Height);
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetLabelLocation();
        }

        private void SetLabelText()
        {
            string labelText = "";

            if (_usePercentage)
                labelText = $"{_value}%";
            else
                labelText = $"{_value}/{_maximum}";

            if (_useText && _text.Length > 0)
                labelText += $" {_text}";

            _label.Text = labelText;
        }

        private void SetLabelLocation()
        {
            _label.Location = new Point(
                (this.Width - _label.Width) / 2,
                (this.Height - _label.Height) / 2);
        }

        private void _label_SizeChanged(object sender, EventArgs e)
        {
            SetLabelLocation();
        }

    }
}
