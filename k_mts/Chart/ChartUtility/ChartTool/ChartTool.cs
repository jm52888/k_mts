using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public abstract class ChartTool : ChartNamedUtility
    {
        #region Property

        [DefaultValue(false)]
        public virtual bool Reset
        {
            get { return false; }
            set { Initialize(); }
        }

        //[DefaultValue(false)]
        public virtual bool IsDrawing { get; set; }

        //[DefaultValue(false)]
        public virtual bool AllowAnchorMoving { get; set; }
        //[DefaultValue(true)]
        public virtual bool AllowMoving { get; set; }
        //[DefaultValue(true)]
        public virtual bool AllowPathEditing { get; set; }
        //[DefaultValue(true)]
        public virtual bool AllowResizing { get; set; }
        //[DefaultValue(true)]
        public virtual bool AllowSelecting { get; set; }
        //[DefaultValue(true)]
        public virtual bool AllowTextEditing { get; set; }
        //[DefaultValue(false)]
        public virtual bool IsSizeAlwaysRelative { get; set; }        
        //[DefaultValue(false)]
        public virtual bool IsInfinitive { get; set; }
        //[DefaultValue(false)]
        public virtual bool IsFreeDrawPlacement { get; set; }

        //[DefaultValue(typeof(Color), "Black"), Editor(typeof(ChartColorEditor), typeof(UITypeEditor)), Editor(typeof(ChartColorEditor), typeof(UITypeEditor))]        
        public virtual Color ForeColor { get; set; }
        //[DefaultValue(typeof(Color), "Transparent"), Editor(typeof(ChartColorEditor), typeof(UITypeEditor))]
        public virtual Color BackColor { get; set; }
        //[DefaultValue(typeof(Color), "Black"), Editor(typeof(ChartColorEditor), typeof(UITypeEditor))]
        public virtual Color LineColor { get; set; }
        //[DefaultValue(typeof(Color), "Transparent"), Editor(typeof(ChartColorEditor), typeof(UITypeEditor))]
        public virtual Color ShadowColor { get; set; }
        //[DefaultValue(typeof(Color), "Transparent"), Editor(typeof(ChartColorEditor), typeof(UITypeEditor))]
        public virtual Color BackSecondaryColor { get; set; }

        //[DefaultValue(1)]
        public virtual int LineWidth { get; set; }
        //[DefaultValue(0)]
        public virtual int ShadowOffset { get; set; }

        //[DefaultValue(typeof(LineAnchorCapStyle), "None")]
        public virtual LineAnchorCapStyle StartCap { get; set; }
        //[DefaultValue(typeof(LineAnchorCapStyle), "None")]
        public virtual LineAnchorCapStyle EndCap { get; set; }
        //[DefaultValue(typeof(GradientStyle), "None")]
        public virtual GradientStyle BackGradientStyle { get; set; }
        //[DefaultValue(typeof(ChartHatchStyle), "None")]
        public virtual ChartHatchStyle BackHatchStyle { get; set; }
        //[DefaultValue(typeof(ChartDashStyle), "Solid")]
        public virtual ChartDashStyle LineDashStyle { get; set; }
        //[DefaultValue(typeof(TextStyle), "Default")]
        public virtual TextStyle TextStyle { get; set; }
        //[DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        public virtual ContentAlignment Alignment { get; set; }        
        public virtual Font Font { get; set; }

        //[DefaultValue(false)]
		public virtual bool UseMagnet { get; set; }
        //[DefaultValue(typeof(Forms.Cursor), "Default")]
		public virtual Forms.Cursor Cursor { get; set; }
        //[DefaultValue(true)]
        public virtual bool LabelVisible { get; set; }
        //[DefaultValue(typeof(LabelAlignmentStyles), "Center")]
        public virtual Color LabelBackColor { get; set; }
        //[DefaultValue(typeof(LabelAlignmentStyles), "Center")]
        public virtual LabelAlignmentStyles LabelAlignment { get; set; }
        //[DefaultValue(0.0f)]
        public virtual float LabelOffsetX { get; set; }
        //[DefaultValue(0.0f)]
        public virtual float LabelOffsetY { get; set; }
        //[DefaultValue("")]
        public virtual string LabelFormat { get; set; }

        //[DefaultValue(false)]
        public virtual bool IsAlwaysPainting { get; set; }

        #endregion

        #region Constructor

        public ChartTool() { Initialize(); }
        public ChartTool(string name) : base(name) { Initialize(); }

        protected virtual void Initialize()
        {
            IsDrawing = false;

            AllowAnchorMoving = false;
            AllowMoving = true;
            AllowPathEditing = true;
            AllowResizing = true;
            AllowSelecting = true;
            AllowTextEditing = true;
            IsSizeAlwaysRelative = false;
            IsInfinitive = false;
            IsFreeDrawPlacement = false;

            ForeColor = Color.Black;
            BackColor = Color.Transparent;
            LineColor = Color.Black;
            ShadowColor = Color.Transparent;
            BackSecondaryColor = Color.Transparent;

            LineWidth = 1;
            ShadowOffset = 0;

            StartCap = LineAnchorCapStyle.None;
            EndCap = LineAnchorCapStyle.None;
            BackGradientStyle = GradientStyle.None;
            BackHatchStyle = ChartHatchStyle.None;
            LineDashStyle = ChartDashStyle.Solid;
            TextStyle = TextStyle.Default;
            Alignment = ContentAlignment.MiddleCenter;
            Font = SystemFonts.DefaultFont;

            UseMagnet = false;
            Cursor = Cursors.Default;
            LabelVisible = true;
            LabelAlignment = LabelAlignmentStyles.Center;
            LabelOffsetX = 0.0f;
            LabelOffsetY = 0.0f;
            LabelFormat = string.Empty;

            IsAlwaysPainting = false;
        }

        #endregion

        #region Method

        public abstract bool Select();
		public abstract void Release();

		#endregion
	}
}
