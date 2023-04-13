﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Controls.Native;
using Windows.UI.Theming;

namespace Windows.UI.Controls
{
    public partial class Window : Form
    {
        private bool _extendFrameIntoClientArea = false;
        private Theme _elementTheme = Theme.Light;

        public uint Glyph { get; set; }

        public bool BackdropApplied;

        public event EventHandler ThemeChanged;

        public Theme ElementTheme
        {
            get => _elementTheme;
            set
            {
                _elementTheme = value;
                OnThemeChanged();
            }
        }

        public bool ExtendFrameIntoClientArea
        { 
            get => _extendFrameIntoClientArea; 
            set 
            {
                _extendFrameIntoClientArea = value; 

            } 
        }

        public Window()
        {
            InitializeComponent();
            this.Load += Window_Load;
        }

        private void Window_Load(object? sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (_extendFrameIntoClientArea)
                {
                    var m = new DWMAPI.MARGINS() { bottomHeight = -1, leftWidth = -1, rightWidth = -1, topHeight = -1 };
                    DWMAPI.DwmExtendFrameIntoClientArea(this.Handle, ref m);
                }
                else
                {
                    var m = new DWMAPI.MARGINS() { bottomHeight = 0, leftWidth = 0, rightWidth = 0, topHeight = 0 };
                    DWMAPI.DwmExtendFrameIntoClientArea(this.Handle, ref m);
                }
            }
        }
        protected virtual void OnThemeChanged()
        {
            switch (_elementTheme)
            {
                case Theme.Dark:
                    var True = 1;
                    DWMAPI.DwmSetWindowAttribute(Handle, DWMAPI.DwmWindowAttribute.UseImmersiveDarkMode, ref True, sizeof(int));

                    if (!BackdropApplied)
                        this.BackColor = ThemeResource.DarkAppBackgroundColor;
                    
                    this.ForeColor = ThemeResource.DarkAppForegroundColor;

                    /**foreach (System.Windows.Forms.Control c in Controls)
                    {
                        if (c.GetType() == typeof(Control))
                        {
                            ((Control)c).ElementTheme = this._elementTheme;
                        }
                        else
                        {
                            c.ForeColor = ThemeResource.DarkAppForegroundColor;
                        }
                    }**/
                    UpdateControlsTheme();
                    break;
                case Theme.Light:
                    var False = 0;
                    DWMAPI.DwmSetWindowAttribute(Handle, DWMAPI.DwmWindowAttribute.UseImmersiveDarkMode, ref False, sizeof(int));

                    if (!BackdropApplied)
                        this.BackColor = ThemeResource.LightAppBackgroundColor;

                    this.ForeColor = ThemeResource.LightAppForegroundColor;

                    /**foreach (System.Windows.Forms.Control c in Controls)
                    {
                        if (c.GetType() == typeof(Control))
                        {
                            ((Control)c).ElementTheme = this._elementTheme;
                        }
                        else
                        {
                            c.ForeColor = ThemeResource.LightAppForegroundColor;
                        }
                    }**/
                    UpdateControlsTheme();
                    break;
            }
            if (ThemeChanged != null)
                ThemeChanged.Invoke(this, EventArgs.Empty);
        }

        void UpdateControlsTheme()
        {
            foreach (System.Windows.Forms.Control c in Controls)
            {
                if (c.GetType() == typeof(Windows.UI.Controls.ProgressBar))
                {
                    ((ProgressBar)c).ElementTheme = this._elementTheme;
                    if (ElementTheme == Theme.Dark)
                        c.ForeColor = ThemeResource.DarkAppForegroundColor;
                    else
                        c.ForeColor = ThemeResource.LightAppForegroundColor;
                }
                else if (c.GetType().BaseType == typeof(Windows.UI.Controls.Control))
                {
                    ((Control)c).ElementTheme = this._elementTheme;
                    if (ElementTheme == Theme.Dark)
                        c.ForeColor = ThemeResource.DarkAppForegroundColor;
                    else
                        c.ForeColor = ThemeResource.LightAppForegroundColor;
                }
                else if (c.GetType().BaseType == typeof(Windows.UI.Controls.TextBox))
                {
                    ((TextBox)c).ElementTheme = this._elementTheme;
                    if (ElementTheme == Theme.Dark)
                        c.ForeColor = ThemeResource.DarkAppForegroundColor;
                    else
                        c.ForeColor = ThemeResource.LightAppForegroundColor;
                }
                else
                {
                    if (ElementTheme == Theme.Dark)
                        c.ForeColor = ThemeResource.DarkAppForegroundColor;
                    else
                        c.ForeColor = ThemeResource.LightAppForegroundColor;
                }
            }
        }
    }
}
